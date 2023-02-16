using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
/// Classe pour charger une scene en passant une string
/// lorsque l'on appuie sur le button Start
///</summary>

public class StartSelectPanel : MonoBehaviour 
{
    [SerializeField] protected string sceneToLoad;

    public void OnGUI() {
        if (sceneToLoad == null) return;
    }

    public void StartGame() {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}