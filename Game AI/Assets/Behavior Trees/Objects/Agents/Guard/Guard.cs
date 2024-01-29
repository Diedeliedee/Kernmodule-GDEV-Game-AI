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
    [SerializeField] private float m_predictionInSeconds = 1f;
    [Space]
    [SerializeField] private float m_damageRange = 1f;
    [SerializeField] private float m_targetRadius = 0.5f;

    [Header("Hard-coded References:")]
    [SerializeField] private Transform[] m_checkpoints;
    [SerializeField] private GameObject m_holster;
    [Space]
    [SerializeField] private PlayerMovement m_player;
    [SerializeField] private PlayerDetection m_detection;

    //  Components:
    private BehaviorTree m_tree;

    //  Dependencies:
    private NavMeshAgent m_agent;
    private Animator m_animator;

    //  Tree info streaming or something:
    private SelfMemory m_selfMemory = new();
    private TimeMemory m_timeMemory = new();
    private TargetMemory m_targetMemory = null;
    private ThreatMemory m_threatMemory = new();
    private WeaponMemory m_weaponMemory = new();
    private CheckpointMemory m_checkpointMemory = null;

    private void Awake()
    {
        m_targetMemory = new(m_targetRadius);
        m_checkpointMemory = new(m_checkpoints);

        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //  Constructing blackboard.
        var blackBoard = new FittedBlackboard();

        //  JOERI, keep in mind that memory classes might no be necessary if a large portion of the tree is doable with actions.
        blackBoard.Add(m_agent);
        blackBoard.Add(m_animator);
        blackBoard.Add(m_selfMemory);
        blackBoard.Add(m_timeMemory);
        blackBoard.Add(m_targetMemory);
        blackBoard.Add(m_threatMemory);

        //  Constructing the branch run when the guard has a weapon and chases the player.
        var chaseBranch = new Sequence(
            new Invert(
                new IsAnimationPlaying("ANIM_Attack")),
            new SetTarget(m_player.transform),
            new PrioritizeSucces(
                new NavigateToTarget()),
            new InRangeOf(m_player.transform, m_damageRange),
            new Action(() => m_animator.CrossFade("ANIM_Attack", 0f)));

        //  Constructing the branch run when the guard needs to arm themselves.
        //  (Add additional hide node when a weapon cannot be located?)
        var armBranch = new Routine(
            new Condition(() => !m_weaponMemory.hasWeapon),
            new Action(() => m_weaponMemory.closestWeaponPickup = FindObjectOfType<WeaponPickup>()),
            new NavigateToTarget(),
            new Action(() => { m_weaponMemory.RegisterWeapon(); m_holster.SetActive(true); }));

        //  Constructing the branch run when the guard is in a patrolling state.
        var patrolBranch = new Routine(
            new Action(() => m_targetMemory.SetTarget(m_checkpointMemory.GetNext().position)),
            new NavigateToTarget(),
            new Wait(1f));

        //  Constructing the branch run when the guard has noticed a threat.
        var searchBranch = new Routine(
            new Action(TestSetTargetToPredictedLocation),
            new NavigateToTarget(),
            new Wait(3f),
            new Action(() => m_threatMemory.Forget()));

        //  Constructing general tree.
        m_tree = new BehaviorTree(
            new Selector(
                new Sequence(
                    new Condition(() => RegisterPlayerUponDetection()),
                    new Selector(
                        armBranch,
                        chaseBranch)),
                new Sequence(
                    new Condition(() => m_threatMemory.hasSeenThreat),
                    new Selector(
                        armBranch,
                        searchBranch)),
                patrolBranch));

        //  Passing the blackboard through the tree.
        m_tree.PassBlackboard(blackBoard);

        void TestSetTargetToPredictedLocation()
        {
            m_targetMemory.SetTarget(m_threatMemory.locationPrediction);
        }
    }

    private void FixedUpdate()
    {
        m_selfMemory.Update(transform, m_agent.velocity);
        m_timeMemory.deltaTime = Time.fixedDeltaTime;

        m_tree?.Tick();
    }

    private bool RegisterPlayerUponDetection()
    {
        var playerSpotted = m_detection.PlayerDetected(out PlayerMovement _player);

        if (playerSpotted)
        {
            m_threatMemory.RegisterThreat(_player.transform, _player.velocity, m_predictionInSeconds);
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        m_tree.Draw(transform.position);
    }
}
