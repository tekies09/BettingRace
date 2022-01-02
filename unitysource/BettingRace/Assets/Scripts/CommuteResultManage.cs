using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuteResultManage : MonoBehaviour
{
    char[] sp = "★".ToCharArray(); //명령어 구분기호
    public static CommuteResultManage Instance;
    public GameObject LoginFailBoard;
    public GameObject LoginSuccessBoard;
    public GameObject RegisterFailBoard;
    public GameObject RegisterSuccessBoard;
    public GameObject IdCheckFailBoard;
    public GameObject IdCheckSuccessBoard;
    public GameObject SettingBoard;
    public GameObject LoginBoard;
    public GameObject RegisterBoard;

    // Start is called before the first frame update
    void Start()
    {
        LoginFailBoard.SetActive(false);
        LoginSuccessBoard.SetActive(false);
        RegisterFailBoard.SetActive(false);
        RegisterSuccessBoard.SetActive(false);
        IdCheckFailBoard.SetActive(false);
        IdCheckSuccessBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
    public void IdCheckSuccess()
    {
        if (RegisterBoard == null)
        {
            RegisterBoard = GameObject.Find("RegisterBoard");
        }
        RegisterBoard.SetActive(false);
        BoardOpen(5);
        
    }
    public void IdCheckFail()
    {
        if (RegisterBoard == null)
        {
            RegisterBoard = GameObject.Find("RegisterBoard");
        }
        RegisterBoard.SetActive(false);
        BoardOpen(4);
    }
    public void RegisterFail()
    {
        if (RegisterBoard == null)
        {
            RegisterBoard = GameObject.Find("RegisterBoard");
        }
        RegisterBoard.SetActive(false);
        BoardOpen(2);
    }
    public void RegisterSuccess()
    {
        if (RegisterBoard == null)
        {
            RegisterBoard = GameObject.Find("RegisterBoard");
        }
        RegisterBoard.SetActive(false);
        BoardOpen(3);
    }
    public void LoginFail()
    {
        if (LoginBoard == null)
        {
            LoginBoard = GameObject.Find("LoginBoard");
        }
        LoginBoard.SetActive(false);
        BoardOpen(0);
    }
    public void LoginSuccess(string splitedMessage)
    {
        string[] result = splitedMessage.Split(sp);
        int k = 0;
        foreach(var i in result)
        {
            switch (k)
            {
                case 0:
                    PlayerPrefs.SetString("Unum", i);
                    break;
                case 1:
                    PlayerPrefs.SetString("UId", i);
                    break;
                case 2:
                    PlayerPrefs.SetString("UWin", i);
                    break;
                case 3:
                    PlayerPrefs.SetString("ULose", i);
                    break;
                case 4:
                    PlayerPrefs.SetString("UBell", i);
                    break;
            }
            k = k + 1;
        }
        if (LoginBoard == null)
        {
            LoginBoard = GameObject.Find("LoginBoard");
        }
        LoginBoard.SetActive(false);
        BoardOpen(1);
    }
    public void ReConnectBoard()
    {
        LoginFailBoard = GameObject.Find("ResultBoardCollect").transform.Find("LoginFailBoard").gameObject;
        LoginSuccessBoard = GameObject.Find("ResultBoardCollect").transform.Find("LoginSuccessBoard").gameObject;
        RegisterFailBoard = GameObject.Find("ResultBoardCollect").transform.Find("RegisterFailBoard").gameObject;
        RegisterSuccessBoard = GameObject.Find("ResultBoardCollect").transform.Find("RegisterSuccessBoard").gameObject;
        IdCheckFailBoard = GameObject.Find("ResultBoardCollect").transform.Find("IdCheckFailBoard").gameObject;
        IdCheckSuccessBoard = GameObject.Find("ResultBoardCollect").transform.Find("IdCheckSuccessBoard").gameObject;
    }
    public void BoardOpen(int code)
    {
        switch (code)
        {
            case 0:
                if (LoginFailBoard == null)
                    ReConnectBoard();
                LoginFailBoard.SetActive(true);
                break;
            case 1:
                if (LoginSuccessBoard == null)
                    ReConnectBoard();
                LoginSuccessBoard.SetActive(true);
                break;
            case 2:
                if (RegisterFailBoard == null)
                    ReConnectBoard();
                RegisterFailBoard.SetActive(true);
                break;
            case 3:
                if (RegisterSuccessBoard == null)
                    ReConnectBoard();
                RegisterSuccessBoard.SetActive(true);
                break;
            case 4:
                if (IdCheckFailBoard == null)
                    ReConnectBoard();
                IdCheckFailBoard.SetActive(true);
                break;
            case 5:
                if (IdCheckSuccessBoard == null)
                    ReConnectBoard();
                IdCheckSuccessBoard.SetActive(true);
                break;
        }
    }
    public void BoardClose(int code)
    {
        switch (code)
        {
            case 0:
                LoginFailBoard.SetActive(false);
                break;
            case 1:
                LoginSuccessBoard.SetActive(false);
                break;
            case 2:
                RegisterFailBoard.SetActive(false);
                break;
            case 3:
                RegisterSuccessBoard.SetActive(false);
                break;
            case 4:
                IdCheckFailBoard.SetActive(false);
                break;
            case 5:
                IdCheckSuccessBoard.SetActive(false);
                break;
        }
    }
}
