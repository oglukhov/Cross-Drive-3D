using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void OnDestroy()
    {
        SceneManager.LoadScene("Game");
    }
}
