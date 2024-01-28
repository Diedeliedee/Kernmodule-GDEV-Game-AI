namespace Joeri.Tools.AI.BehaviorTree
{
    public class Wait : LeafNode
    {
        private Timer m_timer = null;

        public Wait(float _seconds)
        {
            m_timer = new Timer(_seconds);
        }

        public override State OnUpdate()
        {
            if (m_timer.ResetOnReach(board.Get<TimeMemory>().deltaTime)) return State.Succes;
            return State.Running;
        }

        public override void OnAbort()
        {
            m_timer.Reset();
        }
    }
}
