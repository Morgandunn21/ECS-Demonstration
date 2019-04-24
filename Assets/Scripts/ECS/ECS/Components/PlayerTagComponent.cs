using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS.ECS
{
    [Serializable]
    public struct PlayerTag : IComponentData
    {
        public int Value;
    }

    public class PlayerTagComponent : ComponentDataProxy<PlayerTag> { }
}
