using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 管理与玩家状态相关的UI
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

        // 注册玩家血量改变的事件监听，当触发事件时，更新血量相关的UI
        EventBus.Subscribe(new Action<PlayerHealthChanged>(thisEvent =>
        {
            playerHealthSlider.normalizedValue = (float)thisEvent.health / (float)thisEvent.maxHealth;
            playerHealthTmp.SetText(thisEvent.health + " / " + thisEvent.maxHealth);
        }));

        // 注册玩家主动道具变换的事件监听，当触发事件时，更新当前展示的主动道具 如果没有道具，就什么都不展示
        EventBus.Subscribe(new Action<SwitchActiveItem>(thisEvent =>
        {
            if (thisEvent.item != null)
            {
                currentActiveItemImage.sprite = thisEvent.item.Icon;
                currentActiveItemImage.gameObject.SetActive(true);

                if (thisEvent.item.Stackable) // 如果物品可堆叠，显示当前的堆叠数量，如果超过999个，显示999+
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
