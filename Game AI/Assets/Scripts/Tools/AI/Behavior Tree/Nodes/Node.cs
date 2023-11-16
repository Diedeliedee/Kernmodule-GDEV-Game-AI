using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Utilities;

namespace Joeri.Tools.AI.BehaviorTree
{
    public abstract class Node
    {
        private State m_state = State.Failure;

        private Node m_parent       = null;
        private Node[] m_children   = null;

        private Dictionary<string, object> m_dataRepository = new Dictionary<string, object>();

        public State state { get => m_state; }

        public Node parent
        {
            get
            {
                if (m_parent == null)
                    Debug.LogError($"Node of type {this} does not have a parent node.");
                return m_parent;
            }
        }
        public Node[] children
        {
            get
            {
                if (m_children == null)
                    Debug.LogError($"Node of type {this} does not have any children nodes.");
                return m_children;
            }
        }

        public Node(params Node[] childrenNodes)
        {
            //  Attach children to the node.
            m_children = childrenNodes;

            //  Attach this node as the childrens' parent.
            foreach (var node in children) node.AttachParent(this);
        }

        /// <summary>
        /// Sets the passed in node as the this node's parent.
        /// </summary>
        public void AttachParent(Node parentNode)
        {
            m_parent = parentNode;
        }

        /// <summary>
        /// Evaluates the node's desired state.
        /// </summary>
        public virtual State Evaluate()
        {
            return RetrieveState(State.Failure);
        }

        /// <returns>The passed in State, while also changing the node's sate property.</returns>
        public State RetrieveState(State stateToReturn)
        {
            m_state = stateToReturn;
            return    stateToReturn;
        }

        /// <summary>
        /// Stores a value in the node's data repository, retrievable by a key.
        /// </summary>
        public void SetData(string key, object value)
        {
            m_dataRepository[key] = value;
        }

        /// <summary>
        /// Searches in all of the node's upper hierarchy for data retrievable by the passed in key.
        /// </summary>
        /// <returns>The value associated with the key, if it has been found.</returns>
        public object GetData(string key)
        {
            var nodeToCheck = this;

            while (nodeToCheck != null)
            {
                if (m_dataRepository.TryGetValue(key, out object value))
                    return value;

                nodeToCheck = nodeToCheck.m_parent;
            }

            Debug.LogWarning($"Data '{key}' has not been found in node type {this}.");
            return null;
        }

        /// <summary>
        /// The state of a node.
        /// </summary>
        public enum State
        {
            Failure = 0,
            Running = 1,
            Succes  = 2,
        }
    }
}
