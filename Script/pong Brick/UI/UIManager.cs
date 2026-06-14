using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Ball PongBrick;
    public TextMeshProUGUI SpaceStart;
    public TextMeshProUGUI ScoreText;
    public float Score;
    public GameObject PauseMenu;
    private bool isPaused = false;
    public GameObject LoseMenu;
    public TextMeshProUGUI YouLost;
    public TextMeshProUGUI YouWon;

    private void Start()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        LoseMenu.SetActive(false);
        YouLost.gameObject.SetActive(false);
        YouWon.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (!PongBrick.gameStart)
        {
            SpaceStart.gameObject.SetActive(true);
        }
        else
        {
            SpaceStart.gameObject.SetActive(false);
        }

        ScoreText.text = "Score: " + Score;

        if(Score >= 108)
        {
            YouWon.gameObject.SetActive(true);
            LoseMenu.SetActive(true);
            Time.timeScale = 0f;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
       // Time.timeScale = 1f;
        SceneManager.LoadScene("Break Pong");
    }

    public void Lost()
    {
        LoseMenu.SetActive(true);
        YouLost.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
