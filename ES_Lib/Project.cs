using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using _3DEngine;
using GraphicLayer;
using System.Threading;

namespace ES_Lib
{
    public class Project
    {
        private string projectname;
        private bool backward, forward;
        public ArrayList Facts;
        public ArrayList Rules;
        private DFA_Builder CurrentDFA;
        private Parser Parse;
        private string DFA_RE;
        public string[] DFA_CFG;
        public Node ParseRoot = null;
        public DataGridView ErrorExplorerRef;
        public int FactCount = 0;
        public int RuleCount = 0;
        public RichTextBox TExtRef;
        public bool RunOnEnter = false;
        public static _3DEngine.Main GInterface;
        public CompressData CD;
        private Thread t1;
        private GraphicLayer.ControlLine CL;
        public bool AbstractUse = false;
        public string ResultText = "";
        public Node ASTDrawRoot;
        public string InitialPath = "";


        public Project()
        {
            CD = new CompressData();
            CurrentDFA = new DFA_Builder();
            Parse = new Parser();
            //InitilizeEngine();
            //GInterface = new Main();
        }

        public Project(string Name)        
        {
            CD = new CompressData();
            ProjectName = Name;
            CurrentDFA = new DFA_Builder();
            Parse = new Parser();
            //InitilizeEngine();
        }

        #region Properties

        public bool Backward
        {
            get { return backward; }
            set { backward = value; }
        }

        public bool Forward
        {
            get { return forward; }
            set { forward = value; }
        }

        public string ProjectName
        {
            get { return projectname; }
            set { if (value != "")projectname = value; }
        }

        #endregion

        public void InitilizeEngine()
        {
            CheckedITem = new ArrayList();
            Answers = new ArrayList();
            SychronousFacts = new ArrayList();
            DFA_RE= File.ReadAllText(InitialPath+"\\RE.txt");
            DFA_CFG = File.ReadAllLines(InitialPath+"\\CFG.txt");
            CurrentDFA.Build(DFA_RE);
            for (int i = 0; i < DFA_CFG.Length; i++)
            {
                if(DFA_CFG[i]!="")
                    Parse.Productions.Add(DFA_CFG[i]);
            }
            if (CurrentDFA.LanguageTokens.Count == 0)
            {
                CurrentDFA.LanguageTokens.Clear();
                CurrentDFA.GetLanguageTokens(CurrentDFA.root);
                CurrentDFA.ArrangeTokensArray();
            }
            Parse.DFA = CurrentDFA;
            Parse.Initiate();
            Facts = new ArrayList();
            Rules = new ArrayList();

        }

        public int GET_AST(string Data,int Line)
        {
            Parse.DFA = CurrentDFA;
            Parse.Initiate();
            Parse.UserIndex = 0;          
            Parse.OldParseIndex = 0;
            Parse.OldUserIndex = 0;
            Parse.Index = 0;
            Parse.GlobalIndex = 0;
            Parse.TempIndex = 0;
            Parse.ParserIndex = 0;
            Parse.CheckTermConsumed = false;
            if (CurrentDFA.LanguageTokens.Count == 0)
            {
                CurrentDFA.LanguageTokens.Clear();
                CurrentDFA.GetLanguageTokens(CurrentDFA.root);
                CurrentDFA.ArrangeTokensArray();
            }
            Parse.UserEXP = Data.ToLower();
            ParseRoot = Parse.Parse();
            if (ParseRoot == null)
            {
                //Parse.GetError(ParseRoot);                
                //ShowError(Line);
                TExtRef.Text += "\n     Syntax Error : Line " + Line + " at index " + Parse.ErrorIndex;
                return -1;
            }
            else
            {
                ShowError(Line);
                return 1;
            }
        }

        public void ShowError(int Line)
        {
            if (Parse.ErrorTracing.Count > 0)
            {               
                while (Parse.ErrorTracing.Count!=0)
                {
                    ErrorExplorerRef.Rows.Add(Parse.ErrorTracing.Pop(), "", Line, ""); 
                }                                 
            }
            Parse.ErrorTracing.Clear();

        }

