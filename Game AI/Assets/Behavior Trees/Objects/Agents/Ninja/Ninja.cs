using Joeri.Tools.AI.BehaviorTree;
using Joeri.Tools.Patterns;
using Joeri.Tools.Utilities;
using UnityEngine;

namespace GameAI.BehaviorSystem
{
    public class Ninja : Agent
    {
        [SerializeField] private float m_distanceFromPlayer = 3f;
        [SerializeField] private float m_distanceFromHidingSpot = 1f;
        [SerializeField] private float m_guardDetectionRange = 10f;
        [SerializeField] private float m_straightPathEpsilon = 1f;
        [SerializeField] private float m_straightLookEpsilon = 30f;

        private SmokebombHandler m_bomb = null;

        private IHidingCover[] m_hidingCovers = null;
        private PlayerMovement m_player = null;
        private Guard m_guard = null;


        private IHidingCover m_chosenHidingSpot = null;
        private bool m_sawGuardAttack = false;

        protected override void Awake()
        {
            base.Awake();

            m_bomb = GetComponentInChildren<SmokebombHandler>();

            m_hidingCovers = FindObjectsOfType<Pillar>();
            m_player = FindObjectOfType<PlayerMovement>();
            m_guard = FindObjectOfType<Guard>();
        }

        private void FixedUpdate()
        {
            m_tree.Tick();
        }

        protected override BehaviorTree CreateTree(FittedBlackboard _blackboard)
        {
            //  Branch for when all is well and the guard is unaware of the team.
            var followBranch = new Sequence(
            new Action(SetPlayerFollowPosition, "Setting target to Player perimeter."),
            new NavigateToTarget("Following player."));

            //  Branch for when the guard has spotted the player.
            var hideBranch = new NonFailable(
            new Routine(
                new Action(() => m_chosenHidingSpot = GetOptimalHidingSpot(), "Choosing optimal hiding spot."),
                new Sequence(
                    new Action(() => m_targetMemory.SetTarget(m_chosenHidingSpot.GetHidingPosition(m_guard.transform.position, out Vector3 _normal) + _normal * m_distanceFromHidingSpot), "Setting Target to hiding spot!"),
                    new NavigateToTarget("Moving to hiding spot!"),
                    new PrioritizeRunning(
                        new Condition(IsGuardInRange, "Can I still see the guard?")))));

            //  Branch for when it's time to take action, and throw a smokebomb.
            var throwBranch = new NonFailable(
            new Sequence(
                new Action(() => m_targetMemory.SetTarget(m_guard.transform.position), "Setting Target to guard!"),
                new PrioritizeSucces(
                    new NavigateToTarget("Peeking to throw smokebomb..")),
                new Condition(IsGuardVisible, "Can I throw from here?"),
                new Action(() => m_bomb.ThrowBombTo(m_guard.transform.position), "Throwing bomb!"),
                new Action(() => m_sawGuardAttack = false, "Forgetting Guard's attack..")));

            //  Setup of general tree.
            return new BehaviorTree(
                new Selector(
                    new Sequence(
                        new Condition(() => m_sawGuardAttack),
                        new Condition(() => m_bomb.canThrow),
                        throwBranch),
                    new Sequence(
                        new Condition(() => m_guard.isAlerted),
                        new AlwaysSucces(
                            new Sequence(
                                new Condition(() => m_guard.isAttacking),
                                new Action(() => m_sawGuardAttack = true))),
                        hideBranch),
                        followBranch));
        }

        private void SetPlayerFollowPosition()
        {
            var position = m_player.transform.position + (transform.position - m_player.transform.position).normalized * m_distanceFromPlayer;

            m_targetMemory.SetTarget(position);
        }

        private bool IsGuardVisible()
        {
            //  Maybe move this to be a spherecast in the bomb handler itself.

            if (Vector2.Angle(transform.position.Planar().ToDirection(m_guard.transform.position.Planar()), transform.forward.Planar()) > m_straightLookEpsilon) return false;
            if (m_agent.path.corners.Length < 2) return false;
            for (int i = 0; i < m_agent.path.corners.Length; i++)
            {
                if (i == 0) continue;

                var corners = m_agent.path.corners;
                var partialDir = corners[i] - corners[i - 1];
                var pathDir = corners[^1] - corners[0];

                if (Vector2.Angle(partialDir.Planar(), pathDir.Planar()) > m_straightPathEpsilon) return false;
            }
            return true;
        }

        private bool IsGuardInRange()
        {
            var distance = Vector2.Distance(transform.position.Planar(), m_guard.transform.position.Planar());

            return distance < m_guardDetectionRange;
        }

        private IHidingCover GetOptimalHidingSpot()
        {
            var targetPos = m_guard.transform.position;
            var closestHidingSpot = m_hidingCovers[0];
            var smallestDistance = Mathf.Infinity;

            for (int i = 0; i < m_hidingCovers.Length; i++)
            {
                var sqrDistance = (m_hidingCovers[i].position - targetPos).sqrMagnitude;

                //  Compare, and replace if pillar is closer.
                if (sqrDistance < smallestDistance)
                {
                    closestHidingSpot = m_hidingCovers[i];
                    smallestDistance = sqrDistance;
                }
            }
            return closestHidingSpot;
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            m_tree.Draw(transform.position + Vector3.up * m_agent.height);
        }
    }
}