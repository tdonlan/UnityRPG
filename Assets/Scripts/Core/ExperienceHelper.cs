using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityRPG
{
    public class ExperienceHelper
    {
        public static List<long> xpTable = new List<long>(){0,
                                100,
                                205,
                                420,
                                861,
                                1765,
                                3618,
                                7417,
                                15205,
                                31170,
                                63899,
                                130993,
                                268536,
                                550499,
                                1128523,
                                2313472,
                                4742618,
                                9722367,
                                19930852,
                                40858247,
                                83759406
                                };

        public static long getXPNextLevel(int level, long curentXP)
        {
            if (level < xpTable.Count-1)
            {
                return xpTable[level] - curentXP;
            }
            return 0;
        }

        public static float getLevelProgressPercent(int level, long currentXP)
        {
            var prevLevel = level-1;
            if(prevLevel <0) prevLevel = 0;
            var prevLevelXP = xpTable[prevLevel];
            var nextLevelXP = xpTable[level];
            return (float)(currentXP - prevLevelXP) / (float)(nextLevelXP - prevLevelXP);
        }

        //give counters for each level gained with the xp
        public static int getLevelUpCounter(int level, long currentXP, long addXP)
        {
            int counter = 0;
            bool isLeveling = true;
            long newXP = currentXP + addXP;
            while (isLeveling)
            {
                if (xpTable[level + counter] > newXP)
                {
                    isLeveling = false;
                }
                else
                {
                    counter++;
                }
                    
            }
            return counter;
        }

    }
}
