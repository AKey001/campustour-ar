using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KI : MonoBehaviour
{

    public Camera camera;
    // public Gameobject button;
    // public TextMeshProUGUI buttonText;


     Texture2D RTImage()
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        
        // byte[] bytes = image.EncodeToPNG();
        // string fileName = "FILE_"


        return image;

    }

    // Update is called once per frame
    void Update()
    {
        Texture2D image = RTImage();

        // TODO: send Request to mongo
        // json = getFromMongo(image)
        // if statusCode == 200 
            // first = ...
            //  

            // TODO: if result -> show ui
            // button.active = false;
            // buttonText.text = "<category>";

    }
}
