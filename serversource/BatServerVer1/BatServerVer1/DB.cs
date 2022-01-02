using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BatServerVer1
{
    class DB
    {
        internal static string strConn = "Server=betracedb.cmdxiywkcos2.ap-northeast-2.rds.amazonaws.com;Database=BETDB;Uid=tekies0909;Pwd=moon0909;"; //기본 db접속 주소 
        internal static string Login(string id,string pw)
        {
           string result = "Login☆";
           string sql= "select u_num from userinfo where u_id= '" + id + "' and u_password =MD5('" + pw + "') ;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) //조회 결과가 없을 경우
                        {
                            result = result + "Fail";
                            break;
                        }
                        result = result + rdr[0].ToString();
                    }
                    rdr.Close();
                    conn.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
        internal static string RankTop5()
        {
            string sp2 = "★"; // column간 기준 구분 반환
            string sp = "☆";
            string result= "Ranking☆";
            string sql = "select* from result order by b_point desc ,g_win desc limit 5;";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!rdr.HasRows) //조회 결과가 없을 경우
                    {
                        break;
                    }
                    for (int k = 0; k < 4; k++)
                    {
                        if (k == 0)
                        {
                            result = result + Findid(int.Parse(rdr[k].ToString())) + sp2;
                        }
                        else if (k == 3)
                        {
                            result = result + rdr[k].ToString() + sp;
                        }
                        else
                        {
                            result = result + rdr[k].ToString() + sp2;
                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return result;
        }
        internal static string MyRank(int u_num)
        {
            string sp2 = "★"; // column간 기준 구분 반환
            string sp = "☆";
            string result = "MyRank☆";
            string sql = "select * from result where u_num = "+u_num+";";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!rdr.HasRows) //조회 결과가 없을 경우
                    {
                        break;
                    }
                    for (int k = 0; k < 4; k++)
                    {
                        if (k == 0)
                        {
                            result = result + Findid(int.Parse(rdr[k].ToString())) + sp2;
                        }
                        else
                        {
                            result = result + rdr[k].ToString() + sp2;
                        }
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return result;
        }
        internal static string Findid(int u_num)
        {
            string id = "";
            string sql = "select u_id from userinfo where u_num = " + u_num + ";";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (!rdr.HasRows) //조회 결과가 없을 경우
                    {
                        id = "None";
                        break;
                    }
                    id = rdr[0].ToString();
                }
                rdr.Close();
                conn.Close();
            }
            return id;
        }
        internal static string Findu_num(string id)
        {
            string u_num="";
            string sql = "select u_num from userinfo where u_id = '" + id + "';";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (!rdr.HasRows) //조회 결과가 없을 경우
                        {
                            u_num = "None";
                            break;
                        }
                        u_num = rdr[0].ToString();
                    }
                    rdr.Close();
                conn.Close();
            }
            return u_num;
        }
        internal static string Checkidoverlap(string id)
        {
            string result = "Check☆";
            string u_num = Findu_num(id);
            if (u_num.Equals("None"))
            {
                result = result + "Success";
            }
            else
            {
                result = result + "Fail";
            }
            return result;
        }
        internal static string Register(string id,string pw)
        {
            string result = "Register☆";
            string sql = "insert into userinfo(u_id, u_password)values('" + id + "', md5('" + pw + "'));";
            result = result + UsingQuery(sql);
            return result;
        }
        internal static string GameResultUpdate()
        {
            string result = "GameResultUpdate☆";
            string sql = "";
            result = result + UsingQuery(sql);
            return result;
        }
        internal static string UsingQuery(string sql)
        {
            string result = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        result = "Success";
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Fail";
            }
            return result;
        }

    }
}
