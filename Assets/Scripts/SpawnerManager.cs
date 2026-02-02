using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public float startTimeBtwSpawn;
    private float timeBtwSpawn;
    public GameObject[] enemies;
    public WeaponManager weaponManager;
    public List<Spawner> spawners;
    private Player player;

    int maxEnemy = 3;
    int roundCount = 0;

    // Giới hạn số quái tối đa trên map
    public int maxEnemiesOnMap = 20;
    // Giới hạn maxEnemy cao nhất
    public int maxEnemyLimit = 15;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public List<int> GetRandomIndices(int n, int k)
    {
        List<int> allIndices = new List<int>();
        for (int i = 0; i < n; i++)
        {
            allIndices.Add(i);
        }

        List<int> randomIndices = new List<int>();
        int remainingItems = n;
        for (int i = 0; i < k; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, remainingItems);
            randomIndices.Add(allIndices[randomIndex]);
            allIndices[randomIndex] = allIndices[remainingItems - 1];
            remainingItems--;
        }
        return randomIndices;
    }

    private void Update()
    {
        if (timeBtwSpawn <= 0)
        {
            // THÊM: Chỉ spawn nếu số quái chưa đạt giới hạn
            if (weaponManager.Enemies.Count >= maxEnemiesOnMap)
            {
                timeBtwSpawn = startTimeBtwSpawn * 0.5f; // Kiểm tra lại sau nửa thời gian
                return;
            }

            int randEnemyCount = UnityEngine.Random.Range(2, maxEnemy);

            // SỬA: Logic spawn thêm quái
            if (weaponManager.Enemies.Count <= 5)
                randEnemyCount = UnityEngine.Random.Range(maxEnemy - 1, maxEnemy + 1);

            // THÊM: Đảm bảo không spawn quá giới hạn
            int spaceLeft = maxEnemiesOnMap - weaponManager.Enemies.Count;
            randEnemyCount = Mathf.Min(randEnemyCount, spaceLeft);

            // Đảm bảo randEnemyCount không vượt quá số spawner
            randEnemyCount = Mathf.Min(randEnemyCount, spawners.Count);

            if (randEnemyCount > 0)
            {
                List<int> randomIndex = GetRandomIndices(spawners.Count, randEnemyCount);
                foreach (int index in randomIndex)
                {
                    int randEnemy = UnityEngine.Random.Range(0, enemies.Length);
                    spawners[index].spawnEnemy(enemies[randEnemy]);
                }
            }

            timeBtwSpawn = startTimeBtwSpawn;
            roundCount++;

            if (roundCount > 10)
            {
                roundCount = 0;
                //Giới hạn maxEnemy không tăng vô hạn
                maxEnemy = Mathf.Min(maxEnemyLimit, maxEnemy + 1);
            }
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}