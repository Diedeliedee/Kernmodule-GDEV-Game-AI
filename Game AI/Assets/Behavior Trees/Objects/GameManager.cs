using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
