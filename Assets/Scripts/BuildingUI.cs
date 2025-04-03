using UnityEngine;

public class BuildingUI : MonoBehaviour
{

    public void OnFactoryButtonClicked()
    {
         GameManager.instance.BuildAction("Factory");
    }

    public void OnHouseButtonClicked()
    {
       GameManager.instance.BuildAction("Residence");
    }

    public void OnDamButtonClicked()
    {
       GameManager.instance.BuildAction("Dam");
    }
}
