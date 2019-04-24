using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.ECS
{
    public class GameManager : MonoBehaviour
    {
        EntityManager manager;
        public static GameManager GM;
        

        [Header("Bullet Attributes")]
        public GameObject bulletPrefab;
        public float bulletRadius;
        public float bulletSpeed;

        [Header("Enemy Attributes")]
        public float enemyRadius;
        public float enemySpeed;
        public float dirChangeCD;
        public int deadEnemies;
        public GameObject player;
        public GameObject playerColliderPrefab;

        [Header("Spawner Attributes")]
        public GameObject enemyPrefab;
        public GameObject arena;
        public float startRadius;
        public float deathSpawnRadius;
        public int enemiesSpawnedOnDeath;
        public int enemiesToStart;

        [Header("GUI Attributes")]
        public Text enemyCounter;

        private int enemyCount;
        private float randomX, randomZ;
        private float xMax, zMax;
        private float spawnRadius;
        private Vector3 playerPos;
        private int playerIsHit;

        private void Start()
        {
            GM = this;
            deadEnemies = 0;
            playerIsHit = 0;

            manager = World.Active.GetOrCreateManager<EntityManager>();

            playerPos = player.GetComponent<Transform>().position;

            var entity = manager.Instantiate(playerColliderPrefab);

            //initialize components
            manager.SetComponentData(entity, new Position { Value = playerPos });
            manager.SetComponentData(entity, new Rotation { Value = player.GetComponent<Transform>().rotation });
            manager.SetComponentData(entity, new MoveSpeed { Value = 0});
            manager.SetComponentData(entity, new ChaseTarget { Value = -1 });
            manager.SetComponentData(entity, new Radius { Value = enemyRadius });
            manager.SetComponentData(entity, new PlayerTag { Value = 1 });
            manager.SetComponentData(entity, new DirChange { randDir = float3.zero, BaseCD = 0.0f, CurrentCD = 0f });

            spawnerStart();
        }

        private void Update()
        {
            randomX = UnityEngine.Random.Range(-1f, 1f);
            randomZ = UnityEngine.Random.Range(0.5f, 2f);
            playerPos = player.GetComponent<Transform>().position;

            while(deadEnemies > 0)
            {
                enemyDied(Vector3.zero);
                deadEnemies--;
            }

            while(playerIsHit > 0)
            {
                player.GetComponent<PlayerControllerECS>().hit();
                playerIsHit--;
            }

            enemyCounter.text = "Enemies: " + enemyCount;
        }

        private void spawnerStart()
        {
            xMax = arena.transform.localScale.x * 4.7f;
            zMax = arena.transform.localScale.z * 4.7f;

            spawnRadius = Mathf.Min(xMax, zMax);

            spawnEnemy(Vector3.zero, startRadius, enemiesToStart);

            spawnRadius = deathSpawnRadius;
        }

        public void enemyDied(Vector3 p)
        {
            GM.spawnEnemy(p, 0, GM.enemiesSpawnedOnDeath-1);
        }

        private void spawnEnemy(Vector3 p, float minRadius, int count)
        {
            enemyCount += count;

            NativeArray<Entity> entities = new NativeArray<Entity>(count, Allocator.Temp);
            manager.Instantiate(enemyPrefab, entities);

            Vector3 temp;

            for (int i = 0; i < count; i++)
            {
                do
                {
                    temp = UnityEngine.Random.onUnitSphere;
                    temp.y = 0;
                    temp.Normalize();
                    temp *= UnityEngine.Random.Range(minRadius, spawnRadius);
                } while (!inBounds(temp));
                //initialize components
                manager.SetComponentData(entities[i], new Position { Value = new float3(temp + p) });
                manager.SetComponentData(entities[i], new Rotation { Value = Quaternion.identity });
                manager.SetComponentData(entities[i], new Scale { Value = new float3(enemyRadius * 2) });
                manager.SetComponentData(entities[i], new MoveSpeed { Value = enemySpeed });
                manager.SetComponentData(entities[i], new ChaseTarget { Value = 1 });
                manager.SetComponentData(entities[i], new Radius { Value = enemyRadius });
                manager.SetComponentData(entities[i], new EnemyTag { Value = 1 });
                manager.SetComponentData(entities[i], new DirChange { randDir = float3.zero, BaseCD = 1.0f, CurrentCD = 0f });
            }

            entities.Dispose();
        }

        public void spawnBullet(Vector3 p, Quaternion q)
        {
            var entity = manager.Instantiate(bulletPrefab);

            //initialize components
            manager.SetComponentData(entity, new Position { Value = p });
            manager.SetComponentData(entity, new Rotation { Value = q });
            manager.SetComponentData(entity, new Scale { Value = new float3(bulletRadius * 2) });
            manager.SetComponentData(entity, new MoveSpeed { Value = bulletSpeed });
            manager.SetComponentData(entity, new ChaseTarget { Value = 0 });
            manager.SetComponentData(entity, new Radius { Value = bulletRadius });
            manager.SetComponentData(entity, new BulletTag { Value = 1 });
            manager.SetComponentData(entity, new DirChange { randDir = float3.zero, BaseCD = 0.0f, CurrentCD = 0f });
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

        public Vector3 getPlayerPosition()
        {
            return player.GetComponent<Transform>().position;
        }

        public float getRandomX()
        {
            return randomX;
        }

        public float getRandomZ()
        {
            return randomZ;
        }

        public void playerHit()
        {
            playerIsHit++;
        }
    }
}