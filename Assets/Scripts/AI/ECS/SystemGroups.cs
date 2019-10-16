using Unity.Entities;

namespace AI
{
    public class SensorSystemGroup : ComponentSystemGroup
    { }

    // [UpdateAfter(typeof(SensorSystemGroup))]
    // public class InferencerSystemGroup : ComponentSystemGroup { } // TODO: After primitive AI

    [UpdateAfter(typeof(SensorSystemGroup))]
    public class ReasonerSystemGroup : ComponentSystemGroup
    { }

    // public class EvaluatorSystemGroup : ComponentSystemGroup { } // TODO: After primitive AI

    [UpdateAfter(typeof(ReasonerSystemGroup))]
    public class ActuatorSystemGroup : ComponentSystemGroup
    { }
}