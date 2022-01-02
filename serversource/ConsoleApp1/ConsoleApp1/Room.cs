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
    class Room
    {
        internal static List<RoomInfo> roomList = new List<RoomInfo>(); // 방에대한 정보와 접속유저들을 불러와줌
        static string sp2 = "★"; // column간 기준 구분 반환
        static string sp = "☆";  // 반환할 row 가 여러개면 여러개를 구분해주는 기준임
        static char c_sp2 = '★'; //split 용도
        static char c_sp = '☆';   // split 용도
        internal static int returnReadyusers(int Rnumber)
        {
            int count = 0;
            foreach(var i in roomList)
            {
                if (i.RoomNumber == Rnumber)
                {
                    count = i.Ready.Count;
                }
            }
            return count;
        }
        internal static void userReady(int connectnumber,int Rnumber)
        {
          int check =  userReadySetting(connectnumber, Rnumber);
            string username = DB.findUserNickNameByUserConnect(connectnumber);
            string result = "";
            if (check == 0)
            {
                result =  "ready" + sp2 + username;
            }
            else
            {
                result = "unready" + sp2 + username;
            }
            Program.SendMessageRoom(Rnumber, result);
        }
        internal static int userReadySetting(int connectnumber,int Rnumber)
        {
            int check = 0;
            foreach(var i in roomList)
            {
                if (i.RoomNumber == Rnumber)
                {
                    foreach(var k in i.Ready)
                    {
                        if (k == connectnumber)
                            check = 1;
                    }
                    if (check == 0)
                    {
                        i.Ready.Add(connectnumber);
                    }
                    else
                    {
                        i.Ready.Remove(connectnumber);
                    }
                }
            }
            return check;
        }
        internal static string Roomcreate(int connectnumber, string Rname, string Rpw, int limitcount, int Gnumber) // 방 생성후 DB반영 및 roomList에 정보저장
        {
            string Uname = DB.findUserNickNameByUserConnect(connectnumber);
            int newroomNumber = DB.NumberCreate("room");
            string query = "  insert into roomlist values ( " + newroomNumber + " , '" + Rname + "' , '" + Rpw + "' , " + limitcount + " , 'wait' , " + Gnumber + ");" +
                        "update userconnect set roomNumber = " + newroomNumber + " where connectnumber =" + connectnumber + ";";
            string result =DB.UsingQuery(query, "insert", 0);
                
            NewRoomListAdd(newroomNumber, Gnumber, Rname, limitcount, Rpw, connectnumber, "wait", Uname);
            return newroomNumber + sp2 + Rname + sp2 + Rpw + sp2 + limitcount + sp2 + Gnumber;
        }

        internal static void Roomexit(int Connectnumber, int RoomNumber)
        {
            string query = "update userconnect set RoomNumber =null " + " where Connectnumber = " + Connectnumber + ";";
            string result = DB.UsingQuery(query, "update", 0);
            string sendmessage = "";
            string userList = FindRoomInfo(RoomNumber, "usersConnect"); //현재 방리스트에 있는 유저를 가져옴 
            string[] user = userList.Split(c_sp2); // 나눠서 0번자리에는 방장이 오게됨
            if (Connectnumber == int.Parse(user[0])) //방장이 나가는경우
            {
                if (!(user[1].Equals("")))
                { //혼자남아서 나가는 경우를 제외한것 
                    sendmessage = "out" + sp2 + DB.findUserNickNameByUserConnect(Connectnumber) + sp2 + "master" + sp2 + DB.findUserNickNameByUserConnect(int.Parse(user[1]));
                }
                else
                { //본인이 혼자 남아서 나가는경우
                    sendmessage = "out" + sp2 + DB.findUserNickNameByUserConnect(Connectnumber);
                }
            }
            else
            {
                sendmessage = "out" + sp2 + DB.findUserNickNameByUserConnect(Connectnumber);

            }
            Program.SendMessageRoom(RoomNumber, sendmessage);

            RoomListExit(RoomNumber, Connectnumber);
            if (RoomPlayerCount(RoomNumber) == 0) //방이 없어지는경우
            {
                RoomDelete(RoomNumber);
                int x = RoomIndex(RoomNumber); // 
                roomList.RemoveAt(x);
            }
        }

        internal static int RoomIndex(int RNumber) //해당방의 인덱스찾기
        {
            int k = 0;
            foreach (RoomInfo a in roomList)
            {
                if (a.RoomNumber == RNumber)
                {
                    break;
                }
                k = k + 1;
            }
            return k;
        }


        internal static void RoomDelete(int RoomNumber)
        {
            string query = "delete from roomlist where RoomNumber = " + RoomNumber + ";";
            string result = DB.UsingQuery(query, "delete", 0);
        }


        internal static void RoomListExit(int Rnumber, int ConnectNumber)
        {
            foreach (var a in roomList)
            {
                if (a.RoomNumber == Rnumber)
                    a.user.Remove(ConnectNumber);
            }
        }




        internal static string Roomjoin(int connectnumber, int RoomNumber, string RoomPassword)
        {
            string result;
            string result2;
            string query;
            if (!RoomEnableJoinCheck(RoomNumber, RoomPassword))
            { //방이 꽉찾거나 없어졌을때 혹은 비밀번호가 맞지않을때
                result = "unable";
            }
            else
            {
                query = "update userconnect set roomnumber = " + RoomNumber + " where connectnumber =" + connectnumber + ";";
                result = DB.UsingQuery(query, "update", 0);
                string Uname = DB.findUserNickNameByUserConnect(connectnumber);

                result = RoomNumber + sp2 +FindRoomInfo(RoomNumber,"all"); // 들어가기전 방에 대한 접속 정보를 result에넣어줌
                string sendmessage = "in" + sp2 + Uname;
                Program.SendMessageRoom(RoomNumber, sendmessage);
                RoomJoinAdd(RoomNumber, connectnumber, Uname);
            }
            return result;
        }

        internal static bool RoomEnableJoinCheck(int Roomnumber, string RoomPassword)
        {
            int DBPlayerCount; // 방을 만들때 인원제한을 한값 
            int PlayerCount = RoomPlayerCount(Roomnumber); //실제 플레이 인원수
            string query = "select RoomCount from roomlist where RoomNumber = " + Roomnumber + " and RoomPw ='" + RoomPassword + "';";
            string result = DB.UsingQuery(query, "select", 1);
            result = result.Replace(sp2, "").Replace(sp, "");
            if (result=="")
            {
                DBPlayerCount = 0;
            }
            else
            {
                DBPlayerCount = int.Parse(result);
            }
            if ((DBPlayerCount == PlayerCount) || (DBPlayerCount) == 0)
                return false; //방이 없거나 방이 꽉찾으면 못들어감 
            else
                return true;
        }

        internal static int RoomPlayerCount(int Rnumber)
        {
            string query = "select count(ConnectNumber) from userconnect where roomnumber=" + Rnumber + ";";
            string result = DB.UsingQuery(query, "select", 1);
            result = result.Replace(sp2, "").Replace(sp, "");
            if (result=="")
            {
                return 0;
            }
            else
            {
                return int.Parse(result);
            }
        }

        internal static string RoomList(int Gnumber) //중간에 방인원수를 넣어야되서 이것만 여기서 읽어오는기능
        {
            string strConn = "Server=localhost;Database=boardgame;Uid=root;Pwd=dada1023;";
            string result = "";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "select * from roomlist where GameNumber=" + Gnumber + ";";   // sql 쿼리문
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    for (int k = 0; k < 6; k++)
                        if (k != 5)
                        {
                            result = result + rdr[k].ToString() + sp2;
                        }
                        else
                        {
                            result = result + rdr[k].ToString() + sp2 + RoomPlayerCount(int.Parse(rdr[0].ToString())) + sp;
                        }
                }
                if (!rdr.HasRows)
                {
                    result = "NotCreated";
                }
                rdr.Close();
                conn.Close();
            }
            return result;
        }











        internal static void NewRoomListAdd(int Rnumber, int Gnumber, string Rname, int limitcount, string Rpw, int connectnumber, string RoomStatus, string Uname)
        {
            RoomInfo createInfo = new RoomInfo();
            createInfo.RoomNumber = Rnumber;
            createInfo.GameNumber = Gnumber;
            createInfo.limitcount = limitcount;
            createInfo.RoomName = Rname;
            createInfo.RoomPw = Rpw;
            createInfo.user.Add(connectnumber, Uname);
            createInfo.RoomStatus = RoomStatus;
            roomList.Add(createInfo);
        }
        internal static void RoomJoinAdd(int Rnumber, int connectnumber, string Uname)
        {
            foreach (RoomInfo a in roomList)
            {
                if (a.RoomNumber == Rnumber)
                    a.user.Add(connectnumber, Uname);
            }
        }
        internal static string FindRoomInfo(int Rnumber, string type)
        {
            string result = "";
            switch (type)
            {
                case "all":
                    foreach (RoomInfo a in roomList)
                    {
                        if (a.RoomNumber == Rnumber)
                        {
                            result = a.RoomName + sp2 + a.RoomPw + sp2 + a.limitcount + sp2 + a.GameNumber; // all 커맨드시 방이름 ,비밀번호 , 방인원제한 , 게임종류넘버
                            foreach (var i in a.user.Values)
                                result = result + sp2 + i;
                        }
                    }
                    break;
                case "usersConnect":
                    foreach (RoomInfo a in roomList)
                    {
                        if (a.RoomNumber == Rnumber)
                        {
                            foreach (var i in a.user.Keys)
                                result = result + i + sp2;
                        }
                    }
                    break;
            }
            return result;
        }
    }
    class RoomInfo
    {
        public int RoomNumber;
        public int GameNumber;
        public string RoomName;
        public string RoomPw;
        public string RoomStatus;
        public int limitcount;
        public Dictionary<int, string> user = new Dictionary<int, string>();  //커넥션 넘버와 usernickname
        public List<int> Ready = new List<int>();
    }
}
