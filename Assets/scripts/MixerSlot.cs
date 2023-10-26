using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace zoe
{
    public class MixerSlot : MonoBehaviour, IDropHandler
    {
        public bool isSlotA;
        public bool isSlotB;
        public CombinationTable combinationTable;

        private DraggableItem itemA;
        private DraggableItem itemB;



          public void OnDrop(PointerEventData eventData) 
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem item = dropped.GetComponent<DraggableItem>();

            if (isSlotA)
            {
                itemA = item;
                Debug.Log("Item dropped in Slot A: " + itemA.ItemName);
            }

            if (isSlotB)
            {
                itemB = item;
                Debug.Log("Item dropped in Slot B: " + itemB.ItemName);
            }

            if (itemA != null && itemB != null)
            {
                HandleCombination(itemA, itemB);
            }

            item.parent = transform;
        }
    }
        private void HandleCombination(DraggableItem item1, DraggableItem item2)
        {
            string itemNameA = item1.ItemName;
            string itemNameB = item2.ItemName;
            Debug.Log("Item A: " + itemNameA);
            Debug.Log("Item B: " + itemNameB);

            foreach (
                CombinationTable.CombinationEffect combination in combinationTable.combinations
            )
            {
                Debug.Log(
                    "Checking combination: " + combination.itemA + " and " + combination.itemB
                );
                Debug.Log("Item A in combination: " + (item1 == null ? "null" : item1.ItemName));
                Debug.Log("Item B in combination: " + (item2 == null ? "null" : item2.ItemName));

                if (
                    (combination.itemA == itemNameA && combination.itemB == itemNameB)
                    || (combination.itemA == itemNameB && combination.itemB == itemNameA)
                )
                {
                    Debug.Log("Combination Result: " + combination.resultEffect);
                    break;
                }
            }
        }
    }
}
