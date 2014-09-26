using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {

	#region public vars
	public int sizeX = 5;
	public int sizeY = 5;
	public Vector2 slotWidth = new Vector2(50.0f, 50.0f);
	public float _inventoryHeightPercentage = 50.0f;					//Height in percent of window
	public float _inventoryWidthPercentage = 50.0f;					//Width in percent of window, atm equal to height for a square inventory

	public Texture2D inventoryBackground;
	public Texture2D slotBackground;
	public Texture2D slotHighlighted;

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
		Vector2 startPos = new Vector2(20,-20);
		for(int y = 0; y < this.sizeY; y++){
			for(int x = 0; x < this.sizeX; x++){
				//Create a new tile
				Slot newTile = new Slot(counter++, "Slot " + counter, startPos, slotWidth);
				this._Slots.Add(newTile);

				//Set new Startpos
				startPos.x = startPos.x + slotWidth.x + 20;
			}
			startPos.x = 20;
			startPos.y = startPos.y - slotWidth.y - 20;
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
		Debug.Log("This Inventory has a Grid of " + this.sizeX + " by " + this.sizeY + ". A total of " + this._Slots.Count + " Slots");
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
			slot.getUIPanel().transform.parent = _Canvas.transform.Find("Background").transform;
			slot.getUIPanel().GetComponent<RectTransform>().anchoredPosition = new Vector2(slot.getStartPos().x, slot.getStartPos().y);
		}
	}

	private void updateCanvas(){
		//Add 
	}
	#endregion
}
