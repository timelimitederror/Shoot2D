using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 武器
    [HideInInspector]
    public List<Item> availableArms = new List<Item>();

    private Vector2 input;
    private Vector2 mousePos;
    private Animator animator;
    private Rigidbody2D thisRigidbody;
    private int gunNum = 0;

    // 玩家状态
    private int originalHealth = 100; // 这个血量，只和玩家自身有关，和道具加成无关
    private float originalSpeed = 6f; // 这个速度，只和玩家自身有关， 和道具加成无关
    private int maxHealth = 100; // 受道具影响后的血量上限
    private int currentHealth = 10; // 当前血量
    private float speed = 6f; // 受道具影响后的速度
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = Mathf.Max(0, value);
            UpdatePlayerHealthUI();
        }
    }
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            UpdatePlayerHealthUI();
        }
    }
    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
        }
    }

    // 其他装备和道具背包
    [HideInInspector]
    public Item head = null;
    [HideInInspector]
    public Item body = null;
    [HideInInspector]
    public int activeItemNum = 0;
    [HideInInspector]
    public List<Item> activeItems = new List<Item>();
    [HideInInspector]
    public List<Item> passiveItems = new List<Item>();

    void Start()
    {
        animator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody2D>();

        // 刷新UI
        UpdatePlayerHealthUI();
        UpdateActiveItemUI();
    }

    void Update()
    {
        if (!TimeManager.Instance.TimeFlow())
        {
            return;
        }
        SwitchGun();
        SwitchActiveItem();
        UseActiveItem();
        OpenPackage();
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        thisRigidbody.velocity = input.normalized * speed;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        if (input != Vector2.zero)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void SwitchGun() // 按Q或E，在可使用的枪中切换
    {
        if (availableArms.Count == 0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            availableArms[gunNum].gameObject.SetActive(false);
            if (--gunNum < 0)
            {
                gunNum = availableArms.Count - 1;
            }
            availableArms[gunNum].gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            availableArms[gunNum].gameObject.SetActive(false);
            if (++gunNum > availableArms.Count - 1)
            {
                gunNum = 0;
            }
            availableArms[gunNum].gameObject.SetActive(true);
        }
    }

    void SwitchActiveItem() // 按大键盘1或3，切换主动道具
    {
        if (activeItems.Count == 0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeItems[activeItemNum].gameObject.SetActive(false);
            if (--activeItemNum < 0)
            {
                activeItemNum = activeItems.Count - 1;
            }
            activeItems[activeItemNum].gameObject.SetActive(true);
            UpdateActiveItemUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeItems[activeItemNum].gameObject.SetActive(false);
            if (++activeItemNum > activeItems.Count - 1)
            {
                activeItemNum = 0;
            }
            activeItems[activeItemNum].gameObject.SetActive(true);
            UpdateActiveItemUI();
        }
    }

    void UseActiveItem() // 按F使用主动道具
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (activeItems.Count > 0)
            {
                if (activeItemNum >= activeItems.Count)
                {
                    activeItemNum = 0;
                    UpdateActiveItemUI();
                    return;
                }
                activeItems[activeItemNum].ApplyEffect();
                UpdateActiveItemUI();
            }
        }
    }

    void OpenPackage() // 打开背包
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdatePackageUI();
            UIManager.Instance.OpenPackagePanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // 碰撞检测 捡道具
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            switch (item.SlotType)
            {
                case EquipmentSlotType.Arm:
                    AddGun(item);
                    break;
                case EquipmentSlotType.Head:
                    ReplaceEquipment(ref head, item);
                    break;
                case EquipmentSlotType.Body:
                    ReplaceEquipment(ref body, item);
                    break;
                case EquipmentSlotType.Active:
                    AddActiveItem(item);
                    break;
                case EquipmentSlotType.Passive:
                    AddPassiveItem(item);
                    break;
                default:
                    break;
            }

            // 刷新玩家状态
            UpdateEffect();
        }
    }

    private void ReplaceEquipment(ref Item slot, Item item) // 更换头/身体等装备槽
    {
        if (slot == null)
        {
            item.Initialize(this); // 初始化道具
            slot = item;
            return;
        }
        if (slot.ItemId == item.ItemId && item.Stackable) // 堆叠
        {
            slot.AddStack(1);
            item.UnInitialize();
            ObjectPool.Instance.PushObject(item.gameObject);
            return;
        }
        slot.UnInitialize(); // 卸载旧道具，装上新道具
        ObjectPool.Instance.PushObject(slot.gameObject);
        item.Initialize(this); // 初始化道具
        slot = item;
    }

    private void AddGun(Item item) // 添加可用枪支 捡到的item直接回收
    {
        for (int i = 0; i < availableArms.Count; i++) // 如果捡到的是已经有了的武器
        {
            if (availableArms[i].ItemId == item.ItemId)
            {
                if (item.Stackable) // 捡到同样的武器可以升级？
                {
                    availableArms[i].AddStack(1);
                }
                item.UnInitialize();
                ObjectPool.Instance.PushObject(item.gameObject);
                return;
            }
        }
        availableArms.Add(item); // 捡到新武器，直接装备上
        item.Initialize(this); // 初始化道具
        availableArms[gunNum].gameObject.SetActive(false);
        gunNum = availableArms.Count - 1;
        availableArms[gunNum].gameObject.SetActive(true);
    }

    private void AddActiveItem(Item item) // 添加主动道具
    {
        int i = 0;
        for (; i < activeItems.Count; i++)
        {
            if (activeItems[i].ItemId == item.ItemId)
            {
                if (activeItems[i].Stackable)
                {
                    activeItems[i].AddStack(1);
                }
                break;
            }
        }
        if (i >= activeItems.Count)
        {
            item.Initialize(this); // 初始化道具
            activeItems.Add(item);
            if (activeItemNum != activeItems.Count - 1)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            item.UnInitialize();
            ObjectPool.Instance.PushObject(item.gameObject);
        }
        UpdateActiveItemUI();
    }

    private void AddPassiveItem(Item item) // 添加被动道具
    {
        int i = 0;
        for (; i < passiveItems.Count; i++)
        {
            if (passiveItems[i].ItemId == item.ItemId)
            {
                if (passiveItems[i].Stackable)
                {
                    passiveItems[i].AddStack(1);
                }
                break;
            }
        }
        if (i >= passiveItems.Count)
        {
            item.Initialize(this); // 初始化道具
            passiveItems.Add(item);
        }
        else
        {
            item.UnInitialize();
            ObjectPool.Instance.PushObject(item.gameObject);
        }
    }

    private void UpdateEffect() // 重新计算来自道具的增益
    {
        // 复原状态
        maxHealth = originalHealth;
        speed = originalSpeed;
        if (head != null)
        {
            head.Recovery();
        }
        if (body != null)
        {
            body.Recovery();
        }

        for (int i = 0; i < availableArms.Count; i++)
        {
            availableArms[i].Recovery();
        }
        for (int i = 0; i < activeItems.Count; i++)
        {
            activeItems[i].Recovery();
        }
        for (int i = 0; i < passiveItems.Count; i++)
        {
            passiveItems[i].Recovery();
        }

        // 重新计算
        if (head != null)
        {
            head.ApplyEffect();
        }
        if (body != null)
        {
            body.ApplyEffect();
        }
        for (int i = 0; i < availableArms.Count; i++)
        {
            availableArms[i].ApplyEffect();
        }
        for (int i = 0; i < passiveItems.Count; i++)
        {
            passiveItems[i].ApplyEffect();
        }
    }

    public void RemoveArm(Item item)
    {
        if (availableArms.Contains(item))
        {
            item.UnInitialize();
            availableArms.Remove(item);
            if (gunNum >= availableArms.Count)
            {
                gunNum = 0;
                availableArms[gunNum].gameObject.SetActive(true);
            }
            ObjectPool.Instance.PushObject(item.gameObject);
            UpdateActiveItemUI();
        }
    }

    public void RemoveActiveItem(Item item) // 移除主动道具
    {
        if (activeItems.Contains(item))
        {
            item.UnInitialize();
            activeItems.Remove(item);
            if (activeItemNum >= activeItems.Count)
            {
                activeItemNum = 0;
                activeItems[activeItemNum].gameObject.SetActive(true);
            }
            ObjectPool.Instance.PushObject(item.gameObject);
            UpdateActiveItemUI();
        }
    }

    public void RemovePassiveItem(Item item) // 移除被动道具
    {
        if (passiveItems.Contains(item))
        {
            item.UnInitialize();
            passiveItems.Remove(item);
            ObjectPool.Instance.PushObject(item.gameObject);
        }
    }

    public void UpdatePackageUI() // 更新背包UI界面
    {
        EventBus.Publish(new ItemUpdated(head, body, activeItems, passiveItems, availableArms));
    }

    private void UpdateActiveItemUI() // 更新当前主动道具的UI
    {
        if (activeItems.Count == 0)
        {
            EventBus.Publish(new SwitchActiveItem(null));
            return;
        }
        EventBus.Publish(new SwitchActiveItem(activeItems[activeItemNum]));
    }

    private void UpdatePlayerHealthUI() // 更新玩家血量UI
    {
        EventBus.Publish(new PlayerHealthChanged(maxHealth, currentHealth));
    }
}
