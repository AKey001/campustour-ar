using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrobotController : MonoBehaviour
{
    /// <summary>
    /// Animator. Assigned in editor.
    /// </summary>
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Motion_X", 0);
        animator.SetFloat("Motion_Y", 0);
    }

    /// <summary>
    /// See documentation. https://lb3d.co/retrobot-unity-3d-game-asset-documentation/
    /// Plays a given animation. All actions are prefaced with "Do_" triggers in the animator. 
    /// </summary>
    /// <param name="action">Action (one-shot animation) to be played.</param>
    public void DoAction(string action) {
        animator.SetTrigger(action);
    }

    /// <summary>
    /// See documentation. https://lb3d.co/retrobot-unity-3d-game-asset-documentation/
    /// Sets the sideways motion state. 
    /// </summary>
    /// <param name="value">Motion state: -1 to 1.</param>
    public void SetMotionXState(float value) {
        animator.SetFloat("Motion_X", value);
    }
    /// <summary>
    /// See documentation. https://lb3d.co/retrobot-unity-3d-game-asset-documentation/
    /// Sets the forward / backward motion state.
    /// </summary>
    /// <param name="value">Forward / backward motion state. -1 to 1.</param>
    public void SetMotionYState(float value)
    {
        animator.SetFloat("Motion_Y", value);
    }

    /// <summary>
    /// See documentation. https://lb3d.co/retrobot-unity-3d-game-asset-documentation/
    /// Sets the idle state (which occurs as forward or lateral motion states approach zero)    /// 
    /// </summary>
    /// <param name="value">Blends between 3 seperate motion states. 0 to 1.</param>
    public void SetIdleState(float value) {
        animator.SetFloat("Idle_Pattern", value);
    }

    /// <summary>
    /// See documentation. https://lb3d.co/retrobot-unity-3d-game-asset-documentation/
    /// Toggles fight mode. 
    /// </summary>
    /// <param name="isFighting">Toggles flight mode.</param>
    public void SetFightIdle(bool isFighting) {
        animator.SetBool("Is_Fighting", isFighting);
    }
}
