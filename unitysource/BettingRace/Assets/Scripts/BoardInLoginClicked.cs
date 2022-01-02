using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardInLoginClicked : MonoBehaviour
{
    public GameObject LoginButton;
    public InputField IF_Id, IF_pw;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseUp()
    {
        Debug.Log("id : " + IF_Id.text + " pw : " + IF_pw.text);
    }
}
