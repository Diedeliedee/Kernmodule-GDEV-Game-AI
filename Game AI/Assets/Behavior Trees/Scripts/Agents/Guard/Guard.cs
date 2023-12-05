using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Joeri.Tools.AI.BehaviorTree;
using Joeri.Tools.Patterns;

public class Guard : MonoBehaviour
{
    //  Hard-coded references.
    [SerializeField] private Transform[] m_transforms = null;
    [SerializeField] private WeaponPickup m_weaponPickup = null;

    //  Components:
    private BehaviorTree m_tree;

    //  Dependencies:
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //  Constructing blackboard.
        var blackBoard = new FittedBlackboard();

        blackBoard.Add(agent);
        blackBoard.Add(animator);
        blackBoard.Add(new ThreatMemory());

        //  Constructing the branch run when the guard is in a patrolling state.
        var patrolBranch = new Sequence(
            new SetTarget(m_transforms[0].position),
            new NavigateToTarget(),
            new SetTarget(m_transforms[1].position),
            new NavigateToTarget(),
            new SetTarget(m_transforms[2].position),
            new NavigateToTarget(),
            new SetTarget(m_transforms[3].position),
            new NavigateToTarget());

        //  Constructing the branch run when the guard has noticed a threat.
        var grabWeaponBranch = new Sequence(
            new SetTargetToNearestWeapon(),
            new NavigateToTarget(),
            //  Add weapon to combat component.
            );

        var chasePlayerBranch = new Sequence(
            //  Set target to player
            //  Check if player is within attack range
            //  Attack if player is within attack range.

        m_tree = new BehaviorTree();
    }

    private void FixedUpdate()
    {
        m_tree?.Tick();
    }
}
