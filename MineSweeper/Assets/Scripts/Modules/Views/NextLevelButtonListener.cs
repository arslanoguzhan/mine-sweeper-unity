using UnityEngine;
using UnityEngine.SceneManagement;

class NextLevelButtonListener : MonoBehaviour,
    IListener<MouseLeftClick>,
    IListener<TouchTap>,
    IUiElement
{
    void IListener<MouseLeftClick>.OnAction()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void IListener<TouchTap>.OnAction()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}