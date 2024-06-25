using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrobotThrusterControl : MonoBehaviour
{
    public GameObject thruster;
    public bool thrusterActivated = true;

    [Header("[Testing Only]")]
    public bool testing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        thruster.SetActive(thrusterActivated);
    }

    /// <summary>
    /// activate or deactivate the thruster. 
    /// </summary>
    /// <param name="activate">true or false, whether thruster is active</param>
    public void ActivateThruster(bool activate) {
        thruster.SetActive(activate);
    }

    // Update is called once per frame
    void Update()
    {
        if (testing) {
            CheckTestFunctions();
        }   
    }

    /// <summary>
    /// test the truster functionality via keyboard number 9
    /// </summary>
    public void CheckTestFunctions()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            ActivateThruster(!thruster.activeSelf);
        }
    }

}
