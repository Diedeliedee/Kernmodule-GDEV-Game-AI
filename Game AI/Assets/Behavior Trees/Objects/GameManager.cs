using Joeri.Tools.Structure;
using UnityEngine.SceneManagement;

namespace GameAI.BehaviorSystem
{
    public class GameManager : Singleton<GameManager>
    {
        public State state { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void LoadScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }

        public void EndGame()
        {
            state = State.Over;
        }

        public enum State
        {
            Running,
            Over
        }
    }
}