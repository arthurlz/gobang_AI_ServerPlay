using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gobang
{
    /// <summary>
    /// 棋盘类
    /// </summary>
    class ChessBoard
    {
        public Grid[,] chessBoard = new Grid[Globe.BOARD_SIZE, Globe.BOARD_SIZE]; // 保存棋盘

        /// <summary>
        /// 棋盘构造函数
        /// 产生一个空棋盘
        /// </summary>
        public ChessBoard()
        {
            for (int i = 0; i < Globe.BOARD_SIZE; ++i)
                for (int j = 0; j < Globe.BOARD_SIZE; ++j)
                    chessBoard[i, j] = new Grid();
        }

        /// <summary>
        /// 棋盘构造函数
        /// 复制一个棋盘
        /// </summary>
        /// <param name="othr">复制此棋盘</param>
        public ChessBoard(ChessBoard othr)
        {
            for (int i = 0; i < Globe.BOARD_SIZE; ++i)
                for (int j = 0; j < Globe.BOARD_SIZE; ++j)
                    chessBoard[i, j] = new Grid(othr.chessBoard[i, j].type);
        }

        /// <summary>
        /// 放置棋子
        /// </summary>
        /// <param name="x">棋子的横坐标</param>
        /// <param name="y">棋子的纵坐标</param>
        /// <param name="type">棋子的类型（玩家一或者玩家二）</param>
        /// <returns>返回放置棋子是否成功</returns>
        public bool placePiece(int x, int y, int type)
        {
            // 判断是否可以放置棋子
            if (chessBoard[x, y].isEmpty())
            {
                chessBoard[x, y].type = type; // 更新棋盘
                return true;
            }
            return false;
        }

        /// <summary>
        /// 棋盘格类
        /// </summary>
        public class Grid
        {
            public int type;

            /// <summary>
            /// 棋子构造函数
            /// 默认不放置棋子
            /// </summary>
            public Grid()
            {
                type = Globe.EMPTY;
            }

            /// <summary>
            /// 棋子构造函数
            /// </summary>
            /// <param name="t">放置类型</param>
            public Grid(int t)
            {
                type = t;
            }

            /// <summary>
            /// 检察格子的是否可以放置棋子
            /// </summary>
            /// <returns></returns>
            public bool isEmpty()
            {
                return type == Globe.EMPTY ? true : false;
            }

        }
    }
}

