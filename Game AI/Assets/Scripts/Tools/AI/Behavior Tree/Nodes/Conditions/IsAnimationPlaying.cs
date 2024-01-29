using UnityEngine;
using Joeri.Tools.AI.BehaviorTree;

public class IsAnimationPlaying : LeafNode
{
    private readonly string m_name = null;

    public IsAnimationPlaying(string _name)
    {
        m_name = _name;
    }

    public override State OnUpdate()
    {
        if (board.Get<Animator>().GetCurrentAnimatorStateInfo(0).IsName(m_name)) return State.Succes;
        return State.Failure;
    }
}
