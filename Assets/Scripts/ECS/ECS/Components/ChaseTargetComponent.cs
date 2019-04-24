using System;
using Unity.Entities;
using UnityEngine;

namespace ECS.ECS
{
    [Serializable]
    public struct ChaseTarget : IComponentData
    {
        public float Value;
    }

    public class ChaseTargetComponent : ComponentDataProxy<ChaseTarget> { }
}
