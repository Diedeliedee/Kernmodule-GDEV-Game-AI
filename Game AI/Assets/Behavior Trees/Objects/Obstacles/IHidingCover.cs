using UnityEngine;

namespace GameAI.BehaviorSystem
{
    public interface IHidingCover
    {
        public Vector3 position { get; }

        public Vector3 GetHidingPosition(Vector3 _from, out Vector3 _normal);
    }
}