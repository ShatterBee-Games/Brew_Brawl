// zoe - 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace zoe {
public class InventorySlot : MonoBehaviour,IDropHandler
{
   public void OnDrop(PointerEventData eventData) {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem item = dropped.GetComponent<DraggableItem>();
        item.parent = transform.childCount == 0 ? transform : item.parent;
    }
}
}