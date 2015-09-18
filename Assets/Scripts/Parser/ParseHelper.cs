using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;


    public class ParseHelper
    {
        public static string getFullPath(string manifest, string relativePath)
        {
            
            return Path.GetDirectoryName(manifest) + "/" + relativePath;
        }
        //returns a string list from a string delim split
       public static List<string> getSplitList(string str, string delim)
        {
            return str.Split(new string[]{delim},StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        //returns list of string in psuedo-JSON arrays 
        // {element1;element2;element3}
       public static List<string> getSplitListInBlock(string str, string delim, string blockStart, string blockEnd)
       {
           var strBlock = getBlockFromString(str, blockStart, blockEnd);
           return getSplitList(strBlock, delim);
       }

        //returns a block (without the start and end brackets) that is a subset of a string
       private static string getBlockFromString(string str, string blockStart, string blockEnd)
       {
           if (str.Contains(blockStart) && str.Contains(blockEnd))
           {
               int startIndex = str.IndexOf(blockStart) + blockStart.Length;
               int endIndex = str.IndexOf(blockEnd);
               int blockLen = endIndex - startIndex;
               return str.Substring(startIndex, blockLen);
           }
           else
           {
               return "";
           }
           
       }

        //removes a block (and start/end brackets) from a string
        public static string removeBlock(string str, string blockStart, string blockEnd)
       {
           
           if (str.Contains(blockStart) && str.Contains(blockEnd))
           {
               int startIndex = str.IndexOf(blockStart);
               
               return str.Remove(startIndex);
           }
           else
           {
               return str;
           }
       }

    }

