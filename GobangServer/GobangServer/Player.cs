using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace GobangServer
{
    /// <summary>
    /// 连接服务器的客户端玩家们
    /// </summary>
    class Player
    {
        
        public TcpClient client { get; private set; }
        public BinaryReader br { get; private set; }
        public BinaryWriter bw { get; private set; }
        public string userName { get; set; }

        public Player(TcpClient client)
        {
            this.client = client;
            NetworkStream networkStream = client.GetStream();
            br = new BinaryReader(networkStream);
            bw = new BinaryWriter(networkStream);                                                                                                                                                                                                                   
        }

        public void Close()
        {
            br.Close();
            bw.Close();
            client.Close();
        }
    }
}
