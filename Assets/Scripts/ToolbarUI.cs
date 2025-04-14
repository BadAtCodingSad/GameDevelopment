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
        uiManager.ShowUI(uiManager.buildList);
    }

    public void OnWorkerButtonClicked()
    {
        GameManager.instance.SetTool(ToolType.Pickaxe);
        uiManager.ShowUI(uiManager.workersUI);
    }
}
