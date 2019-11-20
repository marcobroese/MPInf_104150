using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Animation für Tranlationsbewegung von Parts
/// Bewegt von der Angegebenen Position das Part zur ursprünglichen position.
/// 
/// bei einer Rotation um (-90,0,0) stellt Unity diese als (-90,(-/+)90, (+/-)90) dar, 
/// durch float Ungenauigkeiten verändern sich die Richtungsvektoren des Transform Objektes.
/// Und es kommt zu einem Ungewoltem verhalten bei der Translation. Um dieses zu verhindern kann das Flag für fixedAxis eingeschaltet werden. 
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
    [Tooltip("Flag für fixed Axis Modus")]
    public bool fixedAxisOff;

    // Position von der aus die Translation zum Ursprung des Parts stattfinden soll
    private Vector3 translation;
    // Ursprungsposition
    private Vector3 defaultPosition;

    private bool isInizalized=false;
  
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = this.transform.localPosition;
        InitTranslation();
        isInizalized = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(fixedAxisOff) InitTranslation();

        if ((this.transform.localPosition - defaultPosition).magnitude < Mathf.Sqrt(translationLength * translationLength) * 0.05f)
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
        if(isInizalized)  this.transform.localPosition= defaultPosition;
    }
}
