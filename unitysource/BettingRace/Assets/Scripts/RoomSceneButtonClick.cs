using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSceneButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    private RoomSceneButtonEvent RSBScript;
    public GameObject RoomSceneButtonManager;
    public GameObject Button;
    public int ButtonCode;
    Renderer Rd_Button;
    void Start()
    {
        Rd_Button = Button.GetComponent<Renderer>();
        RSBScript = RoomSceneButtonManager.GetComponent<RoomSceneButtonEvent>();
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
                RSBScript.PrevButtonClick();
                break;
            case 1:
                RSBScript.RoomChatSend();
                break;
        }
    }
}
