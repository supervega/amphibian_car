using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ES_Lib
{
    public class Rule:Cell
    {
        private bool tested;
        private float certaintyfactor;
        private string ThenPart, ElsePart;
        private bool IFpartIsOperation;
        private ArrayList IFPart;
        private OperationType OPtype;
        private float fuzzyResult;
        private int numOfFiring = 0;

        public struct InnerStruct
        {
            public string IFPart;
            public bool IsTrue;
        }

        public int NumOfFiring
        {
            get
            {
                return numOfFiring;
            }
            set
            {
                numOfFiring = value;
            }

        }

        public enum OperationType
        { OR,AND,NONE};
        public Rule(string Data)
        {
            base.Data = Data;
            IFpartIsOperation = false;
            IFPart = new ArrayList();
            OPtype = OperationType.NONE;
        }

        public float FuzzyResult
        {
            get
            {
                return fuzzyResult;
            }
            set
            {
                fuzzyResult = value;
            }
        }

        public OperationType RuleType
        {
            get
            {
                return OPtype;
            }
            set
            {
                OPtype = value;
            }
        }

        public int Index
        {
            get { return base.Index; }
            set { if (value > 0)base.Index = value; }
        }
        public string Data
        {
            get { return base.Data; }
            set { base.Data = value.Trim(); }
        }

        public float CertaintyFactor
        {
            get { return certaintyfactor; }
            set { certaintyfactor = value; }
        }

        public ArrayList IFPART
        {
            get { return IFPart; }
            set { IFPart = value; }
        }

        public string ELSEPART
        {
            get { return ElsePart; }
            set { ElsePart = value; }
        }

        public string THENPART
        {
            get { return ThenPart; }
            set { ThenPart = value; }
        }
        public bool ISTrue
        {
            get { return tested; }
            set { tested = value; }
        }

        public override bool Modify()
        {
            throw new NotImplementedException();
        }

        public void ProcessRule(bool Ifstate)
        {
            if (Ifstate)
                IFpartIsOperation = true;
            OPtype = OperationType.NONE;
            string[] Att = base.Data.Split(new string[] { "if "," then "," else "}, StringSplitOptions.RemoveEmptyEntries);
            string[] Info=Att[0].Split(new string[]{" OR "},StringSplitOptions.RemoveEmptyEntries);
            if (Info.Length >1)
            {
                for (int i = 0; i < Info.Length; i++)
                {
                    InnerStruct IS = new InnerStruct();
                    IS.IFPart = Info[i];
                    IS.IsTrue = false;
                    IFPart.Add(IS);
                    OPtype = OperationType.OR;
                }
            }
            Info = Att[0].Split(new string[] { " AND " }, StringSplitOptions.RemoveEmptyEntries);
            if (Info.Length > 1)
            {               
                for (int i = 0; i < Info.Length; i++)
                {
                    InnerStruct IS = new InnerStruct();
                    IS.IFPart = Info[i];
                    IS.IsTrue = false;
                    IFPart.Add(IS);
                    OPtype = OperationType.AND;
                }
            }
            if (OPtype == OperationType.NONE)
            {
                InnerStruct IS = new InnerStruct();
                IS.IFPart = Att[0];
                IS.IsTrue = false;
                IFPart.Add(IS);
            }
            ThenPart = Att[1];
            ElsePart = Att[2];
            //base.Data = "If<" + Att[0] + ">\n\tThen<" + Att[1] + ">\n\tElse <" + Att[2] + ">";
        }

        public string Fire(string[] Fact)
        {
            bool IFisTrue = true;
            if (Fact.Length == IFPart.Count)
            {
                for (int i = 0; i < IFPart.Count; i++)
                {
                    InnerStruct IS = (InnerStruct)IFPart[i];
                    if (IS.IFPart == Fact[i])
                        continue;
                    else
                        IFisTrue = false;
                }
                if (IFisTrue)
                    return ElsePart;
                else
                    return ThenPart;
            }
            else
                return "wrong number of arguments.";           
        }

        public Dictionary<string,string> GetAntecedents()
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            foreach (InnerStruct IS in IFPart)
            {
                Data.Add(IS.IFPart.Split(new string[] { " is " }, StringSplitOptions.RemoveEmptyEntries)[0].ToString(), IS.IFPart.Split(new string[] { " is " }, StringSplitOptions.RemoveEmptyEntries)[1].ToString());
            }
            return Data;
        }

        public bool ISFactTrue(string Fact)
        {
            foreach (InnerStruct IS in IFPART)
            {
                if (IS.IFPart.Trim() == Fact.Trim())
                    return true;
                else
                    continue;
            }
            return false;
        }

        
    }
}
