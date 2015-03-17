using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class BattleLog
    {
        public List<string> log { get; set; }
        
        public BattleLog()
        {
            log = new List<string>();
            log.Add("Starting Battle - " + DateTime.Now.ToShortTimeString() + "\n");

        }

        public void AddEntry(string txt)
        {
            log.Add(txt + "\n");
        }

        //DEPRECATED
        public void Print(int num)
        {
            string retval = "";
            int index = 0;
            if (log.Count > num)
            {
                index = log.Count - num;
            }
            for (int i = index; i < log.Count; i++)
            {
                retval += string.Format("{0}. {1}\n", i, log[i]);
            }

            Console.Write(retval);
        }

        //num most recent lines
        //0 = all
        public string PrintLog(int num)
        {
            string retval = "";
            int index = 0;
            if (log.Count > num)
            {
                index = log.Count - num;
            }
            if(num == 0)
            {
                index = 0;
            }
            for (int i = index; i < log.Count; i++)
            {
                retval += string.Format("{0}. {1}\n", i, log[i]);
            }
            return retval;
        }

        //return the 4 most recent log entries in reverse order
        public override string ToString()
        {
            string retval = "";
            int index = 0;
            if(log.Count > 4)
            {
                index = log.Count - 4;
            }
            for(int i=index;i<log.Count;i++)
            {
                retval += string.Format("{0}. {1}\n", i, log[i]);
            }
            return retval;
        }
    }
}
