using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Joeri.Tools.AI.BehaviorTree;
using Joeri.Tools.Patterns;

public class Guard : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private float m_detectionRange = 3f;
    [SerializeField] private float m_fieldOfView = 3f;
    [SerializeField] private int m_detectionResolution = 10;
    [Space]
    [SerializeField] private float m_damageRange = 1f;

    [Header("Hard-coded References:")]
    [SerializeField] private Transform m_viewPoint = null;
    [SerializeField] private Transform[] m_checkpoints = null;
    [SerializeField] private WeaponPickup m_weaponPickup = null;
    [Space]
    [SerializeField] private PlayerMovement m_player = null;

    //  Components:
    private BehaviorTree m_tree;

    //  Dependencies:
    private NavMeshAgent m_agent;
    private Animator m_animator;

    //  Tree info streaming or something:
    private SelfMemory m_selfMemory = new SelfMemory();
    private TimeMemory m_timeMemory = new TimeMemory();

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //  Constructing blackboard.
        var blackBoard = new FittedBlackboard();

        blackBoard.Add(m_agent);
        blackBoard.Add(m_animator);
        blackBoard.Add(m_selfMemory);
        blackBoard.Add(m_timeMemory);
        blackBoard.Add(new TargetMemory());
        blackBoard.Add(new ThreatMemory());
        blackBoard.Add(new WeaponMemory());
        blackBoard.Add(new CheckpointMemory(m_checkpoints));

        //  Constructing the branch run when the guard is in a patrolling state.
        var patrolBranch = new Sequence(
            new SetTargetToNextCheckpoint(),
            new NavigateToTarget());

        //  Constructing the branch run when the guard has noticed a threat.
        //  (Add additional hide node when a weapon cannot be located?)
        var grabWeaponBranch = new Sequence(
            new SetTargetToNearestWeapon(),
            new NavigateToTarget(),
            new RegisterWeaponInInventory());

        //  Constructing the branch run when the guard has a weapon and chases the player.
        var chasePlayerBranch = new Sequence(
            new SetTarget(m_player.transform),
            new PrioritizeSucces(
                new NavigateToTarget()),
            new InRangeOf(m_player.transform, m_damageRange),
            new Sequence(
                new DamagePlayer(),
                new Wait(1f)));

        //  Constructing general tree.
        m_tree = new BehaviorTree(
            new Selector(
                new Sequence(
                    new IsPlayerVisible(m_viewPoint, m_detectionRange, m_fieldOfView, m_detectionResolution),
                    new Selector(
                        new Sequence(
                            new DoIHaveWeapon(),
                            chasePlayerBranch),
                        grabWeaponBranch)),
                new Sequence(
                    new IsThreatPresent(),
                    new Invert(
                        new DoIHaveWeapon()),
                    grabWeaponBranch),
                patrolBranch));

        //  Passing the blackboard through the tree.
        m_tree.PassBlackboard(blackBoard);
    }

    private void FixedUpdate()
    {
        m_selfMemory.Update(transform, m_agent.velocity);
        m_timeMemory.deltaTime = Time.fixedDeltaTime;

        m_tree?.Tick();
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        m_tree.Draw(transform.position);
    }
}
