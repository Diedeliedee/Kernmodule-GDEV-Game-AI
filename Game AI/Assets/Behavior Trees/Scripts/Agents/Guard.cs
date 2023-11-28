using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Joeri.Tools.AI.BehaviorTree;

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
        m_tree = new BehaviorTree(
            new Sequence(
                new NavigateToTransform(agent, m_transforms[0]),
                new NavigateToTransform(agent, m_transforms[1]),
                new NavigateToTransform(agent, m_transforms[2]),
                new NavigateToTransform(agent, m_transforms[3])));
    }

    private void FixedUpdate()
    {
        m_tree?.Tick();
    }
}
