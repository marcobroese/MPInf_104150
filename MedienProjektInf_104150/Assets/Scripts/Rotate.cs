using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Animation für Rotation von Parts
/// Rotiert das Part um die angegebene Achse, Richtung und Geschwindigkeit.
/// Im DebugModus kann die Angegebenen Rotationsparameter zur Laufzeit geändert werde.
/// </summary>
public class Rotate : MonoBehaviour
{
    public enum RotationAxis { forward,right,up };
    public enum RotationDirection { clockwise, counterClockwise };

    [Header("Settings")]
    [Tooltip("Achse um die rotiert wird")]
    public RotationAxis axis;
    [Tooltip("Rotationsrichtung")]
    public RotationDirection dir;
    [Tooltip("Rotationsgeschwindigkeit")]
    public float rotationSpeed;

    //Achse um die rotiert wird
    private Vector3 rotationAxis;
    //Rotationsrichtung
    private float rotatioDirection;

    // Start is called before the first frame update
    public bool debug;

    void Start()
    {
        InitRotation(axis, dir);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(rotationAxis, rotatioDirection * rotationSpeed * Time.deltaTime);
        if(debug) InitRotation(axis, dir);
    }
    /// <summary>
    /// Initalisiert die Rotation und legt die Rotations Achse und Richtung fest 
    /// </summary>
    private void InitRotation (RotationAxis axis, RotationDirection dir)
    {
        switch (axis)
        {
            case RotationAxis.forward:
                rotationAxis = this.transform.forward;
                break;
            case RotationAxis.right:
                rotationAxis = this.transform.right;
                break;
            case RotationAxis.up:
                rotationAxis = this.transform.up;
                break;
        }
        switch (dir)
        {
            case RotationDirection.clockwise:
                rotatioDirection = -1;
                break;
            case RotationDirection.counterClockwise:
                rotatioDirection = 1;
                break;
        }
    }
}
