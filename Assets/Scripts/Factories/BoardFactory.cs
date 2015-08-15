using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public class BoardFactory
    {



        //DEPRECATED
        public static Board getRandomBoard(BattleGame game, int size)
        {
            Board b = new Board(game, size);
            for(int i=0;i<size;i++)
            {
                for(int j=0;j<size;j++)
                {
                    if(game.r.NextDouble() > .95)
                    {
                        b.board[i, j] = getTile('#', i, j);

                    }
                    else
                    {
                        b.board[i, j] = getTile('.', i, j);
                    }
                }
            }

            return b;
        }

        private static TileSpriteLookup getTileSpriteLookupFromTileData(int x, int y, bool isEmpty, TileMapData tileMapData)
        {
            TileSpriteType tileSpriteType = TileSpriteType.Floor;
            if (isEmpty)
            {
                tileSpriteType = TileSpriteType.Wall;
            }
            if (tileMapData.checkEnemySpawnCollision(new Point(x, -y)))
            {
                tileSpriteType = TileSpriteType.EnemyStart;
            }
            else if (tileMapData.checkPlayerSpawnCollision(new Point(x, -y)))
            {
                tileSpriteType = TileSpriteType.PlayerStart;
            }

            TileSpriteLookup tileSpriteLookup = new TileSpriteLookup('_',"",0,isEmpty,tileSpriteType);
            return tileSpriteLookup;

        }

        //construct a custom tile array for use in battle board, from tileMapData
        private static Tile[,] getBoardTileArrayFromTileMapData(TileMapData tileMapData)
        {
            Tile[,] boardTileArray = new Tile[tileMapData.tileArray.GetLength(0), tileMapData.tileArray.GetLength(1)];
            for (int i = 0; i < boardTileArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardTileArray.GetLength(1); j++)
                {
                    TileSpriteLookup tileSpriteLookup = getTileSpriteLookupFromTileData(i,j,tileMapData.tileArray[i,j].empty,tileMapData);
                    Tile tempTile = new Tile(i,j,tileSpriteLookup.isEmpty);
                    boardTileArray[i,j] = tempTile;
                }
            }

            return boardTileArray;
        }

        private static Tile[,] copyTileArray(Tile[,] tileArray1)
        {
            int width = tileArray1.GetLength(0);
            int height = tileArray1.GetLength(1);
            Tile[,] newTileArray = new Tile[width,height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newTileArray[x, y] = tileArray1[x, y];
                }
           
            }

            return newTileArray;

        }

       

        public static Board getBoardFromBattleGameData(BattleGameData battleGameData, BattleGame battleGame)
        {
            Board b = new Board(battleGame, battleGameData.tileMapData.tileArray.GetLength(0));
            b.board = copyTileArray(battleGameData.tileMapData.tileArray);

            return b;
        }

        //DEPRECATED
        public static Board getBoardFromBoardData(BattleGameData gameData, BattleGame game, BoardData boardData)
        {
            var boardLayoutStr = gameData.assetLibrary.boardLayoutDictionary[boardData.boardLayoutName];
            var tileLookupDict = gameData.assetLibrary.tileSpriteLibrary.getTileSpriteDictionary(boardData.tileLookupName);

            char[,] boardCharArray = getCharArrayFromString(boardLayoutStr);
            boardCharArray = flipBoardXAxis(boardCharArray);

            Board b = new Board(game, boardCharArray.GetLength(0));

            for (int i = 0; i < boardCharArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardCharArray.GetLength(1); j++)
                {
                    var tileLookup = tileLookupDict[boardCharArray[i,j]];
                    b.board[j,i] = getTileFromData(tileLookup, j,i);
                }
            }

            return b;

        }

        //flip the board on the horizontal axis so it displays correctly in unity
        private static char[,] flipBoardXAxis(char[,] board1)
        {
            int height = board1.GetLength(0);
            int width = board1.GetLength(1);

            char[,] retvalBoard = new char[height, width];
            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    retvalBoard[i, j] = board1[height - i - 1, j];
                }
            }

            return retvalBoard;

        }

        private static char[,] getCharArrayFromString(string str)
        {
            str = str.Replace("\r", "").Replace("\n", "");

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

        /*
        public static Board getBoardFromBoardData(BattleGame game, BoardData data)
        {
            Board b = new Board(game, data.size);
            for (int i = 0; i < data.boardTileData.GetLength(0); i++)
            {
                for (int j = 0; j < data.boardTileData.GetLength(1); j++)
                {
                    b.board[i, j] = getTileFromData(data.getTileSpriteData(data.boardTileData[i, j]), i, j);
                }
            }
            return b;
        }
         * */

        public static Tile getTileFromData(TileSpriteLookup tileLookup, int x, int y)
        {
            Tile retval = new Tile(x, y);
            retval.TileChar = tileLookup.tileChar;
            retval.tileSheetName = tileLookup.spritesheetName; 
            retval.tileSpriteIndex = tileLookup.spritesheetIndex;
            retval.tileSpriteLookup = tileLookup;

            retval.tempSheetName = string.Empty;
            retval.tempSpriteIndex = 0;
            retval.empty = tileLookup.isEmpty;

            return retval;
        }

      
        public static Tile getTile(char c, int x, int y)
        {
            Tile retval = new Tile(x,y);

            retval.TileChar = c;
            
            retval.tempSheetName = string.Empty;
            retval.tempSpriteIndex = 0;

            switch(c)
            {
                case '.':
                    retval.tileSheetName = "Tiles";
                    retval.tileSpriteIndex = 2;

                    retval.empty = true;
                    
                    break;
                case '#':
                    retval.tileSheetName = "Tiles";
                    retval.tileSpriteIndex = 27;
                    retval.empty = false;
                    break;
                default:
                    retval.tileSheetName = "Tiles";
                    retval.tileSpriteIndex = 2;
                    retval.empty = true;
                    break;
            }
            return retval;
        }
    }
}
