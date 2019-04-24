using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject arena;
    public GameObject player;
    public float startRadius;
    public float deathSpawnRadius;
    public int enemiesSpawnedOnDeath;
    public int enemiesToStart;
    public int enemyCount;
    public Text enemyCountText;

    private float xMax, zMax;
    private float spawnRadius;

    private void Start()
    {
        xMax = arena.transform.localScale.x * 4.7f;
        zMax = arena.transform.localScale.z * 4.7f;

        spawnRadius = Mathf.Min(xMax, zMax);

        for(int i = 0; i < enemiesToStart; i++)
        {
            spawnEnemy(Vector3.zero, startRadius);
            
        }
        enemyCount = enemiesToStart;
        spawnRadius = deathSpawnRadius;
        enemyCountText.text = "Enemy Count: " + enemyCount;
    }

    public void enemyDied(Vector3 p)
    {
        enemyCount--;
        for(int i = 0; i < enemiesSpawnedOnDeath; i++)
        {
            spawnEnemy(p, 0);
            enemyCount++;
            enemyCountText.text = "Enemy Count: " + enemyCount;
        }
    }

    private void spawnEnemy(Vector3 p, float minRadius)
    {
        Vector3 temp;
        do
        {
            temp = Random.onUnitSphere;
            temp.y = 0;
            temp.Normalize();
            temp *= Random.Range(minRadius, spawnRadius);
        } while (!inBounds(temp));

        GameObject enemy = Instantiate(enemyPrefab, p + temp, Quaternion.identity);
        enemy.GetComponent<EnemyController>().setSpawnner(this);
        enemy.GetComponent<EnemyController>().setTarget(player);
    }

    private bool inBounds(Vector3 v)
    {
        bool inBounds = true;

        if (Mathf.Abs(v.x) > xMax || Mathf.Abs(v.z) > zMax)
        {
            inBounds = false;
        }

        return inBounds;
    }
}
