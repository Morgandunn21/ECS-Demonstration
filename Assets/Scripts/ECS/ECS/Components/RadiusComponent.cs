using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ECS
{
    [Serializable]
    public struct Radius : IComponentData
    {
        public float Value;
    }

    public class RadiusComponent : ComponentDataProxy<Radius> { }
}
