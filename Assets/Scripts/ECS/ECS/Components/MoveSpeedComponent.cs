using System;
using Unity.Entities;

namespace ECS.ECS
{
    [Serializable]
    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }

    public class MoveSpeedComponent : ComponentDataProxy<MoveSpeed> { }
}