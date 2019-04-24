using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ECS
{
    [Serializable]
    public struct EnemyTag : IComponentData
    {
        public int Value;
    }

    public class EnemyTagComponent : ComponentDataProxy<EnemyTag> { }
}
