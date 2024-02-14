using Joeri.Tools.AI.BehaviorTree;
using Joeri.Tools.Patterns;
using UnityEngine;
using UnityEngine.AI;

public abstract class Agent : MonoBehaviour
{
    //  Tree info streaming or something:
    protected SelfMemory m_selfMemory = new();
    protected TimeMemory m_timeMemory = new();
    protected TargetMemory m_targetMemory = new();

    //  Components:
    protected BehaviorTree m_tree;

    //  Dependencies:
    protected NavMeshAgent m_agent;
    protected Animator m_animator;

    protected virtual void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        //  Constructing blackboard.
        var blackBoard = new FittedBlackboard();

        blackBoard.Add(m_agent);
        blackBoard.Add(m_animator);
        blackBoard.Add(m_selfMemory);
        blackBoard.Add(m_timeMemory);
        blackBoard.Add(m_targetMemory);

        m_tree = CreateTree(blackBoard);
        m_tree.PassBlackboard(blackBoard);
    }

    protected abstract BehaviorTree CreateTree(FittedBlackboard _blackboard);
}
