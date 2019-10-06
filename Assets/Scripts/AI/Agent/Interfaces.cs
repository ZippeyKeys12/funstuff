using Unity.Entities;

namespace AI
{
    public interface ISensorComponent : IComponentData
    { }

    public interface IReasoner : INamed
    {
        (IAction state, float confidence) Reason(Entity entity);
    }

    public interface IEvaluator : INamed
    {
        (IAction state, float confidence)[] Evaluate((IAction state, float confidence)[] ideas);
    }

    public interface IActuator : INamed
    {
        void Act(Entity entity, IAction[] states);
    }
}