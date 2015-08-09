using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



    public enum FlagType
    {
        boolFlag,
        intFlag,
        stringFlag,
    }

    public enum CompareType
    {
        Equal,
        NotEqual,
        Less,
        LessEqual,
        Greater,
        GreaterEqual,
        Contains,
        Subset
    }

    public class GlobalFlag
    {
        public string name { get; set; }
        public FlagType flagType { get; set; }
        public string value { get; set; }

        public GlobalFlag(string name, FlagType type, string value)
        {
            this.name = name;
            this.flagType = flagType;
            this.value = value;

        }

        public bool checkFlag(string compareValue, CompareType compareType)
        {
            switch(this.flagType)
            {
                case FlagType.boolFlag:
                    switch(compareType)
                    {
                        case CompareType.Equal:
                            return checkFlag_BoolEqual(compareValue);
                        case CompareType.NotEqual:
                            return checkFlag_BoolNotEqual(compareValue);
                        default: return false;
                    }
                case FlagType.intFlag:
                    switch(compareType)
                    {
                        case CompareType.Equal:
                            return checkFlag_IntEqual(compareValue);
                        case CompareType.Less:
                            return checkFlag_IntLess(compareValue);
                        case CompareType.LessEqual:
                            return checkFlag_IntLessEqual(compareValue);
                        case CompareType.Greater:
                            return checkFlag_IntGreater(compareValue);
                        case CompareType.GreaterEqual:
                            return checkFlag_IntGreaterEqal(compareValue);
                        default: return false;
                    }
               
                case FlagType.stringFlag:
                     switch(compareType)
                    {
                         case CompareType.Equal:
                            return checkFlag_StringEqual(compareValue);
                         case CompareType.Contains:
                            return checkFlag_StringContains(compareValue);
                         case CompareType.Subset:
                            return checkFlag_StringSubstring(compareValue);
                         default:
                            return false;
                    }
                    
                default:
                     return false;
            }
        }

        private bool checkFlag_BoolEqual(string strBool)
        {
            return bool.Parse(this.value) == bool.Parse(strBool);
        }

        private bool checkFlag_BoolNotEqual(string strBool)
        {
            return bool.Parse(this.value) != bool.Parse(strBool);
        }

        private bool checkFlag_StringEqual(string str)
        {
            return this.value.Equals(str);
        }

        private bool checkFlag_StringContains(string str)
        {
            return this.value.Contains(str);
        }

        private bool checkFlag_StringSubstring(string str)
        {
            return str.Contains(this.value);
        }

        private bool checkFlag_IntGreater(string strInt)
        {
            return (int.Parse(this.value) > int.Parse(strInt));
        }

        private bool checkFlag_IntGreaterEqal(string strInt)
        {
            return (int.Parse(this.value) >= int.Parse(strInt));
        }

        private bool checkFlag_IntEqual(string strInt)
        {
            return (int.Parse(this.value) == int.Parse(strInt));
        }

        private bool checkFlag_IntLess(string strInt)
        {
            return (int.Parse(this.value) < int.Parse(strInt));
        }

        private bool checkFlag_IntLessEqual(string strInt)
        {
            return (int.Parse(this.value) <= int.Parse(strInt));
        }

    }

    public class GlobalFlags
    {
        public List<GlobalFlag> globalFlagList { get; set; }
        public GlobalFlags()
        {
            globalFlagList = new List<GlobalFlag>();
        }

        private GlobalFlag getFlag(string flagName)
        {
             var flag = (from data in globalFlagList
                        where data.name == flagName
                        select data).FirstOrDefault();
            return flag;
        }

        public bool checkFlag(string flagName, string compareValue, CompareType compareType)
        {
           var flag = getFlag(flagName);
           if (compareType != CompareType.NotEqual)
           {
               if (flag != null)
               {
                   return flag.checkFlag(compareValue, compareType);
               }
               return false;
           }
           else
           {
               if (flag != null)
               {
                   return flag.checkFlag(compareValue, compareType);
               }
               return true; //null is != the flag, so return true
           }
        }

        public void updateFlag(string flagName, string value)
        {
            var flag = getFlag(flagName);

            flag.value = value;
        }

        public void addFlag(string flagName, FlagType type, string value)
        {
            var flag = getFlag(flagName);

            if (flag != null)
            {
                updateFlag(flagName, value);
            }
            else
            {
                globalFlagList.Add(new GlobalFlag(flagName, type, value));
            }
        }
    }

