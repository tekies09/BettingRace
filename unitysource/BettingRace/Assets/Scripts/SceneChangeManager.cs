using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance;
    public int code = 0; //now state code 
    GameObject Connector;
    ConnectServer ConnectorScript;
    // Start is called before the first frame update
    void Start()
    {
        Connector = GameObject.Find("ConnectManager");
        ConnectorScript = Connector.GetComponent<ConnectServer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public void GoFirst()
    {
        ConnectorScript.SceneCode = 0;
        SceneManager.LoadScene("SampleScene");
        ConnectorScript.Resetting(0);
        code = 0;
    }
    public void GoRobby()
    {

        ConnectorScript.SceneCode = 1;
        SceneManager.LoadScene("LobbyScene");
        code = 1;
    }
    public void GoRoom()
    {
        ConnectorScript.SceneCode = 2;
        SceneManager.LoadScene("RoomScene");
        code = 2;
    }
}
