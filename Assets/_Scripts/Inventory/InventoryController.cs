using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {

	#region public vars
	public int sizeX = 5;
	public int sizeY = 5;
	public float _inventoryHeightPercentage = 50.0f;					//Height in percent of window
	public float _inventoryWidthPercentage = 50.0f;					//Width in percent of window, atm equal to height for a square inventory

	public Texture2D inventoryBackground;
	public Texture2D slotBackground;
	public Texture2D slotHighlighted;

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

		debug();
	}

	#region methods
	/// <summary>
	/// Builds the grid.
	/// </summary>
	private void buildGrid(){
		//Build from top left to bootom right
		int counter = 0;
		for(int x = 0; x < this.sizeX; x++){
			for(int y = 0; y < this.sizeY; y++){
				//Create a new tile
				Slot newTile = new Slot(counter++, x, y);
				this._Slots.Add(newTile);
			}
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.I))
			this.toogleInventory();
	}

	/// <summary>
	/// Debug this instance.
	/// </summary>
	public void debug(){
		Debug.Log("This Inventory has a Grid of " + this.sizeX + " by " + this.sizeY + ". A total of " + this._Slots.Count + " Slots");
	}
	#endregion

	#region drawingFunctions
	public void OnGUI(){

		if(!this._visible)
			return;

		//Background
		float calculatedHeight = Screen.height * (_inventoryHeightPercentage / 100);
		float calculatedWidth = calculatedHeight;
		float calculatedYOffset = Screen.height * 0.05f;
		float calculatedXOffset = Screen.width - calculatedYOffset - calculatedWidth;

		GUI.Label(new Rect(calculatedXOffset,calculatedYOffset,calculatedWidth, calculatedHeight), this.inventoryBackground);
	}
	#endregion
}
