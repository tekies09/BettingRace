using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyRankSetting : MonoBehaviour
{
    // Start is called before the first frame update
    public Text MyId, MyWin, MyLose, MyBell;
    void Start()
    {
        MyId.text = PlayerPrefs.GetString("UId");
        MyWin.text = PlayerPrefs.GetString("UWin");
        MyLose.text = PlayerPrefs.GetString("ULose");
        MyBell.text = PlayerPrefs.GetString("UBell");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
