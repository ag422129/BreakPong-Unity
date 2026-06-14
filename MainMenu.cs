using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartBreakPong()
    {
        SceneManager.LoadScene("Break Pong");
    }

    public void StartSpaceShooting()
    {
        SceneManager.LoadScene("Space Shooting");
    }

    public void StartFangKuai()
    {
        SceneManager.LoadScene("Fang Kuai");
    }

    public void StartGoldMiner()
    {
        SceneManager.LoadScene("Gold Miner");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
