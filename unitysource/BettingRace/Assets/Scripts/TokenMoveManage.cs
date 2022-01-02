using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenMoveManage : MonoBehaviour
{
    public GameObject Pow, Gunner, Healer, Bard, Pup;
    private Transform TF_Pow, TF_Gun, TF_Hea, TF_Bar, TF_Pup;
    // Start is called before the first frame update
    void Start()
    {
        TF_Pow = Pow.GetComponent<Transform>();
        TF_Gun = Gunner.GetComponent<Transform>();
        TF_Hea = Healer.GetComponent<Transform>();
        TF_Bar = Bard.GetComponent<Transform>();
        TF_Pup = Pup.GetComponent<Transform>();
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSetting()
    {
        TF_Pow.position = new Vector3(-5.6f, 0.8f, -2f);
        TF_Gun.position = new Vector3(-5.3f, 0.46f, -2f);
        TF_Hea.position = new Vector3(-4.98f, 0.14f, -2f);
        TF_Bar.position = new Vector3(-4.67f, -0.18f, -2f);
        TF_Pup.position = new Vector3(-4.35f, -0.5f, -2f);
    }
}
