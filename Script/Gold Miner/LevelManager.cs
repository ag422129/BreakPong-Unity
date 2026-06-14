using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [System.Serializable]
    public class Level
    {
        public GameObject[] HookObjects;
        public Vector3[] spawnPoints;
        public GameObject Background;
    }

    public Level[] levels;

    [System.Serializable]
    public class SecretLevel
    {
        public GameObject[] HookObjects;
        public Vector3[] spawnPoints;
        public GameObject Background;
    }

    public SecretLevel[] Secretlevels;

    public int currentLevel;

    public float levelTime = 60f;
    public float timer;

    public GM_GameManager GM_gameManager;
    public SecretShopSpawner secretSpawner;
    public Hook hook;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject LastLevelBackground;

    public GameObject exitLevelButton;
    private bool ExitLevel = false;
    public bool BoughtTressureMap = false;
    private bool Spawmed;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LevelManage(0);
        Spawmed = false;
    }
    void Update()
    {
        if (!GM_gameManager.StartTransit)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0f || ExitLevel)
        {
            hook.ResetHook();
            if (!GM_gameManager.StartTransit) { 
            NextLevel();
        }
            GM_gameManager.OnLevelTimeOver();
            ExitLevel = false;
        }

        if (BoughtTressureMap && !Spawmed)
        {
            SecretLevelManage(secretSpawner.SpawnedMapNumber);
            Spawmed = true;
        }

    }

    void LevelManage(int levelNow)
    {
        currentLevel = levelNow;

        timer = levelTime;

        ClearLevelObjects();

        Level level = levels[levelNow];

        GameObject BG = Instantiate(level.Background, new Vector3(0, 0 , 0), Quaternion.identity);
        LastLevelBackground = BG;
        for (int i = 0; i < level.HookObjects.Length; i++)
        {
            GameObject prefab = level.HookObjects[i];
            Vector3 pos = level.spawnPoints[i];
            GameObject hookable = Instantiate(prefab, pos, Quaternion.identity);
            spawnedObjects.Add(hookable);
        }
    }

    void SecretLevelManage(int levelNow)
    {

        timer = levelTime;

        ClearLevelObjects();

        SecretLevel secretlevel = Secretlevels[levelNow];

        GameObject BG = Instantiate(secretlevel.Background, new Vector3(0, 0, 0), Quaternion.identity);
        LastLevelBackground = BG;
        for (int i = 0; i < secretlevel.HookObjects.Length; i++)
        {
            GameObject prefab = secretlevel.HookObjects[i];
            Vector3 pos = secretlevel.spawnPoints[i];
            GameObject hookable = Instantiate(prefab, pos, Quaternion.identity);
            spawnedObjects.Add(hookable);
        }
    }

    void NextLevel()
    {
        RemoveProductEffects();
        exitLevelButton.SetActive(false);
        Spawmed = false;
        if (!BoughtTressureMap) { 
        currentLevel++;
        }

        if (currentLevel >= levels.Length)
        {
            Debug.Log("ALL LEVELS COMPLETE");
            return;
        }
        LevelManage(currentLevel);
    }

    void ClearLevelObjects()
    {
        Destroy(LastLevelBackground);
        foreach (GameObject obj in spawnedObjects)
        {
            if(obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }

    public void NextLevelButtonPressed()
    {
        ExitLevel = true;
        exitLevelButton.SetActive(false);
    }

    void RemoveProductEffects()
    {
        ItemManager.Instance.Power = false;
        ItemManager.Instance.luckBonus = false;
        ItemManager.Instance.Laser = false;
        ItemManager.Instance.GoldX2 = false;
        ItemManager.Instance.DiamondX2 = false;
        ItemManager.Instance.GoldenHook = false;
    }
}
