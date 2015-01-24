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
                        b.board[i, j].empty = false;
                        b.board[i, j].TileChar = '#';
                    }
                    
                }
            }

            return b;
            
        }
    }
}
