using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundButtonClick : MonoBehaviour
{
    public InputField IF_Id=null, IF_pw=null;
    public GameObject Button;
    public GameObject ButtonClickManager;
    BackgroundButtonClickManager ButtonClickManagerScript;
    public int ButtonCode;
    Renderer Rd_Button;
    // Start is called before the first frame update
    void Start()
    {
        ButtonClickManagerScript = ButtonClickManager.GetComponent<BackgroundButtonClickManager>();
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
                ButtonClickManagerScript.LoginButtonClick();
                break;
            case 1:
                ButtonClickManagerScript.RegisterButtonClick();
                break;
            case 2:
                ButtonClickManagerScript.SettingButtonClick();
                break;
            case 3:
                ButtonClickManagerScript.PrevButtonClick();
                break;
            case 4:
                ButtonClickManagerScript.MemoryButtonClick();
                break;
            case 5:
                ButtonClickManagerScript.SoundButtonClick();
                break;
            case 6:
                ButtonClickManagerScript.IdCheckButtonClick(IF_Id.text);
                break;
            case 7:
                ButtonClickManagerScript.RegisterButtonClick(IF_Id.text,IF_pw.text);
                break;
            case 8:
                ButtonClickManagerScript.LoginButtonClick(IF_Id.text, IF_pw.text);
                break;
            case 9:
                ButtonClickManagerScript.IdCheckSuccessBoardOKButtonClick();
                break;
            case 10:
                ButtonClickManagerScript.IdCheckFailBoardOKButtonClick();
                break;
            case 11:
                ButtonClickManagerScript.RegisterSuccessBoardOKButtonClick();
                break;
            case 12:
                ButtonClickManagerScript.RegisterFailBoardOKButtonClick();
                break;
            case 13:
                ButtonClickManagerScript.LoginSuccessBoardOKButtonClick();
                break;
            case 14:
                ButtonClickManagerScript.LoginFailBoardOKButtonClick();
                break;
            default:
                break;
        }
    }
}
