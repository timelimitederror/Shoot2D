using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChanged
{
    public int maxHealth;
    public int health;

    public PlayerHealthChanged(int maxHealth, int health)
    {
        this.maxHealth = maxHealth;
        this.health = health;
    }
}
