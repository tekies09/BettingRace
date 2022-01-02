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
    class Connection
    {
        internal static Dictionary<int, object> ConnectList = new Dictionary<int, object>();

        internal static void ConnectStart(object o, int ConnectUserNumber)
        {
            bool changeCheck = false;
            for(int i=0; i < ConnectList.Count; i++)
            {
                if (ConnectList.Keys.ToList()[i] == ConnectUserNumber)
                {
                    ConnectList[ConnectUserNumber] = o;  // 검색을 통하여 기존에 접속에 다시 연결하는경우 해당 Client로 교체를 진행해줌 
                    changeCheck = true;
                }
            }
            if (!changeCheck)
            {
                ConnectList.Add(ConnectUserNumber, o);  // 검색을 통하여 기존에 접속이 아니라 신규접속일 경우 딕셔너리에 추가를 해줌
            }
        }

        internal static void ConnectEnd(object o, int ConnectUserNumber)
        {
            ConnectList.Remove(ConnectUserNumber);  //게임 종료시에 이거를 호출해서 해주면댐
        }

        internal static object ConnectionReturn(int ConnectUserNumber)
        {
            return ConnectList[ConnectUserNumber];
        }

    }
}
