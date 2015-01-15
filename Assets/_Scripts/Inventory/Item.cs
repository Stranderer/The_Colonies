﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IDragHandler, IEndDragHandler {
	#region private vars
	public int _itemId;
	public string _itemName;
	public string _itemDescr;
	public int _itemValue;
	public bool _stackable;
	private bool _disengaged = false;				//Is sprite desingaged from slot for z-order?


	public Sprite _itemInventorySprite;
	public ItemType _itemType;
	private Transform _parentTransform;
	private GameObject _parentSlot;
	#endregion

	#region initialising
	public void init(int _itemId, string _itemName, string _itemDescr, int _itemValue, Sprite _itemSprite, ItemType _itemType, bool _stackable)
	{
		this._itemId = _itemId;
		this._itemName = transform.name = _itemName;
		this._itemDescr = _itemDescr;
		this._itemValue = _itemValue;
		this._itemInventorySprite = _itemSprite;
		this._itemType = _itemType;
		this._stackable = _stackable;


		transform.parent = transform;
		//Add nessesary components
		gameObject.AddComponent<RectTransform>();
		gameObject.AddComponent<CanvasRenderer>();
		gameObject.AddComponent<Image>();
		//gameObject.AddComponent<EventTrigger>();

		//Add Values to components
		RectTransform rect = this.GetComponent<RectTransform>();
		
		rect.anchorMin = new Vector2(0.5f, 0.5f);
		rect.anchorMax = new Vector2(0.5f, 0.5f);
		rect.pivot = new Vector2(0.5f, 0.5f);

		//Set size
		rect.sizeDelta = new Vector2(40.0f, 40.0f);

		//Set UI background
		gameObject.GetComponent<Image>().sprite = this._itemInventorySprite;

		//Set EventTrigger
		//EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
		//PointerEventData data = new PointerEventData(GameObject.Find ("EventSystem").GetComponent<EventSystem>());

		//trigger.OnPointerDown(data);

	}
	
	#endregion

	#region getter
	public int getId(){
		return this._itemId;
	}
	public string getName(){
		return this._itemName;
	}
	public string getDescription(){
		return this._itemDescr;
	}
	public int getValue(){
		return this._itemValue;
	}
	public ItemType getType(){
		return this._itemType;
	}
	public Sprite getTexture(){
		return this._itemInventorySprite;
	}
	public bool isStackable(){
		return this._stackable;
	}
	public bool isDisengaged(){
		return this._disengaged;
	}
	public Transform getParentTransform(){
		return this._parentTransform;
	}
	public GameObject getSlot(){
		return this._parentSlot;
	}
	#endregion

	#region setter
	public void setId(int id){
		this._itemId = id;
	}
	public void setName(string name){
		this._itemName = name;
	}
	public void setDescription(string desc){
		this._itemDescr = desc;
	}
	public void setValue(int value){
		this._itemValue = value;
	}
	public void setType(ItemType itemType){
		this._itemType = itemType;
	}
	public void setStackable(bool stackable){
		this._stackable = stackable;
	}
	public void setParentTransform(Transform parentTransform){
		this._parentTransform = parentTransform;
		gameObject.transform.parent = parentTransform;
	}
	public void setSlot(GameObject newSlot){
		this._parentSlot = newSlot;
	}
	public void clearSlot(){
		this._parentSlot = null;
	}
	#endregion

	#region methods
	/// <summary>
	/// Uses and/or consumes this Item. The outcome depends on the ItemType property of this item.  
	/// </summary>
	/// <returns>Returns true if item is used or consumed, false if not. (cannot be used or consumed)<c/returns>
	public bool use(){
		return true;
	}

	/// <summary>
	/// Disengages the Item Canvas Sprite from the Slot for top z-position.
	/// </summary>
	public void disengage(){
		
		if(this._disengaged)
			return;
		else
			this._disengaged = true;

		//Set new parent (topCanvas. This places the sprite on top and makes it dragable over the whole canvas)
		gameObject.transform.parent = GameObject.Find ("InventoryCanvasContainer/Canvas").transform;
	}

	/// <summary>
	/// Engage the Item Canvas Sprite back to the slot.
	/// </summary>
	public void engage(){
		if(!this._disengaged)
			return;
		else
			this._disengaged = false;
		
		//Set new parent 
		gameObject.transform.parent = this._parentTransform;
	}
	#endregion

	#region drag functionality
	//Drag
	public virtual void OnDrag(PointerEventData eventData){
		Debug.Log("Dragged");
		disengage();

		//Update Item Icon position
		gameObject.transform.position = Input.mousePosition;
	}

	//Dropping
	public virtual void OnEndDrag(PointerEventData eventData){
		Debug.Log("Dropped");

		//Get EventSystem
		EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		List<RaycastResult> rayCastResults = new List<RaycastResult>();
		eventSystem.RaycastAll(eventData,rayCastResults);

		//Check for a Slot and add or swap items if one is found
		foreach(RaycastResult hit in rayCastResults){
			Slot newSlot = hit.go.GetComponent<Slot>();
			if(newSlot != null){
				Debug.Log("Slot found!");
				Debug.Log (newSlot);
				newSlot.swapItems(this.getSlot());
			}
		}

		//Check if element is over a slot
		//if(eventData.pointerCurrentRaycast){
			//Debug.Log ("Over " + slot.name);
		//}

		if(this._disengaged){
			Debug.Log ("Dropped");
			engage();
			//Update Item Icon position
			gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f,0.0f);
		}
	}
	#endregion

	#region ui methods
	public void setUIPosition(Vector2 position){
		gameObject.GetComponent<RectTransform>().anchoredPosition = position;
	}
	public void setUISize(Vector2 sizeDelta){
		gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}
	#endregion
}
