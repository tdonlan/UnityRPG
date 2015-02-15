using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class BoardFactory
    {
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
