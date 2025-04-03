using UnityEngine;

public class ToolbarUI : MonoBehaviour
{
  public UIManager uiManager;

    public void OnViewButtonClicked()
    {
        GameManager.instance.SetTool(ToolType.View);
        uiManager.ShowUI(uiManager.viewUI);
    }

    public void OnHammerButtonClicked()
    {
        GameManager.instance.SetTool(ToolType.Hammer);
    }
}
