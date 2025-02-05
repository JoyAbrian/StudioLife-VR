using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Button btnNormal, btnFree, btnQuit;

    private void Start()
    {
        btnNormal.onClick.AddListener(StartNormalGame);
        btnFree.onClick.AddListener(StartFreeGame);
        btnQuit.onClick.AddListener(QuitGame);
    }

    public void StartNormalGame()
    {
        GlobalVariables.gameMode = "Normal";
        GlobalVariables.playerMoney = 2000;

        SceneManager.LoadScene("GameScene");
    }

    public void StartFreeGame()
    {
        GlobalVariables.gameMode = "FreeBuild";
        GlobalVariables.playerMoney = 999999999;

        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
