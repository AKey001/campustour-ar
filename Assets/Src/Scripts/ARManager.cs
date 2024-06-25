using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARManager : MonoBehaviour
{

    public ARPlaneManager planeManager;

    public PlaceObjectInputSystem placeObjectInputSystem;

    public KI ki;

    public void SetCurrentPrefab(GameObject gameObject) 
    {
        placeObjectInputSystem.prefab = gameObject;
    }

    public void StartAR()
    {
        ki.enabled = false;
        placeObjectInputSystem.enabled = true;
        planeManager.enabled = true;
    }
    
    public IEnumerator StopAR()
    {
        placeObjectInputSystem.spawned = null;
        ki.enabled = true;
        foreach (var plane in planeManager.trackables)
        {
            Destroy(plane.gameObject);
        }
        planeManager.enabled = false;
        placeObjectInputSystem.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(placeObjectInputSystem.instantiated);
    }
}
