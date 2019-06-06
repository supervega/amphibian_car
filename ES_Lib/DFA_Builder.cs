using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ES_Lib
{
    public class DFA_Builder
    {
        private string RE;
        private string original_RE="";
        private Stack<string> Paranthesis;       
        public State root;
        private string TokenString;
        private bool Flag = true;
        private int ErrorIndex;
        //public Form1 FormRef;
        public ArrayList LanguageTokens;
        private string CurrentToken;
        public char CurrentChar = '\0';

        public string EXP;
        private int Index,IndexTemp;
        private int oldIndex;
        private string TempToken;
        public ArrayList AlternativeResult;
        public bool TokenError;
        public int BoundErrorIndex;
        public ListBox LBpointer;

            
        public DFA_Builder()
        {
            Paranthesis = new Stack<string>();           
            root = null;
            LanguageTokens = new ArrayList();
            CurrentToken = "";
            TempToken = "";
            AlternativeResult = new ArrayList();
        }

        public State Build(string Exp)
        {
            int pos = 0;
            State Current = new State();
            try
            {
                RE = Exp;
                if (Flag)
                {
                    original_RE = Exp;
                    Flag = false;
                }
                for (int i = 0; i < RE.Length; i++)
                {
                    switch (RE[i])
                    {
                        case ' ': break;

                        #region case (

                        case '(': pos++;
                            while (RE[pos] != ')' && pos < RE.Length)
                            {
                                TokenString += RE[pos];
                                pos++;
                            }
                            pos++;
                            string[] OrStates = TokenString.Split('|');
                            ArrayList NextStates = new ArrayList();
                         

                            
                            for (int j = 0; j < OrStates.Length; j++)
                            {
                                State Temp = Build(OrStates[j]);
                                if (Temp != null)
                                {
                                   NextStates.Add(Temp);
                                }
                            }
                           
                                root = new State();
                                root.IS_Initial = true;
                                root.Name = "q0";
                                for (int j = 0; j < NextStates.Count; j++)
                                {
                                    State Temp = (State)NextStates[j];
                                    if (Temp.IS_Loop)
                                    {
                                        root.IS_Loop = true;
                                        for (int k = 0; k < Temp.LoopNextStates.Count; k++)
                                        {
                                            root.LoopNextStates.Add(Temp.LoopNextStates[k]);
                                        }
                                    }
                                    if (Temp.IS_Accepted)
                                        root.IS_Accepted = true;
                                    for (int k = 0; k < Temp.nextstates.Count; k++)
                                    {
                                        root.nextstates.Add(Temp.nextstates[k]);
                                    }                                     
                                }                            
                            i = pos;
                            break;

                        #endregion

                        #region Case \\

                        case '\\':
                            if (RE[i + 1] == 'd')
                            {
                                pos++;
                                if (root == null)
                                {
                                    root = new State();
                                    root.IS_Initial = true;
                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    for (int j = 0; j < 10; j++)
                                    {
                                        TR.TransitionChars.Add(j.ToString());
                                    }
                                    string Rest = "";
                                    pos++;
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        root.nextstates.Add(TR);
                                        Current = root;
                                    }
                                    else
                                    {
                                        root.IS_Accepted = true;
                                        root.nextstates.Add(TR);
                                        Current = root;
                                    }
                                }
                                else
                                {
                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    for (int j = 0; j < 10; j++)
                                    {
                                        TR.TransitionChars.Add(j.ToString());
                                    }
                                    string Rest = "";
                                    pos++;
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                        Current.nextstates.Add(TR);
                                    else
                                    {
                                        Current.IS_Accepted = true;
                                        Current.nextstates.Add(TR);
                                    }
                                }
                            }
                            else
                            {
                                if (RE[i + 1] == 'c')
                                {
                                    pos++;
                                    if (root == null)
                                    {
                                        root = new State();
                                        root.IS_Initial = true;
                                        State.Transition TR = new State.Transition();
                                        TR.TransitionChars = new ArrayList();
                                        TR.TransitionChars.Add("a");
                                        TR.TransitionChars.Add("b");
                                        TR.TransitionChars.Add("c");
                                        TR.TransitionChars.Add("d");
                                        TR.TransitionChars.Add("e");
                                        TR.TransitionChars.Add("f");
                                        TR.TransitionChars.Add("g");
                                        TR.TransitionChars.Add("h");
                                        TR.TransitionChars.Add("i");
                                        TR.TransitionChars.Add("j");
                                        TR.TransitionChars.Add("k");
                                        TR.TransitionChars.Add("l");
                                        TR.TransitionChars.Add("m");
                                        TR.TransitionChars.Add("n");
                                        TR.TransitionChars.Add("o");
                                        TR.TransitionChars.Add("p");
                                        TR.TransitionChars.Add("q");
                                        TR.TransitionChars.Add("r");
                                        TR.TransitionChars.Add("s");
                                        TR.TransitionChars.Add("t");
                                        TR.TransitionChars.Add("u");
                                        TR.TransitionChars.Add("v");
                                        TR.TransitionChars.Add("w");
                                        TR.TransitionChars.Add("x");
                                        TR.TransitionChars.Add("y");
                                        TR.TransitionChars.Add("z");
                                        TR.TransitionChars.Add("A");
                                        TR.TransitionChars.Add("B");
                                        TR.TransitionChars.Add("C");
                                        TR.TransitionChars.Add("D");
                                        TR.TransitionChars.Add("E");
                                        TR.TransitionChars.Add("F");
                                        TR.TransitionChars.Add("G");
                                        TR.TransitionChars.Add("H");
                                        TR.TransitionChars.Add("I");
                                        TR.TransitionChars.Add("J");
                                        TR.TransitionChars.Add("K");
                                        TR.TransitionChars.Add("L");
                                        TR.TransitionChars.Add("M");
                                        TR.TransitionChars.Add("N");
                                        TR.TransitionChars.Add("O");
                                        TR.TransitionChars.Add("P");
                                        TR.TransitionChars.Add("Q");
                                        TR.TransitionChars.Add("R");
                                        TR.TransitionChars.Add("S");
                                        TR.TransitionChars.Add("T");
                                        TR.TransitionChars.Add("U");
                                        TR.TransitionChars.Add("V");
                                        TR.TransitionChars.Add("W");
                                        TR.TransitionChars.Add("X");
                                        TR.TransitionChars.Add("Y");
                                        TR.TransitionChars.Add("Z");
                                        string Rest = "";
                                        pos++;
                                        while (pos < RE.Length)
                                        {
                                            Rest += RE[pos];
                                            pos++;
                                        }
                                        if (Rest != "")
                                            TR.state = Build(Rest);
                                        if (TR.state != null)
                                        {
                                            root.nextstates.Add(TR);
                                            Current = root;
                                        }
                                        else
                                        {
                                            root.IS_Accepted = true;
                                            root.nextstates.Add(TR);
                                            Current = root;
                                        }
                                    }
                                    else
                                    {
                                        State.Transition TR = new State.Transition();
                                        TR.TransitionChars = new ArrayList();
                                        TR.TransitionChars.Add("a");
                                        TR.TransitionChars.Add("b");
                                        TR.TransitionChars.Add("c");
                                        TR.TransitionChars.Add("d");
                                        TR.TransitionChars.Add("e");
                                        TR.TransitionChars.Add("f");
                                        TR.TransitionChars.Add("g");
                                        TR.TransitionChars.Add("h");
                                        TR.TransitionChars.Add("i");
                                        TR.TransitionChars.Add("j");
                                        TR.TransitionChars.Add("k");
                                        TR.TransitionChars.Add("l");
                                        TR.TransitionChars.Add("m");
                                        TR.TransitionChars.Add("n");
                                        TR.TransitionChars.Add("o");
                                        TR.TransitionChars.Add("p");
                                        TR.TransitionChars.Add("q");
                                        TR.TransitionChars.Add("r");
                                        TR.TransitionChars.Add("s");
                                        TR.TransitionChars.Add("t");
                                        TR.TransitionChars.Add("u");
                                        TR.TransitionChars.Add("v");
                                        TR.TransitionChars.Add("w");
                                        TR.TransitionChars.Add("x");
                                        TR.TransitionChars.Add("y");
                                        TR.TransitionChars.Add("z");
                                        TR.TransitionChars.Add("A");
                                        TR.TransitionChars.Add("B");
                                        TR.TransitionChars.Add("C");
                                        TR.TransitionChars.Add("D");
                                        TR.TransitionChars.Add("E");
                                        TR.TransitionChars.Add("F");
                                        TR.TransitionChars.Add("G");
                                        TR.TransitionChars.Add("H");
                                        TR.TransitionChars.Add("I");
                                        TR.TransitionChars.Add("J");
                                        TR.TransitionChars.Add("K");
                                        TR.TransitionChars.Add("L");
                                        TR.TransitionChars.Add("M");
                                        TR.TransitionChars.Add("N");
                                        TR.TransitionChars.Add("O");
                                        TR.TransitionChars.Add("P");
                                        TR.TransitionChars.Add("Q");
                                        TR.TransitionChars.Add("R");
                                        TR.TransitionChars.Add("S");
                                        TR.TransitionChars.Add("T");
                                        TR.TransitionChars.Add("U");
                                        TR.TransitionChars.Add("V");
                                        TR.TransitionChars.Add("W");
                                        TR.TransitionChars.Add("X");
                                        TR.TransitionChars.Add("Y");
                                        TR.TransitionChars.Add("Z");
                                        string Rest = "";
                                        pos++;
                                        while (pos < RE.Length)
                                        {
                                            Rest += RE[pos];
                                            pos++;
                                        }
                                        if (Rest != "")
                                            TR.state = Build(Rest);
                                        if (TR.state != null)
                                            Current.nextstates.Add(TR);
                                        else
                                        {
                                            Current.IS_Accepted = true;
                                            Current.nextstates.Add(TR);
                                        }
                                    }
                                }
                                else
                                {
                                    if (RE[i + 1] == 't')
                                    {
                                        pos++;
                                        if (root == null)
                                        {
                                            root = new State();
                                            root.IS_Initial = true;


                                            State.Transition TR = new State.Transition();
                                            TR.TransitionChars = new ArrayList();
                                            TR.TransitionChars.Add(" ");
                                            string Rest = "";
                                            pos++;
                                            while (pos < RE.Length)
                                            {
                                                Rest += RE[pos];
                                                pos++;
                                            }
                                            if (Rest != "")
                                                TR.state = Build(Rest);
                                            if (TR.state != null)
                                            {
                                                root.nextstates.Add(TR);
                                                Current = root;
                                            }
                                            else
                                            {
                                                root.nextstates.Add(TR);
                                                root.IS_Accepted = true;
                                                Current = root;
                                            }
                                        }
                                        else
                                        {
                                            State.Transition TR = new State.Transition();
                                            TR.TransitionChars = new ArrayList();
                                            TR.TransitionChars.Add(" ");
                                            string Rest = "";
                                            pos++;
                                            while (pos < RE.Length)
                                            {
                                                Rest += RE[pos];
                                                pos++;
                                            }
                                            if (Rest != "")
                                                TR.state = Build(Rest);
                                            if (TR.state != null)
                                                Current.nextstates.Add(TR);
                                            else
                                            {
                                                Current.IS_Accepted = true;
                                                Current.nextstates.Add(TR);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Case [

                        case '[':
                            if (RE.Length == 1)
                            {
                                if (root == null)
                                {
                                    root = new State();
                                    root.IS_Initial = true;


                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    TR.TransitionChars.Add(RE[i]);
                                    string Rest = "";
                                    pos++;
                                    if (pos < RE.Length && RE[pos] == '*')
                                    {
                                        root.IS_Loop = true;
                                        pos++;
                                    }
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    Current = root;
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        if (root.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            for (int j = 0; j < TR.state.nextstates.Count; j++)
                                            {
                                                root.nextstates.Add(TR.state.nextstates[j]);
                                            }
                                        }
                                        else
                                            root.nextstates.Add(TR);
                                    }
                                    else
                                    {
                                        if (root.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            root.IS_Accepted = true;
                                        }
                                        else
                                        {
                                            TR.state = new State();
                                            TR.state.IS_Accepted = true;
                                            root.nextstates.Add(TR);
                                        }
                                    }
                                }
                                else
                                {
                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    TR.TransitionChars.Add(RE[i]);
                                    string Rest = "";
                                    pos++;
                                    if (pos < RE.Length && RE[pos] == '*')
                                    {
                                        Current.IS_Loop = true;
                                        pos++;
                                    }
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            for (int j = 0; j < TR.state.nextstates.Count; j++)
                                            {
                                                Current.nextstates.Add(TR.state.nextstates[j]);
                                            }
                                        }
                                        else
                                            Current.nextstates.Add(TR);
                                    }
                                    else
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            Current.IS_Accepted = true;
                                        }
                                        else
                                        {
                                            TR.state = new State();
                                            TR.state.IS_Accepted = true;
                                            Current.nextstates.Add(TR);
                                        }
                                    }
                                }
                                pos++;
                                i = pos;
                                break;
                            }                       
                            
                            pos++;
                            string Data = "";
                            bool is_loop=false;
                            while (pos < RE.Length && RE[pos] != ']')
                            {
                                Data += RE[pos];
                                pos++;
                            }
                            pos++;
                            if (pos < RE.Length && RE[pos] == '*')
                            {
                                is_loop = true;
                                pos++;
                            }                            
                            string[] Boundaries = Data.Split('-');
                            if (Boundaries.Length != 0)
                            {
                                if (root == null)
                                {
                                    root = new State();
                                    root.IS_Initial = true;
                                    if (is_loop)
                                        root.IS_Loop = true;
                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    if (char.IsDigit(Boundaries[0].ToCharArray()[0]))
                                    {
                                           int left, right;
                                            if (Boundaries.Length == 1)
                                            {
                                                left = Convert.ToInt32(Boundaries[0].Split(' ')[0]);
                                                right = Convert.ToInt32(Boundaries[0].Split(' ')[1]);
                                            }
                                            else
                                            {
                                                left = Convert.ToInt32(Boundaries[0]);
                                                right = Convert.ToInt32(Boundaries[1]);
                                            }
                                            for (int k = left; k <= right; k++)
                                            {
                                                TR.TransitionChars.Add(k.ToString());
                                            }                                       
                                    }
                                    else
                                    {
                                        char left, right;
                                        if (Boundaries.Length == 1)
                                        {
                                            left = char.Parse(Boundaries[0].Split(' ')[0]);
                                            right = char.Parse(Boundaries[0].Split(' ')[1]);
                                        }
                                        else
                                        {
                                            left = Boundaries[0].ToCharArray()[0];
                                            right = Boundaries[1].ToCharArray()[0];
                                        }
                                        if ((int)left < (int)right)
                                        {
                                            for (int k = (int)left; k <= (int)right; k++)
                                            {
                                                TR.TransitionChars.Add(((char)k).ToString());
                                            }
                                        }
                                        else
                                        {
                                            for (int k = (int)right; k <= (int)left; k++)
                                            {
                                                TR.TransitionChars.Add(((char)k).ToString());
                                            }
                                        }
                                    }

                                    string Rest = "";
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        if (root.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            if (TR.state.IS_Loop)
                                            {
                                                State.Transition T = new State.Transition();
                                                T.state = TR.state;
                                                T.TransitionChars = new ArrayList();
                                                for (int j = 0; j < TR.state.LoopNextStates.Count; j++)
                                                {
                                                    T.TransitionChars.Add(TR.state.LoopNextStates[j]);
                                                }
                                                root.nextstates.Add(T);
                                                root.IS_Accepted = true;
                                            }
                                            else
                                                for (int j = 0; j < TR.state.nextstates.Count; j++)
                                                {
                                                    root.nextstates.Add(TR.state.nextstates[j]);
                                                }
                                        }
                                        else
                                            root.nextstates.Add(TR);
                                        Current = root;
                                    }
                                    else
                                    {
                                        if (root.IS_Loop)
                                        {
                                            root.IS_Accepted = true;
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            Current = root;
                                        }
                                        else
                                        {
                                            TR.state = new State();
                                            TR.state.IS_Accepted = true;
                                            root.nextstates.Add(TR);
                                            Current = root;
                                        }
                                    }
                                }
                                else
                                {

                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    if (is_loop)
                                        Current.IS_Loop = true;
                                    if (char.IsDigit(Boundaries[0].ToCharArray()[0]))
                                    {
                                        
                                        if (Boundaries.Length == 1)
                                        {
                                            string[] Chars = Boundaries[0].Split(' ');
                                            for (int j = 0; j < Chars.Length; j++)
                                            {
                                                TR.TransitionChars.Add(Chars[j]);
                                            }
                                        }
                                        else
                                        {
                                            int left, right;
                                            left = Convert.ToInt32(Boundaries[0]);
                                            right = Convert.ToInt32(Boundaries[1]);

                                            for (int k = left; k <= right; k++)
                                            {
                                                TR.TransitionChars.Add(k.ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        char left, right;
                                        if (Boundaries.Length == 1)
                                        {
                                            left = char.Parse(Boundaries[0].Split(' ')[0]);
                                            right = char.Parse(Boundaries[0].Split(' ')[1]);
                                        }
                                        else
                                        {
                                            left = Boundaries[0].ToCharArray()[0];
                                            right = Boundaries[1].ToCharArray()[0];
                                        }
                                        if ((int)left < (int)right)
                                        {
                                            for (int k = (int)left; k <= (int)right; k++)
                                            {
                                                TR.TransitionChars.Add(((char)k).ToString());
                                            }
                                        }
                                        else
                                        {
                                            for (int k = (int)right; k <= (int)left; k++)
                                            {
                                                TR.TransitionChars.Add(((char)k).ToString());
                                            }
                                        }
                                    }
                                    string Rest = "";
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            if (TR.state.IS_Loop)
                                            {
                                                State.Transition T = new State.Transition();
                                                T.state = TR.state;
                                                T.TransitionChars = new ArrayList();
                                                for (int j = 0; j < TR.state.LoopNextStates.Count; j++)
                                                {
                                                    T.TransitionChars.Add(TR.state.LoopNextStates[j]);
                                                }
                                                root.nextstates.Add(T);
                                                root.IS_Accepted = true;
                                            }
                                            else
                                                for (int j = 0; j < TR.state.nextstates.Count; j++)
                                                {
                                                    root.nextstates.Add(TR.state.nextstates[j]);
                                                }
                                        }
                                        else
                                            Current.nextstates.Add(TR);
                                    }
                                    else
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            Current.IS_Accepted = true;                                            
                                        }
                                        else
                                        {
                                            TR.state = new State();
                                            TR.state.IS_Accepted = true;
                                            Current.nextstates.Add(TR);                                            
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string[] Meta = Data.Split(' ');
                                if (root == null)
                                {
                                    root = new State();
                                    root.IS_Initial = true;
                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    for (int j = 0; j < Meta.Length; j++)
                                    {
                                        TR.TransitionChars.Add(Meta[j].ToString());
                                    }

                                    string Rest = "";
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            if (TR.state.IS_Loop)
                                            {
                                                State.Transition T = new State.Transition();
                                                T.state = TR.state;
                                                T.TransitionChars = new ArrayList();
                                                for (int j = 0; j < TR.state.LoopNextStates.Count; j++)
                                                {
                                                    T.TransitionChars.Add(TR.state.LoopNextStates[j]);
                                                }
                                                root.nextstates.Add(T);
                                                root.IS_Accepted = true;
                                            }
                                            else
                                                for (int j = 0; j < TR.state.nextstates.Count; j++)
                                                {
                                                    root.nextstates.Add(TR.state.nextstates[j]);
                                                }
                                        }
                                        else
                                            Current.nextstates.Add(TR);                                        
                                    }
                                    else
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            Current.IS_Accepted = true;                                            
                                        }
                                        else
                                        {
                                            TR.state = new State();
                                            TR.state.IS_Accepted = true;
                                            Current.nextstates.Add(TR);                                           
                                        }
                                    }
                                }
                                else
                                {

                                    State.Transition TR = new State.Transition();
                                    TR.TransitionChars = new ArrayList();
                                    for (int j = 0; j < Meta.Length; j++)
                                    {
                                        TR.TransitionChars.Add(Meta[j].ToString());
                                    }
                                    string Rest = "";
                                    while (pos < RE.Length)
                                    {
                                        Rest += RE[pos];
                                        pos++;
                                    }
                                    if (Rest != "")
                                        TR.state = Build(Rest);
                                    if (TR.state != null)
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            if (TR.state.IS_Loop)
                                            {
                                                State.Transition T = new State.Transition();
                                                T.state = TR.state;
                                                T.TransitionChars = new ArrayList();
                                                for (int j = 0; j < TR.state.LoopNextStates.Count; j++)
                                                {
                                                    T.TransitionChars.Add(TR.state.LoopNextStates[j]);
                                                }
                                                root.nextstates.Add(T);
                                                root.IS_Accepted = true;
                                            }
                                            else
                                                for (int j = 0; j < TR.state.nextstates.Count; j++)
                                                {
                                                    root.nextstates.Add(TR.state.nextstates[j]);
                                                }
                                        }
                                        else
                                            Current.nextstates.Add(TR);
                                    }
                                    else
                                    {
                                        if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            Current.IS_Accepted = true;
                                        }
                                        else
                                        {
                                            TR.state = new State();
                                            TR.state.IS_Accepted = true;
                                            Current.nextstates.Add(TR);
                                        }
                                    }
                                }
                            }
                            i = pos;
                            break;

                        #endregion

                        case ']':
                            if(RE.Length==1)
                            if (root == null)
                            {
                                root = new State();
                                root.IS_Initial = true;


                                State.Transition TR = new State.Transition();
                                TR.TransitionChars = new ArrayList();
                                TR.TransitionChars.Add(RE[i]);
                                string Rest = "";
                                pos++;
                                if (pos < RE.Length && RE[pos] == '*')
                                {
                                    root.IS_Loop = true;
                                    pos++;
                                }
                                while (pos < RE.Length)
                                {
                                    Rest += RE[pos];
                                    pos++;
                                }
                                Current = root;
                                if (Rest != "")
                                    TR.state = Build(Rest);
                                if (TR.state != null)
                                {
                                    if (root.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        for (int j = 0; j < TR.state.nextstates.Count; j++)
                                        {
                                            root.nextstates.Add(TR.state.nextstates[j]);
                                        }
                                    }
                                    else
                                        root.nextstates.Add(TR);
                                }
                                else
                                {
                                    if (root.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        root.IS_Accepted = true;
                                    }
                                    else
                                    {
                                        TR.state = new State();
                                        TR.state.IS_Accepted = true;
                                        root.nextstates.Add(TR);
                                    }
                                }
                            }
                            else
                            {
                                State.Transition TR = new State.Transition();
                                TR.TransitionChars = new ArrayList();
                                TR.TransitionChars.Add(RE[i]);
                                string Rest = "";
                                pos++;
                                if (pos < RE.Length && RE[pos] == '*')
                                {
                                    Current.IS_Loop = true;
                                    pos++;
                                }
                                while (pos < RE.Length)
                                {
                                    Rest += RE[pos];
                                    pos++;
                                }
                                if (Rest != "")
                                    TR.state = Build(Rest);
                                if (TR.state != null)
                                {
                                    if (Current.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        for (int j = 0; j < TR.state.nextstates.Count; j++)
                                        {
                                            Current.nextstates.Add(TR.state.nextstates[j]);
                                        }
                                    }
                                    else
                                        Current.nextstates.Add(TR);
                                }
                                else
                                {
                                    if (Current.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        Current.IS_Accepted = true;
                                    }
                                    else
                                    {
                                        TR.state = new State();
                                        TR.state.IS_Accepted = true;
                                        Current.nextstates.Add(TR);
                                    }
                                }
                            }
                            pos++;
                            i = pos;
                            break;
                        #region Default Case

                        default: if (root == null)
                            {
                                root = new State();
                                root.IS_Initial = true;


                                State.Transition TR = new State.Transition();
                                TR.TransitionChars = new ArrayList();
                                TR.TransitionChars.Add(RE[i]);
                                string Rest = "";
                                pos++;
                                if (pos < RE.Length && RE[pos] == '*')
                                {
                                    root.IS_Loop = true;
                                    pos++;
                                }
                                while (pos < RE.Length)
                                {
                                    Rest += RE[pos];
                                    pos++;
                                }
                                Current = root;
                                if(Rest!="")
                                    TR.state = Build(Rest);
                                if (TR.state != null)
                                {
                                    if (root.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        for (int j = 0; j < TR.state.nextstates.Count; j++)
                                        {
                                            root.nextstates.Add(TR.state.nextstates[j]);
                                        }
                                    }
                                    else
                                        root.nextstates.Add(TR);
                                }
                                else
                                {
                                    if (root.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            root.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        root.IS_Accepted = true;                                       
                                    }
                                    else
                                    {
                                        TR.state = new State();
                                        TR.state.IS_Accepted = true;
                                        root.nextstates.Add(TR);                                        
                                    }
                                }
                            }
                            else
                            {
                                State.Transition TR = new State.Transition();
                                TR.TransitionChars = new ArrayList();
                                TR.TransitionChars.Add(RE[i]);
                                string Rest = "";
                                pos++;
                                if (pos < RE.Length && RE[pos] == '*')
                                {
                                    Current.IS_Loop = true;
                                    pos++;
                                }
                                while (pos < RE.Length)
                                {
                                    Rest += RE[pos];
                                    pos++;
                                }
                                if(Rest!="")
                                    TR.state = Build(Rest);
                                if (TR.state != null)
                                {
                                    if (Current.IS_Loop)
                                        {
                                            for (int j = 0; j < TR.TransitionChars.Count; j++)
                                            {
                                                Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                            }
                                            for (int j = 0; j < TR.state.nextstates.Count; j++)
                                            {
                                                Current.nextstates.Add(TR.state.nextstates[j]);
                                            }
                                        }
                                        else
                                        Current.nextstates.Add(TR);
                                }
                                else
                                {
                                    if (Current.IS_Loop)
                                    {
                                        for (int j = 0; j < TR.TransitionChars.Count; j++)
                                        {
                                            Current.LoopNextStates.Add(TR.TransitionChars[j].ToString());
                                        }
                                        Current.IS_Accepted = true;
                                    }
                                    else
                                    {
                                        TR.state = new State();
                                        TR.state.IS_Accepted = true;
                                        Current.nextstates.Add(TR);
                                    }
                                }
                            }
                            pos++;
                            i = pos;
                            break;

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                if (RE != "")
                {
                    ErrorIndex = original_RE.IndexOf(RE);
                    //FormRef.RE_TXT.SelectionColor = System.Drawing.Color.Red;
                    //if (ErrorIndex >= 0)
                    //    FormRef.RE_TXT.Select(ErrorIndex, RE.Length);
                    System.Windows.Forms.MessageBox.Show("Index = " + ErrorIndex + " " + ex.Message);
                }
            }           
           return Current;
            
        }

        public void GetLanguageTokens(State root)  
        {
            if (root != null)
            {
                if (root.IS_Initial)
                {
                    if (root.IS_Loop && root.IS_Accepted)
                    {
                        for (int i = 0; i < root.LoopNextStates.Count; i++)
                        {
                            if (char.IsLetter(Convert.ToChar(root.LoopNextStates[i])))
                            {
                                LanguageTokens.Add("letter");
                                continue;
                            }
                            else
                                if (char.IsDigit(Convert.ToChar(root.LoopNextStates[i])))
                                {
                                    LanguageTokens.Add("digit");
                                    continue;
                                }
                                else
                                {
                                    LanguageTokens.Add(root.LoopNextStates[i]);
                                    
                                }
                        }
                    }
                    for (int i = 0; i < root.nextstates.Count; i++)
                    {
                        State Temp = ((State.Transition)root.nextstates[i]).state;
                        ArrayList Transitions = ((State.Transition)root.nextstates[i]).TransitionChars;
                        if (Temp.IS_Accepted)
                        {
                            if (Transitions.Count == 1)
                            {
                                CurrentToken += Transitions[0];
                                LanguageTokens.Add(CurrentToken);
                                CurrentToken = CurrentToken.Substring(0, CurrentToken.Length - 1);
                            }
                            else
                                for (int j = 0; j < Transitions.Count; j++)
                                {
                                    if (char.IsLetter(Convert.ToChar(Transitions[j])))
                                    {
                                        LanguageTokens.Add("letter");
                                        continue;
                                    }
                                    else
                                        if (char.IsDigit(Convert.ToChar(Transitions[j])))
                                        {
                                            LanguageTokens.Add("digit");
                                            continue;
                                        }
                                        else
                                        {
                                            LanguageTokens.Add(Transitions[j]);                                            
                                        }
                                }
                            GetLanguageTokens(Temp);
                        }
                        else
                        {
                            if (Transitions.Count == 1)
                            {
                                CurrentToken += Transitions[0];
                            }
                            else
                                for (int j = 0; j < Transitions.Count; j++)
                                {
                                    if (char.IsLetter(Convert.ToChar(Transitions[j])))
                                    {
                                        LanguageTokens.Add("letter");
                                        continue;
                                    }
                                    else
                                        if (char.IsDigit(Convert.ToChar(Transitions[j])))
                                        {
                                            LanguageTokens.Add("digit");
                                            continue;
                                        }
                                        else
                                        {
                                            LanguageTokens.Add(Transitions[j]);                                            
                                        }
                                }
                            GetLanguageTokens(Temp);
                            if(CurrentToken!="")
                            CurrentToken = CurrentToken.Substring(0, CurrentToken.Length - 1);
                        }
                        
                    }
                }
                else
                {
                    for (int i = 0; i < root.nextstates.Count; i++)
                    {
                        State Temp = ((State.Transition)root.nextstates[i]).state;
                        ArrayList Transitions = ((State.Transition)root.nextstates[i]).TransitionChars;
                        if (Temp.IS_Accepted)
                        {
                            if (Temp.IS_Loop)
                            {
                                for (int j = 0; j < Temp.LoopNextStates.Count; j++)
                                {
                                    if (char.IsLetter(Convert.ToChar(Temp.LoopNextStates[j])))
                                    {
                                        LanguageTokens.Add("letter");
                                        continue;
                                    }
                                    else
                                        if (char.IsDigit(Convert.ToChar(Temp.LoopNextStates[j])))
                                        {
                                            LanguageTokens.Add("digit");
                                            continue;
                                        }
                                        else
                                        {
                                            LanguageTokens.Add(Temp.LoopNextStates[j]);                                            
                                        }
                                }
                            }
                            if (Transitions.Count == 1)
                            {
                                CurrentToken += Transitions[0];
                                LanguageTokens.Add(CurrentToken);
                                CurrentToken = CurrentToken.Substring(0, CurrentToken.Length - 1);
                            }
                            else
                                for (int j = 0; j < Transitions.Count; j++)
                                {
                                    if (char.IsLetter(Convert.ToChar(Transitions[j])))
                                    {
                                        LanguageTokens.Add("letter");
                                        continue;
                                    }
                                    else
                                        if (char.IsDigit(Convert.ToChar(Transitions[j])))
                                        {
                                            LanguageTokens.Add("digit");
                                            continue;
                                        }
                                        else
                                        {
                                            LanguageTokens.Add(Transitions[j]);                                           
                                        }
                                }
                            GetLanguageTokens(Temp);
                        }
                        else
                        {
                            if (Temp.IS_Loop)
                            {
                                for (int j = 0; j < Temp.LoopNextStates.Count; j++)
                                {
                                    if (char.IsLetter(Convert.ToChar(Temp.LoopNextStates[j])))
                                    {
                                        LanguageTokens.Add("letter");
                                        continue;
                                    }
                                    else
                                        if (char.IsDigit(Convert.ToChar(Temp.LoopNextStates[j])))
                                        {
                                            LanguageTokens.Add("digit");
                                            continue;
                                        }
                                        else
                                        {
                                            LanguageTokens.Add(Temp.LoopNextStates[j]);                                            
                                        }
                                }
                            }
                            if (Transitions.Count == 1)
                            {
                                CurrentToken += Transitions[0];
                            }
                            else
                                for (int j = 0; j < Transitions.Count; j++)
                                {
                                    if (char.IsLetter(Convert.ToChar(Transitions[j])))
                                    {
                                        LanguageTokens.Add("letter");
                                        continue;
                                    }
                                    else
                                        if (char.IsDigit(Convert.ToChar(Transitions[j])))
                                        {
                                            LanguageTokens.Add("digit");
                                            continue;
                                        }
                                        else
                                        {
                                            LanguageTokens.Add(Transitions[j]);                                            
                                        }
                                }
                            GetLanguageTokens(Temp);
                            if (CurrentToken != "")
                            CurrentToken = CurrentToken.Substring(0, CurrentToken.Length - 1);
                        }
                        
                    }
                }
            }
        }

        public void ArrangeTokensArray()
        {
            ArrayList TMP = new ArrayList();
            LanguageTokens.Add("ERROR");
            LanguageTokens.Add("EOF");
            LanguageTokens.Sort();
            TMP.Add(LanguageTokens[0]);
            int counter=1;
            for (int i = 1; i < LanguageTokens.Count; i++)
            {
                if (LanguageTokens[i] != TMP[i - counter])
                    TMP.Add(LanguageTokens[i]);
                else
                    counter++;
            }
            LanguageTokens.Clear();
            for (int i = 0; i < TMP.Count; i++)
            {
                LanguageTokens.Add(TMP[i]);
            }
        }

        public int GetNextToken()
        {
            TempToken = "";
            oldIndex = Index;
            TokenError = false;
            while (Index < EXP.Length && (EXP[Index] == '\n' || EXP[Index] == '\r'))
            {
                Index++;
            }
            if (Index >= EXP.Length)
                return LanguageTokens.IndexOf("EOF");

            SearchToken(root);
            Index = IndexTemp;
            if (AlternativeResult.Count == 0)
            {
                TokenError = true;
                BoundErrorIndex = Index;
                return oldIndex;
            }
            else
                return Convert.ToInt32(AlternativeResult[AlternativeResult.Count - 1]);
        }

        public int SearchToken(State Rroot)
        {            
            int Result = -1;
            if (root != null)
            {
                int Length = Rroot.nextstates.Count;
                if (Rroot.IS_Loop)
                {

                    char CH = EXP[Index];
                    for (int i = 0; i < Rroot.LoopNextStates.Count; i++)
                    {
                        if (CH.ToString().ToLower() == Convert.ToChar(Rroot.LoopNextStates[i]).ToString().ToLower())
                        {
                            Index++;
                            if (Char.IsDigit(CH))
                            {
                                if (LBpointer != null)
                                {
                                    LBpointer.Items.Add(Rroot.Name + ": digit (" + CH + ")");
                                }
                                CurrentChar = CH;
                                AlternativeResult.Add(LanguageTokens.IndexOf("digit"));
                                IndexTemp = Index;
                                Index--;
                                break;
                                //return 0;
                            }
                            else
                                if (char.IsLetter(CH))
                                {
                                    if (LBpointer != null)
                                    {
                                        LBpointer.Items.Add(Rroot.Name + ": letter (" + CH + ")");
                                    }
                                    CurrentChar = CH;
                                    AlternativeResult.Add(LanguageTokens.IndexOf("letter"));
                                    IndexTemp = Index;
                                    Index--;
                                    break;

                                    //return 0;
                                }
                                else
                                {
                                    if (LBpointer != null)
                                    {
                                        LBpointer.Items.Add(Rroot.Name + ": char (" + CH + ")");
                                    }
                                    CurrentChar = CH;
                                    AlternativeResult.Add(LanguageTokens.IndexOf(CH));                                    
                                    IndexTemp = Index;
                                    Index--;
                                    break;
                                }
                        }
                    }
                }
                for (int i = 0; i < Length; i++)
			        {
                        if (Index >= EXP.Length)
                            break;
                        char CH = EXP[Index];
                        State.Transition Temp = ((State.Transition)Rroot.nextstates[i]);
                        State ST = ((State.Transition)Rroot.nextstates[i]).state;
                       
                        for (int j = 0; j < Temp.TransitionChars.Count; j++)
                        {
                            if (CH.ToString().ToLower() == Convert.ToChar(Temp.TransitionChars[j]).ToString().ToLower())
                            {
                                Index++;
                                if (Temp.state.IS_Accepted)
                                {
                                    if (LBpointer != null)
                                    {
                                        LBpointer.Items.Add(Rroot.Name + ": Accepting State");
                                    }
                                    if (TempToken != "")
                                    {
                                        TempToken += CH.ToString();                                       
                                        IndexTemp = Index;
                                        Index-=2;
                                        AlternativeResult.Add(LanguageTokens.IndexOf((TempToken)));
                                        TempToken = TempToken.Substring(0, TempToken.Length - 2);
                                        return -1;
                                    }
                                    else
                                    {
//                                        CurrentChar = CH;
                                        AlternativeResult.Add(LanguageTokens.IndexOf(CH.ToString()));
                                        IndexTemp = Index;
                                        Index--;
                                        return -1;
                                    }
                                }
                                else
                                {
                                    TempToken += CH;
                                }      
                                Result = SearchToken(ST);                                 
                                break;
                            }
                           
                        }
                        if (Result > -1)
                            break;                       
			        }
               
                  if (Result == -1)
                  {
                      Index--;
                      if(TempToken!="")
                          TempToken = TempToken.Substring(0, TempToken.Length - 1);
                  }
            }
            return Result;
        }

        public void UnGetToken()
        {
            Index = oldIndex;
        }

        public void Initiate()
        {
           Index = 0;
        }

        public int INDEX
        {
            get
            {
                return Index;
            }
            set
            {
                Index = value;
            }
        }

         
    }
}
