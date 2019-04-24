using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ECS
{
    [Serializable]
    public struct Tag : IComponentData
    {
        public int Value;
    }

    public class TagComponent : ComponentDataProxy<Tag> { }
}
