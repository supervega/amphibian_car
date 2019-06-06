using System;
using System.Collections.Generic;
using System.Text;

namespace ES_Lib
{
    public class Stack
    {
        private element Head;
        private int c;

        public Stack()
        {
            Head = null;
            count = 0;
        }

        public int count
        {
            get
            {
                return c;
            }
            set
            {
                c = value;
            }
        }

        public element Pop()
        {
            element e;
            if (count == 0)
                return null;
            else
            {
                count--;
                e = Head;
                Head = Head.Next;
                return e;
            }

        }
        public void Push(element e)
        {
            e.Next = Head;
            Head = e;
            count++;
        }

        public element Top()
        {
            if (count != 0)
                return Head;
            else
                return null;
        }

        public bool Is_Empty()
        {
            if (count != 0)
                return true;
            else
                return false;
        }

        public int Count()
        {
            return count;
        }

        public void Clear()
        {
            element e;
            while (Is_Empty())
                e=Pop();
        }
    }
}
