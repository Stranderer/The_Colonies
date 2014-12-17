using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseItem : MonoBehaviour{
	#region private vars
	private int _itemId;
	private string _itemName;
	private string _itemDescr;
	private int _itemValue;
	private Sprite _itemInventorySprite;
	private ItemType _itemType;
	private bool _stackable;
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
		gameObject.AddComponent<EventTrigger>();

		//Add Values to components
		RectTransform rect = this.GetComponent<RectTransform>();
		
		rect.anchorMin = new Vector2(0.5f, 0.5f);
		rect.anchorMax = new Vector2(0.5f, 0.5f);
		rect.pivot = new Vector2(0.5f, 0.5f);

		//Set size
		rect.sizeDelta = new Vector2(40.0f, 40.0f);

		//Set UI background
		gameObject.GetComponent<Image>().sprite = this._itemInventorySprite;

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
	#endregion

	#region methods
	void OnMouseOver(){
		Debug.Log("Hover over " + this._itemName + this._itemId);
		gameObject.GetComponent<Image>().color = Color.cyan;
	}


	/// <summary>
	/// Uses and/or consumes this Item. The outcome depends on the ItemType property of this item.  
	/// </summary>
	/// <returns>Returns true if item is used or consumed, false if not. (cannot be used or consumed)<c/returns>
	public bool use(){
		return true;
	}
	#endregion

	#region drag functionality
	public void drag(){

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
