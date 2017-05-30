using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace gobang
{
    public partial class gameUI : Form
    {
        Game game = new Game(); // 创建游戏

        Bitmap canvas; // 画布
        Bitmap board; // 棋盘
        Bitmap flag; // 标记
        Bitmap preset; // 前一个落子点

        Graphics g; // 棋盘画板
        Graphics g_b; // 棋盘
        Graphics g_f; // 标记
        Graphics g_p; // 标记


        private int model = Globe.PVC;  //单机模式还是网络模式
        private bool orderTogo; //网络模式先后手

        //网络对战使用
        private string ServerIP; //IP
        private int port;   //端口
        private bool isExit = false;
        private TcpClient client;
        private BinaryReader br;
        private BinaryWriter bw;
        private string nameRightNow;
        private string rivalName;
        private bool waitForRival = false;
        /// <summary>
        /// 初始化棋盘
        /// </summary>
        public gameUI()
        {
            InitializeComponent();

            canvas = new Bitmap(chessBoard.ClientSize.Width, chessBoard.ClientSize.Height);
            board = new Bitmap(chessBoard.ClientSize.Width, chessBoard.ClientSize.Height);
            flag = new Bitmap(chessBoard.ClientSize.Width, chessBoard.ClientSize.Height);
            preset = new Bitmap(chessBoard.ClientSize.Width, chessBoard.ClientSize.Height);

            g = Graphics.FromImage(canvas);
            g_b = Graphics.FromImage(board);
            g_f = Graphics.FromImage(flag);  //chessBoard.CreateGraphics();
            g_p = Graphics.FromImage(preset);

            chessBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(chessBoard_MouseMove);
            chessBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(chessBoard_MouseDown);

            Random r = new Random((int)DateTime.Now.Ticks);
            nameText.Text = "player" + r.Next(100, 999);
        }



        /// <summary>
        /// 菜单选项 - 游戏 - 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 鼠标移动时显示位置标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chessBoard_MouseMove(object sender, MouseEventArgs e)
        {
            // 游戏开始时有效
            if (!game.isOver())
            {
                // 取得横纵索引
                int x = Globe.C2I(e.X);
                int y = Globe.C2I(e.Y);

                // 绘制标记
                g_f.Clear(Globe.CLEAR); // 重绘标记层
                g_f.DrawImage(Globe.SELECTED, Globe.I2C(x), Globe.I2C(y));
                // 重绘
                Draw();
            }
        }

        /// <summary>
        /// 放置棋子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chessBoard_MouseDown(object sender, MouseEventArgs e)
        {
            // 游戏开始时有效
            if (!game.isOver())
            {
                // 取得横纵索引
                int x = Globe.C2I(e.X);
                int y = Globe.C2I(e.Y);


                //****//
                if (model == Globe.PVC) //人机
                {//****//
                    int curUser = game.getCurUser(); // 获取当前行棋人

                    // 单机模式
                    // 放置棋子
                    if (game.placePiece(x, y))
                    {
                        if (PFirstToolStripMenuItem.Checked) //玩家先手
                        {
                            // 绘制棋子
                            DrawPiece(curUser, x, y);
                            // 绘制落子标记
                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                            g_p.DrawImage(Globe.SET, Globe.I2C(x), Globe.I2C(y)); // 绘制落子标记
                            // 重绘
                            Draw();

                            int value = 0;
                            value = game.evaluateState(game.curState, curUser);
                            ;
                            // 判断输赢
                            if (game.isOver())
                            {
                                string info = "执黑棋方";
                                if (MessageBox.Show(info + "获胜!\n点击确认开始新游戏",
                                                    "游戏结束",
                                                    MessageBoxButtons.OKCancel) == DialogResult.OK
                                    )
                                {
                                    PVCStartToolStripMenuItem_Click(null, null);
                                }
                                return;
                            }

                            curUser = game.getCurUser();
                            Point AIPiece = game.placePieceAI(); // AI行棋

                            // 绘制棋子
                            DrawPiece(curUser, AIPiece.X, AIPiece.Y);
                            // 绘制落子标记
                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                            g_p.DrawImage(Globe.SET, Globe.I2C(AIPiece.X), Globe.I2C(AIPiece.Y)); // 绘制落子标记
                            // 重绘
                            Draw();
                            // 判断输赢
                            if (game.isOver())
                            {
                                string info = "执白棋方";
                                if (MessageBox.Show(info + "获胜!\n点击确认开始新游戏",
                                                    "游戏结束",
                                                    MessageBoxButtons.OKCancel) == DialogResult.OK
                                    )
                                {
                                    PVCStartToolStripMenuItem_Click(null, null);
                                }
                            }
                        }
                        else  //AI先
                        {
                            curUser = game.getCurUser();
                            DrawPiece(curUser, x, y);
                            // 绘制落子标记
                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                            g_p.DrawImage(Globe.SET, Globe.I2C(x), Globe.I2C(y)); // 绘制落子标记
                            // 重绘
                            Draw();
                            if (game.isOver())
                            {
                                string info = "执白棋方";
                                if (MessageBox.Show(info + "获胜!\n点击确认开始新游戏",
                                                    "游戏结束",
                                                    MessageBoxButtons.OKCancel) == DialogResult.OK
                                    )
                                {
                                    PVCStartToolStripMenuItem_Click(null, null);
                                    if (PFirstToolStripMenuItem.Checked == false)
                                    {
                                        Point AIPiece1 = game.getStartStep(); // AI行棋
                                        if (game.placePiece(AIPiece1.X, AIPiece1.Y))
                                        {
                                            // 绘制棋子
                                            DrawPiece(Globe.USER_1, AIPiece1.X, AIPiece1.Y);
                                            // 绘制落子标记
                                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                                            g_p.DrawImage(Globe.SET, Globe.I2C(AIPiece1.X), Globe.I2C(AIPiece1.Y)); // 绘制落子标记
                                            // 重绘
                                            Draw();
                                        }

                                    }
                                }
                                return;
                            }



                            Point AIPiece = game.placePieceAI(); // AI行棋
                            curUser = game.getCurUser();
                            DrawPiece(Globe.USER_1, AIPiece.X, AIPiece.Y);
                            // 绘制落子标记
                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                            g_p.DrawImage(Globe.SET, Globe.I2C(AIPiece.X), Globe.I2C(AIPiece.Y)); // 绘制落子标记
                            // 重绘
                            Draw();
                            if (game.isOver())
                            {
                                string info = "执黑棋方";
                                if (MessageBox.Show(info + "获胜!\n点击确认开始新游戏",
                                                    "游戏结束",
                                                    MessageBoxButtons.OKCancel) == DialogResult.OK
                                    )
                                {
                                    PVCStartToolStripMenuItem_Click(null, null);
                                    if (PFirstToolStripMenuItem.Checked == false)
                                    {
                                        Point AIPiece2 = game.getStartStep(); // AI行棋
                                        if (game.placePiece(AIPiece2.X, AIPiece2.Y))
                                        {
                                            // 绘制棋子
                                            DrawPiece(Globe.USER_1, AIPiece2.X, AIPiece2.Y);
                                            // 绘制落子标记
                                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                                            g_p.DrawImage(Globe.SET, Globe.I2C(AIPiece2.X), Globe.I2C(AIPiece2.Y)); // 绘制落子标记
                                            // 重绘
                                            Draw();
                                        }

                                    }
                                }
                                return;
                            }

                        }
                    }

                }
                else if (model == Globe.PVP)// 网络模式
                {
                    //MessageBox.Show("sasasa");
                    int curUserNet = game.getCurUser(); // 获取当前行棋人

                    // 单机模式
                    // 放置棋子
                    if (waitForRival && game.placePieceNet(x, y)) //短路与 重要 placePieceNet会改变棋盘信息
                    {
                        DrawPiece(curUserNet, x, y);
                        // 绘制落子标记
                        g_p.Clear(Globe.CLEAR); // 重绘落子标记
                        g_p.DrawImage(Globe.SET, Globe.I2C(x), Globe.I2C(y)); // 绘制落子标记

                        Draw();
                        SendMessage(string.Format("Step,{0},{1},{2}", rivalName,
                            x.ToString(), y.ToString()));
                        waitForRival = false;
                    }

                    int value = 0;
                    value = game.evaluateState(game.curState, curUserNet);
                    
                    // 判断输赢
                    if (game.isOver())
                    {
                        SendMessage("Lose," + rivalName);
                        if (MessageBox.Show("恭喜你赢了！",
                                            "游戏结束",
                                            MessageBoxButtons.OK) == DialogResult.OK
                            )
                        {
                            game.startGame((curUserNet == Globe.USER_1) ? true : false);  
                            g_b.Clear(Globe.CLEAR); // 清理磁盘
                            g_p.Clear(Globe.CLEAR); // 清理落子标记
                            Draw(); // 重绘
                            waitForRival = true;
                        }
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 根据当前行棋人绘制棋子
        /// </summary>
        /// <param name="curUser">当前行棋人</param>
        /// <param name="x">棋子横坐标</param>
        /// <param name="y">棋子纵坐标</param>
        private void DrawPiece(int curUser, int x, int y)
        {
            if(curUser == Globe.USER_1)
                g_b.DrawImage(Globe.BLACK, Globe.I2C(x), Globe.I2C(y), 30, 30); // 直接增添新棋子
            else
                g_b.DrawImage(Globe.WHITE, Globe.I2C(x), Globe.I2C(y), 30, 30); // 直接增添新棋子
        }



        private delegate void CrossThreadDrawdel();

        private void CrossThreadDraw()
        {
            if (chessBoard.InvokeRequired)
            {
                CrossThreadDrawdel d = new CrossThreadDrawdel(CrossThreadDraw);
                chessBoard.Invoke(d);
            }
            else
            {
                chessBoard.BackgroundImage = canvas; // 设置为背景层
                chessBoard.Refresh();
                chessBoard.CreateGraphics().DrawImage(canvas, 0, 0);
            }
        }


        /// <summary>
        /// 重新绘制棋盘
        /// </summary>
        private void Draw()
        {
            g.DrawImage(Globe.BACKGROUND, 0, 0); // 绘制棋盘
            g.DrawImage(board, 0, 0); // 绘制棋子布局
            g.DrawImage(flag, 0, 0); // 绘制落子标志
            g.DrawImage(preset, 0, 0); // 绘制上一步棋子标志

            CrossThreadDraw();
        }

        /// <summary>
        /// 玩家还是电脑先手
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PFirstToolStripMenuItem.Checked = !PFirstToolStripMenuItem.Checked;
            PVCStartToolStripMenuItem_Click(null, null);
            if(PFirstToolStripMenuItem.Checked == false)
            {
                Point AIPiece = game.getStartStep(); // AI行棋
                if (game.placePiece(AIPiece.X, AIPiece.Y))
                { 
                    // 绘制棋子
                    DrawPiece(Globe.USER_1, AIPiece.X, AIPiece.Y);
                    // 绘制落子标记
                    g_p.Clear(Globe.CLEAR); // 重绘落子标记
                    g_p.DrawImage(Globe.SET, Globe.I2C(AIPiece.X), Globe.I2C(AIPiece.Y)); // 绘制落子标记
                    // 重绘
                    Draw();
                }
                
            }
        }

        /// <summary>
        /// 人机模式开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PVCStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.startGame(PFirstToolStripMenuItem.Checked); // 开始游戏

            g_b.Clear(Globe.CLEAR); // 清理磁盘
            g_p.Clear(Globe.CLEAR); // 清理落子标记
            Draw(); // 重绘
        }

        private void PVPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PVPToolStripMenuItem.Checked = !PVPToolStripMenuItem.Checked;
            if(PVPToolStripMenuItem.Checked)
            {
                NetButton.Enabled = true;
                ConnectServer();
            }else
            {
                NetButton.Enabled = false;
                //未与服务器连接前 client 为 null
                if (client != null)
                {
                    try
                    {
                        SendMessage("Logout," + nameText.Text);
                        isExit = true;
                        br.Close();
                        bw.Close();
                        client.Close();
                        //RemoveUserName(nameRightNow);   
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void ConnectServer()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry localHost = Dns.GetHostEntry(hostName);
                foreach (IPAddress ips in localHost.AddressList)
                {
                    if (ips.AddressFamily == AddressFamily.InterNetworkV6)
                        continue;
                    else
                    {
                        ServerIP = ips.ToString();
                        break;
                    }
                }
                port = 23478;

                try
                {
                    client = new TcpClient();
                    client.Connect(IPAddress.Parse(ServerIP), port);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("配置IP与端口失败，错误原因：" + ex.Message);
                Application.Exit();
            }

            //获取网络流
            try
            {
                NetworkStream networkStream = client.GetStream();
                br = new BinaryReader(networkStream);
                bw = new BinaryWriter(networkStream);
                SendMessage("Login," + nameText.Text);
            }catch(Exception ex)
            {
                MessageBox.Show("获取网络流失败，错误原因：" + ex.Message);
                Application.Exit();
            }
            //将网络流作为二进制读写对象

            nameRightNow = nameText.Text;
            Thread threadReceive = new Thread(new ThreadStart(ReceiveData));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }

        /// <summary>
        /// 向服务端发送消息
        /// </summary>
        /// <param name="message"></param>
        private void SendMessage(string message)
        {
            try
            {
                //将字符串写入网络流，此方法会自动附加字符串长度前缀
                bw.Write(message);
                bw.Flush();
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 处理服务器信息
        /// </summary>
        private void ReceiveData()
        {
            string receiveString = null;
            while (isExit == false)
            {
                try
                {
                    //从网络流中读出字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读出字符串
                    receiveString = br.ReadString();
                }
                catch
                {
                    if (isExit == false)
                    {
                        MessageBox.Show("与服务器失去连接");
                    }
                    break;
                }
                string[] splitString = receiveString.Split(',');
                string command = splitString[0].ToLower();
                switch (command)
                {
                    case "login":   //格式： login,用户名
                        AddOnline(splitString[1]);
                        break;
                    case "logout":  //格式： logout,用户名
                        RemoveUserName(splitString[1]);
                        break;
                    case "start":   //格式： start,用户名
                        DialogResult dr = MessageBox.Show(string.Format("{0}想要和你对弈！", splitString[1]),"",
                            MessageBoxButtons.OKCancel);
                        if(dr == DialogResult.OK)
                        {
                            SendMessage(string.Format("OK,{0}", splitString[1]));
                            model = Globe.PVP;

                            game.startGame(false); // 开始游戏

                            g_b.Clear(Globe.CLEAR); // 清理磁盘
                            g_p.Clear(Globe.CLEAR); // 清理落子标记
                            Draw(); // 重绘

                            waitForRival = false;
                            rivalName = splitString[1];
                            orderTogo = false;
                        }
                        else
                        {
                            SendMessage(string.Format("No,{0}", splitString[1]));
                        }
                        break;
                    case "ok":
                        model = Globe.PVP; //人人模式
                        game.startGame(true); // 开始游戏

                        g_b.Clear(Globe.CLEAR); // 清理磁盘
                        g_p.Clear(Globe.CLEAR); // 清理落子标记
                        Draw(); // 重绘
                        waitForRival = true;
                        orderTogo = true;
                        break;
                    case "no":
                        MessageBox.Show("对方拒绝和你对弈！");
                        break;
                    case "step":
                        //MessageBox.Show("sasasa");
                        int x = Int32.Parse(splitString[1]);
                        int y = Int32.Parse(splitString[2]);
                        game.changeUser();
                        int curUserNet = game.getCurUser(); // 获取当前行棋人

                        // 单机模式
                        // 放置棋子
                        if (game.placePieceNet(x, y))
                        {
                            DrawPiece(curUserNet, x, y);
                            // 绘制落子标记
                            g_p.Clear(Globe.CLEAR); // 重绘落子标记
                            g_p.DrawImage(Globe.SET, Globe.I2C(x), Globe.I2C(y)); // 绘制落子标记

                            Draw();
                            game.changeUser();
                            waitForRival = true;
                        }
                        break;
                    case "lose":    //格式： talk,用户名
                        MessageBox.Show("您输了!",
                                            "游戏结束",
                                            MessageBoxButtons.OK);

                        game.startGame((game.getCurUser() == Globe.USER_1) ? true : false);
                        g_b.Clear(Globe.CLEAR); // 清理磁盘
                        g_p.Clear(Globe.CLEAR); // 清理落子标记
                        Draw(); // 重绘
                        waitForRival = false;
                        break;
                    default:
                        //AddTalkMessage("什么意思啊：" + receiveString);
                        break;
                }
            }
        }


        private delegate void AddOnlineDelegate(string message);
        /// <summary>
        /// 在在线框（lst_Online)中添加其他客户端信息
        /// </summary>
        /// <param name="userName"></param>
        private void AddOnline(string userName)
        {
            if (matchPlayerlistBox.InvokeRequired)
            {
                AddOnlineDelegate d = new AddOnlineDelegate(AddOnline);
                matchPlayerlistBox.Invoke(d, new object[] { userName });
            }
            else
            {
                matchPlayerlistBox.Items.Add(userName);
                matchPlayerlistBox.SelectedIndex = matchPlayerlistBox.Items.Count - 1;
                matchPlayerlistBox.ClearSelected();
            }
        }

        private delegate void RemoveUserNameDelegate(string userName);
        /// <summary>
        /// 在在线框(lst_Online)中移除不在线的客户端信息
        /// </summary>
        /// <param name="userName"></param>
        private void RemoveUserName(string userName)
        {
            if (matchPlayerlistBox.InvokeRequired)
            {
                RemoveUserNameDelegate d = new RemoveUserNameDelegate(RemoveUserName);
                matchPlayerlistBox.Invoke(d, userName);
            }
            else
            {
                matchPlayerlistBox.Items.Remove(userName);
                matchPlayerlistBox.SelectedIndex = matchPlayerlistBox.Items.Count - 1;
                matchPlayerlistBox.ClearSelected();
            }
        }

        private void gameUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //未与服务器连接前 client 为 null
            if (client != null)
            {
                try
                {
                    SendMessage("Logout," + nameText.Text);
                    isExit = true;
                    br.Close();
                    bw.Close();
                    client.Close();
                }
                catch
                {
                }
            }
        }

        private void NetButton_Click(object sender, EventArgs e)
        {
            if (matchPlayerlistBox.SelectedIndex != -1)
            {
                if (matchPlayerlistBox.SelectedItem.ToString() == nameRightNow)
                    MessageBox.Show("请不要选择和自己游戏！");
                else
                {
                    SendMessage("Start," + matchPlayerlistBox.SelectedItem.ToString());
                    rivalName = matchPlayerlistBox.SelectedItem.ToString();
                }
            }
            else
            {
                MessageBox.Show("请先在【当前在线】中选择一个进行游戏");
            }
        }
    }
}
