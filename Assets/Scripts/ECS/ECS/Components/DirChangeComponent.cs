using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ECS
{
    [Serializable]
    public struct DirChange : IComponentData
    {
        public float3 randDir;
        public float BaseCD;
        public float CurrentCD;
    }

    public class DirChangeComponent : ComponentDataProxy<DirChange> { }
}
