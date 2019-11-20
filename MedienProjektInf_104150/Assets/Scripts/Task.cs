using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Outline = cakeslice.Outline;
/// <summary>
/// Repräsentiert eine Task in der Aufbauanleitung.
/// Eine Task beinhaltet ein Part und evtuell ein dazugehöriges Tool
/// </summary>
public class Task : MonoBehaviour
{

    [Tooltip("Part das zu der Task gehört.")]
    public GameObject part;
    [Tooltip("Tool das zu der Task gehört.")]
    public GameObject tool;
    //Ursprungs Position für ein Part
    private Vector3[] resetPart = new Vector3[3];
    //Ursprungs Position für ein Tool
    private Vector3[] resetTool = new Vector3[3];
    //Referenz zu dem WorkStep dem der Task angehört
    private WorkStep workStepReference;
    //Flag ob eine Task erledigt wurde
    public bool taskDone;
    public bool TaskDone
    {
        get { return taskDone; }
        private set
        {
            taskDone = value;
            if (value)
            {
                workStepReference.CheckIfAllTaskDone();
            }
        }
    }
    /// <summary>
    /// Zum Start wird die Ursprungsposition gespeichert
    /// und referenzen für die Selectable gesetzt
    /// </summary>
    private void Start()
    {
        workStepReference = this.transform.parent.GetComponent<WorkStep>();
        SetTool();
        SetSelectableReference();
        GetResetPositions(part, resetPart);
        GetResetPositions(tool, resetTool);
    }
    /// <summary>
    /// Setzt Referenzen in Selectable Objekt
    /// </summary>
    private void SetSelectableReference()
    {
        if (part != null && part.GetComponent<Selectable>()) part.GetComponent<Selectable>().taskReference = this;
        if (tool != null && tool.GetComponent<Selectable>()) tool.GetComponent<Selectable>().taskReference = this;
    }
    /// <summary>
    /// Setzt das Tool, falls nicht gestzt und ein Child mit Tag Tool vorhanden ist
    /// </summary>
    private void SetTool()
    {
        if (tool == null && part != null)
        {
            for (int i = 0; i < part.transform.childCount; i++)
            {
                if (part.transform.GetChild(i).CompareTag("Tool"))
                {
                    tool = part.transform.GetChild(i).gameObject;
                }
            }
        }
    }
    /// <summary>
    /// Speichert die Ursprungs Position zum angegebenem Obj
    /// </summary>
    /// <param name="obj">Obj zu dem Position gespeichert werden soll</param>
    /// <param name="reset">Vector in dem die Position gespeichert werden soll</param>
    private void GetResetPositions(GameObject obj, Vector3[] reset)
    {
        if (obj != null)
        {
            reset[0] = obj.transform.localPosition;
            reset[1] = obj.transform.localEulerAngles;
            reset[2] = obj.transform.localScale;
        }
    }
    /// <summary>
    /// Setzt die Position des angegebenen Objekt zurück
    /// </summary>
    /// <param name="obj">angegebenes Objekt</param>
    /// <param name="reset">Position zum zurücksetzen</param>
    private void ResetObjectTransform(GameObject obj, Vector3[] reset)
    {
        if (obj != null)
        {
            obj.transform.localPosition = reset[0];
            obj.transform.localEulerAngles = reset[1];
            obj.transform.localScale = reset[2];
        }
    }
    /// <summary>
    /// Ausführen einer Task 
    /// Aufruf über Touch Mausklick
    /// </summary>
    public void ExecuteTask()
    {
        ToggleAnimationComponents(part, false);
        part.GetComponent<BoxCollider>().enabled = false;
        if (tool != null)
        {
            ResetObjectTransform(tool, resetTool);
            ToggleAnimationComponents(tool, false);
            tool.SetActive(false);
        }
        TaskDone = true;
    }

    /// <summary>
    /// Initialisiert eine Task
    /// Aktiviert alle dazügehörigen GameObjekte
    /// </summary>
    public void InitTask()
    {
        TaskDone = false;
        if (tool != null)
        {
            tool.SetActive(false);
        }
        if (part != null)
        {
            part.SetActive(false);
        }
    }
    /// <summary>
    /// Aktiviert eine Task und alle dazügehörigen Animationen 
    /// </summary>
    public void ActivateTask()
    {
        TaskDone = false;
        if (tool != null)
        {
            tool.SetActive(true);
            ResetObjectTransform(tool, resetTool);
            ToggleAnimationComponents(tool, true);
        }
        if (part != null)
        {
            part.SetActive(true);
            part.GetComponent<BoxCollider>().enabled = true;
            ResetObjectTransform(part, resetPart);
            ToggleAnimationComponents(part, true);
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
