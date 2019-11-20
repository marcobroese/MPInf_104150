using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Animation für Tranlationsbewegung von Parts
/// Bewegt von der Angegebenen Position das Part zur ursprünglichen position.
/// Im DebugModus kann die Angegebenen Position zur laufzeit geändert werde.
/// </summary>
public class Translate : MonoBehaviour
{
    public enum TranslateAxis { forward, right, up };
    [Header("Settings")]
    [Tooltip("Translations richtung")]
    public TranslateAxis axis;
    [Tooltip("Translations länge")]
    public float translationLength;
    [Tooltip("Translations Geschwindigkeit")]
    public float translationSpeed;
    [Tooltip("Flag für Debug Modus")]
    public bool debug;

    // Position von der aus die Translation zum Ursprung des Parts stattfinden soll
    private Vector3 translation;
    // Ursprungsposition
    private Vector3 defaultPosition;

  
    // Start is called before the first frame update
    void OnEnable()
    {
        defaultPosition = this.transform.position;
        InitTranslation();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(debug) InitTranslation();

        if ((this.transform.position - defaultPosition).magnitude < Mathf.Sqrt(translationLength * translationLength) * 0.05f)
        {
            this.transform.Translate(translation);
        }
        this.transform.Translate(-translation * translationSpeed * Time.deltaTime);
    }
    /// <summary>
    /// Initalisiert die Translation und legt die StartPosition der Translation zum Ursprung fest 
    /// </summary>
    private void InitTranslation()
    {
        switch (axis)
        {
            case TranslateAxis.forward:
                translation = this.transform.forward * translationLength;
                break;
            case TranslateAxis.right:
                translation = this.transform.right * translationLength;
                break;
            case TranslateAxis.up:
                translation = this.transform.up * translationLength;
                break;
        }
    }
    /// <summary>
    /// Bei Disable wird die Animation auf ihren Ursprung zurückgesetzt
    /// </summary>
    private void OnDisable()
    {
        this.transform.position = defaultPosition;
    }
}
