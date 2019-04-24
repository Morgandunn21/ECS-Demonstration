using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ECS
{
    [Serializable]
    public struct BulletTag : IComponentData
    {
        public int Value;
    }

    public class BulletTagComponent : ComponentDataProxy<BulletTag> { }
}
