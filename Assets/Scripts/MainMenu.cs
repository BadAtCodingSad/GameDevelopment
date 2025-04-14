using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void Play() 
    {
        SceneManager.LoadScene(1);
    }

    public void Quit() 
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