        public void ClearErrorFacilities()
        {           
            if(ErrorExplorerRef!=null)
                ErrorExplorerRef.Rows.Clear();
        }

        public string[] GetTokens()
        {
            if (CurrentDFA.LanguageTokens.Count == 0)
            {
                CurrentDFA.LanguageTokens.Clear();
                CurrentDFA.GetLanguageTokens(CurrentDFA.root);
                CurrentDFA.ArrangeTokensArray();
            }
            string[] Data=new string[CurrentDFA.LanguageTokens.Count];
            CurrentDFA.LanguageTokens.CopyTo(Data);
            return Data;
        }

        public void DrawDFA()
        {
            DrawDFA DD = new DrawDFA();
            DD.root = CurrentDFA.root;
            DD.Show();
        }

        public void DrawAST()        
        {
            DrawAST DD = new DrawAST();
            DD.root = ParseRoot;
            DD.Show();
        }

        public void DrawHoffmanTree()
        {
            DrawHoffman DH = new DrawHoffman();
            DH.root = CD.Result;
            DH.Show();
        }

        public void Draw3DAST()
        {
            if (t1!=null && t1.IsAlive)
            {
                CL.root = ParseRoot;
                return;
            }
            t1 = new Thread(new ThreadStart(Run3D));
            t1.Start();
        }
        private void Run3D()
        {
            CL = new GraphicLayer.ControlLine(ParseRoot);
            CL.Test();
            
        }

