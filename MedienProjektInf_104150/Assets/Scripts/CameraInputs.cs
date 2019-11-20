using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Script zum Verarbeiten der Inputs Mittels Maus und Tastertur und Touchscreen.
/// Verabeitung der Eingaben in Form von Zoom, Rotations und Bewegung.
/// Rotation : Touch -> 1 Finger, bewegen | Maus -> Klick und ziehen
/// Zoom : Touch -> 2 Finger, Distanz zwischen Fingern | Maus -> Mausrad
/// Bewegen : Touch -> 2 Finger, paralleles bewegen beider Finger | Maus -> strg Links + Klick und ziehen 
/// </summary>
public class CameraInputs : MonoBehaviour
{
    //Reset Werte für Kamera reset
    private Vector3[] reset = new Vector3[3];
    //Abstand zu Ziel um das Rotiert wird
    private Vector3 offset;
    //Ziel um das Rotiert wird
    private Vector3 target;
    //Referenzz zur KAmera
    private Camera cam;
    //Vector für Alte Maus Position (für Kamera Bewegung)
    private Vector3 oldPosition = Vector3.zero;
    //Warte Zeitzwischen verarbeitungen der Touchscreen Inputs
    private float wait = 0.0f;
    //Flag ob zwei Finger benutzt werden
    private bool twoTouches;
    //Alte Position von Zwei Fingerinput zur Berechnug des Zooms
    private Vector2[] oldTouchTwoInput;

    [Header("Geschwindigkeiten für Kamera Bewegungen")]
    [Range(0.1f, 100f)]
    public float zoomSpeedMouse = 0.5f;
    [Range(0.1f, 100f)]
    public float zoomSpeedTouch = 0.01f;
    [Range(0.1f, 100f)]
    public float moveSpeedMouse = 5.0f;
    [Range(0.1f, 100f)]
    public float moveSpeedTouch = 1.0f;
    [Range(0.1f, 100f)]
    public float rotateSpeedMouse = 1.0f;
    [Range(0.1f, 100f)]
    public float rotateSpeedTouch = 1.0f;

    /// <summary>
    /// Speichert Start Werte der Kamera für Kamera reset
    /// </summary>
    void Awake()
    {
        reset[0] = this.transform.localPosition;
        reset[1] = this.transform.eulerAngles;
        reset[2] = this.transform.localScale;
        cam = Camera.main;
        target = Vector3.zero;
        offset = target + cam.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(HandleTouch());
        }
        else
        {
            HandleMouse();
        }
    }
    /// <summary>
    /// Verarbeitet die Eingaben über Touchscreen.
    /// Wartezeit zwischen Verarbeitung bei 2 Fingern, ansonten fehlerhafte auswertung der Bewegung.
    /// </summary>
    /// <returns></returns>
    private IEnumerator HandleTouch()
    {
        yield return new WaitForSeconds(wait);
        if (wait > 0)
        {
            wait = 0;
        }

        switch (Input.touchCount)
        {
            //Ein Finger eingabe -> Rotation
            case 1:
                if (Input.GetTouch(0).position.y > 500)
                {
                    if (!twoTouches)
                    {
                        Touch touchOneInput = Input.GetTouch(0);
                        if (touchOneInput.phase == TouchPhase.Began)
                        {
                            oldPosition = touchOneInput.position;
                        }
                        RotateCamera(touchOneInput.position);
                        if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                        {
                            wait = 0.25f;
                        }
                    }
                    else
                    {
                        twoTouches = false;
                    }
                }
                break;
            //Zwei Finger eingabe -> Zoom, Bewegen
            case 2:
                if (Input.GetTouch(0).position.y > 500 && Input.GetTouch(1).position.y > 500)
                {
                    twoTouches = true;
                    Vector2[] touchTwoInput = { cam.ScreenToViewportPoint(Input.GetTouch(0).position), cam.ScreenToViewportPoint(Input.GetTouch(1).position) };

                    Vector2 mid = Input.GetTouch(0).position + (Input.GetTouch(1).position - Input.GetTouch(0).position) * 0.5f;
                    if (Input.GetTouch(1).phase == TouchPhase.Began)
                    {
                        oldPosition = mid;
                        oldTouchTwoInput = touchTwoInput;
                    }
                    MoveCamera(mid);

                    float newZoom = Vector2.Distance(touchTwoInput[0], touchTwoInput[1]);
                    float oldZoom = Vector2.Distance(oldTouchTwoInput[0], oldTouchTwoInput[1]);
                    ZoomCamera(-(oldZoom - newZoom));

                    oldTouchTwoInput = touchTwoInput;
                    if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Canceled)
                    {
                        wait = 0.5f;
                    }
                }
                break;
            default:
                break;

        }
    }
    /// <summary>
    /// Verarbeitet die Maus Eingaben in die Bewegungen Scrollen, Zoomen und Bewegenen der Kamera
    /// </summary>
    private void HandleMouse()
    {
        if (Input.mousePosition.y > 500)
        {
            if (Input.GetMouseButtonDown(0))
            {
                oldPosition = Input.mousePosition;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0))
            {
                MoveCamera(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                RotateCamera(Input.mousePosition);
            }

            // Check for scrolling to zoom the camera
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
            ZoomCamera(scroll);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newPosition"></param>
    private void RotateCamera(Vector3 newPosition)
    {
        Vector3 mouseChange = (oldPosition - newPosition);
        mouseChange.x /= Screen.width;
        mouseChange.y /= Screen.height;
        mouseChange *= (Input.touchSupported) ? rotateSpeedTouch : rotateSpeedMouse;
        offset = Quaternion.AngleAxis(-mouseChange.x, transform.up) * Quaternion.AngleAxis(mouseChange.y, transform.right) * offset;
        transform.position = target + offset;
        transform.LookAt(target);

        oldPosition = newPosition;
    }
    /// <summary>
    /// Bewegt die Kamera auf der Ebene, die aufgespannt wird durch (Camera.up / Camera.right).
    /// Translation abhängig von der alten Position der Kamera.
    /// </summary>
    /// <param name="newPosition">Zielposition der Kamera</param>
    private void MoveCamera(Vector3 newPosition)
    {
        Vector3 change = (oldPosition - newPosition) / Screen.width * moveSpeedMouse;
        change *= (Input.touchSupported) ? moveSpeedTouch : moveSpeedMouse;
        Vector3 move = change.x * cam.transform.right + change.y * cam.transform.up;

        transform.Translate(move, Space.World);
        target += move;

        oldPosition = newPosition;
    }
    /// <summary>
    /// Zoomt die Kamera Hinein/ Heraus
    /// </summary>
    /// <param name="zoomDistance">Input über Mausrad/ 2 Finger Touchsreen</param>
    private void ZoomCamera(float zoomDistance)
    {
        float zoom = (Input.touchSupported) ? zoomDistance * zoomSpeedTouch : zoomDistance * zoomSpeedMouse;
        transform.position = this.transform.position + (Vector3.Normalize(target - this.transform.localPosition) * zoom);
        target += Vector3.Normalize(target - this.transform.localPosition) * zoom;
    }
    /// <summary>
    /// Zurücksetzen der Camera Position zur Startposition
    /// </summary>
    public void ResetCamera()
    {
        this.transform.localPosition = reset[0];
        this.transform.eulerAngles = reset[1];
        this.transform.localScale = reset[2];
        cam = Camera.main;
        target = Vector3.zero;
        offset = target + cam.transform.position;
    }
}

