using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just some examples on using the scripts on the Retrobot
/// </summary>
public class RetroBotUseExamples : MonoBehaviour
{
    // Start is called before the first frame update

    /// <summary>
    /// To interact with the Retrobot functions, start by having a reference to it. See code below for examples.
    /// </summary>
    public GameObject retroBot;

    public bool toggleThrusters = false;
    private bool thrusterActive = true;

    public bool demonstrateTurnOffFrontIcon = false;
    private bool turnOffFrontIconDone = false;


    public bool demonstrateChangeIcon = false;
    private bool changeFrontIconDone = false;

    public bool demonstrateChangeIconColor = false;
    private bool changeFrontIconColorDone = false;

    

    // Update is called once per frame
    void Update()
    {

        if (toggleThrusters) {
            toggleThrusters = false;
            retroBot.GetComponent<RetrobotThrusterControl>().ActivateThruster(!thrusterActive);
            thrusterActive = !thrusterActive;
        }

        if (demonstrateTurnOffFrontIcon && !turnOffFrontIconDone)
        {
            //this line here is how you turn off (or on) the FRONT robot icon.
            retroBot.GetComponent<RetrobotIconManager>().ActivateHeadIcons(false);
            turnOffFrontIconDone = true;
            print("Turned off front icon.");
        }

        if (demonstrateChangeIcon && !changeFrontIconDone) {
            retroBot.GetComponent<RetrobotIconManager>().SetIconTexture(5);
            changeFrontIconDone = true;
            print("Changed front icon.");
        }

        if (demonstrateChangeIconColor && !changeFrontIconColorDone)
        {
            retroBot.GetComponent<RetrobotIconManager>().SetIconColor(4);
            changeFrontIconColorDone = true;
            print("Changed front icon.");
        }
    }

}
