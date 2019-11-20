using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Animation für Rotation von Parts
/// Rotiert das Part um die angegebene Achse, Richtung und Geschwindigkeit.
/// Es gibt keinen Debugmodus zur laufzeit, durch Zahlen ungenauigkeit kann es zu immer neuen berechnugen der rotations Achse kommen.
/// Dieses entsteht da Unity eine Rotation (-90,0,0) als (0,(-/+)90, (+/-)90) darstellt
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

    void Start()
    {
        InitRotation(axis, dir);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(rotationAxis, rotatioDirection * rotationSpeed * Time.deltaTime);
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
