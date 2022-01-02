using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButtonClick : MonoBehaviour
{
    private RoomButtonManager RBMScript;
    public GameObject RoomButtonManage;
    public GameObject Button;
    public int ButtonCode;
    Renderer Rd_Button;
    // Start is called before the first frame update
    void Start()
    {
        Rd_Button = Button.GetComponent<Renderer>();
        RBMScript = RoomButtonManage.GetComponent<RoomButtonManager>();
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
                RBMScript.PrevButtonClick();
                break;
            case 1:
                RBMScript.NextButtonClick();
                break;
            case 2:
                RBMScript.OKButtonClick();
                break;
            case 3:
                RBMScript.NewRoomButtonClick();
                break;
            case 4:
                RBMScript.RoomCreateButtonClick();
                break;
        }
    }
}
