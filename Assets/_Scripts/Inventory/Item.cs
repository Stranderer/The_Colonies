using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour{
	#region private vars
	public int _itemId;
	public string _itemName;
	public string _itemDescr;
	public int _itemValue;
	public int _itemCount;
	public bool _itemStackable;

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
		this._itemCount = 1;
		this._itemStackable = _stackable;


		//transform.parent = transform;
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
	public int getCount(){
		return this._itemCount;
	}
	public ItemType getType(){
		return this._itemType;
	}
	public Sprite getTexture(){
		return this._itemInventorySprite;
	}
	public bool isStackable(){
		return this._itemStackable;
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
	public void setCount(int count){
		this._itemCount = count;
	}
	public void setType(ItemType itemType){
		this._itemType = itemType;
	}
	public void setStackable(bool stackable){
		this._itemStackable = stackable;
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
	/// <returns>Returns true if item is used or consumed, false if not. (cannot be used or consumed)</returns>
	public bool use(){
		return true;
	}

	public void increaseCount(){
		this._itemCount++;
	}

	public void decreaseCount(){
		this._itemCount--;
	}

	/// <summary>
	/// Compares to other item.
	/// </summary>
	/// <returns><c>true</c>,if both items are the same, <c>false</c> otherwise.</returns>
	/// <param name="otherItem">Other item.</param>
	public bool compareToOther(Item otherItem){
		Debug.Log("compareToOther()");

		bool isSame = false;

		if(otherItem == null)
			return isSame;

		if(this.getName() == otherItem.getName() && this.getType() == otherItem.getType() && this.isStackable())
			isSame = true;

		return isSame;
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
