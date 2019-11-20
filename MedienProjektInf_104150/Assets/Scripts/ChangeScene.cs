using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script zum Ändern der Scene für Buttons
/// </summary>
public class ChangeScene : MonoBehaviour
{
    /// <summary>
    /// Wechselt zur angegebenen Scene
    /// </summary>
    /// <param name="sceneName">Scenen Name</param>
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
