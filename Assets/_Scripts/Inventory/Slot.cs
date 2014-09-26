using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slot{
	#region private vars
	private int _slotId;
	private string _slotName;
	private Vector2 _slotStartPos;
	private Vector2 _slotSizeDelta;
	private GameObject _UIPanel;
	private BaseItem _occupyingItem;
	#endregion

	#region constructor
	public Slot(){
		this._slotId = -1;
		this._slotName = "Default Slotname";
		this._slotStartPos = new Vector2(0,0);

		_UIPanel = new GameObject(this._slotName);
		_UIPanel.AddComponent<RectTransform>();
		_UIPanel.AddComponent<CanvasRenderer>();
		_UIPanel.AddComponent<Image>();

	}
	public Slot (int slotId, string slotName, Vector2 slotStartPos, Vector2 slotSizeDelta)
	{
		this._slotId = slotId;
		this._slotName = slotName;
		this._slotStartPos = slotStartPos;
		this._slotSizeDelta = slotSizeDelta;

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
		rect.sizeDelta = _slotSizeDelta;
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
	public BaseItem getItem(){
		BaseItem item = this._occupyingItem;
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
	public void setItem(BaseItem newItem){
		this._occupyingItem = newItem;
	}
	#endregion

	#region methods
	public void resetPosition(){
		_UIPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(_slotStartPos.x, _slotStartPos.y);
	}
	#endregion
}
