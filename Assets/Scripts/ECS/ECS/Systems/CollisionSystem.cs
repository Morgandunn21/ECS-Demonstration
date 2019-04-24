using System;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

namespace ECS.ECS
{
    public class MyBuffer : BarrierSystem { }
    public class CollisionSystem : JobComponentSystem
    {
        public struct BulletData
        {
            public readonly int Length;  //Number of Entities with these components
            [ReadOnly] public EntityArray entities; //Array containing these entities
            [ReadOnly] public ComponentDataArray<BulletTag> tag; //Tag of entity
            [ReadOnly] public ComponentDataArray<Position> position;    //position of entity
            [ReadOnly] public ComponentDataArray<Radius> radius;    //radius of entity
        }
        [Inject] private BulletData bData;

        public struct PlayerData
        {
            public readonly int Length;  //Number of Entities with these components
            [ReadOnly] public EntityArray entities; //Array containing these entities
            [ReadOnly] public ComponentDataArray<PlayerTag> tag; //Tag of entity
            [ReadOnly] public ComponentDataArray<Position> position;    //position of entity
        }
        [Inject] private PlayerData pData;

        [Inject] private MyBuffer buffer;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            CollisionJob colJob = new CollisionJob
            {
                cmd = buffer.CreateCommandBuffer().ToConcurrent(),
                Length = bData.Length,
                playerPosition = pData.position,
                playerLength = pData.Length,
                bulletPosition = bData.position,
                bulletRadius = bData.radius
            };

            JobHandle colHandle = colJob.Schedule(this, inputDeps);


            return colHandle;
        }

        //get every enemy with these components
        public struct CollisionJob : IJobProcessComponentData<Position, Radius, EnemyTag>
        {
            [WriteOnly] public EntityCommandBuffer.Concurrent cmd;
            public int Length;
            [ReadOnly] public ComponentDataArray<Position> bulletPosition;
            [ReadOnly] public ComponentDataArray<Radius> bulletRadius;
            [ReadOnly] public int playerLength;
            [ReadOnly] public ComponentDataArray<Position> playerPosition;

            public void Execute([ReadOnly]ref Position position, [ReadOnly]ref Radius radius, [ReadOnly]ref EnemyTag tag)
            {
                float3 p1 = position.Value;
                float r1 = radius.Value;
                float3 p2;
                float r2;

                for (int i = 0; i < Length; i++)
                {
                    p2 = bulletPosition[i].Value;
                    r2 = bulletRadius[i].Value;
                    //if they are colliding
                    if (isCollision(p1, r1, p2, r2))
                    {
                        //die
                        die(position.Value);
                    }
                }

                if (playerLength > 0)
                {
                    p2 = playerPosition[0].Value;
                    r2 = r1;

                    if (isCollision(p1, r1, p2, r2))
                    {
                        GameManager.GM.playerHit();
                    }
                }
            }
            //returns if there is a collision between two spheres
            public bool isCollision(float3 p1, float r1, float3 p2, float r2)
            {
                bool collision = false;

                float dist = math.pow((p2.x - p1.x), 2) + math.pow((p2.z - p1.z), 2);
                float minDist = math.pow((r1 + r2), 2);

                if (dist < minDist*2)
                {
                    collision = true;
                }

                return collision;
            }
            //kills an enemy entity
            public void die(float3 pos)
            {
                Vector3 p = (Vector3)pos;
                GameManager.GM.deadEnemies++;
            }
        }
    }
}
