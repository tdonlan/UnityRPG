using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class ItemHelper
    {
        //return a list of item sets, given the full inventory
        public static List<ItemSet> getItemSetList(List<Item> inventory)
        {

            List<ItemSet> retvalList = new List<ItemSet>();

            var usableItemList = (from data in inventory
                                  where data is UsableItem
                                  select data).ToList();

            foreach(var item in usableItemList)
            {
                int count = usableItemList.Count(x => x.ID == item.ID);
               // int count = usableItemList.Select(x=>x.ID==item.ID).Count();
                ItemSet tempItemSet = new ItemSet(){itemName=item.name,itemID=item.ID,count = count};
                if(!retvalList.Contains(tempItemSet))
                {
                    retvalList.Add(tempItemSet);
                }
            }

            return retvalList;
        }

        public static ItemSet getItemSet(List<Item> inventory, Item i)
        {
            int count = inventory.Count(x => x.ID == i.ID);
            return new ItemSet() { itemName = i.name, itemID = i.ID, count = count };
        }

        public static Item getFirstItemWithID(List<Item> inventory, int ID)
        {
            var item = (from data in inventory
                        where data.ID == ID
                        select data).FirstOrDefault();

            return item;
                      
        }
    }
}
