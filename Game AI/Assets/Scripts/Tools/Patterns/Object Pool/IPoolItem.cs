namespace Joeri.Tools.Patterns.ObjectPool
{
    public interface IPoolItem
    {
        public void OnCreate();

        public void OnSpawn();

        public void OnDespawn();
    }
}