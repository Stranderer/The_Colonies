using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

	#region public vars
	public int slotsX = 5;
	public int slotsY = 8;
	public Vector2 slotSize = new Vector2(50.0f, 50.0f);
	public float inventoryHeightPercentage = 50.0f;					//Height in percent of window
	public float inventoryWidthPercentage = 50.0f;					//Width in percent of window, atm equal to height for a square inventory

    public int slotMarginX = 10;
    public int slotMarginY = 10;

	public Sprite inventoryBackground;
	public Sprite slotBackground;
	public Sprite slotHighlighted;

	public Canvas _Canvas;
	#endregion

	#region private vars
	private bool _visible; 							//can inventory be seen on screen
	private List<Slot> _Slots = new List<Slot>();
	private List<BaseItem> _Items = new List<BaseItem>();
	#endregion

	#region getter
	/// <summary>
	/// Is this inventory visible?
	/// </summary>
	/// <returns><c>true</c> if visible, <c>false</c> otherwise.</returns>
	public bool isVisible(){
		return this._visible;
	}
	#endregion

	#region setters
	/// <summary>
	/// Sets the visible.
	/// </summary>
	/// <param name="visible">If set to <c>true</c> inventory is visible.</param>
	public void toogleInventory(){
		this._visible = !this._visible;
	}
	#endregion
	
	public void Start(){
		Debug.Log("Start(): InventoryController");
        
        //Set inventory size according to settings
        recalculateInventoryCanvas();

		//inizialize inventory and build grid
		buildGrid();

		//Add Panels once to inventory
		addPanels();

		debug();
	}

	#region methods
	/// <summary>
	/// Builds the grid.
	/// </summary>
	private void buildGrid(){
		//Build from top left to bootom right
		int counter = 0;
		Vector2 startPos = new Vector2(slotMarginX,-slotMarginY);
		for(int y = 0; y < this.slotsY; y++){
			for(int x = 0; x < this.slotsX; x++){
				//Create a new tile
				Slot newTile = new Slot(counter++, "Slot " + counter, startPos, slotSize);
                newTile.getUIPanel().GetComponent<Image>().sprite = this.slotBackground;
				this._Slots.Add(newTile);

				//Set new Startpos
				startPos.x = startPos.x + slotSize.x + slotMarginX;
			}
			startPos.x = slotMarginX;
			startPos.y = startPos.y - slotSize.y - slotMarginY;
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.I))
			this.toogleInventory();

		//only update inventory if visible
		if(this._visible)
			this.updateCanvas();
		

		//Show / hide canvas
		this.toogleCanvas(this._visible);

	}

	/// <summary>
	/// Debug this instance.
	/// </summary>
	public void debug(){
		Debug.Log("This Inventory has a Grid of " + this.slotsX + " by " + this.slotsY + ". A total of " + this._Slots.Count + " Slots");
	}
	#endregion

	#region canvas methods
	private void toogleCanvas(bool visible){
		this._Canvas.enabled = visible;

		//if canvas is visible lock mousemove
		MouseLook[] mouseLookScripts = gameObject.GetComponentsInChildren<MouseLook>();
		foreach (MouseLook curScript in mouseLookScripts){
			curScript.setControlable(!visible);
		}

		//show and unlock cursor
		Screen.showCursor = visible;
		Screen.lockCursor = !visible;
	}

	private void addPanels(){
		//Add the Panels
		foreach(Slot slot in _Slots){
			slot.getUIPanel().transform.parent = _Canvas.transform.Find("Background/Content").transform;
			slot.getUIPanel().GetComponent<RectTransform>().anchoredPosition = new Vector2(slot.getStartPos().x, slot.getStartPos().y);
		}
	}

    public void recalculateInventoryCanvas(){
        Vector2 rectDelta = new Vector2(0,0);
    
        //Calc Width
        rectDelta.x = slotMarginX * (slotsX + 1) + slotSize.x * slotsX + _Canvas.transform.Find("Background/Scrollbar").GetComponent<RectTransform>().sizeDelta.x;
        Debug.Log(_Canvas.transform.Find("Background/Scrollbar").GetComponent<RectTransform>().sizeDelta.x);
        //Calc Height
        rectDelta.y = slotMarginY * (slotsY + 1) + slotSize.y * slotsY;
        //Resize Background
        _Canvas.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = rectDelta;

    }

	private void updateCanvas(){
		//Add 
	}
	#endregion
}
