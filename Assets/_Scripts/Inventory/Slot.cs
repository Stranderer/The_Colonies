using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
	#region private vars
	private int _slotId;
	private string _slotName;
	private Vector2 _slotStartPos;
	private Vector2 _slotSizeDelta;
	private GameObject _UIPanel;
	private GameObject _occupyingItem;
	private GameObject _InfoPanel; 			//InfoPanel Reference
	#endregion

	#region initialisation
	public void init(int slotId, string slotName, Vector2 slotStartPos, Vector2 slotSizeDelta, GameObject infoPanel)
	{
		this._slotId = slotId;
		this._slotName = transform.name = slotName;
		this._slotStartPos = slotStartPos;
		this._slotSizeDelta = slotSizeDelta;
		this._InfoPanel = infoPanel;

		resetSize();
		resetPosition();
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
		if(newItem.GetComponent<Item>() == null)
			return false;

		this._occupyingItem = newItem;
		//Set parent of item gameobject
		this._occupyingItem.GetComponent<Item>().setParentTransform(transform);
		this._occupyingItem.GetComponent<Item>().setUIPosition(new Vector2(0,0));
		this._occupyingItem.GetComponent<Item>().setUISize(new Vector2(this._slotSizeDelta.x -10.0f, this._slotSizeDelta.y - 10.0f));

		return true;
	}
	#endregion

	#region methods
	
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData){
		highlight();
		showStats();
	}
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData){
		highlight();
		hideStats();
	}

	public void highlight(){
		if(GetComponent<Image>().color == Color.white)
			GetComponent<Image>().color = new Color(0.9f,0.9f,0.9f);
		else
			GetComponent<Image>().color = Color.white;
	}

	public void showStats(){
		if(this.isFree())
			return;

		//Add Text to InfoPanel
		Text panelText = _InfoPanel.transform.FindChild("Text").GetComponent<Text>();
		panelText.text = "";

		Item item = _occupyingItem.GetComponent<Item>();
		string ln = System.Environment.NewLine;

		string infoText = "";
		infoText += "Item ID: " + item.getId() + ln;
		infoText += "Item Name: " + item.getName() + ln;
		infoText += "Item Value: " + item.getValue() + ln;
		infoText += "Item Beschreibung: " + item.getDescription() + ln;
		infoText += "Item Type: " + item.getType() + ln;
		infoText += "Item isStackable: " + item.isStackable() + ln;

		panelText.text = infoText;

		//Resize infopanel TODO: If infopanel should be resizeable use this
		//_InfoPanel.GetComponent<TextResizer>().resize(new Vector2(-1.0f, 0.0f));

		//Show info
		//_InfoPanel.SetActive(true);
	}

	public void hideStats(){
		//Clear window and hide it
		_InfoPanel.transform.FindChild("Text").GetComponent<Text>().text = "";
		//_InfoPanel.SetActive(false);
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
