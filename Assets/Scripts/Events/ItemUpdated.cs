using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUpdated
{
    public Item head;
    public Item body;
    public List<Item> activeItems;
    public List<Item> passiveItems;
    public List<Item> armItems;

    public ItemUpdated(Item head, Item body, List<Item> activeItems, List<Item> passiveItems, List<Item> armItems)
    {
        this.head = head;
        this.body = body;
        this.activeItems = activeItems;
        this.passiveItems = passiveItems;
        this.armItems = armItems;
    }
}
