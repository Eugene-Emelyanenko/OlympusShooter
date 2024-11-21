using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject coinPrefab;
    public float gameSpeed = 2f;
    public float superGameSpeed = 4f;

    public float enemySpawnInterval = 2f;
    public float coinSpawnInterval;

    public float maxX;
    public float minX;

    private int selectedLevel = 1;
    private float moveSpeed;
    private bool canSpawn = true;

    private void Awake()
    {
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);

        if (selectedLevel == 1)
            moveSpeed = gameSpeed;
        else
            moveSpeed = superGameSpeed;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnCoin());
    }

    IEnumerator SpawnEnemy()
    {
        while (canSpawn)
        {
            // Спавн врагов с интервалом
            yield return new WaitForSeconds(enemySpawnInterval);

            // Случайная позиция спавна по X между minX и maxX
            float spawnX = Random.Range(minX, maxX);
            Vector2 spawnPosition = new Vector2(spawnX, transform.position.y);

            // Создаём врага
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Передаём скорость врагу
            Enemy enemy = newEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetSpeed(moveSpeed);
            }
        }
    }

    IEnumerator SpawnCoin()
    {
        while(canSpawn)
        {
            yield return new WaitForSeconds(coinSpawnInterval);

            // Случайная позиция спавна по X между minX и maxX
            float spawnX = Random.Range(minX, maxX);
            Vector2 spawnPosition = new Vector2(spawnX, transform.position.y);

            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            // Передаём скорость врагу
            Coin coin = newCoin.GetComponent<Coin>();
            if (coin != null)
            {
                coin.SetSpeed(moveSpeed);
            }
        }
    }

    public void StopSpawn()
    {
        canSpawn = false;
    }
}
