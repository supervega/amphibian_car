using System;
using System.Collections.Generic;
using System.Text;

namespace ES_Lib
{
    public class Token
    {
        private string T_Type;
        private string T_Value;

        public Token()
        { }

        public Token(string TOKENTYPE)        
        {
            T_TYPE = TOKENTYPE;
        }

        public Token(string TOKENTYPE, string TOKENVALUE)
        {
            T_TYPE = TOKENTYPE;
            T_VALUE = TOKENVALUE;
        }

        public string T_TYPE
        {
            get {return T_Type; }
            set {T_Type=value; }
        }

        public string T_VALUE
        {
            get { return T_Value; }
            set { T_Value = value; }
        }
       
    }
}
