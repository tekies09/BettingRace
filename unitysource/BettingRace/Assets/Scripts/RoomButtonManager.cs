using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtonManager : MonoBehaviour
{
    public GameObject RLManager;
    public RoomLoadScript RLScript;
    ConnectServer ConnectManagerScript;
    public GameObject ConnectManager;
    public Sprite CreateFail, PageMoveFail;
    private SpriteRenderer NM_SR;
    public GameObject NoticeMent, NoticeBoard, NoticeButton;
    public GameObject NewRoomBoard;
    public Text RoomTitle, RoomPw;
    // Start is called before the first frame update
    void Start()
    {
        RLScript = RLManager.GetComponent<RoomLoadScript>();
        ConnectManager = GameObject.Find("ConnectManager");
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        NoticeBoard = GameObject.Find("BoardCollection").transform.Find("NoticeBoard").gameObject;
        NoticeButton = GameObject.Find("BoardCollection").transform.Find("OKButton").gameObject;
        NoticeMent = GameObject.Find("BoardCollection").transform.Find("NoticeMent").gameObject;
        NM_SR = NoticeMent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PrevButtonClick()
    {
        int page = RLScript.NowPage;
        if (page == 0)
        {
            NoticeBoard.SetActive(true);
            NoticeButton.SetActive(true);
            NoticeMent.SetActive(true);
            NM_SR.sprite = PageMoveFail;
            Debug.Log("last page");
        }
        else
        {
            page = page - 1;
            RLScript.NowPage = page;
            string SendMessage = "RList☆" + (4 * page);
            ConnectManagerScript.SendMessage(SendMessage);
        }

    }
    public void NextButtonClick()
    {
        int page = RLScript.NowPage;
        int LPCheck = RLScript.LastPageCheck;
        if (LPCheck == 1)
        {
            NoticeBoard.SetActive(true);
            NoticeButton.SetActive(true);
            NoticeMent.SetActive(true);
            NM_SR.sprite = PageMoveFail;
        }
        else
        {
            page = page + 1;
            RLScript.NowPage = page;
            string SendMessage = "RList☆" + (4 * page);
            ConnectManagerScript.SendMessage(SendMessage);
        }
    }
    public void OKButtonClick()
    {
        NoticeBoard.SetActive(false);
        NoticeButton.SetActive(false);
        NoticeMent.SetActive(false);
        NM_SR.sprite = PageMoveFail;
    }
    public void NewRoomButtonClick()
    {
        RoomTitle.text  = "";
        RoomPw.text     = "";
        NewRoomBoard.SetActive(true);
    }
    public void RoomCreateButtonClick()
    {
        NewRoomBoard.SetActive(false);
        string UNum = PlayerPrefs.GetString("Unum");
        string Title = RoomTitle.text;
        if (Title.Length == 0)
        {
            int a = Random.Range(0, 5);
            switch (a)
            {
                case 0:
                    Title = "Everyone Come Here";
                    break;
                case 1:
                    Title = "COME COME COME";
                    break;
                case 2:
                    Title = "BTS BRING ME HERE";
                    break;
                case 3:
                    Title = "NO WAY";
                    break;
                default:
                    Title = "Default Room";
                    break;
            }
        }
        string Password = RoomPw.text;
        string SendMessage = "NRoom☆" + UNum + "★" + Title + "★" + Password;
        ConnectManagerScript.SendMessage(SendMessage);
    }
}
