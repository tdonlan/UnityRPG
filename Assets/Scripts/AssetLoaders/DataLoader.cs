using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.IO;


    public class DataLoader
    {
        public static Dictionary<long, object> loadMasterDictionary(string str, Type objectType)
        {
            Dictionary<long, object> masterDictionary = new Dictionary<long, object>();
            var dictList = getDictListFromStrArrayList(ReadStringCSVHelper(str));

            foreach (Dictionary<string, object> dict in dictList)
            {
                object newObject = getObjectFromDict(objectType, dict);

                masterDictionary.Add(getIDFromObject(objectType, newObject), newObject);

            }
            return masterDictionary;

        }

        private static long getIDFromObject(Type t, object o)
        {
            PropertyInfo propInfo = t.GetProperty("ID");
            object ID = propInfo.GetValue(o,null);
            return (long)ID;
        }

        private static Object getObjectFromDict(Type objectType, Dictionary<string, object> dataDictionary)
        {

            Object tempObject = Activator.CreateInstance(objectType);
            foreach (string key in dataDictionary.Keys)
            {

                foreach (PropertyInfo propInfo in objectType.GetProperties())
                {
                    if (propInfo.Name.ToLower().Equals(key.ToLower()))
                    {

                        PropertyInfo newProp = objectType.GetProperty(propInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                        newProp.GetValue(tempObject,null);

                        if (propInfo.PropertyType.IsEnum) //Enum
                        {
                            object enumObj = Enum.Parse(propInfo.PropertyType, dataDictionary[key].ToString());
                            newProp.SetValue(tempObject, enumObj,null);
                        }
                        else if (propInfo.PropertyType.GetInterfaces().Contains(typeof(ICollection)))
                        {
                            List<long> indexList = getListFromObject(dataDictionary[key]);
                            newProp.SetValue(tempObject, indexList, null);
                        }
                        else
                        {
                            newProp.SetValue(tempObject, dataDictionary[key], null);
                        }
                    }
                }
            }

            return tempObject;
        }

        private static List<long> getListFromObject(object o)
        {
            List<long> splitList = new List<long>();
            try
            {
                string objectString = o.ToString();
                if (objectString != null && objectString.Length > 0)
                {
                    objectString = objectString.Replace("{", "").Replace("}", "");

                    splitList = objectString.Split(';').ToList().Select(x => Int64.Parse(x)).ToList();
                }
            }
            catch (Exception ex)
            {
               
            }
            return splitList;
        }

        //---------------------------------------------

        private static List<string[]> ReadStringCSVHelper(string str)
        {

            TextReader reader = new StringReader(str);

            var parser = new CsvHelper.CsvParser(reader);
            List<string[]> csvList = new List<string[]>();

            while (true)
            {
                var row = parser.Read();
                if (row != null)
                {
                    csvList.Add(row);
                }
                else
                {
                    break;
                }
            }

            return csvList;
        }

        private static List<string[]> ReadFileCSVHelper(string filename)
        {
            StreamReader reader = File.OpenText(filename);

            var parser = new CsvHelper.CsvParser(reader);
            List<string[]> csvList = new List<string[]>();

            while (true)
            {
                var row = parser.Read();
                if (row != null)
                {
                    csvList.Add(row);
                }
                else
                {
                    break;
                }
            }

            return csvList;

        }

        private static List<Dictionary<string, object>> getDictListFromStrArrayList(List<string[]> csvList)
        {

            List<Dictionary<string, object>> dictList = new List<Dictionary<string, object>>();

            //first row has key names
            List<string> keyNames = csvList[0].ToList();
            csvList.RemoveAt(0);

            foreach (var row in csvList)
            {
                Dictionary<string, object> rowDict = new Dictionary<string, object>();

                for (int i = 0; i < row.Length; i++)
                {
                    rowDict.Add(keyNames[i], getRowObject(row[i]));
                }

                dictList.Add(rowDict);
            }

            return dictList;

        }

        private static object getRowObject(string data)
        {

            int num;
            if (Int32.TryParse(data, out num))
            {
                return num;
            }
            else if (data == "null")
            {
                return null;
            }
            else
            {
                return data;
            }
        }
    }

