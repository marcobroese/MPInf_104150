using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Wechselt durch die angegebenen Materialien durch und versieht alle angegebenen Parts mit dem Material
/// </summary>
public class MaterialChanger : MonoBehaviour
{
    [Tooltip("Liste der Verfügbaren Materialien")]
    public Material[] possibleMaterials;
    // Liste aller Parts mit dem Tag ("Furniture")
    private GameObject[] furnitureParts;
    //Index des momentanen Materials in possibleMaterials 
    private int idx = 0;
    // Start is called before the first frame update
    void Start()
    {
        furnitureParts = GameObject.FindGameObjectsWithTag("Furniture");
    }
    /// <summary>
    /// Wechselt das Material aller Gameobjekte die in der Liste furnitureParts vorhanden sind
    /// </summary>
    public void NextMaterial()
    {
        idx = (idx + 1) % possibleMaterials.Length;

        foreach (GameObject part in furnitureParts)
        {
            part.GetComponent<MeshRenderer>().material = possibleMaterials[idx];
        }
    }
}
