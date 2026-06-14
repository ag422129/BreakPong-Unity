using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SS_UIManager : MonoBehaviour
{
    public EnemySpawner Spawn;
    public TextMeshProUGUI SpaceStart;
    public TextMeshProUGUI PlayerHP;
    public TextMeshProUGUI WaveText;
    public GameObject PauseMenu;
    private bool isPaused = false;
    public GameObject LoseMenu;
    public TextMeshProUGUI YouLost;
    public TextMeshProUGUI YouWon;
    public int waveNum = 0;
    public RocketShip PlayerHealth;

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

        if (!Spawn.gameStart)
        {
            SpaceStart.gameObject.SetActive(true);
        }
        else
        {
            SpaceStart.gameObject.SetActive(false);
        }

        PlayerHP.text = "HP: " + PlayerHealth.currentHP;

        if (Spawn.Won)
        {
            YouWon.gameObject.SetActive(true);
            LoseMenu.SetActive(true);
            Time.timeScale = 0f;

        }

        if(PlayerHealth.Dead)
        {
            Lost();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void ShowWaveText()
    {
        WaveText.text = "Wave: " + waveNum;
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
        SceneManager.LoadScene("Space Shooting");
    }

    public void Lost()
    {
        LoseMenu.SetActive(true);
        YouLost.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
