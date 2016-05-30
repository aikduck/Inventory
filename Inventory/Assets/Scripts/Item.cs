using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IEndDragHandler {

	public ItemData data;
	public SlotData slotData;
	public int amount = 0;

	private Text stackAmount;

	private Transform originalSlot;
	private Vector3 offset;

	private CanvasGroup canvasGroup;

	// Use this for initialization
	void Start () 
	{
		stackAmount = GetComponentInChildren<Text> ();
		canvasGroup = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (stackAmount != null)
		{
			stackAmount.text = amount.ToString ();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//Check if item is even in the slot
		if(data != null)
		{
			//Store the original parent slot
			originalSlot = transform.parent;

			//Set the parent of this item to the slot panel
			transform.SetParent (originalSlot.parent);

			//Block raycasts upon dragging
			canvasGroup.blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData eventdata)
	{
		//Check if item is even in slot
		if(data != null)
		{
			//Set the position of the item to the event data's position
			transform.position = eventdata.position - (Vector2)offset;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//check if item is even in the slot
		if(data != null)
		{
			offset = (Vector3)eventData.position - transform.position;
		}
	}

	public void OnEndDrag(PointerEventData eventdata)
	{
		
		transform.SetParent (slotData.gameObject.transform);
		transform.position = slotData.gameObject.transform.position;
		canvasGroup.blocksRaycasts = true;
	}
}
