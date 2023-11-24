namespace Joeri.Tools.AI.BehaviorTree
{
    public interface INode
    {
        /// <returns>The state that the evaluation resulted in.</returns>
        public State Evaluate();
    }
}
