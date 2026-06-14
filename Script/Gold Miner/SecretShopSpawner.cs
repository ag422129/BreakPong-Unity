using System.Collections.Generic;
using UnityEngine;

public class SecretShopSpawner : MonoBehaviour
{
    public GameObject[] Products;
    public GameObject[] TressureMaps;
    public Vector3[] spawnPoints;
    public Vector3 MapSpawnPoint;

    public LevelManager levelManager;

    private int NumProducts;
    private int ShopCounts;
    List<int> SpawnedProduct = new List<int>();
    List<GameObject> SpawnedProducts = new List<GameObject>();

    List<int> SpawnedMap = new List<int>();
    public bool MapSlotEmpty;
    private int mapSpawnCooldown = 10;
    public int SpawnedMapNumber;
    private void Start()
    {
        MapSlotEmpty = true;
    }
    public void SpawnSecretShop()
    {
        SpawnedProduct.Clear();
        ClearShop();

        NumProducts = Random.Range(1, 6);
        ShopCounts = 5;

        for (int i = 0; i < NumProducts; i++)
        {
            int index;

            do
            {
                index = Random.Range(0, ShopCounts);
            }
            while (SpawnedProduct.Contains(index));

            SpawnedProduct.Add(index);

            GameObject spawnedproducts = Instantiate(Products[index], spawnPoints[index], Quaternion.identity);
            SpawnedProducts.Add(spawnedproducts);
        }

        if(levelManager.currentLevel >= mapSpawnCooldown && MapSlotEmpty)
        {
            SpawnMap();
            mapSpawnCooldown += Random.Range(1, 5);
        }

    }

    void ClearShop()
    {
        foreach (GameObject obj in SpawnedProducts)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        SpawnedProducts.Clear();
    }

    void SpawnMap()
    {
        int index;
        do
        {
            index = Random.Range(0, TressureMaps.Length);
        }
        while (SpawnedMap.Contains(index));

        SpawnedMap.Add(index);
        SpawnedMapNumber = index;
        Instantiate(TressureMaps[index], MapSpawnPoint, Quaternion.identity);
        MapSlotEmpty = false;
    }

}
