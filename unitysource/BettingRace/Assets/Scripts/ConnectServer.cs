using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using UnityEngine.UI;

public class ConnectServer : MonoBehaviour
{
    private GameObject Scene2Management;
    private Scene2Manager Scene2Script;
    private GameObject RoomLoadManager;
    private RoomLoadScript RLMScript;
    private GameObject ChattingFrontManager;
    private ChattingFront CFMScript;
    private Queue<string> Messagequeue = new Queue<string>();
    public static ConnectServer Instance;
    private TcpClient tc;
    private Thread clientReceiveThread;
    public int SceneCode = 0; //SceneCode   [ 0 : First 1: Robby 2 : Room ]  
    public GameObject Scene0Manager;
    CommuteResultManage Scene0ManagerScript;
    public GameObject Scene1Manager;
    Scene1Manager Scene1ManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        Scene0Manager = GameObject.Find("ServerCommuteResultManager");
        if (Scene0Manager != null)
            Scene0ManagerScript = Scene0Manager.GetComponent<CommuteResultManage>();
        ConnectToServer();
    }
    public void Resetting(int code)
    {
        if (code == 0) {
            Scene0Manager = GameObject.Find("ServerCommuteResultManager");
            Scene0ManagerScript = Scene0Manager.GetComponent<CommuteResultManage>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Messagequeue.Count > 0)
        {
            string serverMessage = Messagequeue.Dequeue(); //여기서 UI 처리.. 
            ServerComProcess(serverMessage);
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void ConnectToServer()
    {
        clientReceiveThread = new Thread(new ThreadStart(Receive));
        clientReceiveThread.IsBackground = true;
        clientReceiveThread.Start();
    }
    public void Receive()
    {
        try
        {
            tc = new TcpClient("127.0.0.1", 5555);
            Byte[] outbuf = new Byte[1024];
            while (true)
            {
                using (NetworkStream stream = tc.GetStream())
                {
                    int length;
                    while ((length = stream.Read(outbuf, 0, outbuf.Length)) != 0)
                    {
                        var incomingData = new byte[length];
                        Array.Copy(outbuf, 0, incomingData, 0, length);
                        string serverMessage = Encoding.GetEncoding("euc-kr").GetString(incomingData);
                        Messagequeue.Enqueue(serverMessage);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Server oFF"+e);
        }
    }
    public void SendMessage(string message)
    {
        if (tc == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = tc.GetStream();
            if (stream.CanWrite)
            {
                byte[] clientMessageAsByteArray = Encoding.GetEncoding("euc-kr").GetBytes(message);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                stream.Flush();
                Debug.Log("smcall message : " + message);
            }
        }
        catch(Exception e)
        {
            Debug.Log("ServerOff" + e);
        }
    }

    public void ServerComProcess(string Message)
    {
        char[] sp = "☆".ToCharArray(); //명령어 구분기호
        string[] result = Message.Split(sp);
        switch (result[0])
        {
            case "Check":
                Checkid(result[1]);
                break;
            case "Login":
                Login(result[1]);
                break;
            case "Register":
                Register(result[1]);
                break;
            case "Ranking":
                 Ranking(result[1]);
                break;
            case "AllChat":
                  AllChat(result[1]);
                break;
            case "RList":
                RoomList(result[1]);
                break;
            case "NRoomA":
                NRoomA(result[1]);
                break;
            case "NRoomB":
                NRoomB(result[1]);
                break;
            case "RChat":
                RChat(result[1]);
                break;
        }
    }
    public void RChat(string SplitedMessage)
    {
        if(SceneCode==2)
        {
            Scene2Management = GameObject.Find("Scene2Management");
            Scene2Script = Scene2Management.GetComponent<Scene2Manager>();
            Scene2Script.RoomChat(SplitedMessage);
        }
    }
    public void NRoomA(string SplitedMessage)
    {
        if ((SceneCode == 1))
        {
            // another people create room
            RoomLoadManager = GameObject.Find("RoomLoadManager");
            RLMScript = RoomLoadManager.GetComponent<RoomLoadScript>();
            string message = "RList☆" + RLMScript.NowPage;
            SendMessage(message);
        }
    }
    public void NRoomB(string SplitedMessage)
    {
        Scene1Manager = GameObject.Find("Scene1Management");
        Scene1ManagerScript = Scene1Manager.GetComponent<Scene1Manager>();
        Scene1ManagerScript.newRoomB(SplitedMessage);
        // i create room
    }
    public void Ranking(string SplitedMessage)
    {
        Debug.Log(SplitedMessage);
        Scene1Manager = GameObject.Find("Scene1Management");
        Scene1ManagerScript = Scene1Manager.GetComponent<Scene1Manager>();
        Scene1ManagerScript.RankTop5(SplitedMessage);
    }
    public void RoomList(string SplitedMessage)
    {
        if (SceneCode == 1)
        {
            RoomLoadManager = GameObject.Find("RoomLoadManager");
            RLMScript = RoomLoadManager.GetComponent<RoomLoadScript>();
            RLMScript.ShowRoomList(SplitedMessage);
        }
    }
    public void AllChat(string SplitedMessage)
    {
        if (SceneCode == 1)
        {
            ChattingFrontManager = GameObject.Find("ChattingFrontManage");
            CFMScript = ChattingFrontManager.GetComponent<ChattingFront>();
            CFMScript.ShowChat(SplitedMessage);
        }
    }
    public void Checkid(string SplitedMessage)
    {
        if(SplitedMessage.Equals("Success"))
        {
            Scene0ManagerScript.IdCheckSuccess();
        }
        else
        {
            Scene0ManagerScript.IdCheckFail();
        }
    }
    public void Login(string SplitedMessage)
    {
        if (SplitedMessage.Equals("Fail"))
        {
            Scene0ManagerScript.LoginFail();
        }
        else
        {
            Scene0ManagerScript.LoginSuccess(SplitedMessage);
        }
    }
    public void Register(string SplitedMessage)
    {
        if (SplitedMessage.Equals("Success"))
        {
            Scene0ManagerScript.RegisterSuccess();
        }
        else
        {
            Scene0ManagerScript.RegisterFail();
        }
    }
}
