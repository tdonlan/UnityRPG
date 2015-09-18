using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class BoardFactory
    {


        public static Board getBoardFromBattleGameData(BattleGameData battleGameData, BattleGame battleGame)
        {
            Board b = new Board(battleGame, battleGameData.tileMapData.battleTileArray.GetLength(0));
            b.board = copyTileArray(battleGameData.tileMapData.battleTileArray);

            return b;
        }

        private static Tile[,] copyTileArray(Tile[,] tileArray1)
        {
            int width = tileArray1.GetLength(0);
            int height = tileArray1.GetLength(1);
            Tile[,] newTileArray = new Tile[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newTileArray[x, y] = tileArray1[x, y];
                }

            }

            return newTileArray;

        }
    }
}
