using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobbyButtonManage : MonoBehaviour
{
    public GameObject ChattingFrontManager;
    private ChattingFront CFMScript;
    GameObject Connector;
    ConnectServer ConnectorScript;
    // Start is called before the first frame update
    void Start()
    {
        Connector = GameObject.Find("ConnectManager");
        ConnectorScript = Connector.GetComponent<ConnectServer>();
        CFMScript = ChattingFrontManager.GetComponent<ChattingFront>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SendButtonClicked(string message)
    {
        string id = PlayerPrefs.GetString("LoggedInId");
        message = "AllChat☆" +id + "★"+ message;
        ConnectorScript.SendMessage(message);
    }
    public void PrevButtonClicked()
    {
        GameObject SceneChangeManagers = GameObject.Find("SceneChangeManagers");
        SceneChangeManager SceneChangeManagersScript = SceneChangeManagers.GetComponent<SceneChangeManager>();
        SceneChangeManagersScript.GoFirst();
    }
}
