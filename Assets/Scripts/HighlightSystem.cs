using UnityEngine;

public class HighlightSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Highlight;
    public bool changed;
    void Start()
    {
        Highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        if(!changed)
            Highlight.SetActive(false);
    }
}
