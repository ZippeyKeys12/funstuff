using Unity.Entities;

namespace AI
{
    public interface ISensor : INamed
    {
        AIContext Sense(Entity entity);
    }

    public interface IReasoner : INamed
    {
        (AIState state, float confidence) Reason(AIContext context);
    }

    public interface IEvaluator : INamed
    {
        (AIState state, float confidence)[] Evaluate((AIState state, float confidence)[] ideas);
    }

    public interface IActuator : INamed
    {
        void Act(Entity entity, AIState[] states);
    }
}