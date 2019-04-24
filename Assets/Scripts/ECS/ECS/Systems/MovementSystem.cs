using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.ECS
{
    public class MovementSystem : JobComponentSystem
    {
        //get every enemy with these components
        public struct MovementJob : IJobProcessComponentData<MoveSpeed, Rotation, Position, DirChange>
        {
            public float deltaTime;
            public float randx, randz;
            public float3 value;
            public float3 randForward;

            public void Execute([ReadOnly]ref MoveSpeed speed, [ReadOnly]ref Rotation rotation, ref Position position, ref DirChange cd)
            {
                if (speed.Value != 0)
                {
                    value = position.Value;

                    if (cd.BaseCD > 0 && cd.CurrentCD <= 0)
                    {                        //(-1 to 1) (1/2 to 2)
                        cd.randDir = new float3(randx, 0f, randz);
                        cd.CurrentCD = cd.BaseCD;
                    }
                    else
                    {
                        cd.CurrentCD -= deltaTime;
                    }

                    randForward = getForwardDirection(rotation.Value, cd.randDir);

                    float3 direction = (math.forward(rotation.Value) + randForward);

                    value += deltaTime * speed.Value * direction;

                    position.Value = value;
                }
            }

            public float3 getForwardDirection(Quaternion rotation, float3 direction)
            {
                float3 forward = math.forward(rotation);
                float theta = math.atan(forward.z / forward.x);
                if(forward.z < 0)
                {
                    theta += (float)math.PI;
                }
                float cosTheta = math.cos(theta);
                float sinTheta = math.sin(theta);
                //mathhhhhh
                forward.x = sinTheta * direction.z + cosTheta * direction.x;
                forward.z = cosTheta * direction.z - sinTheta * direction.x;

                return forward;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob moveJob = new MovementJob
            {
                randx = GameManager.GM.getRandomX(),
                randz = GameManager.GM.getRandomZ(),
                deltaTime = Time.deltaTime
            };


            JobHandle moveHandle = moveJob.Schedule(this, inputDeps);

            return moveHandle;
        }
    }
}