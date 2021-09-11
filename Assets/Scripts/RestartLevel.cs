using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    void OnEnable()
    {
        UIManager.OnReadyToRespwn += Restart;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDisable()
    {
        UIManager.OnReadyToRespwn -= Restart;
    }
}
