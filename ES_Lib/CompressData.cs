using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ES_Lib
{
    public class CompressData
    {
        private int[] Histogram;
        private ArrayList Dictionary = new ArrayList();
        public Histo Result=null;
       
        public CompressData()
        {
            
        }

        public Histo HoffmanCompress(string Data)
        {
            GetHistogram(Data);
            Result= HoffmanCore();
            Result.IsInitial = true;
            return Result;
        }

        public string getStringBitStream(Histo root, string Data)
        {
            string Result = "";
            for (int i = 0; i < Data.Length; i++)
            {
                Result += getLetterBitStream(root, Data[i]);
            }
            return Result;
        }

        public string getLetterBitStream(Histo root,char Letter)
        {
            string Result = "";
            if (root != null)
            {
                for (int i = 0; i < root.nextstates.Count; i++)
                {
                    Result += root.Transition;
                    string Temp = getLetterBitStream((Histo)root.nextstates[i], Letter);
                    if (Temp != "")
                    {
                        Result += Temp;
                        break;
                    }
                    else
                        if (Result != "")
                            Result = Result.Substring(0, Result.Length - 1);
                }
                if (Letter == root.Key[0] && root.Key!="Node")
                    return root.Transition;
                else
                    return Result;
            }
            else
                return "";
        }

        int Counter = 1;
        private Histo HoffmanCore()
        {
            Dictionary.Sort(new Histo());
            
            Histo root = new Histo();
            Histo H1 = new Histo();
            Histo H2=new Histo();
            H1= (Histo)Dictionary[Dictionary.Count-1];
            if (Dictionary.Count > 1)
            {
                H2 = (Histo)Dictionary[Dictionary.Count - 2];
                if (H2.Kind != "Node")
                    H2.Kind = "Leaf";
                H2.Transition = "0";
                root.nextstates.Add(H2);
            }
            if(H1.Kind!="Node")
                H1.Kind = "Leaf";
            H1.Transition="1";
            root.nextstates.Add(H1);
            root.Value = ((Histo)Dictionary[Dictionary.Count - 1]).Value;
            if (Dictionary.Count > 1)
            {
                root.Value += ((Histo)Dictionary[Dictionary.Count - 2]).Value;
                Dictionary.RemoveAt(Dictionary.Count - 1);
            }
            Dictionary.RemoveAt(Dictionary.Count - 1);
            root.Key = "Node  " + Counter;
            root.Kind = "Node";
            Counter++;
            if (Dictionary.Count == 0)
                return root;
            Dictionary.Add(root);
            if (Dictionary.Count > 1)
                return HoffmanCore();
            else
                return root;
        }

        private void GetHistogram(string Data)
        {
            Histogram = new int[256];
            for (int i = 0; i < 256; i++)
            {
                Histogram[i] = 0;
            }
            for (int i = 0; i < Data.Length; i++)
            {
                Histogram[Data[i]]++;
            }
            
            for (int i = 0; i < 256; i++)
            {
                if (Histogram[i] != 0)
                {
                    Histo H = new Histo();
                    H.Key = Convert.ToChar(i).ToString();
                    H.Value = Histogram[i];
                    Dictionary.Add(H);
                }
            }
        }

        

    }

    public class Histo : IComparer
    {
        public string Key;
        public int Value;
        public ArrayList nextstates;
        public string Transition;
        public bool IsInitial;
        public string Kind;

        public Histo()
        {
            nextstates = new ArrayList();
        }

        public int Compare(object x, object y)
        {
            return ((Histo)y).Value.CompareTo(((Histo)x).Value);
            /*
            if (((Histo)x).Value < ((Histo)y).Value)
                return ((Histo)x).Value;
            else
                return ((Histo)y).Value;
             */
        }
    }
}
