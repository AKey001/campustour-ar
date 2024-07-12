using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhanceTouch = UnityEngine.InputSystem.EnhancedTouch;


[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObjectInputSystem : MonoBehaviour
{
    public GameObject prefab;

    public ARManager arManager;

    [HideInInspector]
    public GameObject spawned;
    
    [HideInInspector]
    public GameObject instantiated;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake() 
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        EnhanceTouch.TouchSimulation.Enable();
        EnhanceTouch.EnhancedTouchSupport.Enable();
        EnhanceTouch.Touch.onFingerDown += FingerDown;
    }

    void OnDisable() 
    {
        EnhanceTouch.TouchSimulation.Disable();
        EnhanceTouch.EnhancedTouchSupport.Disable();
        EnhanceTouch.Touch.onFingerDown -= FingerDown;

    }

    private void FingerDown(EnhanceTouch.Finger finger)
    {

        if (finger.index != 0) return;
        
        if (spawned == prefab) return;

        
        if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) {
            ARRaycastHit hit = hits[0];
            Pose pose = hit.pose;
            instantiated = Instantiate(prefab, pose.position, pose.rotation);
            instantiated.GetComponent<RobotAnimation>().arManager = arManager;
            spawned = prefab;

            Vector3 position = instantiated.transform.position;
            position.y = 0;
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.y = 0;
            Vector3 direction = cameraPosition - position;
            Quaternion targetRotation = Quaternion.LookRotation(forward: direction);
            instantiated.transform.rotation = targetRotation;   

            foreach (var plane in planeManager.trackables)
            {
                Destroy(plane.gameObject);
            }
        }
    }

}
