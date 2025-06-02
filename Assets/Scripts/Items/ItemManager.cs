using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviourSinqletonBase<ItemManager>
{
    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    public override void Awake()
    {
        base.Awake();

        TextAsset jsonFile = Resources.Load<TextAsset>("itemsConfig");
        JObject init = JObject.Parse(jsonFile.text);
        List<ItemInfo> itemsInit = init["items"].ToObject<List<ItemInfo>>();

        for (int i = 0; i < itemsInit.Count; i++)
        {
            ItemInfo itemInfo = itemsInit[i];
            GameObject instance = Instantiate(Resources.Load<GameObject>(itemInfo.prefab));
            Sprite icon = Resources.Load<Sprite>(itemInfo.icon);
            Item item = instance.GetComponent<Item>();
            if (item != null)
            {
                item.InitInfo(itemInfo.itemId, itemInfo.itemName, itemInfo.description, icon, (EquipmentSlotType)itemInfo.slotType);
                itemDictionary[item.ItemId] = item;
                item.gameObject.SetActive(false);
                //Debug.Log(item.ItemName + " " + item.Description);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Item item = transform.GetChild(i).GetComponent<Item>();
            if (item != null)
            {
                itemDictionary[item.ItemId] = item;
                item.gameObject.SetActive(false);
            }
        }
    }

    public Item GetItemById(int itemId)
    {
        if (itemDictionary.ContainsKey(itemId))
        { // 从对象池获取对应对象
            return ObjectPool.Instance.GetObject(itemDictionary[itemId].gameObject).GetComponent<Item>();
        }
        return null;
    }

    public class ItemInfo
    {
        public int itemId;
        public string itemName;
        public string description;
        public string icon;
        public string prefab;
        public int slotType;
    }
}
