namespace AI
{
    public interface IAction { }

    public static class ActionUtil
    {
        public static readonly IAction NULL = new NullAction();

        private struct NullAction : IAction { }
    }
}