using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ӱ���������������
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
