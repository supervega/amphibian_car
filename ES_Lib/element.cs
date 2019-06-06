using System;
using System.Collections.Generic;
using System.Text;

namespace ES_Lib
{
    public class element
    {

        public string value;
        public element Next;

        public element()
        {

        }

        public element(string n)
        {
            value = n;
        }
    }
}
