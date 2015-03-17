using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class CoreHelper
    {
        //returns a list of characters A, B, C, etc to use when displaying board
        public static List<char> getLetterList()
        {
            return new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        }

        //2 digits
        public static string getPaddedNum(int num)
        {
            if(num <10)
            {
                return " " + num.ToString();
            }
            else
            {
                return num.ToString();
            }
        }


        public static int displayMenuGetInt(List<string> menu)
        {
            bool valid = false;
            int retval = -1;
            while (!valid)
            {
                foreach (var s in menu)
                {
                    Console.Write(s + "\n");
                }
                Console.Write(">");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid Input");
                }
                else if (Int32.TryParse(input, out retval))
                {
                    if(retval <=0 || retval > menu.Count)
                    {
                        Console.WriteLine("Invalid Input");
                    }
                    else { valid = true; }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            return retval;
        }

        //inclue a regex or a list of accepted values?
        public static string displayMenuGetStr(List<string> menu)
        {
            bool valid = false;
            string input = "";
            while (!valid)
            {
                foreach (var s in menu)
                {
                    Console.Write(s + "\n");
                }
                Console.Write(">");
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid Input");
                }
                {
                    valid = true;
                }
            }
            return input;
        }

       

        public static Point parseStringPoint(string pt)
        {
            Point retval = null;
            string[] ptSplit = pt.Split(',');
            if (ptSplit.Length == 2)
            {
                try
                {
                    char ptChar = (char)ptSplit[0].ToUpper()[0];
                    int y = getLetterList().FindIndex(a => a == ptChar);
                    int x = Int32.Parse(ptSplit[1]);

                    retval = new Point(x, y);
                }
                catch
                {

                }

            }
            return retval;
        }

        //Not used
        public static int RandFromStrRange(Random r, string range)
        {
            string[] strArray = range.Split('-');
            if(strArray.Length == 2)
            {
                try
                {
                    return r.Next(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]));
                }
                catch(Exception ex)
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static int getEffectAmount(Random r, List<ActiveEffect> activeEffects, List<PassiveEffect> passiveEffects, StatType stat )
        {
            int amount = 0;
            foreach(var ae in activeEffects)
            {
                if(ae.statType == stat)
                {
                    amount += r.Next(ae.minAmount, ae.maxAmount);
                }
            }
            foreach(var pe in passiveEffects)
            {
                if(pe.statType == stat)
                {
                    amount += r.Next(pe.minAmount, pe.maxAmount);
                }
            }

            return amount;
        }

        public static int getArmorAmount(List<Armor> equipment)
        {
            int amount = 0;
            foreach(var a in equipment)
            {
                amount += a.armor;
            }
            return amount;
        }

        //Determines if a stat is effected (for buff icons, highlights, etc)
        public static bool checkEffect(List<ActiveEffect> activeList, List<PassiveEffect> passiveList, StatType statType)
        {
            bool retval = false;

            var aeCount = (from data in activeList
                           where data.statType == statType
                           select data).Count();

            var peCount = (from data in passiveList
                           where data.statType == statType
                           select data).Count();

            if (aeCount > 0 || peCount > 0)
            {
                retval = true;
            }

            return retval;
        }
    }
}
