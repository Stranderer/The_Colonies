using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler {
	#region private vars
	public int _slotId;
	private RectTransform _spriteContainer;
	private string _slotName;
	private Vector2 _slotStartPos;
	private Vector2 _slotSizeDelta;
	public GameObject _occupyingItem;
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

		this._spriteContainer = (RectTransform)transform.FindChild("SpriteContainer");
		//Add stackcount info

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

		if(this._occupyingItem == null || this._occupyingItem.GetComponent<Item>().getCount() < 1){
			this._occupyingItem.GetComponent<Item>().setCount(0);
			this._occupyingItem = null;
			return null;
		}

		GameObject item = this._occupyingItem;
		this._occupyingItem = null;

		updateStackInfo();

		return item;
	}

	public GameObject peekItem(){
		return this._occupyingItem;
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
		this._occupyingItem.GetComponent<Item>().setParentTransform(this._spriteContainer.transform);
		this._occupyingItem.GetComponent<Item>().setUIPosition(new Vector2(0,0));
		this._occupyingItem.GetComponent<Item>().setUISize(new Vector2(this._slotSizeDelta.x -10.0f, this._slotSizeDelta.y - 10.0f));

		updateStackInfo();
		//Set parentSlot
		//this._occupyingItem.GetComponent<Item>().setSlot(gameObject);

		return true;
	}
	#endregion

	#region drag functionality
	//Drag
	public virtual void OnDrag(PointerEventData eventData){
		Debug.Log("Dragged");

		//Check if this slot is occupied
		if(this.isFree())
			return;

		if(this._spriteContainer.parent == transform)
			this._spriteContainer.parent = GameObject.Find ("InventoryCanvasContainer/Canvas").transform;


		//Update Item Icon position
		this._spriteContainer.position = Input.mousePosition;

	}
	
	//Dropping
	public virtual void OnEndDrag(PointerEventData eventData){
		Debug.Log("Dropped");

		//Indicates if allready dropped
		bool dropped = false;

		//Check if this slot is occupied
		if(this.isFree())
			return;

		//Get EventSystem
		EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		List<RaycastResult> rayCastResults = new List<RaycastResult>();
		eventSystem.RaycastAll(eventData,rayCastResults);


		foreach(RaycastResult hit in rayCastResults)
			Debug.Log(hit.go);

		//Check for a Slot and add or swap items if one is found
		foreach(RaycastResult hit in rayCastResults){
			Slot newSlot = hit.go.GetComponent<Slot>();
			if(newSlot != null){
				//Add Item if its the same
				if(newSlot.peekItem() != null && peekItem().GetComponent<Item>().compareToOther(newSlot.peekItem().GetComponent<Item>())){
					newSlot.peekItem().GetComponent<Item>().setCount(newSlot.peekItem().GetComponent<Item>().getCount() + peekItem().GetComponent<Item>().getCount());
					//Destroy this object
					Destroy (getItem());
				}else{
					this.swapItems(newSlot.gameObject);
				}
				updateStackInfo();
				newSlot.updateStackInfo();

				dropped = true;

				break;
			}
		}

		//Check if dropped, if not, create worldspace object
		if(!dropped){
			this._occupyingItem.GetComponent<Item>().createWorldSpaceInstance();
		}

		if(this._spriteContainer != null){
			this._spriteContainer.position = this._slotStartPos;
			this._spriteContainer.transform.parent = transform;
			this._spriteContainer.anchoredPosition = new Vector2(0.0f,0.0f);
		}
	}
	#endregion

	#region methods

	public void stackIncrease(){
		if(this.isFree())
			return;

		this._occupyingItem.GetComponent<Item>().increaseCount();
		updateStackInfo();
	}
	public void stackDecrease(){
		if(this.isFree())
			return;

		this._occupyingItem.GetComponent<Item>().decreaseCount();
		updateStackInfo();
	}

	public void updateStackInfo(){
		int stackValue = 0;

		if(this._occupyingItem)
			stackValue = this._occupyingItem.GetComponent<Item>().getCount();

		Text stackInfoText = this._spriteContainer.FindChild("StackInfo").GetComponent<Text>();
		if(stackValue > 1){
			stackInfoText.text = stackValue.ToString();
		}else{
			stackInfoText.text = "";
		}
	}


	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData){
		//Debug.Log (this._slotName + " ENTER");
		//gameObject.GetComponent<Image>().color = new Color32(200,200,200,150);
		showStats();
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData){
		//Debug.Log (this._slotName + " EXIT");
		//gameObject.GetComponent<Image>().color = Color.white;
		hideStats();
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
		infoText += "Slot ID: " + this.getId() + ln;
		infoText += "Slot stackSize: " + this._occupyingItem.GetComponent<Item>().getCount() + ln;

		panelText.text = infoText;

		//Resize infopanel TODO: If infopanel should be resizeable use this
		//_InfoPanel.GetComponent<TextResizer>().resize(new Vector2(-1.0f, 0.0f));

		//Show info
		//_InfoPanel.SetActive(true);
	}

	/// <summary>
	/// Swaps the item of this Slot with the Target slot or simply add the Item to the target if the target is empty or stackable with the same item.
	/// </summary>
	/// <returns><c>true</c>, if items were swaped, <c>false</c> otherwise.</returns>
	/// <param name="otherSlot">target slot.</param>
	public bool swapItems(GameObject otherSlot){
		Debug.Log("swapItems()");


		if(otherSlot == null)
			return false;

		Slot targetSlot = otherSlot.GetComponent<Slot>();
		Item thisItem = getItem().GetComponent<Item>();
		GameObject otherObject = targetSlot.peekItem();
		Item otherItem = null;

		if(otherObject){
			otherItem = targetSlot.getItem().GetComponent<Item>();
		}

		//add Item to other slot
		if(thisItem != null){
			targetSlot.setItem(thisItem.gameObject);
		}
		
		if(otherItem != null){
			setItem(otherItem.gameObject);
		}

		return true;
	}
	/// <summary>
	/// Hides the stats.
	/// </summary>
	public void hideStats(){
		//Clear window and hide it
		_InfoPanel.transform.FindChild("Text").GetComponent<Text>().text = "";
		//_InfoPanel.SetActive(false);
	}
	/// <summary>
	/// is this slot free.
	/// </summary>
	/// <returns><c>true</c>, if free, <c>false</c> otherwise.</returns>
	public bool isFree(){
		return this._occupyingItem == null;
	}
	/// <summary>
	/// Resets the size.
	/// </summary>
	public void resetSize(){
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_slotSizeDelta.x, _slotSizeDelta.y);
	}
	/// <summary>
	/// Resets the position.
	/// </summary>
	public void resetPosition(){
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(_slotStartPos.x, _slotStartPos.y);
	}
	#endregion
}
