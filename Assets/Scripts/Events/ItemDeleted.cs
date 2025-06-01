using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 从背包界面舍弃道具
public class ItemDeleted
{
    public EquipmentSlotType itemType;
    public Item item;

    public ItemDeleted(EquipmentSlotType itemType, Item item)
    {
        this.itemType = itemType;
        this.item = item;
    }
}
