﻿using UnityEngine;
using UnityEngine.AI;
using Joeri.Tools.AI.BehaviorTree;
using Joeri.Tools.Patterns;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Guard : Agent
{
    [Header("Properties:")]
    [SerializeField] private int m_detectionResolution = 10;
    [SerializeField] private float m_predictionInSeconds = 1f;
    [Space]
    [SerializeField] private float m_damageRange = 1f;

    [Header("Hard-coded References:")]
    [SerializeField] private Transform[] m_checkpoints;
    [SerializeField] private GameObject m_holster;

    //  Behavior memory:
    private ThreatMemory m_threatMemory = new();
    private WeaponMemory m_weaponMemory = new();
    private CheckpointMemory m_checkpointMemory = null;

    //  References:
    private WeaponPickup m_pickup;
    private PlayerMovement m_player;
    private PlayerDetection m_detection;

    public bool isAlerted => m_threatMemory.hasSeenThreat;
    public bool isAttacking => m_animator.GetCurrentAnimatorStateInfo(0).IsName("ANIM_Attack");

    protected override void Awake()
    {
        base.Awake();

        m_checkpointMemory = new(m_checkpoints);

        m_pickup = FindObjectOfType<WeaponPickup>();

        m_player = FindObjectOfType<PlayerMovement>();
        m_detection = GetComponentInChildren<PlayerDetection>();
    }

    private void FixedUpdate()
    {
        m_selfMemory.Update(transform, m_agent.velocity);
        m_timeMemory.deltaTime = Time.fixedDeltaTime;

        m_tree?.Tick();
    }

    protected override BehaviorTree CreateTree(FittedBlackboard _blackboard)
    {
        //  JOERI, keep in mind that memory classes might no be necessary if a large portion of the tree is doable with actions.
        _blackboard.Add(m_threatMemory);

        //  Constructing the branch run when the guard has a weapon and chases the player.
        var chaseBranch = new NonFailable(
            new Sequence(
                new Invert(
                    new IsAnimationPlaying("ANIM_Attack")),
                new SetTarget(m_player.transform),
                new PrioritizeSucces(
                    new NavigateToTarget("Navigating to player.")),
                new InRangeOf(m_player.transform, m_damageRange),
                new Action(() => m_animator.CrossFade("ANIM_Attack", 0f))));

        //  Constructing the branch run when the guard needs to arm themselves.
        //  (Add additional hide node when a weapon cannot be located?)
        var armBranch = new Routine(
            new Condition(() => !m_weaponMemory.hasWeapon),
            new Action(() => m_targetMemory.SetTarget(m_pickup.transform.position)),
            new NavigateToTarget("Navigating to weapon."),
            new Action(() => { m_weaponMemory.RegisterWeapon(m_pickup.Pickup()); m_holster.SetActive(true); }));

        //  Constructing the branch run when the guard is in a patrolling state.
        var patrolBranch = new Routine(
            new Action(() => m_targetMemory.SetTarget(m_checkpointMemory.GetNext().position)),
            new NavigateToTarget("Navigating to next checkpoint."),
            new Wait(1f, "Idling at checkpoint."));

        //  Constructing the branch run when the guard has noticed a threat.
        var searchBranch = new Routine(
            new Action(() => m_targetMemory.SetTarget(m_threatMemory.locationPrediction)),
            new NavigateToTarget("Searching for player.."),
            new Wait(3f, "Must've been the wind."),
            new Action(() => m_threatMemory.Forget()));

        //  Constructing general tree.
        return new BehaviorTree(
            new Selector(
                new Sequence(
                    new IsAnimationPlaying("ANIM_Startled"),
                    new Wait()),
                new Sequence(
                    new Condition(() => m_detection.PlayerDetected()),
                    new Action(() => m_threatMemory.UpdateThreatInfo(m_player.transform, m_player.velocity, m_predictionInSeconds)),
                    new Selector(
                        new Sequence(
                            new Condition(() => !m_threatMemory.hasSeenThreat),
                            new Action(() => m_threatMemory.RegisterThreat()),
                            new Action(() => m_animator.CrossFade("ANIM_Startled", 0f))),
                        new Selector(
                            armBranch,
                            chaseBranch))),
                new Sequence(
                    new Condition(() => m_threatMemory.hasSeenThreat),
                    new Selector(
                        armBranch,
                        searchBranch)),
                patrolBranch));
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        m_tree.Draw(transform.position+ Vector3.up * m_agent.height);
        m_threatMemory.Draw();
    }
}
