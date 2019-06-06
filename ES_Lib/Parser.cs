using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ES_Lib
{
    public class Parser
    {
        public ArrayList Productions;
        public DFA_Builder DFA;
        public string EXP;
        public string UserEXP;
        public int UserIndex,OldUserIndex,OldParseIndex,ParserIndex;
        public ArrayList NonTerminalNames;
        public Stack<string> ErrorTracing=new Stack<string>();
        public Node Result;
        public bool CheckTermConsumed = false;
        public bool EOF = false;
        public int ErrorIndex = 0;

             
        
 
        private string[] NTN;
        public int Index, TempIndex,GlobalIndex;

        public Parser()
        {
            Productions = new ArrayList();
            NonTerminalNames = new ArrayList();
            UserIndex = 0;
            UserEXP = "";
        }

        public struct NonTerminalsIndeces
        {
            public int Index;
            public string NonTerminalName;
        }

             
        public void GetNonTerminalNames()
        {
            for (int i = 0; i < Productions.Count; i++)
            {
                NonTerminalNames.Add(Productions[i].ToString().Split('=', '>')[0]);
            }
        }

        /*
        public Node Parse(string Rule)
        {          
            Node ResultNode=new Node();
            int Num = 0;
            int N2 = 0,N=0;
            bool Jump = true, BreakSequence = false;
            for (int i = 0; i < Productions.Count; i++)
            {
                string[] ST = new string[1];
                ST[0] = "=>";
                string Name = Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                if (Rule != "")
                    if (Name != Rule)
                        continue;

                if (Name == "Statement" && !CheckTermConsumed)
                {
                    ErrorTracing.Push("UnDefined Token at "+DFA.INDEX.ToString());
                    return null;
                }
                string ResultOfRule = Productions[i].ToString().Split(ST,2,StringSplitOptions.RemoveEmptyEntries)[1];

                string[] SubRules = ResultOfRule.Split('|');
                
                for (int j = 0; j < SubRules.Length; j++)
                {                   
                    Jump = true;
                    string[] Terminals = SubRules[j].Split(NTN, StringSplitOptions.RemoveEmptyEntries);                    
                    for (int k = 0; k < Terminals.Length; k++)
                    {
                        Jump = false; 
                        DFA.EXP = Terminals[k];
                        string Res = "";                        
                        DFA.Initiate();
                        Index = DFA.GetNextToken();                        
                        if (Index >= 0 && !DFA.TokenError)
                            Res = DFA.LanguageTokens[Index].ToString();
                        else
                        {
                            ErrorTracing.Push("Error In your RE at Index : " + DFA.INDEX.ToString() + " With NonTerminal :" + SubRules[j]+" at "+DFA.EXP);
                            return null;
                        }
                        DFA.AlternativeResult.Clear();
                        while (Res != "EOF")
                        {
                            string TempExp = DFA.EXP;
                            TempIndex = DFA.INDEX;
                            DFA.EXP = UserEXP;
                            DFA.INDEX = UserIndex;
                            int CurrentIndex = DFA.GetNextToken();
                            string Res2 = "";
                            if (CurrentIndex >= 0 && !DFA.TokenError)
                                Res2 = DFA.LanguageTokens[CurrentIndex].ToString();
                            else
                            {
                                ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " With NonTerminal :" + SubRules[j]+" at "+UserEXP);
                                return null;
                            }
                            UserIndex = DFA.INDEX;
                            DFA.INDEX = TempIndex;
                            DFA.EXP = TempExp;
                            if (Res2 == "EOF")
                            {                                
                                //MessageBox.Show(ResultNode.NodeKind + " is Missing at Index " + DFA.INDEX.ToString());
                                return ResultNode;                                
                            }
                            if (Res != Res2)
                            {
                                DFA.UnGetToken();
                                UserIndex = DFA.INDEX;
                                DFA.AlternativeResult.Clear();

                                if (j + 1 < SubRules.Length && SubRules[j + 1] == Rule)
                                {                                 
                                    GlobalIndex = 0;
                                    return ResultNode;
                                }
                                else
                                {
                                    BreakSequence = true;
                                    break;
                                }
                            }
                            if (Name == "Term" || Name == "Block")
                                CheckTermConsumed = true;
                            ResultNode.NodeKind = Name;
                            if (Res2 == "letter" || Res2=="digit")
                                ResultNode.Value += Res2 + " " + DFA.CurrentChar.ToString();
                            else
                                ResultNode.Value += Res2;
                            DFA.AlternativeResult.Clear();
                            Index = DFA.GetNextToken();
                            Res = "";
                            BreakSequence = false;
                            if (Index >= 0 && !DFA.TokenError)
                                Res = DFA.LanguageTokens[Index].ToString();
                            else
                            {
                                ErrorTracing.Push("Error In your RE at Index : " + TempIndex.ToString() + " With NonTerminal :" + SubRules[j]+" at "+TempExp);
                                return null;
                            }
                        } 

                        if (BreakSequence)
                            break;
                        Num += Terminals[k].Length;
                        bool Test = false;
                        N = ResultOfRule.IndexOf(SubRules[j],GlobalIndex) + Num;
                        if (BreakSequence && N!=0)
                            GlobalIndex = N;
                        int TE = 0;
                        Node TT = null;
                    
                        for (int p = 0; p < NonTerminalNames.Count; p++)
                        {
                            if (N>=0 && ResultOfRule.Length >= N + NonTerminalNames[p].ToString().Length)
                                if (ResultOfRule.Substring(N, NonTerminalNames[p].ToString().Length) == NonTerminalNames[p].ToString())
                                {
                                    TE = NonTerminalNames[p].ToString().Length;
                                    //Result2 = Parse(NonTerminalNames[p].ToString());                                   
                                    TT = Parse(NonTerminalNames[p].ToString());                                    
                                    Test = true;
                                }
                            if (TT!=null)
                            {
                                ResultNode.nextstates.Add(TT);                              
                                break;
                            }
                            if (N + NonTerminalNames[p].ToString().Length < SubRules[j].Length && Test)
                            {
                                N += NonTerminalNames[p].ToString().Length;
                                p = 0;
                            }
                        }
                        Num += TE; 
                    }                  
                    
                    if (Jump)
                    {
                        bool Test2 = false;
                        Jump = false;
                        N2 = ResultOfRule.IndexOf(SubRules[j],GlobalIndex) + Num;
                        if (BreakSequence && N != 0)
                            GlobalIndex = N;
                        int TE2 = 0;
                        Node TT2 = null;
                        for (int p = 0; p < NonTerminalNames.Count; p++)
                        {
                            if (N2>=0 && ResultOfRule.Length >= N2 + NonTerminalNames[p].ToString().Length)
                                if (ResultOfRule.Substring(N2, NonTerminalNames[p].ToString().Length) == NonTerminalNames[p].ToString())
                                {
                                    TE2 = NonTerminalNames[p].ToString().Length;
                                    //Result3 = Parse(NonTerminalNames[p].ToString());                                   
                                    TT2 = Parse(NonTerminalNames[p].ToString());
                                    Test2 = true;
                                }
                            if (TT2!=null)
                            {
                                ResultNode.nextstates.Add(TT2);                               
                                break;
                            }                           
                            if (N2 + NonTerminalNames[p].ToString().Length < SubRules[j].Length && Test2)
                            {
                                N2 += NonTerminalNames[p].ToString().Length;
                                p = -1;
                            }
                        }
                        Num += TE2;
                       
                    }

                }
            }            
            return ResultNode;
        }
        

        


        public Node Parse()
        {
            string[] ST = new string[1];
            ST[0] = "=>";
            for (int i = 0; i < Productions.Count; i++)
            {
                Node Temp = SubParse(Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1], 0,true);
                if(Temp!=null)
                    return Temp;
            }
            return null;
        }

        public string Sender = "";
        public bool SenderFlag = false;
        public Node SubParse(string Rule,int ParserIndex,bool Lock)
        {
            Node root = new Node();
            int TTemp = 0;
            bool ReturnFlag = false;                 
            DFA.EXP = UserEXP;
            DFA.INDEX = UserIndex;
            OldUserIndex = UserIndex;
            int CurrentIndex = DFA.GetNextToken();
            string Res = "";
            if (CurrentIndex >= 0 && !DFA.TokenError)
                Res = DFA.LanguageTokens[CurrentIndex].ToString();
            else
            {
                ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " at " + UserEXP);
                return null;
            }
            char TempCurrentChar = DFA.CurrentChar;
            UserIndex = DFA.INDEX;
            string[] ST = new string[1];
            ST[0] = "=>";
            string[] SubRules = Rule.Split('|');
            bool flag = true;
            for (int i = 0; i < SubRules.Length; i++)
            {
                DFA.EXP = SubRules[i];
                DFA.INDEX = ParserIndex;
                OldParseIndex = ParserIndex;
                CurrentIndex = DFA.GetNextToken();
                string Res2 = "";
                if (CurrentIndex >= 0 && !DFA.TokenError)
                    Res2 = DFA.LanguageTokens[CurrentIndex].ToString();
                else
                {
                    ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " at " + UserEXP);
                    return null;
                }
                ParserIndex = DFA.INDEX;
                bool CheckRes = false;

                while(Res2 == Res)
                {
                    ReturnFlag = true;
                    for (int j = 0; j < Productions.Count; j++)
                    {
                        if (Rule == Productions[j].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1])
                        {
                            root.Value = Productions[j].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                            break;
                        }
                    }                    
                    root.NodeKind = root.NodeKind + Res2;
                    if (Res == "letter" || Res == "digit")
                    {                        
                        root.RunValue = TempCurrentChar.ToString();
                    }
                    else
                        if(Res!=">" && Res!="<")
                            root.RunValue = Res;
                    CheckRes = true;
                    DFA.EXP = UserEXP;
                    if (TTemp > UserIndex)
                    {
                        DFA.INDEX = TTemp;
                        OldUserIndex = TTemp;
                    }
                    else
                        DFA.INDEX = UserIndex;
                    OldUserIndex = UserIndex;
                    CurrentIndex = DFA.GetNextToken();
                    Res = "";
                    if (CurrentIndex >= 0 && !DFA.TokenError)
                        Res = DFA.LanguageTokens[CurrentIndex].ToString();
                    else
                    {
                        ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " at " + UserEXP);
                        return null;
                    }
                    UserIndex = DFA.INDEX;

                    DFA.EXP = SubRules[i];
                    DFA.INDEX = ParserIndex;
                    OldParseIndex = ParserIndex;
                    CurrentIndex = DFA.GetNextToken();
                    Res2 = "";
                    if (CurrentIndex >= 0 && !DFA.TokenError)
                        Res2 = DFA.LanguageTokens[CurrentIndex].ToString();
                    else
                    {
                        ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " at " + UserEXP);
                        return null;
                    }
                    ParserIndex = DFA.INDEX;
                }
                //if(CheckRes)
                TTemp = UserIndex;
                UserIndex = OldUserIndex;
                

                bool check = false;
                for (int j = 0; j < Productions.Count; j++)
                {
                    if (Res2 == Productions[j].ToString().Split(ST,2,StringSplitOptions.RemoveEmptyEntries)[0])
                    {
                        if(Lock)
                            Sender = Res2;
                        Node Temp = SubParse(Productions[j].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1], 0,false);
                        if (Temp != null)
                        {
                            check = true;
                            root.nextstates.Add(Temp);
                            break;

                        }
                        else
                        {
                            UserIndex = TTemp;
                            if (SenderFlag)
                            {
                                SenderFlag = false;
                                if (Sender != Res2)
                                {
                                    OldUserIndex = TTemp;
                                    
                                }
                            }
                            break;
                        }
                    }
                }
                bool breakparsing = false;
                if (check)
                {
                    int CheckRes2 = 0;
                    do
                    {
                        root.NodeKind = root.NodeKind+ Res2;
                        
                        for (int j = 0; j < Productions.Count; j++)
                        {
                            if (Rule == Productions[j].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1])
                            {
                                root.Value = Productions[j].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                                break;
                            }
                        }
                        CheckRes2++;
                        CheckRes = true;
                        DFA.EXP = UserEXP;
                        DFA.INDEX = UserIndex;
                        OldUserIndex = UserIndex;
                        CurrentIndex = DFA.GetNextToken();
                        Res = "";
                        if (CurrentIndex >= 0 && !DFA.TokenError)
                            Res = DFA.LanguageTokens[CurrentIndex].ToString();
                        else
                        {
                            ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " at " + UserEXP);
                            return null;
                        }
                        UserIndex = DFA.INDEX;
                        if (Res == "letter" || Res == "digit")
                        {                            
                            root.RunValue = DFA.CurrentChar.ToString();
                        }
                        else
                        {
                            if (Res != "EOF" && Res!=">" && Res!="<")
                            {
                                root.RunValue = Res;
                            }
                        }

                        DFA.EXP = SubRules[i];
                        DFA.INDEX = ParserIndex;
                        OldParseIndex = ParserIndex;
                        CurrentIndex = DFA.GetNextToken();
                        Res2 = "";
                        if (CurrentIndex >= 0 && !DFA.TokenError)
                            Res2 = DFA.LanguageTokens[CurrentIndex].ToString();
                        else
                        {
                            ErrorTracing.Push("Error In your RE at Index : " + UserIndex.ToString() + " at " + UserEXP);
                            return null;
                        }
                        ParserIndex = DFA.INDEX;
                    } while (Res2 == Res && Res!="EOF");
                    if (Res == "EOF")
                        return root;
                    if (Res2!="EOF" && CheckRes2 == 1)
                    {
                        ErrorTracing.Push("Semantic Error at " + Res2);
                        UserIndex = 0;
                        OldUserIndex = 0;
                        TTemp = 0;
                        SenderFlag = true;
                        return null;
                        
                    }                   
                    if (Res2 != "EOF" && CheckRes2>=2)
                    {
                        UserIndex = OldUserIndex;
                        ParserIndex = OldParseIndex;
                        i--;
                        breakparsing = true;
                    }
                    if (Res2 == "EOF" && CheckRes2 == 1)
                    {
                        UserIndex = OldUserIndex;
                        return root;
                    }root
                    
                }
                if(!breakparsing)
                    ParserIndex = 0;
            }
            UserIndex = OldUserIndex;            
            if (ReturnFlag)
                return root;
            else
                return null;
        }

        int MaxX = 0;
             
        public Node Parse()
        {
            MaxX = GetMaxX();
            return SubParse(new Node(), 0, 0);
        }
        public Node SubParse(Node root, int X, int Y)
        {
            int ParserIndex=0;
            int MaxY = GetMaxY(X);
            bool First = false,NT=false;
            string rule="",S1="",S2="";
            rule = GetRule(X, Y);
            do
            {
                S1 = GetNextRuleToken(rule,ParserIndex);
                S2 = GetNextUserToken();
                if (S1 == S2)
                {
                    Node T = new Node();
                    T.NodeKind = S1;
                    if (S2 == "letter" || S2 == "digit")
                    {
                        root.RunValue = DFA.CurrentChar.ToString();
                    }
                    root.nextstates.Add(T);
                }
                if (S2 == "EOF")
                    return root;
            } while (S1 == S2);
            
                UnGetUserToken();
                if (IsRule(S1))
                {
                    root.NodeKind = GetNonTerminalName(rule);
                    ArrayList NonTerminals = GetNonTerminals(rule);
                    for (int i = 0; i < NonTerminals.Count; i++)
                    {
                        ParserIndex = 0;
                        Coordinate CO = GetCoordinate(NonTerminals[i].ToString());
                        Node Temp = SubParse(new Node(), CO.X, CO.Y);
                        if (Temp != null)
                        {
                            root.nextstates.Add(Temp);
                            if (IsRule(GetNextRuleToken(rule, ParserIndex)))
                            {
                                ParserIndex = OldParseIndex;
                                continue;
                            }
                            else
                            {
                                First = false;
                                do
                                {
                                    S1 = GetNextRuleToken(rule, ParserIndex);
                                    S2 = GetNextUserToken();
                                    if (S1 == S2)
                                    {
                                        First = true;
                                        Node T = new Node();
                                        T.NodeKind = S1;
                                        if (S2 == "letter" || S2 == "digit")
                                        {
                                            root.RunValue = DFA.CurrentChar.ToString();
                                        }
                                        root.nextstates.Add(T);
                                    }
                                    if (S2 == "EOF")
                                        return root;
                                } while (S1 == S2);
                                if (!First)
                                    return null;
                            }
                        }
                        else
                        {
                            root.NodeKind = GetNonTerminalName(rule);
                            Node Temp2 = null;
                            do
                            {
                                UserIndex = 0;
                                ParserIndex = 0;
                                Temp2 = null;
                                if (Y + 1 >= MaxY)
                                {
                                    if (X + 1 < MaxX)
                                    {
                                        X++;
                                        Temp2 = SubParse(root, X, 0);
                                    }
                                    else
                                        return null;
                                }
                                else
                                {
                                    Y++;
                                    Temp2 = SubParse(root, X, Y);
                                }
                                if (Temp2 != null)
                                    return Temp2;
                            } while (Temp2 == null);
                        }
                    }
                }            
            return null;
        }
         */

        int MaxX = 0;
        bool Finish = false;
        public Node Parse()
        {
            MaxX = GetMaxX();
            int X = 0;
            ErrorIndex = 0;
            return SubParse(new Node(), 0,ref X,0,0,NonTerminalNK(0));
        }

        public Node SubParse(Node root, int ParserI,ref int UserI, int X, int Y,string NK)
        {
            int MaxY = GetMaxY(X);
            string rule = GetRule(X, Y);
            if(UserI>ErrorIndex)
                ErrorIndex = UserI;
            int OldU = UserI;
            int OldP = ParserI;
            if (Check(root, rule, ref ParserI, ref UserI))
            {
                ParserI = OldParseIndex;
                string T = GetNextRuleToken(rule,ref ParserI);
                if (T == "EOF")
                    return root;
                if (!IsRule(T))
                    return null;
                else
                {
                    //root.NodeKind = GetNonTerminalName(rule);
                    root.NodeKind = NK;
                    ArrayList NonTerminals = GetNonTerminals(rule);
                    bool Go = false;
                    int TI = UserI;
                    for (int i = 0; i < NonTerminals.Count; i++)
                    {
                        Coordinate Co = GetCoordinate(NonTerminals[i].ToString());                      
                        Node Temp = SubParse(new Node(), 0,ref TI, Co.X, Co.Y,NonTerminalNK(Co.X));//ti=60
                        if (Temp != null)
                        {
                            //if (i > 0)
                            //    ParserI += NonTerminals[i].ToString().Length;
                            root.nextstates.Add(Temp);
                            UserI = TI;
                            bool Res=Check(root,rule,ref ParserI,ref UserI);
                            TI = UserI;
                            if ((Res && UserI == UserEXP.Length - 1) || UserI == UserEXP.Length)
                                Finish = true;

                            if (!Res)
                            {
                                ParserI = OldParseIndex;
                                T = GetNextRuleToken(rule, ref ParserI);
                                if (!IsRule(T) && T != "EOF")
                                {
                                    TI = UserI;
                                    root.nextstates.Clear();
                                    Go = true;
                                    break;
                                }
                                if (T == "EOF" && UserI < UserEXP.Length && OldU == 0 && !Finish)
                                    return null;
                            }
                            if (EOF)
                            {
                                EOF = false;
                                return root;
                            }
                        }
                        else
                        {
                            TI = UserI;
                            root.nextstates.Clear();
                            Go = true;
                            break;
                        }
                    }
                    if (Go)
                    {
                        Node Temp2 = null;
                        root.nextstates.Clear();
                        int Old = OldU;
                        do
                        {
                            Finish = false;
                            Old = OldU;
                            Temp2 = null;
                            if (Y + 1 >= MaxY)
                            {
                                /*
                                if (X + 1 < MaxX)
                                {
                                    X++;
                                    Temp2 = SubParse(root, 0, ref Old, X, 0, NonTerminalNK(X));
                                }
                                else
                                 */
                                    return null;
                            }
                            else
                            {
                                Y++;                                                 
                                Temp2 = SubParse(root, 0, ref Old, X, Y,NonTerminalNK(X));
                            }
                            if (Temp2 != null)
                            {
                                root = Temp2;
                                root.NodeKind = NK;
                                UserI = Old;
                            }
                        } while (Temp2 == null);
                    }
                }
            }
            else
            {
                ParserI = OldParseIndex;
                string T = GetNextRuleToken(rule, ref ParserI);
                if (T == "EOF")
                    return root;
                if (!IsRule(T) && X<7)
                    return null;
                else
                {
                    ArrayList NonTerminals=new ArrayList();
                    bool Goo = false;
                    if (X >= 7)
                        Goo = true;
                    else
                    {
                        NonTerminals = GetNonTerminals(rule);
                    }
                    int TII = OldU;
                    for (int i = 0; i < NonTerminals.Count; i++)
                    {
                        Coordinate Co = GetCoordinate(NonTerminals[i].ToString());                      
                        Node Temp = SubParse(new Node(), 0, ref TII, Co.X, Co.Y,NonTerminalNK(Co.X));
                        if (Temp != null)
                        {
                            MaxY = GetMaxY(X);
                            if (i > 0)
                                ParserI += NonTerminals[i].ToString().Length;
                            root.NodeKind = NK;
                            //root.NodeKind = GetNonTerminalName(rule);
                            UserI = TII;
                            root.nextstates.Add(Temp);
                            bool Res=Check(root, rule, ref ParserI, ref UserI);
                            TII = UserI;
                            if ((Res && UserI == UserEXP.Length - 1) || UserI == UserEXP.Length)
                                Finish = true;
                            
                            if (!Res)
                            {
                                ParserI = OldParseIndex;
                                T = GetNextRuleToken(rule, ref ParserI);
                                if (!IsRule(T) && T!="EOF")
                                {
                                    TII = OldU;
                                    root.nextstates.Clear();
                                    Goo = true;
                                    break;
                                }
                                if (T == "EOF" && UserI < UserEXP.Length && OldU==0 && !Finish)
                                    return null;
                            }
                             
                            if (EOF)
                            {
                                EOF = false;
                                return root;
                            }                            
                        }
                        else
                        {
                            TII = OldU;
                            root.nextstates.Clear();
                            Goo = true;
                            break;
                        }
                    }
                    if (Goo)
                    {
                        Node Temp2 = null;
                        int Oldd;
                        do
                        {
                            Finish = false;
                            root.nextstates.Clear();
                            Oldd = OldU;
                            Temp2 = null;
                            if (Y + 1 >= MaxY)
                            {
                                /*
                                if (X + 1 < MaxX)
                                {
                                    X++;
                                    Temp2 = SubParse(root,0, ref Oldd, X,0, NonTerminalNK(X));
                                }
                                else                                
                                 */
                                    return null;
                            }
                            else
                            {
                                Y++;
                               
                                Temp2 = SubParse(root,0, ref Oldd, X, Y,NonTerminalNK(X));
                            }
                            if (Temp2 != null)
                            {
                                root = Temp2;
                                root.NodeKind = NK;
                                UserI = Oldd;
                            }
                                //root.nextstates.Add(Temp2);
                        } while (Temp2 == null);
                    }
                }
            }
            return root;
        }

        public bool Check(Node root,string Rule,ref int ParseI,ref int UserI)
        {
            string S1 = "", S2 = "";
            bool First = false;
            ParserIndex = ParseI;
            UserIndex = UserI;
            bool CheckString = false;
            do
            {
                S1 = GetNextRuleToken(Rule,ref ParserIndex);
                S2 = GetNextUserToken(ref UserIndex);
                if (S1 == "\"" && S2.Length > 0)
                {
                    S1 = S2;
                    ParserIndex++;
                    CheckString = true;
                }
                if (S1 == S2)
                {
                    if (S2 == "EOF")
                    {
                        EOF = true;
                        UserI = UserIndex;
                        return true;
                    }
                    
                    Node T = new Node();
                    T.NodeKind = S1;
                    if (S2 == "letter" || S2 == "digit")
                    {
                        root.RunValue = DFA.CurrentChar.ToString();
                    }
                    if (CheckString)
                    {
                        T.NodeKind = "String";
                        T.RunValue = S1;
                        T.Value = S1;
                        CheckString = false;
                    }
                    root.nextstates.Add(T);
                    First = true;
                }
            } while (S1 == S2);
            ParseI = ParserIndex;
            UnGetUserToken();
            UserI = UserIndex;
            return First;
        }

        public int GetMaxY(int X)
        {
            ST[0] = "=>";
            return Productions[X].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1].Split('|').Length;
        }

        public int GetMaxX()
        {
            return Productions.Count;
        }

        string[] ST = new string[1];

        public string GetNextRuleToken(string Rule,ref int ParserI)
        {
            DFA.EXP = Rule;
            DFA.INDEX = ParserI;
            OldParseIndex = ParserI;
            int CurrentIndex = DFA.GetNextToken();
            string Res2 = "";
            if (CurrentIndex >= 0 && !DFA.TokenError)
                Res2 = DFA.LanguageTokens[CurrentIndex].ToString();
            else
            {
                ErrorTracing.Push("Error In your RE at Index : " + ParserI.ToString() + " at " + Rule);
                return null;
            }
            ParserI = DFA.INDEX;
            return Res2;
        }

        public string GetNextUserToken(ref int UserI)
        {
            DFA.EXP = UserEXP;
            DFA.INDEX = UserI;
            OldUserIndex = UserI;
            int CurrentIndex = DFA.GetNextToken();
            string Res = "";
            if (CurrentIndex >= 0 && !DFA.TokenError)
                Res = DFA.LanguageTokens[CurrentIndex].ToString();
            else
            {
                ErrorTracing.Push("Error In your RE at Index : " + UserI.ToString() + " at " + UserEXP);
                return null;
            }
            if (Res == "\"" && DFA.INDEX<UserEXP.Length)
            {
                Res = "";
                while(UserEXP[DFA.INDEX] != '\"')
                {
                    Res += UserEXP[DFA.INDEX];
                    DFA.INDEX++;
                }
                DFA.INDEX++;
            }
            UserI = DFA.INDEX;
            return Res;
        }

        public string GetRule(int X, int Y)
        {
            return Productions[X].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1].Split('|')[Y];
        }

        /*
        public void UnGetRuleToken()
        {
            ParserIndex = OldParseIndex;
        }
         */

        public void UnGetUserToken()
        {
            UserIndex = OldUserIndex;
        }

        public bool IsRule(string R)
        {
            for (int i = 0; i < Productions.Count; i++)
            {
                if (R == Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0])
                    return true;
            }
            return false;
        }

        public ArrayList GetNonTerminals(string R)
        {
            ArrayList AR = new ArrayList();
            for (int i = 0; i < Productions.Count; i++)
            {
                string Temp=Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                if (R.StartsWith(Temp))
                {
                    AR.Add(Temp);
                    R = R.Substring(Temp.Length);
                    i = -1;
                }
                if (i == Productions.Count - 1 && R.Length != 0)
                {
                    R = R.Substring(1);
                    i = -1;
                }
            }
            return AR;
        }

        public string NonTerminalNK(int X)
        {
            return Productions[X].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public struct Coordinate
        {
            public int X;
            public int Y;
        }

        public Coordinate GetCoordinate(string R)
        {
            Coordinate Temp = new Coordinate();
            Temp.X = -1;
            Temp.Y = -1;
            for (int i = 0; i < Productions.Count; i++)
            {
                if (R == Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0])
                {
                    Temp.X = i;
                    Temp.Y = 0;
                    return Temp;
                }
            }            
            return Temp;
        }

        public string GetNonTerminalName(string X)
        {
            for (int i = 0; i < Productions.Count; i++)
            {
                string[] Data = Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[1].Split('|');
                for (int j = 0; j < Data.Length; j++)
                {
                    if (Data[j] == X)
                        return Productions[i].ToString().Split(ST, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                }
            }
            return null;
        }

        private string[] STT = new string[1];
        private bool EmptyIdentifier = false;
        public string GetError(Node Data)
        {
            string result = "";
            if (Data != null && Data.NodeKind != "")
            {
                STT[0] = "=>";
                for (int i = 0; i < Productions.Count; i++)
                {
                    string Name = Productions[i].ToString().Split(STT, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (Name == Data.NodeKind)
                    {
                        string ResultOfRule = Productions[i].ToString().Split(STT, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                        string[] SubRules = ResultOfRule.Split('|');
                        bool flag = false;
                        for (int j = 0; j < SubRules.Length; j++)
                        {
                            string[] Terminals = SubRules[j].Split(NTN, StringSplitOptions.RemoveEmptyEntries);
                            string Sum = "";
                            for (int p = 0; p < Terminals.Length; p++)
                            {
                                Sum += Terminals[p];
                            }
                            bool test = false;
                            try
                            {
                                if (char.IsLetter(Terminals[0][0]) || char.IsDigit(Terminals[0][0]))
                                    test = true;
                            }
                            catch(Exception ex)
                            {}

                            if (Sum==Data.Value || Sum=="" || (test && Data.Value.Split(' ')[0]=="letter") || (test && Data.Value.Split(' ')[0]=="digit"))
                            {
                                flag = true;
                                if(EmptyIdentifier)
                                    EmptyIdentifier=false;
                                if (Sum == "<>")
                                    EmptyIdentifier = true;
                                for (int k = 0; k < Data.nextstates.Count; k++)
                                {
                                    result = GetError((Node)Data.nextstates[k]);
                                }
                                break;                                   
                            }                                
                        }
                        if(!flag)
                            ErrorTracing.Push(Name + " Error");
                        if(EmptyIdentifier)
                            ErrorTracing.Push("Identifier Cannot Be empty..");

                        break;
                    }
                }               
            }
            return result;
        }

        public void Initiate()
        {
            NonTerminalNames.Clear();
            GetNonTerminalNames();
            NTN = new string[NonTerminalNames.Count];
            NonTerminalNames.CopyTo(NTN);
            DFA.Initiate();
            Finish = false;
        }

        public string MathParsing(string Expression)
        {
            Modules M = new Modules();
            return Convert.ToString(M.Evaluate(M.ConvertToFully(Expression)));
        }
        
         
    }
}
