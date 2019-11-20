using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
/// <summary>
/// QR-Code Reader liest QR-Codes aus und läde die Ausgelesene Scene wenn sie Vorhanden ist. 
/// </summary>
public class QRReader : MonoBehaviour
{
    /**
     * Quelle:
     * https://medium.com/@adrian.n/reading-and-generating-qr-codes-with-c-in-unity-3d-the-easy-way-a25e1d85ba51
     **/
    [Header("Settings")]
    [Tooltip("Liste der Namen der Vorhandenen Scenen zu denen Gewechselt werden kann")]
    public string[] scenes;
    [Tooltip("Referenz zum LoadButton")]
    public Image loadButton;
    [Tooltip("Referenz zur Text Komponente")]
    public TextMeshProUGUI tmp;


    //Referenz zum Image auf dem das Kamerabild angezeigt werden soll
    private RawImage rawimage;
    //Referenz zur WebCamTexture
    private WebCamTexture webcamTexture;
    //Result von QR-Scan
    private Result result;
    //Flag ob Scan ist aktic
    private bool scanActive;
    //Coroutine für Scan
    private Coroutine co;

    /// <summary>
    /// Setup für WebcamTexture startet Kamera Aufnahme
    /// </summary>
    void Start()
    {
        rawimage = GameObject.Find("CameraImage").GetComponent<RawImage>();
        webcamTexture = new WebCamTexture
        {
            requestedHeight = Screen.height,
            requestedWidth = Screen.width
        };
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        if (webcamTexture != null)
        {
            webcamTexture.Play();
        }
    }
    /// <summary>
    /// Startet QR-Code Scanner
    /// </summary>
    public void StartScan()
    {
        scanActive = true;
        co = StartCoroutine(Scan());
    }
    /// <summary>
    /// Corouine für QR-Code Scanner.
    /// Scanner scant alle 0.5s 
    /// </summary>
    /// <returns></returns>
    IEnumerator Scan()
    {
        while (scanActive)
        {
            tmp.text = "Scan...";
            yield return new WaitForSeconds(0.2f);
            // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
            try
            {
                Debug.Log(scanActive);
                tmp.text = "";
                IBarcodeReader barcodeReader = new BarcodeReader();
                // decode the current frame
                result = barcodeReader.Decode(webcamTexture.GetPixels32(),
                 webcamTexture.width, webcamTexture.height);
                if (result != null && result.Text != "")
                {
                    tmp.text = result.Text;
                    UpdateLoadButton();
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            if (result != null)
            {
                if (result.Text != "")
                {
                    tmp.text = result.Text;
                    scanActive = false;
                    if (co != null)
                        StopCoroutine(co);
                    co = null;
                }
            }
            else
            {
                tmp.text = "";
            }
        }

    }
    /// <summary>
    /// Lädt gescannte Scene
    /// </summary>
    public void LoadScene()
    {
        if (result != null && result.Text != null)
            SceneManager.LoadScene(result.Text, LoadSceneMode.Single);
    }
    /// <summary>
    /// Ändert das Aussehen des Load Buttons, wenn es eine Scene mit dem Gescannten namen gibt
    /// </summary>
    private void UpdateLoadButton()
    {

        for (int i = 0; i < scenes.Length; i++)
        {
            if (result.Text == scenes[i])
            {
                loadButton.color = Color.green;
                return;
            }
            else
            {
                loadButton.color = Color.red;
            }
        }

    }
}
