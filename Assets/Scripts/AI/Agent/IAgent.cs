using System;

namespace AI {
    public interface IAgent {
        void Think();

        T GetSensor<T>() where T : ISensor;
        ISensor GetSensor(string name);

        T GetReasoner<T>() where T : IReasoner;
        IReasoner GetReasoner(string name);

        T GetActuator<T>() where T : IActuator;
        IActuator GetActuator(string name);
    }
}