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

        public static Tile getTileFromData(TileSpriteLookup tileLookup, int x, int y)
        {
            Tile retval = new Tile(x, y);
            retval.TileChar = tileLookup.tileChar;
            retval.tileSheetName = tileLookup.spritesheetName; 
            retval.tileSpriteIndex = tileLookup.spritesheetIndex;

            retval.tempSheetName = string.Empty;
            retval.tempSpriteIndex = 0;
            retval.empty = tileLookup.isEmpty;

            return retval;
        }

       //DEPRECATED
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
