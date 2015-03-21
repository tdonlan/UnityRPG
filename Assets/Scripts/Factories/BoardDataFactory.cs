using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityRPG
{

    public class BoardDataFactory
    {
        //These boards will be loaded from external JSON

        public static BoardData getDungeonBoard(TileSpriteLibrary tileLibrary)
        {
            char[,] simpleCharBoard = getCharArrayFromString(getSimpleBoardString());
            BoardData boardData = new BoardData(simpleCharBoard,tileLibrary.getTileSpriteList("Dungeon"),"Dungeon1",null);
            return boardData;
        }

        public static char[,] getCharArrayFromString(string str)
        {
            int len = (int)Math.Sqrt((double)str.Length);  //assuming board string is square
            char[,] charArray = new char[len, len];

             char[] flatCharArray = str.ToCharArray();
             for (int i = 0; i < len; i++)
             {
                 for (int j = 0; j < len; j++)
                 {
                     charArray[i, j] = flatCharArray[i * len + j];
                 }
             }

             return charArray;
        }

        public static string getSimpleBoardString()
        {

            return @"###################
                        #.................#
                        #.................#
                        #.........##......#
                        #.................#
                        #.................#
                        #....##......E....#
                        #....#........E...#
                        #............E....#
                        #.....#....E..E...#
                        #.PP...#.....E....#
                        #.PP...#......E...#
                        #.PP.........E....#
                        #.....#...........#
                        #.................#
                        #.................#
                        #.......###.......#
                        #.................#
                        #.................#
                        ###################";

        }
    }

}