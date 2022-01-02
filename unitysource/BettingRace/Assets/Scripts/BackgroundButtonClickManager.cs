using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundButtonClickManager : MonoBehaviour
{
    public int CheckStatusCode=0; // 0 -> Click Available 1 -> Already Clicked
    // Start is called before the first frame update
    public GameObject SettingBoard;
    public GameObject LoginBoard;
    public GameObject RegisterBoard;
    public GameObject MemoryButton;
    public GameObject SoundButton;
    public GameObject ConnectManager;
    public Sprite SoundOn, SoundOff , MemoryOn , MemoryOff;
    SpriteRenderer SR_MB,SR_SB;
    ConnectServer ConnectManagerScript;
    public InputField LIF_Id , LIF_Pw, RIF_Id, RIF_Pw;
    private string loggedinId, loggedinPw;

    void Start()
    {
        SettingBoard.SetActive(false);
        RegisterBoard.SetActive(false);
        LoginBoard.SetActive(false);
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        SR_MB = MemoryButton.GetComponent<SpriteRenderer>();
        SR_SB = SoundButton.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoginButtonClick()
    {
        Debug.Log("LoginClicked");

        if (CheckStatusCode == 0)
        {
            LIF_Id.text = "";
            LIF_Pw.text = "";
            LoginBoard.SetActive(true);
            Debug.Log("loginButtonClick");
            if (PlayerPrefs.GetInt("Memory") == 1)
            {
                LIF_Id.text = LIF_Id.text + PlayerPrefs.GetString("Id");
                LIF_Pw.text = LIF_Pw.text + PlayerPrefs.GetString("Pw");
                SR_MB.sprite = MemoryOn;
            }
            else
            {
                SR_MB.sprite = MemoryOff;
            }
        }
        CheckStatusCode = 1;
    }
    public void RegisterButtonClick()
    {

        if (CheckStatusCode == 0)
        {
            RIF_Id.text = "";
            RIF_Pw.text = "";
            RegisterBoard.SetActive(true);
            Debug.Log("RegisterButtonClick");
        }
        CheckStatusCode = 1;
    }
    public void SettingButtonClick()
    {
        if (CheckStatusCode == 0)
        {
            SettingBoard.SetActive(true);
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                SR_SB.sprite = SoundOn;
            }
            else
            {
                SR_SB.sprite = SoundOff;
            }
        }
        CheckStatusCode = 1;
    }
    public void PrevButtonClick()
    {
        if (CheckStatusCode == 1)
        {
            SettingBoard.SetActive(false);
            RegisterBoard.SetActive(false);
            LoginBoard.SetActive(false);
            Debug.Log("PrevButtonClick");
        }
        CheckStatusCode = 0;
    }
    public void MemoryButtonClick()
    {
        if (PlayerPrefs.GetInt("Memory") == 1)
        {
            PlayerPrefs.SetInt("Memory", 0);
            SR_MB.sprite = MemoryOff;
        }
        else
        {
            PlayerPrefs.SetInt("Memory", 1);
            SR_MB.sprite = MemoryOn;
        }
    }
    public void SoundButtonClick()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            SR_SB.sprite = SoundOff;
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            SR_SB.sprite = SoundOn;
        }
    }
    public void IdCheckButtonClick(string id)
    {
        ConnectManager = GameObject.Find("ConnectManager");
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        string message = "Check☆" + id;
        Debug.Log("idcheckbuttonclick message : " +message);
        ConnectManagerScript.SendMessage(message);
    }
    public void RegisterButtonClick(string id,string pw)
    {
        ConnectManager = GameObject.Find("ConnectManager");
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        string message = "Register☆" + id + "★"+pw;
        Debug.Log("RegisterButtonClick message : " + message);
        ConnectManagerScript.SendMessage(message);
    }
    public void LoginButtonClick(string id,string pw)
    {
        ConnectManager = GameObject.Find("ConnectManager");
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        loggedinId = id;
        loggedinPw = pw;
        if (PlayerPrefs.GetInt("Memory") == 1)
        {
            PlayerPrefs.SetString("Id", id);
            PlayerPrefs.SetString("Pw", pw);
            SR_MB.sprite = MemoryOn;
        }
        string message = "Login☆" + id + "★" + pw;
        Debug.Log("RegisterButtonClick message : " + message);
        ConnectManagerScript.SendMessage(message);
    }

    public void IdCheckSuccessBoardOKButtonClick()
    {
        GameObject IdCheckSuccessBoard = GameObject.Find("IdCheckSuccessBoard");
        IdCheckSuccessBoard.SetActive(false);
        RegisterBoard.SetActive(true);
    }
    public void IdCheckFailBoardOKButtonClick()
    {
        GameObject IdCheckFailBoard = GameObject.Find("IdCheckFailBoard");
        IdCheckFailBoard.SetActive(false);
        RegisterBoard.SetActive(true);
    }
    public void LoginSuccessBoardOKButtonClick()
    {
        PlayerPrefs.SetString("LoggedInId", loggedinId);
        PlayerPrefs.SetString("LoggedInPw", loggedinPw);
        CheckStatusCode = 0;
        GameObject LoginSuccessBoard = GameObject.Find("LoginSuccessBoard");
        LoginSuccessBoard.SetActive(false);
        GameObject SceneChangeManagers = GameObject.Find("SceneChangeManagers");
        SceneChangeManager SceneChangeManagersScript = SceneChangeManagers.GetComponent<SceneChangeManager>();
        SceneChangeManagersScript.GoRobby();
    }
    public void LoginFailBoardOKButtonClick()
    {
        GameObject LoginFailBoard = GameObject.Find("LoginFailBoard");
        LoginFailBoard.SetActive(false);
        LoginBoard.SetActive(true);
        loggedinId = "";
        loggedinPw = "";
    }
    public void RegisterSuccessBoardOKButtonClick()
    {
        CheckStatusCode = 0;
        GameObject RegisterSuccessBoard = GameObject.Find("RegisterSuccessBoard");
        RegisterSuccessBoard.SetActive(false);

    }
    public void RegisterFailBoardOKButtonClick()
    {
        GameObject RegisterFailBoard = GameObject.Find("RegisterFailBoard");
        RegisterFailBoard.SetActive(false);
        RegisterBoard.SetActive(true);
    }
}
