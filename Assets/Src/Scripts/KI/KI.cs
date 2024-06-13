using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.Sentis;

public class KI : MonoBehaviour
{
    public float timespan;  // seconds
    private float time;

    private readonly string URL = "http://campustour.antonkiessling.de/api/search";

    public Camera mainCamera;
    public PanelManager panel;
    public Text poi;

    public ModelAsset modelAsset;
    private Model transformerModel;
    private IWorker worker;


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
        // // The Render Texture in RenderTexture.active is the one
        // // that will be read by ReadPixels.
        // var currentRT = RenderTexture.active;
        // RenderTexture.active = mainCamera.targetTexture;

        // // Render the camera's view.
        // mainCamera.Render();

        // // Make a new texture and read the active Render Texture into it.
        // Texture2D image = new Texture2D(mainCamera.targetTexture.width, mainCamera.targetTexture.height);
        // image.ReadPixels(new Rect(0, 0, mainCamera.targetTexture.width, mainCamera.targetTexture.height), 0, 0);
        // image.Apply();

        // // Replace the original active Render Texture.
        // RenderTexture.active = currentRT;

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

        return screenshot;
    }

    private List<double> ExtractVector(Texture2D image)
    {
        // TensorFloat inputTensor = TextureConverter.ToTensor(image);
        TensorFloat inputTensor = TextureConverter.ToTensor(image, width: 224, height: 224, channels: 1);

        worker.Execute(inputTensor);
        TensorFloat outputTensor = worker.PeekOutput() as TensorFloat;
        
        outputTensor.CompleteOperationsAndDownload();
        float[] outputData = outputTensor.ToReadOnlyArray();

        outputTensor.Dispose();

        List<double> vector = outputData.Select(value => (double) value).ToList();
        return vector;
    }


    private IEnumerator Process(List<double> vector) {
        Body body = new()
        {
            vector = vector
        };
        string bodyContent = JsonUtility.ToJson(body); 

        UnityWebRequest request = new UnityWebRequest(URL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyContent);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("fetched from api");

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("success");
            ImageCategory category = JsonUtility.FromJson<ImageCategory>(request.downloadHandler.text );

            poi.text = category.category;
            panel.Open();
        }
        else 
        {
            Debug.Log("not found");
            panel.Close();
        }
        
    }

    void Awake() 
    {
        transformerModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(BackendType.GPUCompute, transformerModel);
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
            Debug.Log("image captured" + image);
            
            List<double> vector = ExtractVector(image);
            Debug.Log("extracted vector " + vector);

            StartCoroutine(Process(vector));
    
            time = Time.time + timespan;
        }
    }

    void OnDisable()
    {
        if (worker != null) 
        {
            worker.Dispose();
        }
    }
}
