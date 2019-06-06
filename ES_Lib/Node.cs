using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ES_Lib
{
    public class Node
    {
        public string NodeKind;
        public string Value;
        public string RunValue;
        public ArrayList nextstates;
        public bool isInitial;
        public string ActionFileName;

        public Histo HoffmanValue;
        
        public Node()
        {
            NodeKind = "";
            Value = "";
            RunValue = "";
            ActionFileName = "";
            nextstates = new ArrayList();
            isInitial = false;           
        }
    }
}
