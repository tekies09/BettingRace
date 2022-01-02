using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            AysncEchoServer().Wait();
        }
        async static Task AysncEchoServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 6110);   //6110 포트로 열었음
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
            string sendmessage;
            TcpClient tc = (TcpClient)o;

            int MAX_SIZE = 1024;  // 사이즈
            NetworkStream stream = tc.GetStream(); //연결됨 stream을 가져옴 

            // 비동기 수신            
            var buff = new byte[MAX_SIZE];
            var nbytes = await stream.ReadAsync(buff, 0, buff.Length).ConfigureAwait(false); // 클라이언트에서 보낸걸 여기에 담음
            if (nbytes > 0)
            {
                string msg = Encoding.GetEncoding("euc-kr").GetString(buff, 0, nbytes); // 전송된걸 다시 문자로 바꿔줌
                Console.WriteLine($"{msg} at {DateTime.Now}");  // 메세지와 보낸시간표시
                char[] sp = "☆".ToCharArray(); //명령어 구분기호
                char[] sp2 = "★".ToCharArray(); //자료 구분 기호
                string[] result = msg.Split(sp);   //
                string[] result2 = result[1].Split(sp2);

                switch (result[0])
                {
                    case "unique":
                        sendmessage = DB.check(result2[0], "", "unique");
                        SendMessage(tc, sendmessage);
                        break;
                    case "login":
                        sendmessage = DB.check(result2[0], result2[1], "login");
                        SendMessage(tc, sendmessage);
                        break;
                    case "signup":
                        sendmessage = DB.signup(result2[0], result2[1], result2[2], result2[3]);
                        SendMessage(tc, sendmessage);
                        break;
                    case "nroom":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        sendmessage = Room.Roomcreate(int.Parse(result2[0]), result2[1], result2[2], int.Parse(result2[3]), int.Parse(result2[4]));
                        SendMessage(Connection.ConnectionReturn(int.Parse(result2[0])), sendmessage); //커넥션에서 가져와서 통신해줌 
                        break;
                    case "join":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        sendmessage = Room.Roomjoin(int.Parse(result2[0]), int.Parse(result2[1]), result2[2]);
                        SendMessage(Connection.ConnectionReturn(int.Parse(result2[0])), sendmessage); //커넥션에서 가져와서 통신해줌 
                        // SendMessage(tc, sendmessage);
                        break;
                    case "rlist":
                        sendmessage = Room.RoomList(int.Parse(result2[0]));
                        SendMessage(tc, sendmessage);
                        break;
                    case "exit":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        Room.Roomexit(int.Parse(result2[0]), int.Parse(result2[1]));
                        break;
                    case "rand":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        int Rnumber = DB.RandomRoomNumber(int.Parse(result2[1]));
                            sendmessage = Room.Roomjoin(int.Parse(result2[0]), Rnumber, "");
                            SendMessage(Connection.ConnectionReturn(int.Parse(result2[0])), sendmessage);
                        break;
                    case "rank":
                        sendmessage = DB.RankList(int.Parse(result2[0]));
                        SendMessage(tc, sendmessage);
                        break;
                    case "myrank":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        sendmessage = DB.MyRank(int.Parse(result2[0]), int.Parse(result2[1]));
                        SendMessage(Connection.ConnectionReturn(int.Parse(result2[0])), sendmessage);
                        break;
                    case "racestart":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        Race.RaceStart(int.Parse(result2[0]), int.Parse(result2[1]));
                        break;
                    case "tokengiveup":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        Race.tokengiveup(int.Parse(result2[0]), int.Parse(result2[1]));
                        break;
                    case "pulltoken":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        Race.tokenpull(int.Parse(result2[0]), int.Parse(result2[1]));
                        break;
                    case "statoken":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        Race.tokendispose(int.Parse(result2[0]), int.Parse(result2[1]), result2[2]);
                        break;
                    case "ready":
                        Connection.ConnectStart(tc, int.Parse(result2[0]));
                        Room.userReady(int.Parse(result2[0]), int.Parse(result2[1]));
                        break;
                    default:
                        SendMessage(tc, msg);
                        break;
                }
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
        internal static void SendMessageRoom(int RoomNumber, string sendmessage)
        {
            object o;
            char[] sp2 = "★".ToCharArray();
            string userList = Room.FindRoomInfo(RoomNumber, "usersConnect");
            string[] user = userList.Split(sp2);
            foreach(var i in user)
            {
                if (!(i.Equals("")))
                {
                    SendMessage(Connection.ConnectionReturn(int.Parse(i)), sendmessage);
                }
               
            }
        }


    }

}
