using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// This implements a basic inventory with drag and drop functionality. 
/// The InventoryController can be added to every GameObject and provides it with a simple Inventory.
/// </summary>
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
    
	private GameObject _CanvasContainer;
	private GameObject _Canvas;
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
        
		createInventoryCanvas(this.inventoryWidthPercentage, this.inventoryHeightPercentage);

		/*
        //Set inventory size according to settings
        recalculateInventoryCanvas();

		//inizialize inventory and build grid
		buildGrid();

		//Add Panels once to inventory
		addPanels();
		*/
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

		return;

		this._Canvas.GetComponent<Canvas>().enabled = visible;

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
			slot.getUIPanel().transform.parent = _Canvas.transform.Find("InventoryBackground/ContentMask/SlotContent/ScrollContent").transform;
			slot.getUIPanel().GetComponent<RectTransform>().anchoredPosition = new Vector2(slot.getStartPos().x, slot.getStartPos().y);
		}
	}

	/// <summary>
	/// Creates the inventory canvas.
	/// </summary>
	/// <param name="width">
	/// Defines the width of the inventory in percent.
	/// </param>
	public void createInventoryCanvas(float width, float height){
		Debug.Log("createInventoryCanvas()");

		//create canvascontainer
		this._CanvasContainer = new GameObject("InventoryCanvasContainer");
		this._CanvasContainer.layer = 5;

		//create canvas
		this._Canvas = new GameObject("BaseCanvas");
		this._Canvas.layer = 5;
		this._Canvas.transform.parent = this._CanvasContainer.transform;
		this._Canvas.AddComponent<Canvas>();
		this._Canvas.AddComponent<GraphicRaycaster>();

		//Set Viewport
		this._Canvas.GetComponent<Canvas>().renderMode = new RenderMode();

		//Create Background
		GameObject background = new GameObject("InventoryBackground");
		background.transform.parent = this._Canvas.transform;
		background.layer = 5;
		background.AddComponent<RectTransform>();
		background.AddComponent<CanvasRenderer>();
		background.AddComponent<Image>();

		//Create Slotcontent
		GameObject slotContent = new GameObject("SlotContent");
		slotContent.transform.parent = background.transform;
		slotContent.layer = 5;
		slotContent.AddComponent<Image>();
		slotContent.AddComponent<ScrollRect>();
		slotContent.AddComponent<Mask>();

		//Create ScrollContent
		GameObject scrollContent = new GameObject("ScrollContent");
		scrollContent.transform.parent = slotContent.transform;
		scrollContent.layer = 5;
		scrollContent.AddComponent<Image>();

		//Create scrollbar
		GameObject scrollBar = createSlider("ScrollBar", slotContent, 5);

		//Configure Background
		background.GetComponent<Image>().sprite = this.inventoryBackground;
		//Position x-Right / y-Center of screen
		background.GetComponent<RectTransform>().anchorMin = new Vector2(1f,0.5f);
		background.GetComponent<RectTransform>().anchorMax = new Vector2(1f,0.5f);
		//Calculate background size
		background.GetComponent<RectTransform>().sizeDelta = calculateInventoryCanvasSize();
		//Move 200px to left
		background.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, 0);

		//Configure SlotContent
		slotContent.GetComponent<RectTransform>().sizeDelta = new Vector2(
			this._Canvas.transform.Find("InventoryBackground").GetComponent<RectTransform>().sizeDelta.x - 30,
			this._Canvas.transform.Find("InventoryBackground").GetComponent<RectTransform>().sizeDelta.y -30
		);

		//Configure ScrollRect
		slotContent.GetComponent<ScrollRect>().horizontal = false;
		slotContent.GetComponent<ScrollRect>().vertical = true;
		slotContent.GetComponent<ScrollRect>().content = scrollContent.GetComponent<RectTransform>();

		//Configure ScrollContent
		scrollContent.GetComponent<Image>().color = new Color(0.1f,0.1f,0.1f,0.5f);
		scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(
			this._Canvas.transform.Find("InventoryBackground").GetComponent<RectTransform>().sizeDelta.x - 30,
			600.0f
		);

		//Configure ScrollBar

	}

	/// <summary>
	/// Creates the slider.
	/// </summary>
	/// <returns>The slider.</returns>
	/// <param name="name">Name.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="targetLayer">Target layer.</param>
	public GameObject createSlider(string name, GameObject parent, int targetLayer){

		//Create Slider GameObject
		GameObject slider = new GameObject(name);
		slider.transform.parent = parent.transform;
		slider.layer = targetLayer;
		slider.AddComponent<Image>();
		slider.AddComponent<Slider>();

		slider.GetComponent<Image>().sprite = Resources.Load<Sprite>("Background");

		//Create FillArea
		GameObject fillArea = new GameObject("FillArea");
		fillArea.transform.parent = slider.transform;
		fillArea.AddComponent<RectTransform>();

		//Create Fill
		GameObject fill = new GameObject("Fill");
		fill.transform.parent = fillArea.transform;
		fill.AddComponent<Image>();
		fill.GetComponent<Image>().sprite =  Resources.Load<Sprite>("GUISprite");
		fill.GetComponent<Image>().color = new Color(0,0,0,0); //invisible

		//Create Handle Area
		GameObject handleArea = new GameObject("HandleArea");
		handleArea.transform.parent = slider.transform;
		handleArea.AddComponent<RectTransform>();

		//Create Handle
		GameObject handle = new GameObject("Handle");
		handle.transform.parent = handleArea.transform;
		handle.AddComponent<Image>();
		handle.GetComponent<Image>().sprite =  Resources.Load<Sprite>("GUISprite");
		handle.GetComponent<Image>().color = new Color(0.9f,0.9f,0.9f, 0.9f);

		//Set overall deltaSize of slider
		slider.GetComponent<RectTransform>().sizeDelta = new Vector2(30.0f, 300.0f);
		
		//Add Rects to slider
		slider.GetComponent<Slider>().fillRect = fill.GetComponent<RectTransform>();
		slider.GetComponent<Slider>().handleRect = handle.GetComponent<RectTransform>();


		//Configure Fill and handles
		//FillArea
		fillArea.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
		fillArea.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
		fillArea.GetComponent<RectTransform>().sizeDelta = new Vector2(10.0f, 5.0f);
		fillArea.GetComponent<RectTransform>().localPosition = new Vector3(10.0f, 5.0f, 0);

		//HandleArea
		handleArea.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
		handleArea.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
		handleArea.GetComponent<RectTransform>().sizeDelta = new Vector2(10.0f, 10.0f);
		handleArea.GetComponent<RectTransform>().localPosition = new Vector3(10.0f, 10.0f, 0);

		//Fill
		fill.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1.0f);
		fill.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
		fill.GetComponent<RectTransform>().sizeDelta = new Vector2(10.0f, 0.0f);
		fill.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);

		//Handle
		handle.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1.0f);
		handle.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
		handle.GetComponent<RectTransform>().sizeDelta = new Vector2(10.0f, 0.0f);
		handle.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 0.0f);


		//Configure Slider
		slider.GetComponent<Slider>().direction = Slider.Direction.TopToBottom;
		slider.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.5f);

		return slider;
	}

	/// <summary>
	/// Calculates the size of the inventory canvas.
	/// </summary>
	/// <returns>The inventory canvas size.</returns>
	public Vector2 calculateInventoryCanvasSize(){
		Vector2 rectDelta = new Vector2(0,0);

		//X
		rectDelta.x = slotMarginX * (slotsX + 1) + slotSize.x * slotsX;
		//Y
		rectDelta.y = slotMarginY * (slotsY + 1) + slotSize.y * slotsY;

		return rectDelta;
	}

    public void recalculateInventoryCanvas(){
        
		return;

		Vector2 rectDelta = new Vector2(0,0);
    
        //Calc Width
		rectDelta.x = slotMarginX * (slotsX + 1) + slotSize.x * slotsX + _Canvas.transform.Find("InventoryBackground/Scrollbar").GetComponent<RectTransform>().sizeDelta.x;
		Debug.Log(_Canvas.transform.Find("InventoryBackground/Scrollbar").GetComponent<RectTransform>().sizeDelta.x);
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
