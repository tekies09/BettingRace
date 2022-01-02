using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene1Manager : MonoBehaviour
{
    char[] sp = "★".ToCharArray(); //명령어 구분기호

    public GameObject NoticeBoard,NoticeOKButton,NoticeMent;
    public Text  FirstId, FirstWin, FirstLose, FirstBell, SecondId, SecondWin, SecondLose, SecondBell,
        ThirdId, ThirdWin, ThirdLose, ThirdBell,FourthId, FourthWin, FourthLose, FourthBell, FifthId, FifthWin, FifthLose, FifthBell;
    // Start is called before the first frame update
    void Start()
    {
        UIInitialized();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void newRoomB(string splitedMessage)
    {
        string[] result = splitedMessage.Split(sp);
        if (result[0].Equals("Fail"))
        {
            NoticeBoard.SetActive(true);
            NoticeOKButton.SetActive(true);
            NoticeMent.SetActive(true);
        }
        else
        {
            string Userid = PlayerPrefs.GetString("UId");
            int k = 0;
            foreach (var i in result)
            {
                switch (k)
                {
                    case 0:
                        PlayerPrefs.SetString("RNum", i);
                        break;
                    case 1:
                        PlayerPrefs.SetString("RName", i);
                        break;
                    case 2:
                        break;
                    case 3:
                        PlayerPrefs.SetString("RPw", i);
                        break;
                }
                k = k + 1;
            }
            PlayerPrefs.SetString("Master", Userid);
            GameObject SceneChangeManagers = GameObject.Find("SceneChangeManagers");
            SceneChangeManager SceneChangeManagersScript = SceneChangeManagers.GetComponent<SceneChangeManager>();
            SceneChangeManagersScript.GoRoom();
        }
    }
    public void RankTop5(string splitedMessage)
    {
        string[] result = splitedMessage.Split(sp);
        int k = 0;
        foreach (var i in result)
        {
            switch (k)
            {
                case 0:
                    FirstId.text = i;
                    break;
                case 1:
                    FirstWin.text = i;
                    break;
                case 2:
                    FirstLose.text = i;
                    break;
                case 3:
                    FirstBell.text = i;
                    break;
                case 4:
                    SecondId.text = i;
                    break;
                case 5:
                    SecondWin.text = i;
                    break;
                case 6:
                    SecondLose.text = i;
                    break;
                case 7:
                    SecondBell.text = i;
                    break;
                case 8:
                    ThirdId.text = i;
                    break;
                case 9:
                    ThirdWin.text = i;
                    break;
                case 10:
                    ThirdLose.text = i;
                    break;
                case 11:
                    ThirdBell.text = i;
                    break;
                case 12:
                    FourthId.text = i;
                    break;
                case 13:
                    FourthWin.text = i;
                    break;
                case 14:
                    FourthLose.text = i;
                    break;
                case 15:
                    FourthBell.text = i;
                    break;
                case 16:
                    FifthId.text = i;
                    break;
                case 17:
                    FifthWin.text = i;
                    break;
                case 18:
                    FifthLose.text = i;
                    break;
                case 19:
                    FifthBell.text = i;
                    break;
            }
            k = k + 1;
        }
    }
    public void UIInitialized()
    {
        FirstId.text = "";
        FirstWin.text = "";
        FirstLose.text = "";
        FirstBell.text = "";
        SecondId.text = "";
        SecondWin.text = "";
        SecondLose.text = "";
        SecondBell.text = "";
        ThirdId.text = "";
        ThirdWin.text = "";
        ThirdLose.text = "";
        ThirdBell.text = "";
        FourthId.text = "";
        FourthWin.text = "";
        FourthLose.text = "";
        FourthBell.text = "";
        FifthId.text = "";
        FifthWin.text = "";
        FifthLose.text = "";
        FifthBell.text = "";
    }
}
