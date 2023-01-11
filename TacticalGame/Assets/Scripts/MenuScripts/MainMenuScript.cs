/*  MainMenuScript
    Permet de g�rer le menu principale et faire fonctionn� tout les boutons correctement.
    Fait Par: Vincent Comptois et Mathieu Beaupr�
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Canvas teamMenu;
    private Canvas mainMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuCanvas = GetComponent<Canvas>();
        teamMenu.enabled = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PlayTest");
    }

    public void TeamMenuChange()
    {
        mainMenuCanvas.enabled = false;
        teamMenu.enabled = true;
    }

    public void BackToMainMenu()
    {
        teamMenu.enabled = false;
        mainMenuCanvas.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit(); // Debug.Log("Quit Game!") Code de debug car la commande de Quit ne fonctionne pas dans l'engin Unity
    }
}
