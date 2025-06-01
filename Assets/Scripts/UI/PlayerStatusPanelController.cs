using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ���������״̬��ص�UI
public class PlayerStatusPanelController : MonoBehaviour
{
    public Image currentActiveItemImage;
    public TextMeshProUGUI currentActiveItemCount;
    public Slider playerHealthSlider;
    public TextMeshProUGUI playerHealthTmp;

    // Start is called before the first frame update
    void Start()
    {
        currentActiveItemImage.gameObject.SetActive(false);
        currentActiveItemCount.gameObject.SetActive(false);

        // ע�����Ѫ���ı���¼��������������¼�ʱ������Ѫ����ص�UI
        EventBus.Subscribe(new Action<PlayerHealthChanged>(thisEvent =>
        {
            playerHealthSlider.normalizedValue = (float)thisEvent.health / (float)thisEvent.maxHealth;
            playerHealthTmp.SetText(thisEvent.health + " / " + thisEvent.maxHealth);
        }));

        // ע������������߱任���¼��������������¼�ʱ�����µ�ǰչʾ���������� ���û�е��ߣ���ʲô����չʾ
        EventBus.Subscribe(new Action<SwitchActiveItem>(thisEvent =>
        {
            if (thisEvent.item != null)
            {
                currentActiveItemImage.sprite = thisEvent.item.Icon;
                currentActiveItemImage.gameObject.SetActive(true);

                if (thisEvent.item.Stackable) // �����Ʒ�ɶѵ�����ʾ��ǰ�Ķѵ��������������999������ʾ999+
                {
                    int count = thisEvent.item.StackCount;
                    if (count > 999)
                    {
                        currentActiveItemCount.SetText("999+");
                    }
                    else
                    {
                        currentActiveItemCount.SetText(count + "");
                    }
                    currentActiveItemCount.gameObject.SetActive(true);
                }
                else
                {
                    currentActiveItemCount.gameObject.SetActive(false);
                }
            }
            else
            {
                currentActiveItemImage.gameObject.SetActive(false);
                currentActiveItemCount.gameObject.SetActive(false);
            }
        }));
    }
}
