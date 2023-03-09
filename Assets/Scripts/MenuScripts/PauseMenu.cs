using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenu;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Enable&Disable Menu 

            //Change Pause GameState
        }
    }
}