        public object RUN(Node Root,int NexStateIndex)
        {
            if (Root != null)
            {
                string Next="";
                if (Root.NodeKind.ToLower() == "conjunction" || Root.NodeKind.ToLower() == "disjunction" || Root.NodeKind.ToLower()=="block" || Root.NodeKind.ToLower()=="mathexp")
                    Next = Root.NodeKind;
                else
                if (Root.nextstates.Count > 0 && Root.nextstates[0] != null)
                    Next = ((Node)Root.nextstates[NexStateIndex]).NodeKind;
                else
                {
                    if (Root.NodeKind == "String")
                        return Root.RunValue;
                    if (Root.NodeKind == "_" || Root.NodeKind == "+" || Root.NodeKind == "." || Root.NodeKind == "-" || Root.NodeKind == "*" || Root.NodeKind == "/" 
                        || Root.NodeKind=="if" || Root.NodeKind=="then" || Root.NodeKind=="else")
                        return Root.NodeKind;
                    else
                        return "";
                }
                Next = Next.ToLower();
                    switch (Next)
                    {
                        case "conjunction": string DisResult = "";
                            for (int i = 0; i < Root.nextstates.Count; i++)
                            {
                                if (i == 1)
                                    DisResult += " AND ";
                                else
                                    DisResult += (string)RUN((Node)Root.nextstates[i], i);
                            }
                            return DisResult;    
                        case "disjunction": DisResult = "";
                            for (int i = 0; i < Root.nextstates.Count; i++)
                            {                                
                                if(i==1)
                                    DisResult += " OR ";                                                              
                                else
                                    DisResult += (string)RUN((Node)Root.nextstates[i], i);
                            }
                            return DisResult;      
                        case "block": int SuccessNum = 0; string StringResult = "";
                            for (int i = 0; i < Root.nextstates.Count; i++)
                            {
                                object RE=null;
                                if(((Node)Root.nextstates[0]).NodeKind=="MathExp")
                                    RE = RUN((Node)Root.nextstates[0], 0);
                                else
                                    RE = RUN((Node)Root.nextstates[i],0);
                                if (RE != null)
                                {
                                    try
                                    {
                                        bool R = (bool)RE;
                                        return R;
                                    }
                                    catch (Exception ex)
                                    {
                                        if((string)RE!="")
                                            StringResult += " "+(string)RE+" ";
                                    }
                                }
                            }                            
                            return StringResult;                            
                        case "encode": string encodedData = "";
                            for (int i = 0; i < ((Node)Root.nextstates[1]).nextstates.Count; i++)
                            {
                                if(((Node)((Node)Root.nextstates[1]).nextstates[i]).NodeKind=="MathExp")
                                    encodedData += RUN((Node)Root.nextstates[1], i);
                                else
                                    encodedData += RUN((Node)((Node)Root.nextstates[1]).nextstates[i], i);
                            }                            
                            string Res=CD.getStringBitStream(CD.HoffmanCompress(encodedData),encodedData);
                            if(!AbstractUse)
                            ResultText += "\nEncoding Result: " + Res;
                            TExtRef.Text += "\nEncoding Result: " + Res;
                            return Res;
                            break;
                        case "htree": DrawHoffmanTree(); break;
                        case "addf": string Data = "";
                            bool checkSource = false;
                            if (((Node)((Node)Root.nextstates[1]).nextstates[0]).NodeKind == "MathExp")
                            {
                                Data += RUN((Node)Root.nextstates[1],0);
                                checkSource = true;
                            }
                            else
                                Data += RUN((Node)((Node)Root.nextstates[1]).nextstates[1],0);
                            ES_Lib.Fact F = new Fact(Data);
                            F.Index = FactCount;
                            F.ProcessFact();
                            FactCount++;
                            Facts.Add(F);
                            foreach (Rule R in Rules)
                            {
                                for (int i = 0; i < R.IFPART.Count;i++ )
                                {
                                    Rule.InnerStruct IS = (Rule.InnerStruct)R.IFPART[i];
                                    if (IS.IFPart.Trim() == F.Data.Trim())
                                    {
                                        R.IFPART.Remove(IS);
                                        IS.IsTrue = true;
                                        R.IFPART.Add(IS);
                                    }
                                }
                            }
                            if (!checkSource && !AbstractUse)
                            {
                                ResultText += "\nAdded Fact: " + F.Data;
                                TExtRef.Text += "\nAdded Fact: " + F.Data;
                            }
                            if (Data != "" && F != null)
                                return true;
                            break;
                        case "addr": Data = "";
                            if (((Node)Root.nextstates[2]).NodeKind != "Rule" && !AbstractUse)
                            {
                                ResultText += "\nCannot Add Rule : No Rule To be Added.";
                                TExtRef.Text += "\nCannot Add Rule : No Rule To be Added.";
                                return null;
                            }
                            bool ifPart = false;
                            for (int i = 0; i < ((Node)Root.nextstates[2]).nextstates.Count; i++)
                            {
                                string LocalData = RUN((Node)Root.nextstates[2], NexStateIndex) + " ";
                                Data += LocalData;
                                NexStateIndex++;
                                if (LocalData.Trim() == "if" || LocalData.Trim()=="then" || LocalData.Trim()=="else")
                                {
                                    try
                                    {
                                        Data += RUN((Node)((Node)Root.nextstates[2]).nextstates[i + 1], NexStateIndex).ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        Data += (string)RUN((Node)((Node)Root.nextstates[2]).nextstates[i+1], NexStateIndex);
                                    }
                                    NexStateIndex++;
                                    i++;
                                }
                            }
                            NexStateIndex = 0;
                            Rule RD = new Rule(Data);
                            float CF = 1;
                            try
                            {
                                CF = (float)Convert.ToDouble(((Node)Root.nextstates[4]).RunValue);
                            }
                            catch (Exception ex)
                            { }
                            RD.CertaintyFactor = CF;
                            RD.Index = RuleCount;
                            RuleCount++;
                            RD.ProcessRule(ifPart);
                            Rules.Add(RD);
                            if (!AbstractUse)
                            {
                                ResultText += "\n" + RD.Data;
                                TExtRef.Text += "\n" + RD.Data;
                            }
                            break;
                        case "rule": ifPart = false; Data = "";
                            NexStateIndex = 0;
                            for (int i = 0; i < ((Node)Root.nextstates[0]).nextstates.Count; i++)
                            {
                                if ((i == 3 && !ifPart) || (i == 5 && ifPart))
                                {
                                    NexStateIndex++;
                                    continue;
                                }
                                object REsp = RUN((Node)Root.nextstates[0], NexStateIndex);
                                try
                                {
                                    string LocalData = (string)REsp + " ";
                                    Data += LocalData;
                                    if (i == 1)
                                    {
                                        for (int j = 0; j < Facts.Count; j++)
                                        {
                                            Fact TF = (Fact)Facts[j];
                                            if (TF.Data == LocalData.Trim())
                                            {
                                                ifPart = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        if (LocalData.Trim() != "then" && LocalData.Trim() != "else" && LocalData.Trim() != "if" && !AbstractUse)
                                        {
                                            ResultText += "\n" + LocalData;
                                            TExtRef.Text += "\n" + LocalData;
                                        }
                                }
                                catch (Exception ex)
                                {
                                    bool boolRES = (bool)REsp;
                                    if (i == 1 && boolRES)
                                        ifPart = true;
                                }
                                NexStateIndex++;
                            }
                            break;
                        case "modr,": string[] Old_New = new string[2];
                            for (int j = 0; j < Old_New.Length; j++)
                            {
                                Old_New[j]=(string)RUN((Node)Root.nextstates[j],0);       
                            }
                            try
                            {
                                int RIndex = Convert.ToInt32(Old_New[0]);
                                for (int j = 0; j < Rules.Count; j++)
                                {
                                    Rule NewR = (Rule)Rules[j];
                                    if (NewR.Index == RIndex)
                                    {
                                        NewR.Data = Old_New[1];
                                        if (!AbstractUse)
                                        {
                                            ResultText += "\n" + NewR.Data;
                                            TExtRef.Text += "\n" + NewR.Data;
                                        }
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {                                
                                for (int j = 0; j < Rules.Count; j++)
                                {
                                    Rule NewR = (Rule)Rules[j];
                                    if (NewR.Data == Old_New[0])
                                    {
                                        NewR.Data = Old_New[1];
                                        if (!AbstractUse)
                                        {
                                            ResultText += "\n" + NewR.Data;
                                            TExtRef.Text += "\n" + NewR.Data;
                                        }
                                        break;
                                    }
                                }
                            }
                            
                            break;
                        case "modf,": string[] Old_New2 = new string[2];
                            for (int j = 0; j < Old_New2.Length; j++)
                            {
                                Old_New2[j] = (string)RUN((Node)Root.nextstates[j],0);
                            }
                            try
                            {
                                int RIndex = Convert.ToInt32(Old_New2[0]);
                                for (int j = 0; j < Facts.Count; j++)
                                {
                                    Fact NewR = (Fact)Facts[j];
                                    if (NewR.Index == RIndex)
                                    {
                                        NewR.Data = Old_New2[1];
                                        if (!AbstractUse)
                                        {
                                            ResultText += "\n" + NewR.Data;
                                            TExtRef.Text += "\n" + NewR.Data;
                                        }
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                for (int j = 0; j < Rules.Count; j++)
                                {
                                    Fact NewR = (Fact)Facts[j];
                                    if (NewR.Data == Old_New2[0])
                                    {
                                        NewR.Data = Old_New2[1];
                                        if (!AbstractUse)
                                        {
                                            ResultText += "\n" + NewR.Data;
                                            TExtRef.Text += "\n" + NewR.Data;
                                        }
                                        break;
                                    }
                                }
                            }
                            break;
                        case "clear": Facts.Clear(); break;
                        case "ast": DrawAST DD = new DrawAST();
                            Node Temp = new Node();
                            Temp.NodeKind = "Main";
                            Temp.isInitial = true;
                            Temp.Value = ".";
                            Temp.nextstates.Add(ASTDrawRoot);
                            DD.root = ASTDrawRoot;
                            DD.Show();break;
                        case "explain": if (!AbstractUse)
                                ResultText += "\n Under Construction.."; 
                                TExtRef.Text += "\n Under Construction.."; break;                            
                        case "facts":string FactsData="";
                            Fact FCC=null;
                            for (int j = 0; j < Facts.Count; j++)
                            {
                                FCC=(Fact)Facts[j];
                                if(FCC.DegreeOfBelieve>-1)
                                    FactsData += FCC.Index + " : " + FCC.Data+" with "+FCC.DegreeOfBelieve+" degree of believe.\n";
                                else
                                       FactsData += FCC.Index + " : " + FCC.Data+"\n";
                            }
                            if (!AbstractUse)
                            {
                                if (FCC != null && FCC.DegreeOfBelieve != -1)
                                {
                                    ResultText += "\n" + FactsData;
                                    TExtRef.Text += "\n" + FactsData;
                                }
                                else
                                {
                                    ResultText += "\n" + FactsData;
                                    TExtRef.Text += "\n" + FactsData;
                                }
                            }
                             return FactsData;
                            break;
                        case "rrules": for (int j = 0; j < Rules.Count; j++)
                            {
                                Rule FC = (Rule)Rules[j];
                                if (!AbstractUse)
                                {
                                    ResultText += "\n" + FC.Index + " : " + FC.Data;
                                    TExtRef.Text += "\n" + FC.Index + " : " + FC.Data;
                                }
                            }
                            break;
                        case "delf": if (Root.nextstates.Count == 0)
                            {
                                Parse.ErrorTracing.Push("Cannot Delete Fact.");
                                return null;
                            }
                            string Data2 = (string)RUN((Node)Root.nextstates[0],0);
                            for (int j = 0; j < Facts.Count; j++)
                            {
                                Fact FT = (Fact)Facts[j];
                                if (FT.Data == Data2)
                                {
                                    Facts.Remove(FT);
                                    if (!AbstractUse)
                                    {
                                        ResultText += "\nDeleted at index " + FT.Index;
                                        TExtRef.Text += "\nDeleted at index " + FT.Index;
                                    }
                                    if (Facts.Count > 0)
                                        j--;
                                }
                            }                            
                            break;
                        case "delr": string Data3 = (string)RUN((Node)Root.nextstates[0],0);
                            for (int j = 0; j < Rules.Count; j++)
                            {
                                Rule RE = (Rule)Rules[j];
                                if (RE.Data == Data3)
                                {
                                    Rules.Remove(RE);
                                    if (!AbstractUse)
                                    {
                                        ResultText += "\nDeleted at index " + RE.Index;
                                        TExtRef.Text += "\nDeleted at index " + RE.Index;
                                    }
                                    if (Rules.Count > 0)
                                        j--;
                                }
                            }    
                            break;
                        case "mathexp": string MathData = "";
                            for (int j = 1; j < Root.nextstates.Count-1; j++)
                            {
                                MathData += RUN((Node)Root.nextstates[j],0);
                            }
                            if (!AbstractUse)
                            {
                                ResultText += "\n" + Parse.MathParsing(MathData);
                                TExtRef.Text += "\n" + Parse.MathParsing(MathData);
                            }
                            return Parse.MathParsing(MathData);
                            break;
                        case "save": string FilePath = ((Node)((Node)Root.nextstates[1]).nextstates[1]).Value;
                            string SavedData = (string)RUN((Node)((Node)Root.nextstates[1]).nextstates[3],0);
                            string[] RealSavedData = SavedData.Split('\n');
                            try
                            {
                                File.WriteAllLines(FilePath, RealSavedData);
                                if (!AbstractUse)
                                    ResultText += "\nData has been saved successfully.";
                            }
                            catch (Exception ex)
                            {
                                if (!AbstractUse)
                                {
                                    ResultText += "\n" + ex.Message;
                                    TExtRef.Text += "\n" + ex.Message;
                                }
                            }
                            break;//savef<"path",data>
                        case "commands": break;// list engine commands
                        case "getf": break;
                        case "getr": break;
                        case "getfs": break;//make facts group
                        case "getrs": break;
                        case "oc": break; // Open channel
                        case "send": break;// send none encrypted data
                        case "sendc": break;//send cyphered text
                        case "listen": break;//listen to the opened channel
                        case "listenc": break;//listen to the cyphered opened channel
                        case "fattrib": break;//get fact attribute (Indexed)
                        case "clears": ResultText = "";
                            TExtRef.Text = ""; break;//Clear Screen

                        case "exit": Application.Exit();                            
                            break;
                        default: if (Root.NodeKind.ToLower() == "lstatement" || Root.NodeKind.ToLower() == "dstatement" || Root.NodeKind.ToLower() == "pstatement" || Root.NodeKind.ToLower() == "mstatement")
                            {                                
                                    if (Root.nextstates.Count >= 2)
                                    {
                                        for (int i = 0; i < Root.nextstates.Count; i++)
                                        {
                                            Root.RunValue = Root.RunValue + RUN((Node)Root.nextstates[i],0);
                                        }
                                        return Root.RunValue;
                                    }
                                    else
                                        return Root.RunValue;
                            }
                            else
                                return RUN((Node)Root.nextstates[NexStateIndex], 0); break;
                    }
                }
            return "";
        }
        public bool DoChaining()
        {
            if (Parse.ErrorTracing.Count!=0)
            {
                Parse.ErrorTracing.Push("Couldn't Fire Rule : Error in Re or CFG.");
                return false;
            }
            for (int j = 0; j < Rules.Count; j++)
            {
                Rule CurrentR = (Rule)Rules[j];
                
                if (!CurrentR.ISTrue)
                {
                    for (int i = 0; i < Facts.Count; i++)
                    {
                        Fact Current = (Fact)Facts[i];
                        if (CurrentR.ISFactTrue(Current.Data))
                        {
                            CurrentR.ISTrue = true;
                            if (!AbstractUse)
                            {
                                ResultText += "\nRule with Index " + CurrentR.Index + " Has Been Fired.\n\t==> Fact ";
                                TExtRef.Text += "\nRule with Index " + CurrentR.Index + " Has Been Fired.\n\t==> Fact ";
                            }
                            ES_Lib.Fact F = new Fact(CurrentR.THENPART);
                            F.Index = FactCount;
                            F.ProcessFact();
                            FactCount++;
                            Facts.Add(F);
                            if (!AbstractUse)
                            {
                                ResultText += CurrentR.THENPART + " has been added To WM";
                                TExtRef.Text += CurrentR.THENPART + " has been added To WM";
                            }
                            j = -1;
                            break;
                        }
                    }
                }
            }

            for (int j = 0; j < Rules.Count; j++)
            {
                Rule Temp = (Rule)Rules[j];
                Temp.ISTrue = false;
            }
            return true;
        }


        ArrayList SychronousFacts = new ArrayList();
        Fact TempF = new Fact();
        ArrayList CheckedITem = new ArrayList();
        ArrayList Answers = new ArrayList();

        public Fact DoChaining(bool StepByStep)
        {
            TempF = null;
            CheckedITem.Add(((Fact)Facts[0]).Data);
            Answers.Add(((Fact)Facts[1]).Data);
            if (Parse.ErrorTracing.Count != 0)
            {
                Parse.ErrorTracing.Push("Couldn't Fire Rule : Error in Re or CFG.");
                return null;
            }
            for (int j = 0; j < Rules.Count; j++)
            {
                Rule CurrentR = (Rule)Rules[j];

                if (!CurrentR.ISTrue)
                {
                    if (CurrentR.RuleType == Rule.OperationType.NONE)
                    {
                        for (int i = 0; i < Facts.Count; i++)
                        {
                            Fact Current = (Fact)Facts[i];
                            int NumofFiring = 0;
                            foreach (Fact F in Facts)
                            {
                                if (F.Data.ToLower().Trim() == Current.Data.ToLower().Trim())
                                    NumofFiring++;
                            }
                            if (CurrentR.ISFactTrue(Current.Data) && NumofFiring > CurrentR.NumOfFiring)
                            {
                                CurrentR.ISTrue = true;
                                CurrentR.NumOfFiring++;
                                if (!AbstractUse)
                                {
                                    ResultText += "\nRule with Index " + CurrentR.Index + " Has Been Fired.\n\t==> Fact ";
                                    TExtRef.Text += "\nRule with Index " + CurrentR.Index + " Has Been Fired.\n\t==> Fact ";
                                }
                                ES_Lib.Fact F = new Fact(CurrentR.THENPART);
                                F.Index = FactCount;
                                F.ProcessFact();
                                FactCount++;
                                if (StepByStep)
                                    if (TempF == null)
                                        TempF = F;
                                    else
                                        SychronousFacts.Add(F);
                                else
                                    Facts.Add(F);
                                if (!AbstractUse)
                                {
                                    ResultText += CurrentR.THENPART + " has been added To WM";
                                    TExtRef.Text += CurrentR.THENPART + " has been added To WM";
                                }
                                j = -1;                               
                                break;
                            }
                        }
                    }
                    if (CurrentR.RuleType == Rule.OperationType.AND)
                    {
                        Fact Current = null;
                        int Flag = 0;
                        ArrayList TempData = new ArrayList();
                        foreach (Rule.InnerStruct IS in CurrentR.IFPART)
                        {                            
                            for (int i = 0; i < Facts.Count; i++)
                            {
                                Current = (Fact)Facts[i];
                                int NumofFiring = 0;
                                foreach (Fact F in Facts)
                                {
                                    if (F.Data.ToLower().Trim() == Current.Data.ToLower().Trim())
                                        NumofFiring++;
                                }
                                if (CurrentR.ISFactTrue(Current.Data) && NumofFiring > CurrentR.NumOfFiring && !TempData.Contains(Current.Data))
                                {
                                    Flag ++;
                                    TempData.Add(Current.Data);
                                    break;         
                                }
                            }
                            if (Flag==CurrentR.IFPART.Count)
                            {
                                CurrentR.ISTrue = true;
                            }                            
                        }
                        if (CurrentR.ISTrue)
                        {
                            CurrentR.NumOfFiring++;
                            if (!AbstractUse)
                            {
                                ResultText += "\nRule with Index " + CurrentR.Index + " Has Been Fired.\n\t==> Fact ";
                                TExtRef.Text += "\nRule with Index " + CurrentR.Index + " Has Been Fired.\n\t==> Fact ";
                            }
                            ES_Lib.Fact F = new Fact(CurrentR.THENPART);
                            F.Index = FactCount;
                            F.ProcessFact();
                            FactCount++;
                            if (StepByStep)
                                if (TempF == null)
                                    TempF = F;
                                else
                                    SychronousFacts.Add(F);
                            else
                                Facts.Add(F);
                            if (!AbstractUse)
                            {
                                ResultText += CurrentR.THENPART + " has been added To WM";
                                TExtRef.Text += CurrentR.THENPART + " has been added To WM";
                            }                    
                            j = -1;                            
                        }
                    }
                }
            }

            for (int k = 0; k < Rules.Count; k++)
            {
                Rule Temp = (Rule)Rules[0];
                Rules.Remove(Temp);
                Temp.ISTrue = false;
                Rules.Add(Temp);
            }
            if (TempF==null && SychronousFacts.Count == 0)
                return null;
            else
            {
                if (TempF != null)
                {
                    if (CheckedITem.Contains(TempF.Data))
                    {
                        Facts.Clear();
                        Facts.Add(TempF);
                        Fact FF = new Fact(Answers[CheckedITem.IndexOf(TempF.Data)].ToString());
                        FF.ProcessFact();
                        Facts.Add(FF);
                        return DoChaining(true);
                    }
                    else
                        return TempF;
                }
                else
                {
                    Fact Temp = (Fact)SychronousFacts[SychronousFacts.Count - 1];
                    Facts.Add(Temp);
                    SychronousFacts.Remove(Temp);
                    return Temp;
                }
            }
        }

       
        public static void SetDrawingWindow(IntPtr PTR)
        {
            GInterface.hContainer = PTR;
            GInterface.InitOpenGL(PTR);
        }

        public static void Draw()
        {
            GInterface.Render();
        }

        public void DisposeRendering()
        {
            //GInterface.Destroy();
            if (t1 != null && t1.IsAlive)
                t1.Abort();
                
        }
        
    }
}