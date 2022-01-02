using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobbyButtonClick : MonoBehaviour
{
    public InputField IF_Chat = null;
    public GameObject Button;
    public GameObject ButtonClickManager;
    RobbyButtonManage ButtonClickManagerScript;
    public int ButtonCode;
    Renderer Rd_Button;
    // Start is called before the first frame update
    void Start()
    {
        ButtonClickManagerScript = ButtonClickManager.GetComponent<RobbyButtonManage>();
        Rd_Button = Button.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        Rd_Button.material.color = new Color(Rd_Button.material.color.r, Rd_Button.material.color.g, Rd_Button.material.color.b, 0.5f);

    }
    private void OnMouseUp()
    {
        Rd_Button.material.color = new Color(Rd_Button.material.color.r, Rd_Button.material.color.g, Rd_Button.material.color.b, 1.0f);
        switch (ButtonCode)
        {
            case 0:
                ButtonClickManagerScript.PrevButtonClicked();
                break;
            case 1:
                ButtonClickManagerScript.SendButtonClicked(IF_Chat.text);
                IF_Chat.text = "";
                break;
        }
    }
}
