// zoe - 2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace zoe {
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public string ItemName;
    [HideInInspector] public Transform parent;
    public void OnBeginDrag(PointerEventData eventData) {
        parent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parent);
        image.raycastTarget = true;
    }


   
   
}
}