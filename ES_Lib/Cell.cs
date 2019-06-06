using System;
using System.Collections.Generic;
using System.Text;

namespace ES_Lib
{
    abstract public class Cell
    {
        protected int Index;
        protected string Data;
        
       
        abstract public bool Modify();
      
    }
}
