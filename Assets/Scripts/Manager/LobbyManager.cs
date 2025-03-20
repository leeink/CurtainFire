using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : Singleton<LobbyManager>
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.Home))
            {
                //홈버튼
            }
            if (Input.GetKeyDown(KeyCode.Menu))
            {
                //메뉴 버튼
            }
        }
    }

    public void GameStart()
    {
        LoadingController.LoadScene("Scenes/GameScene");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
