using System.Collections.Generic;
using UnityEngine;

public class ShopPrefabSpawn : MonoBehaviour
{
    public GameObject[] Products;
    public Vector3[] spawnPoints;

    public LevelManager levelManager;

    private int NumProducts;
    private int ShopCounts;
    List<int> SpawnedProduct = new List<int>();
    List<GameObject> SpawnedProducts = new List<GameObject>();
    public void SpawnShop()
    {
        SpawnedProduct.Clear();
        ClearShop();

        ProductFilter();

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
    }

    void ProductFilter()
    {
        if(levelManager.currentLevel >= 11)
        {
            NumProducts = Random.Range(4, 8);
            ShopCounts = 7;
        }
        else if (levelManager.currentLevel >= 8)
        {
            NumProducts = Random.Range(3, 7);
            ShopCounts = 6;
        }
        else if (levelManager.currentLevel >= 6)
        {
            NumProducts = Random.Range(2, 6);
            ShopCounts = 5;
        }
        else
        {
            NumProducts = Random.Range(1, 5);
            ShopCounts = 4;
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

    }
