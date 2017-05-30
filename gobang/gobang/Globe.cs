using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace gobang
{
    /// <summary>
    /// 存储游戏中的公共常量及方法
    /// </summary>
    static class Globe
    {
        public const int BOARD_SIZE = 15; // 棋盘尺寸
        public const int GRID_SIZE = 30; // 棋盘格尺寸
        
        public const int EMPTY = 0; // 未放置棋子
        public const int USER_1 = 1; // 玩家一的棋子
        public const int USER_2 = 2; // 玩家二的棋子

        public const int PVP = 1; // 人人对弈
        public const int PVC = 2; // 人机对弈

        //public const int EASY = 2; // 容易
        //public const int NORMAL = 3; // 普通
        //public const int HARD = 4; // 困难
        //public const int PRO = 5; // 专家

        public const int ERROR_INDEX = -1; // 错误的索引

        public const int EVA_EMPTY = 0; // 评价函数 - 无子
        public const int EVA_MY = 1; // 评价函数 - 待评价子
        public const int EVA_OP = 2; // 评价函数 - 对方子或不能下子处

        public const int MAX_NODE = 2;
        public const int MIN_NODE = 1;

        public const int INF = 99999999; // 无穷大

        public static Color CLEAR = Color.FromArgb(0, 0, 0, 0); // 透明色

        public static Image BACKGROUND = global::gobang.Properties.Resources.bg; // 棋盘背景图
        public static Image BLACK = global::gobang.Properties.Resources.black; // 选中标记图标
        public static Image WHITE = global::gobang.Properties.Resources.white; // 落子标记图标

        public static Image SELECTED = global::gobang.Properties.Resources.selected; // 选中标记图标
        public static Image SET = global::gobang.Properties.Resources.set; // 落子标记图标

        /// <summary>
        /// 将单一坐标转换为棋盘索引
        /// </summary>
        /// <param name="coordinate">单一坐标</param>
        /// <returns>返回索引值</returns>
        static public int C2I(int coordinate)
        {
            // 计算索引
            int index = coordinate / GRID_SIZE;
            // 保证索引合法
            return (index < BOARD_SIZE && index >= 0) ? index : ERROR_INDEX;
        }

        /// <summary>
        /// 将索引值转换为棋盘格中心的单一坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns>返回棋盘格的中心单一坐标</returns>
        static public int I2C_C(int index)
        {
            return index * GRID_SIZE + GRID_SIZE / 2;
        }

        /// <summary>
        /// 将索引值转换为棋盘格的单一坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns>返回棋盘格的坐标</returns>
        static public int I2C(int index)
        {
            return index * GRID_SIZE;
        }

        /// <summary>
        /// 根据给定type
        /// 判定A是待判断棋子
        /// 或无棋子
        /// 或对方棋子
        /// </summary>
        /// <param name="A">待判断棋子的类型</param>
        /// <param name="type">我方棋子的类型</param>
        /// <returns></returns>
        static public int getPieceType(int A, int type)
        {
            return A == type ? EVA_MY : (A == EMPTY ? EVA_EMPTY : EVA_OP);
        }

        static public int getPieceType(ChessBoard.Grid[,] board, int x, int y, int type)
        {
            // 超出边界按对方棋子算
            if (x < 0 || y < 0 || x >= BOARD_SIZE || y >= BOARD_SIZE) return EVA_OP;
            else return getPieceType(board[x, y].type, type);
        }
    }
}
