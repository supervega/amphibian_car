using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ES_Lib
{
    public class State
    {
        public ArrayList nextstates,LoopNextStates;
        private string name;
        private bool is_Accept;
        private bool is_Initial;
        private bool is_Loop;
       
        public State()        
        {
            nextstates = new ArrayList();
            LoopNextStates = new ArrayList();
            name = "";
            is_Accept = false;
            is_Initial = false;
            is_Loop = false;
        }

        public struct Transition
        {
            public State state;
            public ArrayList TransitionChars;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IS_Accepted
        {
            get { return is_Accept; }
            set { is_Accept = value; }
        }

        public bool IS_Initial
        {
            get { return is_Initial; }
            set { is_Initial = value; }
        }

        public bool IS_Loop
        {
            get { return is_Loop; }
            set { is_Loop = value; }
        }
    }
}
