using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankTop5Setting : MonoBehaviour
{
    ConnectServer ConnectManagerScript;
    public GameObject ConnectManager;
    // Start is called before the first frame update
    void Start()
    {
        ConnectManager = GameObject.Find("ConnectManager");
        ConnectManagerScript = ConnectManager.GetComponent<ConnectServer>();
        string newSetting = "Ranking☆";
        ConnectManagerScript.SendMessage(newSetting);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
