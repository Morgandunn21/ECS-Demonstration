using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.ECS
{
    public class RotationSystem : JobComponentSystem
    {
        struct RotationJob : IJobProcessComponentData<Position, ChaseTarget, Rotation>
        {
            public Vector3 target;

            private Vector3 pos;
            private float xDif, zDif, yRotation;
            private Quaternion q;

            public void Execute(ref Position position, [ReadOnly]ref ChaseTarget chaseTarget, ref Rotation rotation)
            {
                if (chaseTarget.Value > 0)
                {
                    pos = position.Value;
                    xDif = target.x - pos.x;
                    zDif = target.z - pos.z;

                    yRotation = Mathf.Rad2Deg * Mathf.Atan(xDif / zDif);

                    if (zDif < 0)
                    {
                        yRotation += 180;
                    }

                    q = Quaternion.Euler(0, yRotation, 0);

                    rotation.Value = q;
                }
                else if(chaseTarget.Value < 0)
                {
                    position.Value = target;
                }
            }
        }

        

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            RotationJob rotJob = new RotationJob
            {
                target = GameManager.GM.getPlayerPosition()
            };

            JobHandle rotHandle = rotJob.Schedule(this, inputDeps);

            return rotHandle;
        }
    }
}
