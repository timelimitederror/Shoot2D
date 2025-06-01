using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviourSinqletonBase<UIManager>
{
    public GameObject packagePanel;

    public void OpenPackagePanel()
    {
        if (packagePanel != null)
        {
            TimeManager.Instance.StopTime();
            packagePanel.SetActive(true);
        }
    }
}
