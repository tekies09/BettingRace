using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSceneButtonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PrevButtonClick()
    {
        GameObject SceneChangeManagers = GameObject.Find("SceneChangeManagers");
        SceneChangeManager SceneChangeManagersScript = SceneChangeManagers.GetComponent<SceneChangeManager>();
        SceneChangeManagersScript.GoRobby();
    }
    public void RoomChatSend()
    {

    }
}
