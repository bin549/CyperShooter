using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public List<Item> items;
    public ItemSnapOffsets itemSnapOffsets;

    private void Awake()
    {
        items = new List<Item>();
    }

    public void SetItemSnapOffsets(ItemSnapOffsets itemSnapOffsets)
    {
        this.itemSnapOffsets = itemSnapOffsets;
    }
}
 