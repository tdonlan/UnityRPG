using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleRPG2
{
    public class PatternFactory
    {
        /*
         *              .#.
         *              #X#
         *              .#.
         */
        public static List<Point> getFourAdj()
        {
            return new List<Point>() { 
                new Point(-1,0),
                new Point(1,0),
                new Point(0,-1),
                new Point(0,1)
            };
        }

        /*
         *              ###
         *              ###
         *              ###
         */
        public static List<Point> getNineSquare()
        {
            return new List<Point>() { 
                new Point(-1,-1),
             new Point(-1,0),
              new Point(-1,1),
               new Point(0,-1),
                new Point(0,0),
                 new Point(0,1),
                  new Point(1,-1),
                   new Point(1,0),
                    new Point(1,1)
            };
        }


        /*
         *              ###
         *              #X#
         *              ###
         */
        public static List<Point> getEightAdj()
        {
            return new List<Point>() { 
                new Point(-1,-1),
                 new Point(-1,0),
                  new Point(-1,1),
                   new Point(0,-1),
                     new Point(0,1),
                      new Point(1,-1),
                       new Point(1,0),
                        new Point(1,1)
            };
        }
    }
}
