using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    public GameObject mark1;
    public GameObject mark2;
    public GameObject mark3;
    public GameObject mark4;
    public GameObject mark5;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Item item;
            if (mark1.transform.childCount == 0)
            {
                item = ItemManager.Instance.GetItemById(1);
                item.transform.SetParent(mark1.transform, false);
                item.gameObject.SetActive(true);
            }

            if (mark2.transform.childCount == 0)
            {
                item = ItemManager.Instance.GetItemById(2);
                item.transform.SetParent(mark2.transform, false);
                item.gameObject.SetActive(true);
            }

            if (mark3.transform.childCount == 0)
            {
                item = ItemManager.Instance.GetItemById(3);
                item.transform.SetParent(mark3.transform, false);
                item.gameObject.SetActive(true);
            }

            if (mark4.transform.childCount == 0)
            {
                item = ItemManager.Instance.GetItemById(4);
                item.transform.SetParent(mark4.transform, false);
                item.gameObject.SetActive(true);
            }
        }
    }
}
