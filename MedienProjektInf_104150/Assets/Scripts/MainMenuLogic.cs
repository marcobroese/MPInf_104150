using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Erzeugt im MainMenue ein Prefab zum Scenen wechsel.
/// Für jede Scene muss ein Sprite und eine Name angegeben werden.
/// </summary>
public class MainMenuLogic : MonoBehaviour
{

    [Tooltip("Sprites für Scenes")]
    public Sprite[] sceneSprites;
    [Tooltip("Nemen der Scenes")]
    public string[] sceneNames;
    [Tooltip("Referenz zum Display")]
    public Transform display;
    [Tooltip("Prefab für die Einträge im Menue")]
    public GameObject scenePrefab;

    // Position für Prefebs auf dem Dispaly
    private Vector2 pos;

    private void Start()
    {
        InitDisplay();
        pos = new Vector2();
    }
    /// <summary>
    /// Beendet die App
    /// Aufruf über Button
    /// </summary>
    public void QuitApp()
    {
        Application.Quit();
    }
    /// <summary>
    /// Erzeugt für jeden angegbenen Sprite ein Eintrag, in der Liste der verfügbaren Scenen
    /// </summary>
    public void InitDisplay()
    {
        foreach (Transform child in display.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < sceneSprites.Length; i++)
        {
            GameObject instance = Instantiate(scenePrefab, display);
            instance.GetComponent<SelectableScene>().SetContent(sceneSprites[i], sceneNames[i]);
            if (i % 2 == 0)
            {
                pos.x = -220;
                if (i != 0) pos.y -= 420;
            }
            else
            {
                pos.x = 220;
            }

            instance.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
