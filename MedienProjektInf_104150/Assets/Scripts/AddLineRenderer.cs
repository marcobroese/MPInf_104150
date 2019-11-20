using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script zum setzen der Optionen des Line Renderers.
/// Zeichnet von der aktuellen Position zur Zielposition Mittels eines LineRenderes eine Linie,
/// Eigenschaften können mittels der Parameter angepasst werden.
/// </summary>
public class AddLineRenderer : MonoBehaviour
{
    public enum LineRenderOption { Op1, Op2, Op3, Op4 }
    public enum LineRendererDir { forward, right, up };
    [Header("Settings")]
    [Tooltip("Optionen für Linie, Linien Material, Linien Dicke")]
    public LineRenderOption renderOption;

    [Tooltip("Linien Richtung, abhängig von Ausrichtung des GamebOjekts")]
    public LineRendererDir lineDirection;

    [Tooltip("Linien Länge")]
    public float lineLength = 1;


    //Referenz zu LineRendererOptions
    public Constants.LineRendererOption op;

    //Referenz zum LineRenderer
    private LineRenderer lineRenderer;
    //StartPosition der Linie
    private Vector3 startPos;
    //Linien Richtung
    private Vector3 lineDir;

    /// <summary>
    /// Bei Enable des Scripts werden Optionen für Line Renderer gesetzt.
    /// </summary>
    private void Start()
    {
        startPos = this.transform.position;
        InitLineDirection(lineDirection);
        InitLineRendererOption(renderOption);
        InitLineRenderer();
    }

    /// <summary>
    /// In Debug Mode kann Ende der Linie verändert und angepasst werden, sowie Linien Optionen.
    /// Sonst wird eine Linie von der Momentanen Position zum Ziel Geszechnet und nur die momentane Position geupdated.
    /// </summary>
    void FixedUpdate()
    {
        lineRenderer.SetPosition(0, this.transform.position);

        InitLineDirection(lineDirection);
        InitLineRendererOption(renderOption);
        lineRenderer.SetPosition(1, startPos + lineDir * lineLength);

    }
    /// <summary>
    /// Wählt unterschiedliche Anzeigeoptionen für Line Renderer aus.
    /// Unterschiedliche Anzeige Modi(Strichmaterial / Strichdicke)
    /// </summary>
    /// <param name="option">Option</param>
    private void InitLineRendererOption(LineRenderOption option)
    {
        switch (option)
        {
            case LineRenderOption.Op1:
                op = Constants.lineRenderOption1;
                break;
            case LineRenderOption.Op2:
                op = Constants.lineRenderOption2;
                break;
            case LineRenderOption.Op3:
                op = Constants.lineRenderOption3;
                break;
            case LineRenderOption.Op4:
                op = Constants.lineRenderOption4;
                break;
        }
    }
    /// <summary>
    /// Initialisiert Line Renderer, sucht LineRenderer auf GameObjekt
    /// </summary>
    private void InitLineRenderer()
    {
        if (lineRenderer == null)
        {
            lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        }
        SetLineRendererOptions();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, this.transform.position + lineDir * lineLength);

    }
    /// <summary>
    /// Setzt Line Renderer Optionen 
    /// </summary>
    private void SetLineRendererOptions()
    {
        lineRenderer.startWidth = op.stdStartWidth;
        lineRenderer.endWidth = op.stdEndWidth;
        lineRenderer.material = op.stdMaterial;
        lineRenderer.textureMode = op.textureMode;
    }

    /// <summary>
    /// Setzt die Line Zeichenrichtung.
    /// Abbhängigi von Objektausrichtung.
    /// </summary>
    /// <param name="axis">Zeichenrichtung</param>
    private void InitLineDirection(LineRendererDir axis)
    {
        switch (axis)
        {
            case LineRendererDir.forward:
                lineDir = this.transform.forward;
                break;
            case LineRendererDir.right:
                lineDir = this.transform.right;
                break;
            case LineRendererDir.up:
                lineDir = this.transform.up;
                break;
        }
    }

    /// <summary>
    /// Toggle zum aktivieren des Line Renderers
    /// </summary>
    /// <param name="toggle">toggle</param>
    public void Toggle(bool toggle)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = toggle;
        }
    }
}
