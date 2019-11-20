using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stellt den Zustand für den Startbildschirm her
/// </summary>
public class Initialisation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToggleAnimations(false);
    }
    private void ToggleAnimations(bool toogle)
    {
        ToggleAnimationComponents(toogle);
        foreach (GameObject tool in GameObject.FindGameObjectsWithTag("Tool"))
        {
            tool.SetActive(toogle);
        }
    }
    /// <summary>
    /// Aktiviert/Deaktiviert alle Animations Komponenten an den Parts
    /// </summary>
    /// <param name="toggle">zu setzender Zustand</param>
    private void ToggleAnimationComponents(bool toggle)
    {
        foreach (AddLineRenderer a in this.GetComponentsInChildren<AddLineRenderer>())
        {
            a.Toggle(toggle);
            a.enabled = toggle;
        }
        foreach (Rotate r in this.GetComponentsInChildren<Rotate>())
        {
            r.enabled = toggle;
        }
        foreach (Translate t in this.GetComponentsInChildren<Translate>())
        {
            t.enabled = toggle;
        }
        foreach (Outline o in this.GetComponentsInChildren<Outline>())
        {
            o.enabled = toggle;
        }
    }
}
