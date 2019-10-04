using Unity.Entities;

namespace AI
{
    public interface ISensor : INamed
    {
        AIContext Sense(Entity entity);
    }

    public interface IReasoner : INamed
    {
        (IAction state, float confidence) Reason(AIContext context);
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