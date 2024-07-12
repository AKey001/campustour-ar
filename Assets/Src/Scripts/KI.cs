using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.Sentis;
using Unity.Collections;
using System.Data;
using UnityEngine.XR.ARFoundation;

public class KI : MonoBehaviour
{
    public float timespan;  // seconds
    private float time;

    [System.Serializable]
    public struct POI {
      public string category;
      public GameObject prefab;
    }
     
    public List<POI> pois;

    // private readonly string URL = "https://campustour.antonkiessling.de/api/search";
    // private readonly string URL = "http://localhost:5173/api/search";
    private readonly string URL = "https://campustour-web.vercel.app/api/search";

    public Camera mainCamera;
    public PanelManager panel;
    public ARManager arManager;
    public Text poi;


    public class ImageCategory
    {
        public string category;
        public double score;
    }

    public class Body
    {
        public List<double> vector;
    }


    private Texture2D CaptureRealTimeImage()
    {
        // Erstellt ein RenderTexture mit der Größe des Bildschirms
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        // Erstellt eine neue Texture2D mit der Größe des RenderTextures
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(rect, 0, 0);
        screenshot.Apply();

        // Setzt die Kamera-Render-Textur zurück
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Quadrat zum Croppen vorbereiten
        int size = Mathf.Min(screenshot.width, screenshot.height); // Nimmt die kleinere Dimension als Größe des Quadrats
        int x = (screenshot.width - size) / 2; // Berechnet die X-Position für das mittlere Quadrat
        int y = (screenshot.height - size) / 2; // Berechnet die Y-Position für das mittlere Quadrat
        Rect cropArea = new Rect(x, y, size, size);

        Texture2D cropped = new Texture2D((int)cropArea.width, (int)cropArea.height);

        // Kopieren der Pixel aus der Zuschneideregion
        Color[] pixels = screenshot.GetPixels(
            (int)cropArea.x,
            (int)cropArea.y,
            (int)cropArea.width,
            (int)cropArea.height
        );
        cropped.SetPixels(pixels);
        cropped.Apply();

        return screenshot;
    }

    private IEnumerator Process(Texture2D image)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", image.EncodeToPNG(), "image.png", "image/png");

        using UnityWebRequest request = UnityWebRequest.Post(URL, form);

        yield return request.SendWebRequest();

        Debug.Log("fetched from api: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("success");
            ImageCategory category = JsonUtility.FromJson<ImageCategory>(request.downloadHandler.text);

            arManager.SetCurrentPrefab(pois.Find(poi => poi.category == category.category).prefab);

            poi.text = category.category;
            panel.Open();
        }
        else
        {
            Debug.Log("not found");
            ImageCategory category = JsonUtility.FromJson<ImageCategory>(request.downloadHandler.text);

            arManager.SetCurrentPrefab(pois[2].prefab);
            
            poi.text = category.score + ": " + category.category;
            panel.Open();
        }
    }

    void Start()
    {
        time = Time.time + timespan;
    }

    void Update()
    {
        if (time <= Time.time)
        {
            Texture2D image = CaptureRealTimeImage();

            StartCoroutine(Process(image));

            time = Time.time + timespan;
        }
    }

}
