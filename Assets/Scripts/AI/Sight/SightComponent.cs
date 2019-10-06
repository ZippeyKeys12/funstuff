using Unity.Collections;
using Unity.Entities;

namespace AI.Sensors.Sight
{
    public struct SightComponent : ISensorComponent
    {
        public float SightRange { get; }

        public SightComponent(float sightRange)
        {
            SightRange = sightRange;
        }
    }
}