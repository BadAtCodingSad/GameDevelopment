using Unity.VisualScripting;
using UnityEngine;

public class ANIMATEWORD : MonoBehaviour
{

    public Animator mAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseEnter()
    {
        if (mAnimator != null) {
            mAnimator.SetTrigger("StartAnim");
        }
        Debug.Log("hit");
    }

    private void OnMouseExit()
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("EndAnim");
        }
    }
}
