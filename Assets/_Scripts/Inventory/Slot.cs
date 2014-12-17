using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slot : MonoBehaviour{
	#region private vars
	private int _slotId;
	private string _slotName;
	private Vector2 _slotStartPos;
	private Vector2 _slotSizeDelta;
	private GameObject _UIPanel;
	private GameObject _occupyingItem;
	#endregion

	#region initialisation
	public void init(int slotId, string slotName, Vector2 slotStartPos, Vector2 slotSizeDelta)
	{
		this._slotId = slotId;
		this._slotName = transform.name = slotName;
		this._slotStartPos = slotStartPos;
		this._slotSizeDelta = slotSizeDelta;

		resetSize();
		resetPosition();
/*
		_UIPanel = new GameObject(this._slotName);
		_UIPanel.AddComponent<RectTransform>();
		_UIPanel.AddComponent<CanvasRenderer>();
		_UIPanel.AddComponent<Image>();

		//Add Values to components
		RectTransform rect = _UIPanel.GetComponent<RectTransform>();

		rect.anchorMin = new Vector2(0.0f, 1.0f);
		rect.anchorMax = new Vector2(0.0f, 1.0f);
		rect.pivot = new Vector2(0.0f, 1.0f);

		rect.anchoredPosition = new Vector2(_slotStartPos.x, _slotStartPos.y);
		rect.sizeDelta = _slotSizeDelta;*/
	}
	#endregion

	#region getter
	public int getId(){
		return this._slotId;
	}
	public string getName(){
		return this._slotName;
	}
	public Vector2 getStartPos(){
		return this._slotStartPos;
	}

	/// <summary>
	/// Gets the item and removes it from slot.
	/// </summary>
	/// <returns>The item.</returns>
	public GameObject getItem(){
		GameObject item = this._occupyingItem;
		this._occupyingItem = null;
		return item;
	}

	public GameObject getUIPanel(){
		return this._UIPanel;
	}

	#endregion

	#region setters
	public void setId(int id){
		this._slotId = id;
	}
	public void setName(string name){
		this._slotName = name;
	}
	public void setStartPos(Vector2 startPos){
		this._slotStartPos = startPos;
	}
	public bool setItem(GameObject newItem){

		//Is this an item
		if(newItem.GetComponent<BaseItem>() == null)
			return false;

		this._occupyingItem = newItem;
		//Set parent of item gameobject
		this._occupyingItem.transform.parent = transform;
		this._occupyingItem.GetComponent<BaseItem>().setUIPosition(new Vector2(0,0));
		this._occupyingItem.GetComponent<BaseItem>().setUISize(new Vector2(this._slotSizeDelta.x -10.0f, this._slotSizeDelta.y - 10.0f));

		return true;
	}
	#endregion

	#region methods
	public void highlight(){
		if(GetComponent<Image>().color == Color.white)
			GetComponent<Image>().color = new Color(0.9f,0.9f,0.9f);
		else
			GetComponent<Image>().color = Color.white;
	}

	public void showStats(){
		if(this.isFree())
			return;

	}

	public void hideStats(){
		if(this.isFree())
			return;
	}

	public bool isFree(){
		return this._occupyingItem == null;
	}
	public void resetSize(){
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_slotSizeDelta.x, _slotSizeDelta.y);
	}
	public void resetPosition(){
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(_slotStartPos.x, _slotStartPos.y);
	}
	#endregion
}
