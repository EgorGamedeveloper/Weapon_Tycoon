using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image image;
    

    public void UpdateSlot(ItemData item)
    {
        image.sprite = item.icon;
    }
}
