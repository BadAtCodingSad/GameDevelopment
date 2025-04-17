using UnityEngine;

public class TweenManager : MonoBehaviour
{
    public static TweenManager tweenManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tweenManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UIHover(GameObject gameObject) 
    {
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f).setEaseSpring();
    }
    public void UIExitHover(GameObject gameObject)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.3f).setEaseSpring();
    }
}
