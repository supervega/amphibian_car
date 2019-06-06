using System;
using System.Collections.Generic;
using System.Text;

namespace ES_Lib
{
    public class Modules
    {
        char[] separators2 = new char[6] { '+', '-', '*', '/',')','(' };
        public Modules()
        { }

        public string ConvertToFully(string Originaltext)
        {
            char[] separators=new char[4]{'+','-','*','/'};
            string[] NumberText = Originaltext.Split(separators);
            char[] Operations=new char[NumberText.Length-1];
            int countOP = 0;
            for (int c = 0; c < Originaltext.Length; c++)
			{
                if (Originaltext[c] == '+' || Originaltext[c] == '-' || Originaltext[c] == '*' || Originaltext[c] == '/')
                {
                    Operations[countOP] = Originaltext[c];
                    countOP++;
                }
			}
            string[] text=new string[NumberText.Length+(NumberText.Length-1)];
            int OpCount=0;
            int NumCount=0;
            for (int x = 0; x < text.Length; x++)
            {
                if (x % 2 == 0)
                {
                    text[x] = NumberText[NumCount];
                    NumCount++;
                }
                else
                {
                    text[x] = Convert.ToString(Operations[OpCount]);
                    OpCount++;
                }
            }
            string oldsub="", CurrentNumber="", CurrentOperation="";
            Stack st=new Stack();
            bool firstRun = false;
            oldsub="("+text[0]+text[1]+")";            
            for (int i = 0; i <= text.Length+1 ; i = i + 2)
            {
                if (firstRun)
                {
                    for (int a = 0; a < oldsub.Length; a++)
                    {
                        element e = new element(Convert.ToString(oldsub[a]));
                        st.Push(e);
                    }
                    CurrentNumber = Convert.ToString(text[i]);
                    try
                    {
                        CurrentOperation = Convert.ToString(text[i + 1]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        int count = 0, OldLength = 0;
                        element[] TMP = new element[oldsub.Length];
                        for (int j = oldsub.Length - 1; j > 0; j--)
                        {
                            if (oldsub[j] != '+' && oldsub[j] != '-' && oldsub[j] != '*' && oldsub[j] != '/')
                            {
                                TMP[j] = st.Pop();
                                count++;
                            }
                            else
                                break;
                        }
                        OldLength = oldsub.Length;
                        oldsub = "";
                        for (int h = OldLength - count; h > 0; h--)
                        {
                            element e = new element();
                            e = st.Pop();
                            oldsub = e.value + oldsub;
                        }
                        st.Clear();
                        oldsub = oldsub + CurrentNumber;
                        for (int s = TMP.Length; s > 0; s--)
                        {
                            if (TMP[s - 1] != null)
                                oldsub = oldsub + TMP[s-1].value;
                        }
                        break;
                    }
                                       
                    if (CurrentOperation != "")
                    {
                        if (CurrentOperation == "+" || CurrentOperation == "-")
                        {
                            if (oldsub != "")
                            {
                                int count = 0, OldLength = 0;
                                element[] TMP = new element[oldsub.Length];
                                for (int j = oldsub.Length - 1; j > 0; j--)
                                {
                                    if (oldsub[j] != '+' && oldsub[j] != '-' && oldsub[j] != '*' && oldsub[j] != '/')
                                    {
                                        TMP[j] = st.Pop();
                                        count++;
                                    }
                                    else
                                        break;
                                }
                                OldLength = oldsub.Length;
                                oldsub = "";
                                for (int h = OldLength - count; h > 0; h--)
                                {
                                    element e = new element();
                                    e = st.Pop();
                                    oldsub = e.value + oldsub;
                                }
                                st.Clear();
                                oldsub = oldsub + CurrentNumber;
                                for (int s = TMP.Length; s > 0; s--)
                                {
                                    if (TMP[s - 1] != null)
                                        oldsub = oldsub + TMP[s-1].value;
                                }
                                oldsub = "(" + oldsub + CurrentOperation + ")";
                            }
                        }
                    }
                    if (CurrentOperation == "*" || CurrentOperation == "/")
                    {
                        if (oldsub != "")
                        {
                            int count = 0, OldLength = 0;
                            element[] TMP = new element[oldsub.Length];
                            for (int j = oldsub.Length - 1; j > 0; j--)
                            {
                                if (oldsub[j] != '+' && oldsub[j] != '-' && oldsub[j] != '*' && oldsub[j] != '/')
                                {
                                    TMP[j] = st.Pop();
                                    count++;
                                }
                                else
                                    break;
                            }
                            OldLength = oldsub.Length;
                            oldsub = "";
                            for (int h = OldLength - count; h > 0; h--)
                            {
                                element e = new element();
                                e = st.Pop();
                                oldsub = e.value + oldsub;
                            }
                            st.Clear();
                            oldsub = oldsub + "(" + CurrentNumber + CurrentOperation + ")";
                            for (int s = TMP.Length; s > 0; s--)
                            {
                                if(TMP[s-1]!=null)
                                    oldsub = oldsub + TMP[s-1].value;
                            }                            
                        }
                    }
                }
                firstRun = true;
            }
                return oldsub;
        }

        public string ConvertToPostFix(string text1)
        {
            string result="";
            Stack st=new Stack();
            string[] Numbers = text1.Split(separators2,System.StringSplitOptions.RemoveEmptyEntries);
            char[] Operations = new char[Numbers.Length - 1];
            int countOP = 0;
            for (int c = 0; c < text1.Length; c++)
            {
                if (text1[c] == '+' || text1[c] == '-' || text1[c] == '*' || text1[c] == '/')
                {
                    Operations[countOP] = text1[c];
                    countOP++;
                }
            }
            string[] Paransis = new string[text1.Length - (Numbers.Length + (Numbers.Length - 1))];
            int Pcount = 0;
            for (int k = 0; k < text1.Length; k++)
            {
                if (text1[k] == '(' || text1[k] == ')')
                {
                    Paransis[Pcount] = Convert.ToString(text1[k]);
                    Pcount++;
                }
            }
            string[] text = new string[text1.Length];
            int OpCount = 0;
            int NumCount = 0;
            int Para = 0;
            int MainCount = 0;
            for (int x = 0; x < text1.Length; x++)
            {
                if (text1[x] == '(' || text1[x] == ')')
                {
                    text[MainCount] = Paransis[Para];
                    Para++;
                    MainCount++;
                }
                else
                    if (text1[x] == '+' || text1[x] == '-' || text1[x] == '*' || text1[x] == '/')
                    {
                        text[MainCount] = Convert.ToString(Operations[OpCount]);
                        OpCount++;
                        MainCount++;
                    }
                    else
                    {
                        text[MainCount] = Numbers[NumCount];
                        x = (x + Numbers[NumCount].Length) - 1;
                        NumCount++;
                        MainCount++;
                    }
            }
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != null)
                {
                    if (text[i] == "(")
                    {
                        element e = new element();
                        e.value = "(";
                        st.Push(e);
                    }
                    else
                        if (text[i] == ")")
                        {
                            if (st.Is_Empty())
                            {
                                element ee = new element();
                                element ee2 = new element();
                                ee = st.Top();
                                if (ee.value != "(")
                                {
                                    while (ee.value != "(")
                                    {
                                        ee2 = st.Pop();
                                        result = result + ee2.value + "  ";
                                        ee = st.Top();
                                    }
                                    ee = st.Pop();
                                }
                            }
                        }
                        else
                        {
                            if (text[i] == "+" || text[i] == "-")
                            {
                                element e2 = new element();
                                element top = new element();
                                top = st.Top();
                                while (top.value == "+" || top.value == "-" || top.value == "*" || top.value == "/")
                                {
                                    e2 = st.Pop();
                                    result = result + e2.value + "  ";
                                    top = st.Top();
                                }
                                element e4 = new element();
                                e4.value = text[i].ToString();
                                st.Push(e4);
                            }
                            else
                            {
                                if (text[i] == "*" || text[i] == "/")
                                {
                                    element e2 = new element();
                                    element top2 = new element();
                                    top2 = st.Top();
                                    while (top2.value == "*" || top2.value == "/")
                                    {
                                        e2 = st.Pop();
                                        result = result + e2.value + "  ";
                                        top2 = st.Top();
                                    }
                                    element e3 = new element();
                                    e3.value = text[i].ToString();
                                    st.Push(e3);
                                }
                                else
                                    result = result + text[i].ToString() + "  ";
                            }
                        }
                }
            }
            return result;
        }
       

