namespace AI {
    public interface ISensor : INamed {
        AIContext Sense(AIContext context);
    }
}