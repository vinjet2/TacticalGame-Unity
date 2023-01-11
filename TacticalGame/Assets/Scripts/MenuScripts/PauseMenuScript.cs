/*  PauseMenuScript
    Permet au joueur de mettre le jeu en pause
    Fait Par: Vincent Comptois et Mathieu Beaupré
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    private Canvas pauseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas = GetComponent<Canvas>();
        pauseCanvas.enabled = false;
    }

    public void PauseMenu()
    {
        if(Time.timeScale > 0.5f)
        {
            pauseCanvas.enabled = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            pauseCanvas.enabled = false;
            Time.timeScale = 1.0f;
        }
    }
}
