using UnityEngine;
using System.Collections;

public class BaseItem {
	#region private vars
	private int _itemId;
	private string _itemName;
	private string _itemDescr;
	private int _itemValue;
	private Texture2D _itemInventoryTexture;
	private ItemType _itemType;
	private bool _stackable;
	#endregion

	#region enums
	public enum ItemType {
		Weapon,
		Consumable,
		Quest,
		Ressource
	}
	#endregion

	#region constructors
	public BaseItem (int _itemId, string _itemName, string _itemDescr, int _itemValue, Texture2D _itemTexture, ItemType _itemType, bool _stackable)
	{
		this._itemId = _itemId;
		this._itemName = _itemName;
		this._itemDescr = _itemDescr;
		this._itemValue = _itemValue;
		this._itemInventoryTexture = _itemTexture;
		this._itemType = _itemType;
		this._stackable = _stackable;
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
}
