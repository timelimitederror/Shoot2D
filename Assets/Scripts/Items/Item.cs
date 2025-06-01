using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���߻���
public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected int itemId = 0;
    public int ItemId { get { return itemId; } }
    [SerializeField]
    protected string itemName = "�µ���";
    public string ItemName { get { return itemName; } }
    [SerializeField]
    protected string description = "��������";
    public string Description { get { return description; } }
    [SerializeField]
    protected Sprite icon; // ����ͼƬ
    public Sprite Icon { get { return icon; } }
    [SerializeField]
    protected EquipmentSlotType slotType; // ��������
    public EquipmentSlotType SlotType { get { return slotType; } }
    [SerializeField]
    protected bool stackable = false; // �Ƿ���Զѵ�
    public bool Stackable { get { return stackable; } }
    protected int stackCount = 1; // ��ǰ�ѵ�����
    public int StackCount { get { return stackCount; } }
    [SerializeField]
    protected int maxStackCount = 1; // ���ѵ�����

    protected PlayerMovement player; // װ�����ߵ����

    // ������Դ�󣬸��������������Ϣ
    public virtual void InitInfo(int itemId, string itemName, string description, Sprite icon, EquipmentSlotType slotType)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.description = description;
        this.icon = icon;
        this.slotType = slotType;
    }

    // װ�����ʼ�����ߣ������߱�ʰȡʱ���ã�
    public virtual void Initialize(PlayerMovement owner)
    {
        player = owner;
        transform.SetParent(owner.transform, false);
        //ApplyEffect();
    }

    // ���߱�����ʱ����
    public virtual void UnInitialize()
    {
        transform.SetParent(null, false);
    }

    // Ӧ�õ���Ч��
    public abstract void ApplyEffect();

    // �Ƴ������������ߵļӳ�Ч�� ->�����õ��߼ӳ��������� �ѵ���������Ӱ��
    public virtual void Recovery()
    {
        // �������չ
    }

    // ���Ӷѵ�����
    public virtual void AddStack(int amount = 1)
    {
        if (!stackable) return;

        stackCount = Mathf.Clamp(stackCount + amount, 1, maxStackCount);
        UpdateStackEffect();
    }

    // ���¶ѵ�Ч�������ѵ������仯ʱ���ã�
    protected virtual void UpdateStackEffect()
    {

    }
}
