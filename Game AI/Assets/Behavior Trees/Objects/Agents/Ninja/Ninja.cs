using Joeri.Tools.AI.BehaviorTree;
using Joeri.Tools.Patterns;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Agent
{
    [SerializeField] private float m_distanceFromPlayer = 3f;
    [SerializeField] private float m_distanceFromHidingSpot = 1f;
    [SerializeField] private float m_guardDetectionRange = 10f;
    [SerializeField] private float m_straightPathEpsilon = 1f;

    private SmokebombHandler m_bomb = null;

    private IHidingCover[] m_hidingCovers = null;
    private PlayerMovement m_player = null;
    private Guard m_guard = null;


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
        var followBranch = new Sequence(
            new Action(SetPlayerFollowPosition, "Setting target to Player perimeter."),
            new NavigateToTarget("Following player."));

        var hideBranch = new Routine(
            new Action(() => m_targetMemory.SetTarget(GetOptimalHidingPosition(out Vector3 _normal))),
            new NavigateToTarget("Moving to hiding spot!"));

        var throwBranch = new NonFailable(
            new Sequence(
                new Action(() => m_targetMemory.SetTarget(m_guard.transform.position)),
                new NavigateToTarget(),
                new Condition(PathIsStraight, "Peeking to throw smokebomb.."),
                new Action(() => m_bomb.ThrowBombTo(m_guard.transform.position), "Throwing bomb!")));

        return new BehaviorTree(
            new Selector(
                new Sequence(
                    new Condition(() => m_guard.isAlerted),
                    new Selector(
                        new Sequence(
                            new Condition(() => m_guard.isAttacking, "Waiting for Guard to attack."),
                            new Condition(() => m_bomb.canThrow, "Waiting for smokebomb to recharge.."),
                            throwBranch),
                        hideBranch)),
                followBranch));
    }

    private void SetPlayerFollowPosition()
    {
        var position = m_player.transform.position + (transform.position - m_player.transform.position).normalized * m_distanceFromPlayer;

        m_targetMemory.SetTarget(position);
    }

    private void SetPeekPosition()
    {

    }

    private bool PathIsStraight()
    {
        for (int i = 0; i < m_agent.path.corners.Length; i++)
        {
            if (i == 0) continue;

            var corners = m_agent.path.corners;
            var partialDir = corners[i] - corners[i - 1];
            var pathDir = corners[^1] - corners[0];

            if (Vector3.Angle(partialDir, pathDir) > m_straightPathEpsilon) return false;
        }
        return true;
    }

    private Vector3 GetOptimalHidingPosition(out Vector3 _normal)
    {
        var targetPos = m_guard.transform.position;
        var hidingSpots = new List<IHidingCover>(m_hidingCovers.Length);
        var closestHidingSpot = m_hidingCovers[0];
        var smallestDistance = Mathf.Infinity;

        for (int i = 0; i < hidingSpots.Count; i++)
        {
            var sqrDistance = (hidingSpots[i].position - targetPos).sqrMagnitude;

            //  Cull out hiding spots too far away to be valid.
            if (sqrDistance > m_guardDetectionRange * m_guardDetectionRange)
            {
                hidingSpots.RemoveAt(i);
                --i;
                continue;
            }

            //  Compare, and replace if pillar is closer.
            if (sqrDistance < smallestDistance)
            {
                closestHidingSpot = hidingSpots[i];
                smallestDistance = sqrDistance;
            }
        }

        return closestHidingSpot.GetHidingPosition(targetPos, out _normal) + _normal * m_distanceFromHidingSpot;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        m_tree.Draw(transform.position + Vector3.up * m_agent.height);
    }
}
