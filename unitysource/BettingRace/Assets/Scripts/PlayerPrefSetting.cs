using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!(PlayerPrefs.HasKey("PlayerPrefSet")))
        {
            PlayerPrefs.SetInt("PlayerPrefSet", 1);
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Memory", 0);
            PlayerPrefs.SetString("Unum", "-1");
            PlayerPrefs.SetString("Id", "");
            PlayerPrefs.SetString("Pw", "");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