        public double Evaluate(string OriginalParanthis)
        {
            Stack st = new Stack();
            string[] Numbers = OriginalParanthis.Split(separators2,System.StringSplitOptions.RemoveEmptyEntries);
            char[] Operations = new char[Numbers.Length - 1];
            int countOP = 0;
            for (int c = 0; c < OriginalParanthis.Length; c++)
            {
                if (OriginalParanthis[c] == '+' || OriginalParanthis[c] == '-' || OriginalParanthis[c] == '*' || OriginalParanthis[c] == '/')
                {
                    Operations[countOP] = OriginalParanthis[c];
                    countOP++;
                }
            }            
            string[] Paransis = new string[OriginalParanthis.Length-(Numbers.Length + (Numbers.Length - 1))];
            int Pcount=0;
            for (int k = 0; k < OriginalParanthis.Length; k++)
            {
                if (OriginalParanthis[k] == '(' || OriginalParanthis[k] == ')')
                {
                    Paransis[Pcount] = Convert.ToString(OriginalParanthis[k]);
                    Pcount++;
                }
            }
            string[] Paranthis = new string[OriginalParanthis.Length];
            int OpCount = 0;
            int NumCount = 0;
            int Para=0;
            int MainCount = 0;
            for (int x = 0; x < OriginalParanthis.Length; x++)
            {
                if (OriginalParanthis[x] == '(' || OriginalParanthis[x] == ')')
                {
                    Paranthis[MainCount] = Paransis[Para];
                    Para++;
                    MainCount++;
                }
                else
                    if (OriginalParanthis[x] == '+' || OriginalParanthis[x] == '-' || OriginalParanthis[x] == '*' || OriginalParanthis[x] == '/')
                    {
                        Paranthis[MainCount] = Convert.ToString(Operations[OpCount]);
                        OpCount++;
                        MainCount++;
                    }
                    else
                    {
                        Paranthis[MainCount] = Numbers[NumCount];
                        x = (x + Numbers[NumCount].Length)-1;
                        NumCount++;
                        MainCount++;
                    }
            }
            for (int i = 0; i < Paranthis.Length; i++)
            {
                if(Paranthis[i]!=null)
                {
                    if (Paranthis[i] != ")")
                    {
                        element e = new element(Paranthis[i]);
                        st.Push(e);
                    }
                    else
                    {
                        element[] e2 = new element[3];
                        for (int j = 0; j < 3; j++)
                        {
                            e2[j] = st.Pop();
                        }
                        element e3 = new element();
                        element Nop = new element();
                        if (e2[1].value == "+")
                        {
                            e3.value = Convert.ToString(Convert.ToDouble(e2[2].value) + Convert.ToDouble(e2[0].value));
                            Nop = st.Pop();
                            st.Push(e3);
                        }
                        if (e2[1].value == "-")
                        {
                            e3.value = Convert.ToString(Convert.ToDouble(e2[2].value) - Convert.ToDouble(e2[0].value));
                            Nop = st.Pop();
                            st.Push(e3);
                        }
                        if (e2[1].value == "*")
                        {
                            e3.value = Convert.ToString(Convert.ToDouble(e2[2].value) * Convert.ToDouble(e2[0].value));
                            Nop = st.Pop();
                            st.Push(e3);
                        }
                        if (e2[1].value == "/")
                        {
                            e3.value = Convert.ToString(Convert.ToDouble(e2[2].value) / Convert.ToDouble(e2[0].value));
                            Nop = st.Pop();
                            st.Push(e3);
                        }
                    }
                }
            }
            element result = new element();
            result = st.Pop();
            return Convert.ToDouble(result.value);
        }
    }
}
