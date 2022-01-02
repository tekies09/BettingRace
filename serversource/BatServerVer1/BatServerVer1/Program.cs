using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace BatServerVer1
{
    class Program
    {
        internal static List<TcpClient> userList = new List<TcpClient>();
        internal static char[] sp = "☆".ToCharArray(); //명령어 구분기호
        internal static char[] sp2 = "★".ToCharArray(); //자료 구분 기호
        static void Main(string[] args)
        {
            BatServer().Wait();
        }
        async static Task BatServer()
        {
            Console.WriteLine("server on");
            TcpListener listener = new TcpListener(IPAddress.Any,5555);   // 5555 포트로 열었음
            listener.Start();   //리스너 시작 
            while (true)
            {
                // 비동기 Accept                
                TcpClient tc = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                // 새 쓰레드에서 처리
                Task.Factory.StartNew(AsyncTcpProcess, tc);
            }
        }
        async static void AsyncTcpProcess(object o)
        {
            TcpClient tc = (TcpClient)o;
            userList.Add(tc);
            Console.WriteLine("new client here");
            int MAX_SIZE = 1024;  // 사이즈
            NetworkStream stream = tc.GetStream();
            var buff = new byte[MAX_SIZE];
            try
            {
                var nbytes = await stream.ReadAsync(buff, 0, buff.Length).ConfigureAwait(false); // 클라이언트에서 보낸걸 여기에 담음
                if (nbytes > 0)
                {
                    string msg = Encoding.GetEncoding("euc-kr").GetString(buff, 0, nbytes); // 전송된걸 다시 문자로 바꿔줌
                    Console.WriteLine($"{msg} at {DateTime.Now}");  // 메세지와 보낸시간표시
                    ClientCommunicate(tc, msg);
                }
                else
                {
                    userList.Remove(tc);
                    Console.WriteLine("client disconnect");
                }
            }
            catch(Exception e)
            {
                userList.Remove(tc);
                Console.WriteLine("client disconnect1");
            }

        }
        internal static void SendMessage(object o, string sendmessage)
        {
            int MAX_SIZE = 1024;
            var buff = new byte[MAX_SIZE];
            TcpClient tc = (TcpClient)o;
            NetworkStream stream = tc.GetStream();
            Console.WriteLine($"{sendmessage} 보냄 at {DateTime.Now}");
            buff = Encoding.GetEncoding("euc-kr").GetBytes(sendmessage);
            stream.Write(buff, 0, buff.Length);
            stream.Flush();
        }
        internal static void ClientCommunicate(object o ,string Message)
        {
            TcpClient tc = (TcpClient)o;
            string[] result = Message.Split(sp);
            // string[] result2 = result[1].Split(sp2);

            switch (result[0])
            {
                case "Check":
                    Checkid(tc,result[1]);
                    break;
                case "Login":
                    Login(tc, result[1]);
                    break;
                case "Register":
                    Register(tc, result[1]);
                    break;
                case "Ranking":
                    Ranking(tc);
                    break;
                case "MyRank":
                    MyRank(tc, int.Parse(result[1]));
                    break;
                case "AllChat":
                    Allchat(result[1]);
                    break;

            }
        }
        internal static void Checkid(object o,string id)
        {
            Console.WriteLine("Checkid start");
            TcpClient tc = (TcpClient)o;
            string result = "";
            result = DB.Checkidoverlap(id);
            Console.WriteLine("Checkid end");
            SendMessage(tc,result);
        }
        internal static void Login(object o,string result1)
        {
            TcpClient tc = (TcpClient)o;
            string[] result2 = result1.Split(sp2);
            string result = "";
            result = DB.Login(result2[0], result2[1]);
            SendMessage(tc, result);
        }
        internal static void Register(object o,string result1)
        {
            TcpClient tc = (TcpClient)o;
            string[] result2 = result1.Split(sp2);
            string result = "";
            result = DB.Register(result2[0], result2[1]);
            SendMessage(tc, result);
        }
        internal static void Ranking(object o)
        {
            TcpClient tc = (TcpClient)o;
            string result = "";
            result = DB.RankTop5();
            SendMessage(tc, result);
        }
        internal static void MyRank(object o , int u_num)
        {
            TcpClient tc = (TcpClient)o;
            string result = "";
            result = DB.MyRank(u_num);
            SendMessage(tc, result);
        }
        internal static void Allchat(string result1)
        {
            string[] result2 = result1.Split(sp2);
            string result = result2[0] + " : " + result2[1];
            foreach (var o in userList)
            {
                SendMessage(o, result);
            }
        }
    }
}
