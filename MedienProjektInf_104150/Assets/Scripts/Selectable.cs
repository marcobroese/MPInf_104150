using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Erlaubt Objekte mit per Touch oder Maus auszuwählen durch die taskReferenz
/// </summary>
public class Selectable : MonoBehaviour
{

    [Tooltip("Referenz auf die mit dem Part verbundene Task")]
    public Task taskReference;

    [Tooltip("Bild für die Textuellebeschreibung")]
    public Sprite image;
    [Tooltip("Textuellebeschreibung")]
    public string description;
    [Tooltip("Name des Parts für die Textuellebeschreibung")]
    public string partName;
}
