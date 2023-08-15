using Unity.Entities;

namespace AI.Agent
{
    public interface IReasoner : INamed
    {
        (IAction state, float confidence) Reason(Entity entity);
    }

    public interface IEvaluator : INamed
    {
        (IAction state, float confidence)[] Evaluate((IAction state, float confidence)[] ideas);
    }
}