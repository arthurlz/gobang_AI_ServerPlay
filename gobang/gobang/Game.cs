using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace gobang
{
    /// <summary>
    /// 游戏类
    /// </summary>
    class Game
    {
        public ChessBoard curState; // 保存当前棋盘

        bool isStart; // 游戏是否开始
        int curUser; // 当前行棋人

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void startGame(bool whoFirst)
        {
            // 先产生新的棋盘
            curState = new ChessBoard();
            // 开始游戏
            isStart = true;
            if (whoFirst)
                curUser = Globe.USER_1;
            else
                curUser = Globe.USER_2;
        }

        /// <summary>
        /// 转换当前游戏角色
        /// </summary>
        public void changeUser()
        {
            curUser = nextUser(curUser);
        }

        /// <summary>
        /// 给出下一步的行棋人
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private int nextUser(int user)
        {
            return user == Globe.USER_1 ? Globe.USER_2 : Globe.USER_1;
        }

        /// <summary>
        /// 当前行棋人放置棋子
        /// 1.尝试放置棋子
        /// 2.放置失败返回失败
        /// 3.放置成功检察游戏是否结束
        /// 4.转换游戏角色后返回成功
        /// </summary>
        /// <param name="x">棋子的横坐标</param>
        /// <param name="y">棋子的纵坐标</param>
        /// <returns>返回放置棋子是否成功</returns>
        public bool placePiece(int x, int y)
        {
            if (curState.placePiece(x, y, curUser))
            {
                // 检察行棋人是否胜利
                if (isWin(x, y))
                {
                    isStart = false; // 游戏结束
                    return true;
                }
                changeUser(); // 转换游戏角色
                return true;
            }
            return false;
        }

        public bool placePieceNet(int x, int y)
        {
            if (curState.placePiece(x, y, curUser))
            {
                // 检察行棋人是否胜利
                if (isWin(x, y))
                {
                    isStart = false; // 游戏结束
                    return true;
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// 检察当前行棋人是否胜利
        /// </summary>
        /// <param name="x">最后一枚棋子横坐标</param>
        /// <param name="y">最后一枚棋子纵坐标</param>
        /// <returns>返回当前行棋人是否胜利</returns>
        private bool isWin(int x, int y)
        {
            if(evaluatePiece(curState, x, y, curUser) >= GameSetting.EVA_FIVE) return true;
            return false;
        }

        /// <summary>
        /// 结束游戏
        /// </summary>
        private void gameOver()
        {
            isStart = false;
        }

        /// <summary>
        /// 查询游戏是否已经结束
        /// </summary>
        /// <returns>返回游戏是否结束</returns>
        public bool isOver()
        {
            return !isStart;
        }

        /// <summary>
        /// 获取当前行棋人
        /// </summary>
        /// <returns>返回当前行棋人</returns>
        public int getCurUser()
        {
            return curUser;
        }

        /// <summary>
        /// 评价函数
        /// 评价一行（一个方向）的棋子
        /// 以center作为评估位置进行评价
        /// </summary>
        /// <param name="line">
        /// 包含17个数字 最左边和最右边的数字为方便测试边界
        /// -1 0 1 2 3 4 5 6 7 8 ...
        ///  X X X X X X X X X X
        /// 代表待评价的一行棋子
        /// 0 - 无子
        /// 1 - 待评价方子
        /// 2 - 对方子或无法下子
        /// </param>
        /// <returns></returns>
        private int evaluateLine(int[] line, bool ALL)
        {
            int value = 0; // 评估值
            int cnt = 0; // 连子数
            int blk = 0; // 封闭数

            // 从左向右扫描
            for (int i = 0; i < Globe.BOARD_SIZE; ++i)
            {
                if (line[i] == Globe.EVA_MY) // 找到第一个己方的棋子
                {
                    // 还原计数
                    cnt = 1;
                    blk = 0;
                    // 看左侧是否封闭
                    if (line[i - 1] == Globe.EVA_OP) ++blk;
                    // 计算连子数
                    for (i = i + 1; i < Globe.BOARD_SIZE && line[i] == Globe.EVA_MY; ++i, ++cnt) ;
                    // 看右侧是否封闭
                    if (line[i] == Globe.EVA_OP) ++blk;
                    // 计算评估值
                    value += getValue(cnt, blk);
                }
            }
            return value;
        }

        /// <summary>
        /// 评价函数
        /// 评价一行（一个方向）的棋子（前后4格范围内）
        /// 以center作为评估位置进行评价
        /// </summary>
        /// <param name="line">
        /// 包含9个数字
        /// 0 1 2 3 4 5 6 7 8
        /// X X X X O X X X X
        /// 代表待评价旗子及其左右四个棋子
        /// 0 - 无子
        /// 1 - 待评价方子
        /// 2 - 对方子或无法下子
        /// </param>
        /// <returns></returns>
        private int evaluateLine(int[] line)
        {
            int cnt = 1; // 连子数
            int blk = 0; // 封闭数
            // 向两个方向考察
            // 向左
            for (int i = 3; i >= 0; --i)
            {
                if (line[i] == Globe.EVA_MY) ++cnt;
                else if (line[i] == Globe.EVA_OP)
                {
                    ++blk;
                    break;
                }
                else break;
            }
            // 向右
            for (int i = 5; i < 9; ++i)
            {
                if (line[i] == Globe.EVA_MY) ++cnt;
                else if (line[i] == Globe.EVA_OP)
                {
                    ++blk;
                    break;
                }
                else break;
            }

            return getValue(cnt, blk);
        }

        /// <summary>
        /// 根据连字数和封堵数
        /// 给出一个评价值
        /// </summary>
        /// <param name="cnt">连字数</param>
        /// <param name="blk">封堵数</param>
        /// <returns></returns>
        private int getValue(int cnt, int blk)
        {
            if (blk == 0) // 活棋
            {
                switch (cnt)
                {
                    case 1: return GameSetting.EVA_ONE;
                    case 2: return GameSetting.EVA_TWO;
                    case 3: return GameSetting.EVA_THREE;
                    case 4: return GameSetting.EVA_FOUR;
                    default: return GameSetting.EVA_FIVE;
                }
            }
            else if (blk == 1) // 单向封死
            {
                switch (cnt)
                {
                    case 1: return GameSetting.EVA_ONE_S;
                    case 2: return GameSetting.EVA_TWO_S;
                    case 3: return GameSetting.EVA_THREE_S;
                    case 4: return GameSetting.EVA_FOUR_S;
                    default: return GameSetting.EVA_FIVE;
                }
            }
            else // 双向堵死
            {
                if (cnt >= 5) return GameSetting.EVA_FIVE;
                else return GameSetting.EVA_ZERO;
            }
        }

        /// <summary>
        /// 对一个状态的一个位置
        /// 放置一种类型的棋子的优劣进行估价
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="x">位置的横坐标</param>
        /// <param name="y">位置的纵坐标</param>
        /// <param name="type">棋子的类型</param>
        /// <returns>评估值</returns>
        private int evaluatePiece(ChessBoard state, int x, int y, int type)
        {
            int value = 0; // 估价值

            int[] line = new int[17]; // 线状态

            bool[] flagX = new bool[8]; // 横向边界标志
            flagX[0] = x - 4 < 0; flagX[1] = x - 3 < 0; flagX[2] = x - 2 < 0; flagX[3] = x - 1 < 0;
            flagX[4] = x + 1 > 14; flagX[5] = x + 2 > 14; flagX[6] = x + 3 > 14; flagX[7] = x + 4 > 14;

            bool[] flagY = new bool[8]; // 纵向边界标志
            flagY[0] = y - 4 < 0; flagY[1] = y - 3 < 0; flagY[2] = y - 2 < 0; flagY[3] = y - 1 < 0;
            flagY[4] = y + 1 > 14; flagY[5] = y + 2 > 14; flagY[6] = y + 3 > 14; flagY[7] = y + 4 > 14;

            line[4] = Globe.EVA_MY; // 设置中心棋子

            // 横
            line[0] = flagX[0] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x - 4, y].type, type));
            line[1] = flagX[1] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x - 3, y].type, type));
            line[2] = flagX[2] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x - 2, y].type, type));
            line[3] = flagX[3] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x - 1, y].type, type));

            line[5] = flagX[4] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x + 1, y].type, type));
            line[6] = flagX[5] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x + 2, y].type, type));
            line[7] = flagX[6] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x + 3, y].type, type));
            line[8] = flagX[7] ? Globe.EVA_OP : (Globe.getPieceType(state.chessBoard[x + 4, y].type, type));

            value += evaluateLine(line);

            // 纵
            line[0] = flagY[0] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y - 4].type, type);
            line[1] = flagY[1] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y - 3].type, type);
            line[2] = flagY[2] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y - 2].type, type);
            line[3] = flagY[3] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y - 1].type, type);

            line[5] = flagY[4] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y + 1].type, type);
            line[6] = flagY[5] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y + 2].type, type);
            line[7] = flagY[6] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y + 3].type, type);
            line[8] = flagY[7] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x, y + 4].type, type);

            value += evaluateLine(line);

            // 左上-右下
            line[0] = flagX[0] || flagY[0] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 4, y - 4].type, type);
            line[1] = flagX[1] || flagY[1] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 3, y - 3].type, type);
            line[2] = flagX[2] || flagY[2] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 2, y - 2].type, type);
            line[3] = flagX[3] || flagY[3] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 1, y - 1].type, type);

            line[5] = flagX[4] || flagY[4] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 1, y + 1].type, type);
            line[6] = flagX[5] || flagY[5] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 2, y + 2].type, type);
            line[7] = flagX[6] || flagY[6] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 3, y + 3].type, type);
            line[8] = flagX[7] || flagY[7] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 4, y + 4].type, type);

            value += evaluateLine(line);

            // 右上-左下
            line[0] = flagX[7] || flagY[0] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 4, y - 4].type, type);
            line[1] = flagX[6] || flagY[1] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 3, y - 3].type, type);
            line[2] = flagX[5] || flagY[2] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 2, y - 2].type, type);
            line[3] = flagX[4] || flagY[3] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x + 1, y - 1].type, type);

            line[5] = flagX[3] || flagY[4] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 1, y + 1].type, type);
            line[6] = flagX[2] || flagY[5] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 2, y + 2].type, type);
            line[7] = flagX[1] || flagY[6] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 3, y + 3].type, type);
            line[8] = flagX[0] || flagY[7] ? Globe.EVA_OP : Globe.getPieceType(state.chessBoard[x - 4, y + 4].type, type);

            value += evaluateLine(line);

            return value;
        }

        /// <summary>
        /// 评价一个棋面上的一方
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="type">评价方</param>
        /// <returns></returns>
        public int evaluateState(ChessBoard state, int type)
        {
            int value = 0;
            // 线状态
            int[][] line = new int[6][];
            line[0] = new int[17];
            line[1] = new int[17];
            line[2] = new int[17];
            line[3] = new int[17];
            line[4] = new int[17];
            line[5] = new int[17];
            int lineP;

            // 方便检查边界
            for (int p = 0; p < 6; ++p)
                line[p][0] = line[p][16] = Globe.EVA_OP;
            
            // 从四个方向产生
            for (int i = 0; i < Globe.BOARD_SIZE; ++i)
            {
                
                // 产生线状态
                lineP = 1;
                for (int j = 0; j < Globe.BOARD_SIZE; ++j)
                {
                    line[0][lineP] = Globe.getPieceType(state.chessBoard, i, j, type); // |
                    line[1][lineP] = Globe.getPieceType(state.chessBoard, j, i, type); // -
                    line[2][lineP] = Globe.getPieceType(state.chessBoard, i + j, j, type); // \  
                    line[3][lineP] = Globe.getPieceType(state.chessBoard, i - j, j, type); // /
                    line[4][lineP] = Globe.getPieceType(state.chessBoard, j, i + j, type); // \
                    line[5][lineP] = Globe.getPieceType(state.chessBoard, Globe.BOARD_SIZE - j - 1, i + j, type); // /
                    ++lineP;
                }
                // 评估线状态
                int special = i == 0 ? 4 : 6;
                for (int p = 0; p < special; ++p)
                {
                    value += evaluateLine(line[p], true);
                }
            }

            return value;
        }

        ////////////////////////////////  AI部分  /////////////////////////////////////////////
        ////////////////////////////////  AI部分  /////////////////////////////////////////////
        ////////////////////////////////  AI部分  /////////////////////////////////////////////
        ////////////////////////////////  AI部分  /////////////////////////////////////////////

        int MAX_DEPTH = 3; // 最大搜索层数

        /// <summary>
        /// 判断给定状态下
        /// x, y位置是否有必要进行搜索
        /// 若x, y位置周围1格内有棋子则有必要搜索
        /// </summary>
        /// <param name="state">给定棋局</param>
        /// <param name="x">判断位置的横坐标</param>
        /// <param name="y">判断位置的纵坐标</param>
        /// <returns>是否有必要搜索</returns>
        private bool canSearch(ChessBoard state, int x, int y)
        {
           
            int tmpx = x - 1;
            int tmpy = y - 1;
            for (int i = 0; tmpx < Globe.BOARD_SIZE && i < 3; ++tmpx, ++i)
            {
                int ty = tmpy;
                for (int j = 0; ty < Globe.BOARD_SIZE && j < 3; ++ty, ++j)
                {
                    if (tmpx >= 0 && ty >= 0 && state.chessBoard[tmpx, ty].type != Globe.EMPTY)
                        return true;
                    else
                        continue;
                }
            }
            return false;
        }

        
        /// <summary>
        /// 博弈树
        /// </summary>
        /// <param name="state">待转换的状态</param>
        /// <param name="x">状态转换的横坐标</param>
        /// <param name="y">状态转换的纵坐标</param>
        /// <param name="type">当前层的标记：MAX或MIN</param>
        /// <param name="depth">当前层深</param>
        /// <param name="alpha">父层alpha值</param>
        /// <param name="beta">父层beta值</param>
        /// <returns></returns>
        private int minMax(ChessBoard state, int x, int y, int type, int depth, int alpha, int beta)
        {
            // 产生新的状态
            ChessBoard newState = new ChessBoard(state);
            newState.placePiece(x, y, nextType(type));
            
            int weight = 0;

            int max = -Globe.INF; // 下层权值上界
            int min = Globe.INF; // 下层权值下界

            if (depth < MAX_DEPTH) // 在一定深度内进行搜索
            {
                // 已输或已胜则不继续搜索
                if (evaluatePiece(newState, x, y, nextType(type)) >= GameSetting.EVA_FIVE)
                {
                    if (type == Globe.MIN_NODE) return GameSetting.EVA_FIVE; // 我方胜
                    else return -GameSetting.EVA_FIVE;
                }
                // 向下扩展
                int i, j;
                for (i = 0; i < Globe.BOARD_SIZE; ++i)
                {
                    for (j = 0; j < Globe.BOARD_SIZE; ++j)
                    {
                        if (newState.chessBoard[i, j].type == Globe.EMPTY && canSearch(newState, i, j)) // 先判断有无搜索必要
                        {
                            weight = minMax(newState, i, j, nextType(type), depth + 1, min, max); // 扩展此节点

                            if (weight > max) max = weight; // 更新下层上界
                            if (weight < min) min = weight; // 更新下层下界

                            // alpha-beta 剪枝
                            if (type == Globe.MAX_NODE)
                            {
                                if (max >= alpha) return max;
                            }
                            else
                            {
                                if (min <= beta) return min;
                            }
                            
                        }
                        else continue; // 无搜索必要则跳过
                    }
                }

                // 扩展完毕
                if (type == Globe.MAX_NODE) return max; // 最大层给出最大值
                else return min; // 最小层给出最小值
            }
            else
            {
                weight = evaluateState(newState, Globe.MAX_NODE); // 评估我方局面
                weight -= type == Globe.MIN_NODE ? evaluateState(newState, Globe.MIN_NODE) * 10 : evaluateState(newState, Globe.MIN_NODE); // 评估对方局面
                return weight; // 搜索到限定层后给出权值
            }
        }

        /// <summary>
        /// AI 行棋
        /// </summary>
        public Point placePieceAI()
        {
            int weight,temp = -Globe.INF;
            int x = 0, y = 0;
            for (int i = 0; i < Globe.BOARD_SIZE; ++i)
            {
                for (int j = 0; j < Globe.BOARD_SIZE; ++j)
                {
                    if (curState.chessBoard[i, j].type == Globe.EMPTY && canSearch(curState, i, j)) // 先判断有无搜索必要
                    {
                        weight = minMax(curState, i, j, nextType(Globe.MAX_NODE), 1, -Globe.INF, Globe.INF); // 扩展此节点

                        if (weight > temp)
                        {
                            temp = weight; // 更新下层上界
                            x = i;
                            y = j;
                        }
                    }
                    else continue; // 无搜索必要则跳过
                }
            }
            placePiece(x, y); // AI在最优点行棋
            return new Point(x, y);
        }
        
        /// <summary>
        /// 给出后继节点的类型
        /// </summary>
        /// <param name="type">节点类型</param>
        /// <returns></returns>
        private int nextType(int type)
        {
            return type == Globe.MAX_NODE ? Globe.MIN_NODE : Globe.MAX_NODE;
        }

        /// <summary>
        /// AI第一步走的时候“随机的一步”
        /// </summary>
        /// <returns></returns>
        public Point getStartStep()
        {
            Random rd = new Random();
            int x = rd.Next(5, 11); //x方向
            int y = rd.Next(5, 11);
            return new Point(x, y); 
        }
    }
}
