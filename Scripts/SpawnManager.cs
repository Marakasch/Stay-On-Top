using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyHardPrefab;
    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI enemiesLeftText;
    public GameObject startScreen;
    public GameObject endScreen;
    private float spawnRange = 14.0f;
    private int enemyCount;
    private int hardEnemyCount;
    public int waveNumber;
    public bool isGameActive;
    
    [Header("PowerUps")]
    public GameObject[] playerBuffs;
    public int maxSpawnCounter = 3;
    public int powerUpCounter;
    public int fireUpCounter;
    public int jumpUpCounter;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            int totalEnemiesLeft = TotalEnemyCounter();
            enemiesLeftText.text = $"Enemies left: {totalEnemiesLeft}";
            
            if (enemyCount == 0)
            {
                waveNumber++;
                waveCounterText.text = $"Wave: {waveNumber}"; 
                SpawnEnemyWave(waveNumber);
                SpawnPowerUps();
                SpawnFireUps(waveNumber);
                SpawnJumpUps(waveNumber);
            }
        }
    }

    private int TotalEnemyCounter()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        hardEnemyCount = FindObjectsByType<EnemyHard>(FindObjectsSortMode.None).Length;
        enemyCount += hardEnemyCount;

        return enemyCount;
    } 
    
    private void SpawnEnemyWave(int enemiesToSpawn)
    {

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
        
        if (enemiesToSpawn % 2 == 0)
        {
            int hardEnemiesToSpawn = enemiesToSpawn / 2;
            for (int j = 0; j < hardEnemiesToSpawn; j++)
            {
                Instantiate(enemyHardPrefab, GenerateSpawnPosition(), enemyHardPrefab.transform.rotation);
            }
        }
    }
    
    private void SpawnPowerUps()
    {
        if (powerUpCounter < maxSpawnCounter)
        {
            powerUpCounter++;
            Instantiate(playerBuffs[0], GenerateSpawnPosition(), playerBuffs[0].transform.rotation);
        }
    }
    
     private void SpawnFireUps(int powerUpsToSpawn)
    {
        if (powerUpsToSpawn % 2 == 0)
        {
            if (fireUpCounter < maxSpawnCounter)
            {
                fireUpCounter++;
                Instantiate(playerBuffs[1], GenerateSpawnPosition(), playerBuffs[1].transform.rotation);
            }
        }
    }
    
    private void SpawnJumpUps(int powerUpsToSpawn)
    {
        if (powerUpsToSpawn % 3 == 0)
        {
            if (jumpUpCounter < maxSpawnCounter)
            {
                jumpUpCounter++;
                Instantiate(playerBuffs[2], GenerateSpawnPosition(), playerBuffs[2].transform.rotation);
            }
        }
    }
    
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        
        Vector3 randomPos = new Vector3(spawnPosX, 0.1f, spawnPosZ);

        return randomPos;
    }

    public void StartGame()
    {
        isGameActive = true;
        startScreen.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void GameOver()
    {
        isGameActive = false;
        endScreen.SetActive(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
