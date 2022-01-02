using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene2Manager : MonoBehaviour
{
    public GameObject ChattingPanel;
    private Text Chat;
    public int ChattingRealCount = 0;
    private List<string> ChattingList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        Chat = ChattingPanel.GetComponent<Text>();
        for (int i = 0; i < 3; i++)
        {
            ChattingList.Add("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RoomChat(string SplitedMessage)
    {
        string ShowText = "";
        if (ChattingRealCount < 3)
        {
            ChattingList[ChattingRealCount] = SplitedMessage;
            ShowText = ChattingList[0];
            for (int i = 1; i < 5; i++)
                ShowText = ShowText + System.Environment.NewLine + ChattingList[i];
        }
        else
        {
            ShowText = ChattingList[1];
            for (int i = 2; i < 5; i++)
                ShowText = ShowText + System.Environment.NewLine + ChattingList[i];
            ShowText = ShowText + System.Environment.NewLine + SplitedMessage;
        }
        Chat.text = ShowText;
        ChattingRealCount = ChattingRealCount + 1;
    }
}
