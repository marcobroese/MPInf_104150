using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Haupt Verwaltungs Script der Anleitung.
/// Beinhaltet alle Worksteps die nacheinander abgearbeite warden sollen.
/// </summary>
public class InstructionManual : MonoBehaviour
{
    [Tooltip("Geordnete Liste der Worksteps")]
    public WorkStep[] workSteps;

    [Tooltip("Momentan aktiver WorkStep")]
    public WorkStep currentWorkStep;
    //Referenz zum nextWorkStepButton
    private GameObject nextWorkStepButton;
    //Referenz zum startButton
    private GameObject startButton;
    //Referenz zum UI menue
    private GameObject menue;

    /// <summary>
    /// Setzt zum Start Referenzen
    /// </summary>
    private void Start()
    {
        nextWorkStepButton = GameObject.FindGameObjectWithTag("NextWorkStepButton");
        startButton = GameObject.FindGameObjectWithTag("StartButton");
        menue = GameObject.FindGameObjectWithTag("Menue");
        nextWorkStepButton.SetActive(false);
        menue.SetActive(false);
        currentWorkStep = workSteps[0];

    }
    /// <summary>
    /// Initialisiert alle WorkSteps
    /// </summary>
    public void StartManual()
    {
        foreach (WorkStep ws in workSteps)
        {
            ws.InitWorkStep();
        }
        currentWorkStep.ActivateWorkStep();
    }
    /// <summary>
    /// Wenn alle WorkSteps abgearbeitet wurden, wird der Anfangszustand wieder hergestellt
    /// </summary>
    public void CheckIfAllWorkStepsDone()
    {
        if (AllWorkStepsDone())
        {
            nextWorkStepButton.SetActive(false);
            menue.SetActive(false);
            startButton.SetActive(true);
            currentWorkStep = workSteps[0];
        }
    }
    /// <summary>
    /// Aktiviert den Nächsten Workstep, mit allen dazugehörigen Parts
    /// </summary>
    public void ActivateNextWorkStep()
    {
        nextWorkStepButton.SetActive(false);
        menue.SetActive(true);
        currentWorkStep.ToggleHideWorkSteps(false);
        currentWorkStep = NextWorkStep();
        currentWorkStep.ActivateWorkStep();
    }
    /// <summary>
    /// Gibt den nächsten Nicht abgeschlossenen Workstep zurück
    /// </summary>
    /// <returns>der nächste nicht abgeschlossene Workstep</returns>
    private WorkStep NextWorkStep()
    {
        foreach (WorkStep ws in workSteps)
        {
            if (!ws.WorkStepDone)
            {
                return ws;
            }
        }
        return workSteps[workSteps.Length - 1];
    }
    /// <summary>
    /// Wenn alle Arbeiten des Momentanen Worksteps erledigt sind, wird der Next Button Sichtbar und das menue unsichtbar.
    /// </summary>
    public void CurrentWorkStepDone()
    {
        if (currentWorkStep.WorkStepDone)
        {
            nextWorkStepButton.SetActive(true);
            menue.SetActive(false);
        }
    }
    /// <summary>
    /// Prüft, ob alle WorkSteps erledigt sind.
    /// </summary>
    /// <returns>Ergebnis, ob alle WorkSteps erledigt sind</returns>
    private bool AllWorkStepsDone()
    {
        foreach (WorkStep ws in workSteps)
        {
            if (!ws.WorkStepDone)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Setzt den Letzten WorkStep zurück.
    /// Aufruf über Button
    /// </summary>
    public void UndoLast()
    {
        foreach (Task t in currentWorkStep.tasks)
        {
            if (t.TaskDone)
            {
                currentWorkStep.ActivateWorkStep();
                return;
            }
        }

        int targetIdx = 0;
        while (workSteps[targetIdx] != currentWorkStep)
        {
            targetIdx++;
        }
        targetIdx--;
        currentWorkStep = workSteps[0];
        foreach (WorkStep ws in workSteps)
        {
            ws.InitWorkStep();
        }
        ActivateNextWorkStep();
        while (targetIdx > 0 && currentWorkStep != workSteps[targetIdx])
        {
            SkipWorkStep();
            ActivateNextWorkStep();
        }
    }
    /// <summary>
    /// Skipt den Nächsten WorkStep.
    /// Aufruf über Button
    /// </summary>
    public void SkipWorkStep()
    {
        foreach (Task t in currentWorkStep.tasks)
        {
            t.ExecuteTask();
        }
    }
}
