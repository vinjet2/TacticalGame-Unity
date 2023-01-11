/*  LoginMenuScript
    Permet de gérer la connection du joueur au jeu et authentifiant son Username et so Mot De Passe
    Fait Par: Vincent Comptois et Mathieu Beaupré
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginMenuScript : MonoBehaviour
{
    public Button loginbtn;
    public Button registerbtn;
    public TextMeshProUGUI errorText;

    private string filePath;

    private string usernameText;
    private string passwordText;
    private string loginError = "Username or Password Error!";
    private string registerSuccess = "Successfully Registered!";
    private string registerError = "Username already exists.";

    ArrayList userInfo;

    // Start is called before the first frame update
    void Start()
    {
        errorText.enabled = false;
        filePath = Application.dataPath + "/userInformation.txt"; // Set FilePath towards User Database

        if (File.Exists(filePath)) // Check if filePath already exists or not. If not, create the file
        {
            userInfo = new ArrayList(File.ReadAllLines(filePath));
        }
        else
        {
            File.WriteAllText(filePath, "");
        }
    }

    public void setUsername(string input) // Allows TextMeshProUGUI variable to be modified so display matches
    {
        this.usernameText = input;
    }

    public void setPassword(string input) // Allows TextMeshProUGUI variable to be modified so display matches
    {
        this.passwordText = input;
    }

    public void infoUserCheck() // Register New User
    {
        bool ifExists = false;

        userInfo = new ArrayList(File.ReadAllLines(filePath));

        foreach( var i in userInfo)
        {
            if (i.ToString().Contains(usernameText))
            {
                ifExists = true;
                break;
            }
        }

        if (ifExists)
        {
            errorText.text = registerError;
            errorText.color = Color.red;
            errorText.enabled = true;
        }
        else
        {
            userInfo.Add(usernameText + ":" + passwordText);
            File.WriteAllLines(filePath, (string[])userInfo.ToArray(typeof(string)));
            errorText.text = registerSuccess;
            errorText.color = Color.green;
            errorText.enabled = true;
        }
    }

    public void loginCheck() // Checks if Login Information already exists
    {
        bool ifExists = false;

        userInfo = new ArrayList(File.ReadAllLines(filePath));

        foreach(var i in userInfo)
        {
            if (i.ToString().Substring(0, i.ToString().IndexOf(":")).Equals(usernameText) &&
                i.ToString().Substring(i.ToString().IndexOf(":") + 1).Equals(passwordText))
            {
                ifExists = true;
                break;
            }
        }

        if (ifExists) // Check if Login Info is Valid or not
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            errorText.text = loginError;
            errorText.color = Color.red;
            errorText.enabled = true;
        }
    }
}
