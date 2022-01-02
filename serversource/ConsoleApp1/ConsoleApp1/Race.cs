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

    class GameResult
    {
        public int RoomNumber;
        public List<int> token = new List<int>(); // 사용되는 토큰 개수 player수로 초기화
        public string turn; // 플레이어들의 차례 
        public string nextturn;
        public List<int> tokenset = new List<int>();
        public List<int> section1 = new List<int>();
        public List<int> section2 = new List<int>();              // 토큰 코드    0   1       2      3       4 
        public List<int> section3 = new List<int>();            // 배열순서는 좀비,미이라,구미호,뱀파이어,강시순임
        public bool TokenGiveUp = false; //토큰 포기의 유무 
        public int ZombieResult = 0; //0 총 게임진행중 이동된칸    //13이 되면 게임을 종료할거임
        public int MummyResult = 0;  //1
        public int FoxResult = 0;    //2
        public int VampireResult = 0; //3
        public int CGhostResult = 0;  //4
        public Dictionary<string, string> user = new Dictionary<string, string>(); // 키는 유저커넥트 , 값은 응원하는 토큰 1★2 이런식으로 저장
    }
    class Race
    {
        internal static void gameready() { }
        internal static List<GameResult> gamelist = new List<GameResult>();
        internal static void RaceStart(int ConnectNumber , int Rnumber)
        {
            int readycount = Room.returnReadyusers(Rnumber);
            int count = Room.RoomPlayerCount(Rnumber);
            if((count < 4) ||(readycount!=(count-1)))
            {
                object master = Connection.ConnectionReturn(ConnectNumber);
                Program.SendMessage(master, "fail");
            }
            else
            {
                Program.SendMessageRoom(Rnumber, "GameStart");
                RoomSetting(Rnumber, count);
                TokenSending(Rnumber);
                RoundStart(ConnectNumber, Rnumber);
                RoomstatusUpdate(Rnumber, "start");
            }
        }

        internal static void RoomstatusUpdate(int Rnumber, string type)
        {
            string query = "update roomlist set roomstatus = '"+type+"' where roomnumber = " + Rnumber + ";";
            string result;
            result = DB.UsingQuery(query, "update", 0);
            foreach(var i in Room.roomList)
            {
                if (i.RoomNumber == Rnumber)
                {
                    i.RoomStatus = type;
                }
            }
        }
        internal static void TurnEnd(int ConnectNumber, int Rnumber) //last면 true 반환
        {
            string nextuser = "";
            int nextuserconnect;
            int turn = TurnbyConnect(ConnectNumber, Rnumber);
            int players = Room.RoomPlayerCount(Rnumber);
            int willlastuserconnect = ConnectbyTurn(players - 1, Rnumber);
            if (turn == players)
            {
                if (!checkgiveup(Rnumber))
                {
                    nextturnsetting(willlastuserconnect, Rnumber);
                }                
                RoundEnd(Rnumber);
            }
            else
            {
                nextuserconnect = ConnectbyTurn(turn + 1, Rnumber);
                nextuser = DB.findUserNickNameByUserConnect(nextuserconnect);
                Program.SendMessageRoom(Rnumber, "turn★" + nextuser);
            }
        }

        internal static bool checkgiveup(int Rnumber)
        {
            bool result=false;
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    result = i.TokenGiveUp;
                }
            }
            return result;
        }
        internal static void tokengiveup(int ConnectNumber , int Rnumber)
        {
            
            string user = DB.findUserNickNameByUserConnect(ConnectNumber);
            string result = "giveup★"+user;
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                    i.TokenGiveUp = true;
            }
            Program.SendMessageRoom(Rnumber, result);
            nextturnsetting(ConnectNumber, Rnumber);
            int turn = TurnbyConnect(ConnectNumber, Rnumber);
            int players = Room.RoomPlayerCount(Rnumber);
            TurnEnd(ConnectNumber, Rnumber);
        }

        internal static void nextturnsetting(int ConnectNumber, int Rnumber)
        {
            string result = "";
            int count = 0;
            int calcount = 0;
            //매개변수로 마지막 차례가 될 connectnumber를 받음
            char[] sp2 = "★".ToCharArray();
            string rt = RoomTurn(Rnumber);
            String[] users = rt.Split(sp2);
            foreach(var user in users)
            {
                if (user.Equals(ConnectNumber.ToString()))
                {
                    break;
                }
                count = count + 1;
            }
            int playercount = Room.RoomPlayerCount(Rnumber);
            for(int k = 0; k < playercount; k++)
            {
                calcount = count - k;
                if (calcount < 0)
                    calcount = calcount + playercount;
                result = users[calcount] + "★" + result;
            }
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    i.nextturn = result;
                }
            }

        }

        internal static void tokenpull(int ConnectNumber , int Rnumber) //토큰뽑아서주는거
        {
            string result="";
            string user = "";
            int tokens=0;
            int turn = TurnbyConnect(ConnectNumber,Rnumber);
            int players = Room.RoomPlayerCount(Rnumber);
            Random random = new Random();
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    for (int qq = 0; qq < 3; qq++)
                    {
                        tokens = random.Next(0, i.token.Count());
                        i.token.Remove(i.token[tokens]);
                        result = result + tokens.ToString() + "★";
                    }
                }
            }
            user = DB.findUserNickNameByUserConnect(ConnectNumber);
            result = user + "★" + result;
            Program.SendMessageRoom(Rnumber, result);

        }

        internal static void tokendispose(int ConnectNumber ,int Rnumber, string result)   //  몇번구역△토큰번호△토큰갯수 이렇게 받아옴 같은구역에 두어도 그냥 두번 쓰셈 
        {
            char[] split = "▲".ToCharArray();
            String[] tokenlist = result.Split(split);
            foreach (var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    foreach(var token in tokenlist)
                    {
                        switch (token[0])
                        {
                            case '1':
                                i.section1[int.Parse(token[1].ToString())] = i.section1[int.Parse(token[1].ToString())] + int.Parse(token[2].ToString());
                                break;
                            case '2':
                                i.section2[int.Parse(token[1].ToString())] = i.section2[int.Parse(token[1].ToString())] + int.Parse(token[2].ToString());
                                break;
                            case '3':
                                i.section3[int.Parse(token[1].ToString())] = i.section3[int.Parse(token[1].ToString())] + int.Parse(token[2].ToString());
                                break;
                        }
                    }
                }
            }
            bettingsendresult(Rnumber);
            TurnEnd(ConnectNumber, Rnumber);
        }
        internal static void bettingsendresult(int Rnumber)
        {
            string result="";
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    for(int count = 0; count < 5; count++)
                    {
                        if (i.section1[count] != 0)
                        {
                            result = result + 1 + "△" + count + "△" + i.section1[count] + "▲";
                        }
                        if (i.section2[count] != 0)
                        {
                            result = result + 2 + "△" + count + "△" + i.section2[count] + "▲";
                        }
                        if (i.section3[count] != 0)
                        {
                            result = result + 3 + "△" + count + "△" + i.section3[count] + "▲";
                        }
                    }
                }
            }
            Program.SendMessageRoom(Rnumber, result);
        }
        internal static void ResultAdd(int Rnumber,int TokenCode,int Count)
        {
            foreach(var i in gamelist)
            {
                if(i.RoomNumber== Rnumber)
                {
                    switch (TokenCode)
                    {
                        case 0:
                            i.ZombieResult = i.ZombieResult + Count;
                            break;

                        case 1:
                            i.MummyResult = i.MummyResult + Count;
                            break;

                        case 2:
                            i.FoxResult = i.FoxResult + Count;
                            break;

                        case 3:
                            i.VampireResult = i.VampireResult + Count;
                            break;

                        case 4:
                            i.CGhostResult = i.CGhostResult + Count;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        internal static void pullnextturn(int Rnumber)
        {
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    i.turn = i.nextturn;
                }
            }
        }
        internal static void RoundEnd(int Rnumber)
        {
            string turn = "";
            int winner3 = -1;
            int winner2 = -1;
            int winner1 = -1;
            Program.SendMessageRoom(Rnumber, "RoundEnd");
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    winner3 =FindSectionWinner(i.section3);
                    RoundEnd2(Rnumber, winner3, 3);
                    if (GameEndCheck(Rnumber))
                    {
                        GameEnd(Rnumber);
                        break;
                    }
                    winner2 =FindSectionWinner(i.section2);
                    RoundEnd2(Rnumber, winner2, 2);
                    if (GameEndCheck(Rnumber))
                    {
                        GameEnd(Rnumber);
                        break;
                    }
                    winner1 =FindSectionWinner(i.section1);
                    RoundEnd2(Rnumber, winner1, 1);
                    if (GameEndCheck(Rnumber))
                    {
                        GameEnd(Rnumber);
                        break;
                    }
                }
            }
            Program.SendMessageRoom(Rnumber, "RoundStart");
            pullnextturn(Rnumber);
            int firstTurnConnect = ConnectbyTurn(1, Rnumber);
            RoundStart(firstTurnConnect, Rnumber);

        }

        internal static void RoundEnd2(int Rnumber, int TokenCode, int Count)
        {
            if (TokenCode != -1)
            {
                ResultAdd(Rnumber, TokenCode, Count);
            }
            Program.SendMessageRoom(Rnumber, "move★" + TokenCode + "★"+Count);
        }

        internal static int FindFirst(List<int> list)
        {
            int max = 0;
            for (int j = 0; j < 5; j++)
            {
                if (list[j] > max)
                {
                    max = list[j];
                }
            }
            return max;
        }
        internal static int FindSectionWinner(List<int> section)
        {
            List<int> sect1 = new List<int>();
            int winner = -1;
            int first = FindFirst(section);
            for (int i = 0; i < 5; i++)
            {
                if (section[i] == first)
                    sect1.Add(i);
            }
            if (sect1.Count == 1)
            {
                winner = sect1[0];
            }
            else
            {
                foreach (var k in sect1)
                {
                    section[k] = 0;
                }
                sect1.Clear();
                first = FindFirst(section);
                for (int i = 0; i < 5; i++)
                {
                    if (section[i] == first)
                        sect1.Add(i);
                }
                if (sect1.Count == 1)
                {
                    winner = sect1[0];
                }
                else
                {
                    foreach (var l in sect1)
                    {
                        section[l] = 0;
                    }
                    sect1.Clear();
                    first = FindFirst(section);
                    for (int i = 0; i < 5; i++)
                    {
                        if (section[i] == first)
                            winner = i;
                    }
                }
            }
            int sum = 0;
            foreach (var i in section)
            {
                sum = sum + i;
            }
            if (sum == 0)
            {
                winner = -1;
            }
            return winner;
            }

        internal static bool GameEndCheck(int Rnumber)
        {
            bool result = false;
            foreach(var i in gamelist)
            {
                if((i.MummyResult>=13)|| (i.CGhostResult >= 13)|| (i.FoxResult >= 13)|| (i.VampireResult >= 13)|| (i.ZombieResult >= 13))
                    {
                    result = true;
                }
            }
            return result;
        }

        internal static void RoundStart(int ConnectNumber , int Rnumber)
        {
            string first = "";
            first = DB.findUserNickNameByUserConnect(ConnectNumber);
            Program.SendMessageRoom(Rnumber, "turn★" + first);
        }

        internal static void GameEnd(int Rnumber)
        {
            Program.SendMessageRoom(Rnumber, "GameEnd");
            char[] sp = "☆".ToCharArray();
            foreach (var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                   string rank = TokenRank(i.ZombieResult, i.MummyResult, i.FoxResult, i.VampireResult,i.CGhostResult);
                    string[] result = rank.Split(sp);
                    Program.SendMessage(Rnumber, "first★" + result[0] + "★" + result[1]);
                    finalTokenSend(Rnumber);
                    finalresult(Rnumber, result[0], result[1]);
                }
            }
            RoomstatusUpdate(Rnumber, "wait");
            RoomSettingClear(Rnumber);
        }
        internal static void finalTokenSend(int Rnumber)
        {
            string result="tokens";
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    foreach(var k in i.user)
                    {
                        result = result + "★" + k.Key + "★" + k.Value ;
                    } 
                }
            }
            Program.SendMessageRoom(Rnumber,result);
        }

        internal static void finalresult(int Rnumber,string firstToken, string secondToken)
        {
            string sendmessage = "result★winner";
            char[] sp2 = "★".ToCharArray();
            //firstToken 3점 SecondToken   1★2★ , 1번토큰과 2번토큰이 공동 2등일수있음 그냥 2점줌 공동의 여부상관없이
            string[] seconds;
            seconds = secondToken.Split(sp2);
            Dictionary<string, int> score = new Dictionary<string, int>();
            List<int> checkfirst = new List<int>();
            // score의 1등말 2등말 점수매칭 
            string userlist = Room.FindRoomInfo(Rnumber, "usersConnect"); // 1★2★쭊쭊쭊
            string[] result = userlist.Split(sp2);
            int playerCount = Room.RoomPlayerCount(Rnumber);
            string tokens;
            string[] token;
            int first;
            for(int k = 0; k < playerCount; k++)
            {
                tokens = UserToken(Rnumber, int.Parse(result[k]));
                token = tokens.Split(sp2);
                foreach(var i in token)
                {
                    if (i.Equals(firstToken))
                    {
                        score[result[k]] = score[result[k]] + 3;
                    }
                    foreach(var bb in seconds)
                    {
                        if (i.Equals(bb))
                        {
                            score[result[k]] = score[result[k]] + 2;
                        }
                    }
                }
            }
            foreach(var u in score.Values)
            {
                checkfirst.Add(u);
            }
            first = FindFirst(checkfirst);
            foreach(var r in score)
            {
                if (r.Value == first)
                {
                    sendmessage = sendmessage + "★" + DB.findUserNickNameByUserConnect(int.Parse(r.Key));
                    InsertResult(int.Parse(r.Key), 1, "first");
                }
            }
            sendmessage = "★loser";
            foreach (var r in score)
            {
                if (r.Value != first)
                {
                    sendmessage = sendmessage + "★" + DB.findUserNickNameByUserConnect(int.Parse(r.Key));
                    InsertResult(int.Parse(r.Key), 1, "second");
                }
            }
            Program.SendMessageRoom(Rnumber, sendmessage);

        }


        internal static string UserToken(int Rnumber,int userConnect)
        {
            string result = "";
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    result = i.user[userConnect.ToString()];
                }
            }
            return result;
        }

        internal static string TokenRank(int zombie,int mummy,int fox, int vamp, int cghost)
        {
            string result = "";
            string firstToken = "";
            string secondToken = "";
            List<int> tokenresult = new List<int>();
            tokenresult.Add(zombie);
            tokenresult.Add(mummy);
            tokenresult.Add(fox);
            tokenresult.Add(vamp);
            tokenresult.Add(cghost);
            int first = FindFirst(tokenresult);
            for(int t=0; t < 5; t++)
            {
                if (tokenresult[t] == first)
                {
                    firstToken = t.ToString();
                    tokenresult[t] = 0;
                }   
            }
            first = FindFirst(tokenresult);
            for (int q=0; q < 5; q++)
            {
                if (tokenresult[q] == first)
                {
                    secondToken = secondToken + q.ToString()+"★";
                }
            }
            result = firstToken + "☆" + secondToken;
            return result;
        }

        internal static string tokenrankresult(string token, string rank)
        {
            string result = "";
            result = rank + "★" + token;
            return result;
        }
        internal static void settingtoken(int Rnumber)
        {
            foreach(var i in gamelist)
                if(i.RoomNumber == Rnumber)
                {
                    foreach (var one in i.tokenset)
                        i.token.Add(one);
                }
        }



        internal static void TokenSending(int Rnumber)
        {
            foreach(var i in gamelist)
                if (i.RoomNumber == Rnumber)
                {
                    foreach(var k in i.user.Keys)
                    {
                        Program.SendMessage(Connection.ConnectionReturn(int.Parse(k)), i.user[k]);
                    }
                }
        }

        internal static int TurnbyConnect(int ConnectNumber,int Rnumber)
        {
            char[] sp2 = "★".ToCharArray();
            int count = 1;
            string rt = RoomTurn(Rnumber);
            String[] users = rt.Split(sp2);
            foreach (var user in users)
            {
                if (user.Equals(ConnectNumber.ToString()))
                {
                    break;
                }
                count = count + 1;
                    }
            return count;
        }
        internal static int ConnectbyTurn(int Turn, int Rnumber)
        {
            int connectnumber = 0;
            char[] sp2 = "★".ToCharArray();
            string rt = RoomTurn(Rnumber);
            String[] users = rt.Split(sp2);
            connectnumber = int.Parse(users[Turn - 1]);
            return connectnumber;
        }
        
        internal static string RoomTurn(int Rnumber)
        {
            string result = "";
            foreach(var i in gamelist)
            {
                if (i.RoomNumber == Rnumber)
                {
                    result = i.turn;
                }
            }
            return result;
        }
        internal static int GameIndex(int RNumber) //해당방의 인덱스찾기
        {
            int k = 0;
            foreach (var i in gamelist)
            {
                if (i.RoomNumber == RNumber)
                {
                    break;
                }
                k = k + 1;
            }
            return k;
        }


        internal static void RoomSettingClear(int Rnumber)
        {
            int x = GameIndex(Rnumber); // 여기 수정좀 해야함 잘못짯음 
            gamelist.RemoveAt(x);
        }
            internal static void RoomSetting(int Rnumber , int UserCount)
        {
            //이함수 안에서 구분선은 반환값 으로 오는거 제외하고 sp2로 통일
            char[] sp2 = "★".ToCharArray();
            char[] sp = "☆".ToCharArray();
            string turn="";
            string userList = Room.FindRoomInfo(Rnumber, "usersConnect");
            string pairs = CreateRandomPair(UserCount); 
            string[] pair = pairs.Split(sp);  //string 으로 페어를 자름
            String[] users = userList.Split(sp2); // 
            string alltoken = pairs.Replace('☆', ',').Replace('★', ',');
            string[] alltokens = alltoken.Split(',');
            GameResult newsetting = new GameResult();
            newsetting.RoomNumber = Rnumber;
            for (int i = 0; i < UserCount; i++)
            {
                turn = turn + users[i] + "★";
                newsetting.user.Add(users[i], pair[i]);
                for (int j = 0; j < 5; j++)
                    newsetting.token.Add(j);
            }
            for(int t=0; t < 5; t++)
            {
                newsetting.section1.Add(0);
                newsetting.section2.Add(0);
                newsetting.section3.Add(0);
            }
            newsetting.turn = turn;
            foreach(var k in alltokens)
            {
                //  newsetting.token.Remove(int.Parse(k));  이버전은 변환에러나서 좀 더럽게 바꿈
                    switch (k)
                    {
                        case "0":
                        newsetting.token.Remove(0);
                            break;
                        case "1":
                        newsetting.token.Remove(1);
                        break;
                        case "2":
                        newsetting.token.Remove(2);
                        break;
                        case "3":
                        newsetting.token.Remove(3);
                        break;
                        case "4":
                        newsetting.token.Remove(4);
                        break;
                    }
            }
            foreach (var one in newsetting.token)
                newsetting.tokenset.Add(one);
            gamelist.Add(newsetting);
        }

        internal static string CreateRandomPair(int UserCount)
        {
            
            int a, b;
            string k = "";
            Random random = new Random();
            for (int j = 0; j < UserCount; j++)
            {
                a = random.Next(0, 5);
                while (true)
                {
                    b = random.Next(0, 5);
                    if (b != a)
                    {
                        break;
                    }
                }
                k = k + a + "★" + b + "☆";
            }
            return k;
        }

        internal static void InsertResult(int userconnect, int Gnumber, string rank)
        {
            int usernumber = int.Parse(DB.findUserNumberByUserConnect(userconnect));
            string query = "";
            if (Gnumber == 1 && rank.Equals("first"))
            {
                query = " update result set bellpoint = bellpoint+1 gamewin = gamewin+1 , where usernumber = " + usernumber + ";";   // sql 쿼리문
            }
            else if (Gnumber == 1 && rank.Equals("second"))
            {
                query = " update result set gamelose = gamelose+1 , where usernumber = " + usernumber + ";";
            }
            DB.UsingQuery(query, "update", 0);
        }



    }
}
