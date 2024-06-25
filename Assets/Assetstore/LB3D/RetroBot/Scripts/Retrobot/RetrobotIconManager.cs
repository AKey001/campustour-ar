using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrobotIconManager : MonoBehaviour
{
    [Header("Icons Active?")]
    public bool headIconsActive = true;
    public bool frontIconActive = true;

    [Header("Icon Objects")]
    public GameObject iconF;
    public GameObject iconL;
    public GameObject iconR;

    [Header("Icon Colors")]
    public Color[] iconColors;
    private int currentIconColorIndex;

    [Header("Icon Textures")]
    public Texture[] iconTextures;    
    public Renderer rendF;
    public Renderer rendL;
    public Renderer rendR;
    private int currentIconTextureIndex;

    [Header("[Testing Only]")]
    public bool testing = false;

    // Start is called before the first frame update
    void Start()
    {
        ActivateHeadIcons(headIconsActive);
        ActivateFrontIcon(headIconsActive);   
    }

   /// <summary>
   /// Do you really need documentation for this? :) 
   /// </summary>
    void Update()
    {
        if (testing) {
            CheckTestFunctions();
        }
    }

    /// <summary>
    /// testing only...
    /// </summary>
    public void CheckTestFunctions() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetIconTexture(32);
            SetIconColor(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetIconTexture(19);
            SetIconColor(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetIconTexture(1);
            SetIconColor(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            ActivateFrontIcon(!frontIconActive);
            ActivateHeadIcons(!headIconsActive);

        }
    }

    /// <summary>
    /// Activate or deactivate the front icon. 
    /// </summary>
    /// <param name="setActive">true or false - whether front icon is active or deactivated</param>
    public void ActivateFrontIcon(bool setActive) {
        iconF.SetActive(setActive);        
        frontIconActive = setActive;
    }

    /// <summary>
    /// Activate or deactivate the  head icons.
    /// </summary>
    /// <param name="setActive">true or false - whether head icons are active or deactivated</param>
    public void ActivateHeadIcons(bool setActive) {
        iconL.SetActive(setActive);
        iconR.SetActive(setActive);
        headIconsActive = setActive;
    }

    /// <summary>
    /// Set the icon texture.
    /// </summary>
    /// <param name="iconTextureIndex">index of texture from the iconTextures array set in editor.</param>
    public void SetIconTexture(int iconTextureIndex) {
        rendF.material.mainTexture = iconTextures[iconTextureIndex];
        rendL.material.mainTexture = iconTextures[iconTextureIndex];
        rendR.material.mainTexture = iconTextures[iconTextureIndex];
        currentIconTextureIndex = iconTextureIndex;
    }

    /// <summary>
    /// Sets the color of the icon.
    /// </summary>
    /// <param name="iconColorIndex">index of color  from the iconColors array set in editor.</param>
    public void SetIconColor(int iconColorIndex) {
        rendF.material.SetColor("_Color", iconColors[iconColorIndex]);
        rendL.material.SetColor("_Color", iconColors[iconColorIndex]);
        rendR.material.SetColor("_Color", iconColors[iconColorIndex]);
        currentIconColorIndex = iconColorIndex;
    }

    /// <summary>
    /// Gets the index of currently displayed icon. This is for your own use in your game. It is set in the @SetIconIndex function.
    /// </summary>
    /// <returns>index of current texture in use</returns>
    public int GetIconTextureIndex() {
        return currentIconTextureIndex;
    }
    /// <summary>
    /// Gets the index of current icon color. This is for your own use in your game. It is set in the @SetIconColor function.
    /// </summary>
    /// <returns>index of current color in use</returns>
    public int GetIconIconColor() {
        return currentIconColorIndex;
    }

}
