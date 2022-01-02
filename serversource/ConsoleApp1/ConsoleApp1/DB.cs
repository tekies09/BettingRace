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
    class DB
    {
        internal static string strConn = "Server=localhost;Database=boardgame;Uid=root;Pwd=dada1023;"; //기본 db접속 주소 

        static string sp2 = "★"; // column간 기준 구분 반환
        static string sp = "☆";  // 반환할 row 가 여러개면 여러개를 구분해주는 기준임
        static char c_sp2 = '★'; //split 용도
        static char c_sp = '☆';   // split 용도


        internal static String signup(string id, string password, string phone, string gender)
        {
            int usernumber = NumberCreate("user");
            int connectnumber = NumberCreate("connect");
            string query = "insert into userinfo values (" + usernumber + ", '" + id + "'); insert into userconnect(connectnumber,usernumber,u_pw,u_phone,u_gender) values (" +
                connectnumber+","+usernumber+ ",HEX(AES_ENCRYPT('" + password + "','o')),'" + phone+"','"+gender+ "'); insert into result values (" + usernumber + ",1,0,0,0); " +
                "insert into result values (" + usernumber + ",2,0,0,0);"; //가입시 userinfo 에 한번넣어주고 userconnect에 연결해서 넣고 result에 기본 게임승률 정보를 저장함
            string result = UsingQuery(query, "insert", 0);
            return result;
        }

        internal static int RandomRoomNumber(int GameNumber)
        {
            int roomnumber = 0;
            string query = "select roomnumber from roomlist where roompw ='' and gamenumber = "+GameNumber+" order by rand() limit 1;";
            string result = UsingQuery(query, "select", 1);
            if (!(result==""))
            {
                roomnumber = int.Parse(result.Replace(sp2, "").Replace(sp, ""));
            }
            return roomnumber;
        }
        internal static string RankList(int Gnumber)
        {
            string result = "";
            string strConn = "Server=localhost;Database=boardgame;Uid=root;Pwd=dada1023;";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "select usernumber,bellpoint,gamewin,gamelose from result where GameNumber = " + Gnumber+" order by bellpoint desc ,gamewin desc limit 5;";   // sql 쿼리문
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    for (int k = 0; k < 4; k++)
                        if (k == 0)
                        {
                            result = result + findUserNickNameByUserNumber(int.Parse(rdr[k].ToString()))+sp2;
                        }
                        else if(k==3)
                        {
                            result = result + rdr[k].ToString() + sp;
                        }
                        else
                        {
                            result = result + rdr[k].ToString() + sp2;
                        }
                }
            }
            return result;
        }
        internal static string MyRank(int ConnectNumber, int Gnumber)
        {
            int UserNumber = int.Parse(findUserNumberByUserConnect(ConnectNumber));
            string query = "select bellpoint,gamewin,gamelose from result where UserNumber = " + UserNumber + " and GameNumber = " + Gnumber + ";";
            string result = UsingQuery(query, "select", 3);
            result = result.Replace(sp, "");
            return result;
        }

        internal static String check(string id,string password,string type) //아이디중복검사 & 로그인을 처리 게임내에서 제공하는 회원가입, 로그인으로 하는경우 연동 x
        {
            string result = "";
            string usernumber = findUserNumberByUserNickName(id);
            if ((usernumber=="")) // 해당 아이디가 DB에 없는 경우
            {
                if (!(type.Equals("login"))) //중복검사의 경우
                {
                    result = "unique"; 
                }
                else
                {
                    result = "loginFail";
                }
            }
            else   //해당 아이디가 DB에 있음 
            {
                if (type.Equals("login")){  //로그인인 경우
                    string query = "select u_phone,u_gender,u_regdate from userconnect where usernumber = " + usernumber + " and u_pw =  HEX(AES_ENCRYPT('" + password + "','o'));";
                result = UsingQuery(query, "select", 3);
                if (!(result=="")) // 로그인이 된 경우 -- 앱에서 제공하는로그인으로
                {
                    string connectnumber =findConnectNumberByUserNickName(id);
                        result = id + sp2 + password + sp2 + usernumber + sp2 + connectnumber + sp2 + result.Replace(sp, ""); //
                    }
                else
                {
                    result = "loginFail";
                }
                }
                else
                {
                    result = "noneunique";
                }
            }
            return result;
        }

        internal static String findUserNickNameByUserConnect(int UserConnect)
        {
            string usernumber = findUserNumberByUserConnect(UserConnect);
            string query = "select usernickname from userinfo where usernumber = " + usernumber + " ;";
            string usernickname = UsingQuery(query, "select", 1);
            usernickname = usernickname.Replace(sp2, "").Replace(sp, ""); //반환된 별의 값 제거
            return usernickname;
        }
        internal static String findUserNickNameByUserNumber(int UserNumber)
        {
            string query = "select usernickname from userinfo where usernumber = " + UserNumber + " ;";
            string usernickname = UsingQuery(query, "select", 1);
            usernickname = usernickname.Replace(sp2, "").Replace(sp, ""); //반환된 별의 값 제거
            return usernickname;
        }
        internal static String findUserNumberByUserConnect(int UserConnect)
        {
            string query = "select usernumber from userconnect where connectnumber = " + UserConnect + " ;";
            string usernumber = UsingQuery(query, "select", 1);
            usernumber = usernumber.Replace(sp2, "").Replace(sp, ""); //반환된 별의 값 제거
            return usernumber;
        }
        internal static String findUserNumberByUserNickName(string UserNickName)
        {
            string query = "select usernumber from userinfo where usernickname = '" + UserNickName + "' ;";
            string usernumber = UsingQuery(query, "select", 1);
            usernumber = usernumber.Replace(sp2, "").Replace(sp, ""); //반환된 별의 값 제거
            return usernumber;
        }
        internal static String findGameResultByUserNumber(int UserNumber,int GameNumber)
        {
            string query = "select bellpoint,gamewin,gamelose from result where usernumber = " + UserNumber + " and gamenumber = " + GameNumber + " ;";
            string usernumber = UsingQuery(query, "select", 1);
            usernumber = usernumber.Replace(sp2, "").Replace(sp, ""); //반환된 별의 값 제거
            return usernumber;
        }
        internal static String findConnectNumberByUserNickName(string UserNickName)
        {
            string usernumber = findUserNumberByUserNickName(UserNickName);
            usernumber = usernumber.Replace(sp2, "").Replace(sp, "");
            string query = "select connectnumber from userconnect where usernumber = " + usernumber + ";";
            string connectnumber = UsingQuery(query, "select", 1);                                    // 추후에 로그인후 계정 연동을 추가적으로 하는게 가능하다면 이파트를 수정
            connectnumber = connectnumber.Replace(sp2, "").Replace(sp, "");
            return connectnumber;

        }

        internal static int NumberCreate(string type) //유저넘버 , 유저코넥트넘버 , 방넘버를 만들어주는 기능 
        {
            int number = 1;
            string query = "";
            switch (type)
            {
                case "user":
                    query = "select usernumber from userinfo order by usernumber;";
                    break;
                case "connect":
                    query = "select connectnumber from userconnect order by connectnumber;";
                    break;
                case "room":
                    query = "select roomnumber from roomlist order by roomnumber;";
                    break;
            }
            string numberlist = UsingQuery(query, "select", 1);
            if (!(numberlist==""))
            {
                numberlist = numberlist.Replace(sp, "");
                string[] numbers = numberlist.Split('★');
                foreach (var i in numbers)
                {
                    if (!i.Equals(number.ToString()))
                        break;
                    number = number + 1;
                }
            }
                return number;
        }
        internal static string UsingQuery (string sql, string type, int returnColumn)
        {
            string result ="";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr;

                    if (type.Equals("select")) { 
                             rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                if (!rdr.HasRows) //조회 결과가 없을 경우
                                {
                                    result = "None";
                                    break;
                                }
                                for (int i = 0; i < returnColumn; i++)
                                    result = result + rdr[i].ToString() +sp2;
                                result = result + sp;
                            }
                            rdr.Close();
                    }
                    else { 
                            cmd.ExecuteNonQuery();
                            result = "success";
                    }
                    conn.Close();
                }  
                }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return "fail";
            }
            return result;
        }
    }
}
