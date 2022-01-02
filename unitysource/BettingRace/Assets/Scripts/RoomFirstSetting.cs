using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstSetting : MonoBehaviour
{
    ConnectServer ConnectManagerScript;
    public GameObject ConnectManager;
    // Start is called before the first frame update
    void Start()
    {
        ConnectManager = GameObject.Find("ConnectManager");
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        string newSetting = "RList☆0";
        ConnectManagerScript.SendMessage(newSetting);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Delay()
    {
        Debug.Log("0.05초 지남");
    }
}
