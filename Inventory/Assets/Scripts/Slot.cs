using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IDropHandler{

	public SlotData data;
	public Inventory inventory;

	public void OnDrop(PointerEventData eventData)
	{
		GameObject droppedGameObject = eventData.pointerDrag;
		Item droppedItem = droppedGameObject.GetComponent<Item> ();

		//Check if slot is empty

		if (data.itemData == null)
		{
			//move dropped item into this slot
			droppedItem.slotData.itemData = null;
			droppedItem.slotData = data;
		}
		else // the slot is not empty
		{
			//Get current item that occupies the slot
			GameObject currentItem = data.itemData.gameObject;

			//get the item script attached to that item
			Item item = currentItem.GetComponent<Item> ();

			//Set the item's slot to the dropped item's slot
			item.slotData = droppedItem.GetComponent<Item> ().slotData;

			//Set parent of current item to the dropped item
			item.transform.SetParent (droppedItem.slotData.gameObject.transform);

			//Set the position of item to the new parent
			item.transform.position = droppedItem.slotData.gameObject.transform.position;

			//Set values inside of dropped item

			//Set slot to new slot
			droppedItem.slotData = data;

			//Set parent to new parent
			droppedItem.transform.SetParent (transform);

			//Set position to new position
			droppedItem.transform.position = transform.position;
		}
	}
}
