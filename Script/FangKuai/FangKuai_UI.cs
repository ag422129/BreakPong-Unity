using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FangKuai_UI : MonoBehaviour
{
    public TextMeshProUGUI SpaceStart;
    public float Score = 0;
    public float TargetScore = 100;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TargetScoreText;
    public TextMeshProUGUI NextGoalText;
    public TextMeshProUGUI YouWon;
    public TextMeshProUGUI YouLost;

    private bool isPaused = false;
    public GameObject PauseMenu;
    public GameObject LoseMenu;
    public GameObject NextGoalMenu;
    public GameObject WonMenu;

    public bool GameLost = false;
    private bool GameWon = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!fangKuaiMove.GameStart)
        {
            SpaceStart.gameObject.SetActive(true);
        }
        else
        {
            SpaceStart.gameObject.SetActive(false);
        }

        ScoreText.text = "Score: " + Score;

        TargetScoreText.text = "Goal: " + TargetScore;

        NextGoalText.text = "Your Next Goal is: " + TargetScore;

        if (Score >= 1000 && !GameWon)
        {
            Won();
        }

        if (!GameWon && Score >= TargetScore)
        {
            NextGoal();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
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

    public void Resume()
    {
        PauseMenu.SetActive(false);
        NextGoalMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        GameLost = false;
        GameWon = false;
        Time.timeScale = 1f;
        Score = 0;        // 🔥 必须
        TargetScore = 10; // 🔥 必须
        fangKuaiMove.GameStart = false;
        SceneManager.LoadScene("Fang Kuai");
        Time.timeScale = 1f;
    }

    public void Lost()
    {
        LoseMenu.SetActive(true);
        YouLost.gameObject.SetActive(true);
        GameLost = true;
        Time.timeScale = 0f;
    }

    public void NextGoal()
    {
        NextGoalMenu.SetActive(true);
        Time.timeScale = 0f;
        TargetScore += 100;

    }
    public void Won()
    {
        GameWon = true;
        WonMenu.SetActive(true);
        YouWon.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

}

