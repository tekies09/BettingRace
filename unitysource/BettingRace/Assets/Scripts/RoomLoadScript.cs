using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomLoadScript : MonoBehaviour
{
    public int NowPage = 0;
    public int LastPageCheck; //  0: NotLastPage 1: LastPage
    public int FirstRoomEnterCode, SecondRoomEnterCode, ThirdRoomEnterCode, FourthRoomEnterCode;// 0 : NotExist 1 : UnRock 2 : Rock
    // Start is called before the first frame update
    public Text FirstRoomNo,FirstRoomName,FirstRoomStatus,SecondRoomNo, SecondRoomName, SecondRoomStatus, ThirdRoomNo, ThirdRoomName, ThirdRoomStatus, FourthRoomNo, FourthRoomName, FourthRoomStatus;
    public GameObject FirstRock, SecondRock, ThirdRock, FourthRock;
    private SpriteRenderer FR_SR, SR_SR, TR_SR, FhR_SR;
    public Sprite Rock, UnRock;
    char[] sp = "★".ToCharArray(); //명령어 구분기호
    void Start()
    {
        FR_SR  = FirstRock.GetComponent<SpriteRenderer>();
        SR_SR  = SecondRock.GetComponent<SpriteRenderer>();
        TR_SR  = ThirdRock.GetComponent<SpriteRenderer>();
        FhR_SR = FourthRock.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowRoomList(string message)
    {
        LastPageCheck = 1;
        Debug.Log(message);
        string[] result = message.Split(sp);
        FirstRoomNo.text = "";
        FirstRoomName.text = "";
        FirstRoomStatus.text = "";
        SecondRoomNo.text = "";
        SecondRoomName.text = "";
        SecondRoomStatus.text = "";
        ThirdRoomNo.text = "";
        ThirdRoomName.text = "";
        ThirdRoomStatus.text = "";
        FourthRoomNo.text = "";
        FourthRoomName.text = "";
        FourthRoomStatus.text = "";
        FR_SR.sprite = UnRock;
        SR_SR.sprite = UnRock;
        TR_SR.sprite = UnRock;
        FhR_SR.sprite = UnRock;
        FirstRoomEnterCode = 0;
        SecondRoomEnterCode = 0;
        ThirdRoomEnterCode = 0;
        FourthRoomEnterCode = 0;
        int k = 0;
        foreach (var i in result)
        {
            switch (k)
            {
                case 0:
                    FirstRoomNo.text = "No."+i;
                    break;
                case 1:
                    FirstRoomName.text = i;
                    break;
                case 2:
                    FirstRoomStatus.text = i;
                    break;
                case 3:
                    if (i.Length == 0)
                    {
                        FirstRoomEnterCode = 1;
                    }
                    else
                    {
                        FR_SR.sprite = Rock;
                        FirstRoomEnterCode = 2;
                    }
                    break;
                case 4:
                    SecondRoomNo.text = "No." + i;
                    break;
                case 5:
                    SecondRoomName.text = i;
                    break;
                case 6:
                    SecondRoomStatus.text = i;
                    break;
                case 7:
                    if (i.Length == 0)
                    {
                        SecondRoomEnterCode = 1;
                    }
                    else
                    {
                        SR_SR.sprite = Rock;
                        SecondRoomEnterCode = 2;
                    }
                    break;
                case 8:
                    ThirdRoomNo.text = "No." + i;
                    break;
                case 9:
                    ThirdRoomName.text = i;
                    break;
                case 10:
                    ThirdRoomStatus.text = i;
                    break;
                case 11:
                    if (i.Length == 0)
                    {
                        ThirdRoomEnterCode = 1;
                    }
                    else
                    {
                        TR_SR.sprite = Rock;
                        ThirdRoomEnterCode = 2;
                    }
                    break;
                case 12:
                    FourthRoomNo.text = "No." + i;
                    break;
                case 13:
                    FourthRoomName.text = i;
                    break;
                case 14:
                    FourthRoomStatus.text = i;
                    break;
                case 15:
                    if (i.Length == 0)
                    {
                        FourthRoomEnterCode = 1;
                    }
                    else
                    {
                        FhR_SR.sprite = Rock;
                        FourthRoomEnterCode = 2;
                    }
                    LastPageCheck = 0;
                    break;
            }
            k = k + 1;
        }
    }
}
