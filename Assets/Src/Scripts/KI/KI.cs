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
    public float timespan = 5f;  // seconds
    private float time;

    private string URL = "http://localhost:5173/api/search";

    public PanelManager panel;
    public Text poi;

    private ModelAsset modelAsset;
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
        Camera mainCamera = Camera.main;
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = mainCamera.targetTexture;

        // Render the camera's view.
        mainCamera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(mainCamera.targetTexture.width, mainCamera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, mainCamera.targetTexture.width, mainCamera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;

        return image;
    }

    private List<double> ExtractVector(Texture2D image)
    {
        TensorFloat inputTensor = TextureConverter.ToTensor(image);

        worker.Execute(inputTensor);
        TensorFloat outputTensor = worker.PeekOutput() as TensorFloat;
        
        outputTensor.CompleteOperationsAndDownload();
        float[] outputData = outputTensor.ToReadOnlyArray();

        List<double> vector = outputData.Select(value => (double) value).ToList();
        return vector;
    }


    private IEnumerator Process(List<double> vector) {
        Body body = new Body();
        body.vector = vector;

        string bodyContent = JsonUtility.ToJson(body); 

        Debug.Log("body: " + bodyContent);

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
            string wrappedResponse = "{\"items\":" + request.downloadHandler.text + "}";
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

    void Start()
    {
        modelAsset = Resources.Load("/Src/Scripts/KI/vit-base-patch16-224-in21k/model.onnx") as ModelAsset;
        transformerModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(BackendType.GPUCompute, transformerModel);

        time = Time.time + timespan;
    }
    
    void Update()
    {
        if (time <= Time.time)
        {
            Texture2D image = CaptureRealTimeImage();
            Debug.Log("image captured");
            
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
