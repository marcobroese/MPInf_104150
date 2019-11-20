using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Repräsentiert einen WorkStep in der Anleitung
/// </summary>
public class WorkStep : MonoBehaviour
{
    [Tooltip("Alle Tasks die zum WorkSteps gehören")]
    public Task[] tasks;
    [Tooltip("Alle GameObjekte die keine eigene Task sind aber Teil des WorkSteps")]
    public GameObject[] workStepParts;
    [Tooltip("Alle WorkSteps die abgeschlossen sind aber nicht angezeigt werden sollen")]
    public WorkStep[] workStepsToHide;
    //Referenz zum TasksDisplay
    private TasksDisplay tasksDisplay;
    //Referenz zum InstructionManual
    private InstructionManual instructionManualReference;
    //Reset Position
    private Vector3[][] reset;
    //Flag ob ein WorkStep erledigt wurde
    public bool workStepDone;
    public bool WorkStepDone
    {
        get { return workStepDone; }
        set
        {
            workStepDone = value;
            if (value)
            {
                instructionManualReference.CurrentWorkStepDone();
                instructionManualReference.CheckIfAllWorkStepsDone();
            }

        }
    }
    private void Awake()
    {
        tasksDisplay = GameObject.FindGameObjectWithTag("TasksDisplay").GetComponent<TasksDisplay>();
        instructionManualReference = this.transform.parent.GetComponent<InstructionManual>();
        reset = new Vector3[workStepParts.Length][];
        for (int i = 0; i < workStepParts.Length; i++)
        {
            {
                reset[i] = new Vector3[3];
                reset[i][0] = workStepParts[i].transform.localPosition;
                reset[i][1] = workStepParts[i].transform.localEulerAngles;
                reset[i][2] = workStepParts[i].transform.localScale;
            }
        }
    }
    /// <summary>
    /// Testet ob alle Task erfüllt wurden und setzt den Workstep auf erfüllt falls dem so ist
    /// </summary>
    public void CheckIfAllTaskDone()
    {
        tasksDisplay.UpdateDisplay();
        if (AllTaskDone())
        {
            foreach(GameObject obj in workStepParts)
            {
                ToggleAnimationComponents(obj, false);
            }
            WorkStepDone = true;
        }
    }
    /// <summary>
    /// Initialisiert den WorkStep
    /// </summary>
    public void InitWorkStep()
    {
        WorkStepDone = false;
        for (int i = 0; i < workStepParts.Length; i++)
        {
            workStepParts[i].SetActive(false);
        }
        foreach (Task t in tasks)
        {
            t.InitTask();
        }
    }
    /// <summary>
    /// Aktiviert den WorkSetp
    /// </summary>
    public void ActivateWorkStep()
    {
        WorkStepDone = false;
        ToggleWorkStepParts(true);
        ToggleHideWorkSteps(true);
        InitTasks();
        tasksDisplay.InitDisplay();
    }
    /// <summary>
    /// Setzt Transform eines Angegebenen Objektes auf angegebene Werte
    /// </summary>
    /// <param name="obj">Objekt zum zurücksetzte</param>
    /// <param name="reset">Werte auf die gesetzt werden soll</param>
    private void ResetObjectTransform(GameObject obj, Vector3[] reset)
    {
        obj.transform.localPosition = reset[0];
        obj.transform.localEulerAngles = reset[1];
        obj.transform.localScale = reset[2];
    }
    /// <summary>
    /// Gibt an ob alle Tasks erfüllt wurden im WorkStep
    /// </summary>
    /// <returns>ob alle Tasks Erledigt wurden</returns>
    private bool AllTaskDone()
    {
        foreach (Task t in tasks)
        {
            if (!t.TaskDone)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Activiert alle Tasks
    /// </summary>
    private void InitTasks()
    {
        foreach (Task t in tasks)
        {
            t.ActivateTask();
        }
    }
    /// <summary>
    /// Toggelt alle workStepParts 
    /// Setz Position zurück und togglet Animationen
    /// </summary>
    /// <param name="toogle">toggle</param>
    private void ToggleWorkStepParts(bool toogle)
    {
        for (int i = 0; i < workStepParts.Length; i++)
        {
            workStepParts[i].SetActive(toogle);
            ResetObjectTransform(workStepParts[i], reset[i]);
            ToggleAnimationComponents(workStepParts[i], toogle);
        }
    }
    /// <summary>
    /// Toggelt alle WorkSteps in workStepsToHide und togglet die Parts in den dazugehörengen Tasks
    /// </summary>
    /// <param name="toogle">Wert zu setzen</param>
    public void ToggleHideWorkSteps(bool toogle)
    {
        foreach (WorkStep ws in workStepsToHide)
        {
            foreach (GameObject obj in ws.workStepParts)
            {
                obj.SetActive(!toogle);
            }
            foreach (Task t in ws.tasks)
            {
                t.part.SetActive(!toogle);
            }
        }
    }
    /// <summary>
    /// Toggelt alle an dem angegebenen Objekt befindlichen Animations Komponenten
    /// </summary>
    /// <param name="obj">Objekt an dem Komponenten getoggelt werden sollen</param>
    /// <param name="toggle">toggle</param>
    private void ToggleAnimationComponents(GameObject obj, bool toggle)
    {
        if (obj != null)
        {
            if (obj.GetComponent<AddLineRenderer>() != null)
            {
                obj.GetComponent<AddLineRenderer>().Toggle(toggle);
                obj.GetComponent<AddLineRenderer>().enabled = toggle;
            }
            if (obj.GetComponent<Rotate>() != null)
            {
                obj.GetComponent<Rotate>().enabled = toggle;
            }
            if (obj.GetComponent<Translate>() != null)
            {
                obj.GetComponent<Translate>().enabled = toggle;
            }
            if (obj.GetComponent<Outline>() != null)
            {
                obj.GetComponent<Outline>().enabled = toggle;
            }
        }
    }
}
