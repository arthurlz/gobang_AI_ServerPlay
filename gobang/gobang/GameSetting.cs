using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gobang
{

    /// <summary>
    /// 存储游戏的各种设定
    /// </summary>
    static class GameSetting
    {
        public static int startingUser = Globe.USER_1; // 先手玩家
        public static int model = Globe.PVP; // 对弈模式
        //public static int difficulty = Globe.HARD; // AI难度

        // 静态估价表
        public const int EVA_ZERO = 0;
        public const int EVA_ONE = 10;
        public const int EVA_ONE_S = 1;
        public const int EVA_TWO =  80;
        public const int EVA_TWO_S = 10;
        public const int EVA_THREE = 500;
        public const int EVA_THREE_S = 100;
        public const int EVA_FOUR = 10000;
        public const int EVA_FOUR_S = 1000;
        public const int EVA_FIVE = 100000;
        
    }
}
