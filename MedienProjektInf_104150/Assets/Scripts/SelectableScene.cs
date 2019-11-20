using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script für das Scene Prefab setzt den Content des Prefabs.
/// Erlaubt das wechseln der Scene über Mausklick / Touch
/// </summary>
public class SelectableScene : MonoBehaviour
{
    public string sceneName;
    /// <summary>
    /// Setzt die Informationen im Prefab
    /// </summary>
    /// <param name="sceneImage">zusetzendes Bild</param>
    /// <param name="name">zusetzendes Name</param>
    public void SetContent(Sprite sceneImage, string name)
    {
        sceneName = name;
        this.GetComponentInChildren<Image>().sprite = sceneImage;
        this.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }
    /// <summary>
    /// Wechselt die Scene 
    /// </summary>
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
