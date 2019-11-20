using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Erlaubt das Auswählen von Selectable Objekts mittels Raycast und das verfolständigen der dazugehörigen Task
/// </summary>
public class SelectObj : MonoBehaviour
{
    public float thickness;

    // Update is called once per frame
    void Update()
    {
        RayCaster();
    }
    /// <summary>
    /// Erzeugt ein Ray an der Position der Maus/Touch in die Scene hinen 
    /// </summary>
    private void RayCaster()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).position.y > 500)
        {
            CastRay(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0f));
        }
        else if (Input.GetMouseButton(0) && Input.mousePosition.y > 500)
        {
            CastRay(Input.mousePosition);
        }

    }
    /// <summary>
    /// Castet ein Ray in die Scenen, bei einem Hit mit einem Selectable wird die Task abgeschlossen
    /// </summary>
    /// <param name="screenPos"></param>
    private void CastRay(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.SphereCast(ray, thickness, out RaycastHit hit))
        {
            if (hit.transform.gameObject.GetComponent<Selectable>())
            {
                hit.transform.gameObject.GetComponent<Selectable>().taskReference.ExecuteTask();
            }
        }
    }


}
