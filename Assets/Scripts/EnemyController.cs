using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Slider slider;
    private int health = 1000;
    private int MaxHealth = 1000;

    void Start()
    {
        UpdateUI();
    }

    public void Damage(int value)
    {
        health = Mathf.Clamp(health - value, 0, MaxHealth);
        UpdateUI();
    }

    private void UpdateUI()
    {
        tmp.SetText(health + " / " + MaxHealth);
        slider.normalizedValue = (float)health / (float)MaxHealth;
    }
}
