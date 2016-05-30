using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SlotData
{
	public GameObject gameObject;
	public ItemData itemData;

	public SlotData(GameObject gameObject, ItemData itemData)
	{
		this.gameObject = gameObject;
		this.itemData = itemData;
	}
}

[RequireComponent(typeof(ItemDatabase))]
public class Inventory : MonoBehaviour {

	[Header("UI")]

	public int slotAmount = 20;
	public GameObject slotPanel;

	[Header("Prefabs")]

	public GameObject slotPrefab;
	public GameObject itemPrefab;

	[Header("Items / Slots")]

	public List<ItemData> items = new List<ItemData>();
	public List<SlotData> slots = new List<SlotData>();



	private ItemDatabase database;

	// Use this for initialization
	void Start () 
	{
		//Get the item database
		database = GetComponent<ItemDatabase>();

		//Loop through by the slot amount
		for (int i = 0; i < slotAmount; i++) 
		{
			//create all the slots under slotPanel
			GameObject clone = Instantiate (slotPrefab);
			clone.transform.SetParent (slotPanel.transform);

			//Create a new slotdata variable
			SlotData slotData = new SlotData(clone, null);

			//Get slot component from list
			Slot slot = clone.GetComponent<Slot>();
			slot.inventory = this;
			slot.data = slotData;

			//Add new slots to list
			slots.Add(slotData);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			AddItemById (1, 8);
		}
	}

	public void AddItemById(int id, int itemAmount = 1)
	{
		//Get an item from database by id
		ItemData newItem = database.GetItemById (id);

		//Get an empty slot in our inventory
		SlotData newSlot = GetEmptySlot();

		//Check is newItem and new slot is not equal to null
		if(newItem != null && newSlot != null)
		{
			//Check if item can be stacked
			if (HasStacked(newItem, itemAmount))
			{
				return;
			}

			//Set the empty slot
			newSlot.itemData = newItem;

			//Create a new item instance
			GameObject itemGameObject = Instantiate(itemPrefab);

			//Set item to be in same position as slot
			itemGameObject.transform.position = newSlot.gameObject.transform.position;

			//Set it's parent in the hierachy
			itemGameObject.transform.SetParent(newSlot.gameObject.transform);
			itemGameObject.name = newItem.Title;

			//Set the item's Gameobject
			newItem.gameObject = itemGameObject;

			//get the image component of that item and set it
			Image image = itemGameObject.GetComponent<Image>();
			image.sprite = newItem.sprite;

			//Get the item component and set it's data
			Item item = itemGameObject.GetComponent<Item>();
			item.data = newItem;
			item.slotData = newSlot;
		}
	}

	public SlotData GetEmptySlot()
	{
		//Loop through all of our slots
		for (int i = 0; i < slots.Count; i++) 
		{
			//Check if the item inside is null
			if (slots[i].itemData == null)
			{
				//Return that slot
				return slots[i];
			}
		}

		print ("No empty slots");
		return null;
	}

	bool HasStacked (ItemData itemToAdd, int itemAmount = 1)
	{
		//Check if item is stackable
		if (itemToAdd.Stackable)
		{
			//Check if a slot already has an item in inventory
			SlotData occupiedSlot = GetSlotWithItemData (itemToAdd);

			if (occupiedSlot != null)
			{
				//increase the item's amount variable for text
				ItemData itemData = occupiedSlot.itemData;
				Item item = itemData.gameObject.GetComponent<Item> ();
				item.amount += itemAmount;

				return true;
			}
		}

		return false;
	}

	//Find an item in the inventory
	SlotData GetSlotWithItemData(ItemData item)
	{
		for (int i = 0; i < slots.Count; i++) 
		{
			ItemData currentItem = slots [i].itemData;

			if (currentItem != null && currentItem.Id == item.Id)
			{
				return slots [i];
			}
		}

		return null;
	}
}
