using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script für die Textuelle anzeige der Tasks
/// Erzegt Prefabs die entsprechend dem Status der Task sind
/// </summary>
public class TasksDisplay : MonoBehaviour
{
    //Referenz zum InstructionManual
    public InstructionManual instructionManual;
    [Tooltip("Referenz zum Prefab für eine Abgeschlossene Task")]
    public GameObject prefabDone;
    [Tooltip("Referenz zum Prefab für eine nicht Abgeschlossene Task")]
    public GameObject prefabNotDone;
    //Referenz zum display
    public Transform display;
    //Lister aller Tasks die angezeigt werden
    public GameObject[] tasksPrefabs;

    /// <summary>
    /// Akualisiert das Display und erzeugt neue Einträge
    /// Es wird das Prefab genutzt das dem Status der Task entspricht
    /// </summary>
    public void UpdateDisplay()
    {
        for (int i = 0; i < tasksPrefabs.Length; i++)
        {
            if (instructionManual.currentWorkStep.tasks[i].TaskDone)
            {
                if (!tasksPrefabs[i].GetComponent<TaskText>().done)
                {
                    Destroy(tasksPrefabs[i]);
                    GameObject instance = InstanziateTMP(prefabDone);
                    tasksPrefabs[i] = instance;
                    instance.GetComponent<TaskText>().tmpBauteil.text = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().partName;
                }
            }
            else
            {
                if (tasksPrefabs[i].GetComponent<TaskText>().done)
                {
                    Destroy(tasksPrefabs[i]);
                    GameObject instance = InstanziateTMP(prefabNotDone);
                    tasksPrefabs[i] = instance;
                    instance.GetComponent<TaskText>().image.sprite = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().image;
                    instance.GetComponent<TaskText>().tmpBauteil.text = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().partName;
                    instance.GetComponent<TaskText>().tmpBeschreibung.text = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().description;
                }
            }
        }
        UpdatePositions();
    }
    /// <summary>
    /// Aktualisiert die Höhen der Tasks nachdem eine Task absgeschlossen wurde
    /// </summary>
    private void UpdatePositions()
    {

        int posY = 0;
        foreach (GameObject obj in tasksPrefabs)
        {
            obj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, posY, 0);
            posY -= obj.GetComponent<TaskText>().done ? 55 : 110;
        }
        display.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -posY);
    }

    /// <summary>
    /// Initialisiert das Display mit Prefabs für nicht abgeschlossene Tasks
    /// </summary>
    public void InitDisplay()
    {
        foreach (Transform child in display.transform)
        {
            Destroy(child.gameObject);
        }
        tasksPrefabs = new GameObject[instructionManual.currentWorkStep.tasks.Length];
        for (int i = 0; i < tasksPrefabs.Length; i++)
        {
            GameObject instance = InstanziateTMP(prefabNotDone);
            tasksPrefabs[i] = instance;
            instance.GetComponent<TaskText>().image.sprite = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().image;

            instance.GetComponent<TaskText>().tmpBauteil.text = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().partName;
            instance.GetComponent<TaskText>().tmpBeschreibung.text = instructionManual.currentWorkStep.tasks[i].part.GetComponent<Selectable>().description;
        }
        UpdatePositions();
    }

    /// <summary>
    /// Instanziert ein neues prefab und setzt Texte entsprechend des Prefabs
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns>instance</returns>
    public GameObject InstanziateTMP(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, display);

        instance.GetComponent<TaskText>().tmpBauteil = instance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (instance.transform.childCount > 2) instance.GetComponent<TaskText>().tmpBeschreibung = instance.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        return instance;
    }

}
