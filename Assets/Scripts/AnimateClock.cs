using UnityEngine;

public class AnimateClock : MonoBehaviour
{
    [Tooltip("Your hour‑hand Animator")]
    public Animator animator;

    /// <summary>
    /// Jump to the given hour (0–11) on a 12‑hour animation
    /// </summary>
    public void SetHour(int hour)
    {
        Debug.Log("SetHour fired!  hour=" + hour);
        // wrap into 0..11
        int h = hour % 12;
        // normalize (0 → 12h = 1.0)
        float t = h / 12f;

        // play the state from that normalized time
        animator.Play("Hour Hand Anim", 0, t);
        // freeze so it doesn’t continue playing
        animator.speed = 0f;
    }
}