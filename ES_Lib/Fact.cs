using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ES_Lib
{
    public class Fact:Cell
    {
        public bool IsTrue;
        public ArrayList Attributes;
        private float degreeOfblieve;

        public Fact()
        {
            Attributes = new ArrayList();
            IsTrue = false;
            degreeOfblieve = -1;
        }

        public Fact(string Data)
        {
            Attributes = new ArrayList();
            IsTrue = false;
            base.Data = Data;
            degreeOfblieve = -1;
        }

        public float DegreeOfBelieve
        {
            get
            {
                return degreeOfblieve;
            }
            set
            {
                degreeOfblieve = value;
            }
        }

        public string Data
        {
            get { return base.Data; }
            set { base.Data = value; }
        }
        public int Index
        {
            get { return base.Index; }
            set { if (value > 0)base.Index = value; }
        }

        public override bool Modify()
        {
            throw new NotImplementedException();
        }

        public void ProcessFact()
        {
            string[] Att = base.Data.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < Att.Length; i++)
            {
                Attributes.Add(Att[i]);
            }
            base.Data = "";
            for (int i = 0; i < Att.Length; i++)
            {
                if (Att.Length > 1)
                    base.Data += Att[i] + " ";
                else
                    base.Data = Att[0];
            }
           
            IsTrue = true;
        }
      
    }
}
