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
	public int slotsX = 5;											//Initiate slot count in x
	public int slotsY = 8;											//Initiate slot count in y
	public Vector2 slotSize = new Vector2(50.0f, 50.0f);			//Size of each slot
	public float inventoryHeightPercentage = 50.0f;					//Height in percent of window
	public float inventoryWidthPercentage = 50.0f;					//Width in percent of window, atm equal to height for a square inventory

    public int slotMarginX = 10;									//Margin between slots in x
    public int slotMarginY = 10;									//Margin between slots in y

	public Sprite inventoryBackground;								//Sprite for InventoryBackground
	public Sprite slotBackground;									//Sprite for SlotBackground
	public Sprite slotHighlighted;									//Sprite for Slots on hover or active
    
	public GameObject slotPrefapp;									//Prefapp for a basic slot

	private GameObject _CanvasContainer;							//Holds the inventory root-canvas
	private GameObject _Canvas;										//Holds the actual inventory-canvas
	private GameObject _InfoPanel;									//Holds the info panel for hover infos
	#endregion

	#region private vars
	private bool _visible; 							//can inventory be seen on screen
	private List<GameObject> _Slots = new List<GameObject>();
	private List<GameObject> _Items = new List<GameObject>();
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
        
		//Create inventory canvas
		createInventoryCanvas(this.inventoryWidthPercentage, this.inventoryHeightPercentage);

		//inizialize slots and build grid
		initializeSlotGrid();

		//Add Panels once to inventory
		addPanels();

		debug();
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

	#region methods
	/// <summary>
	/// Initializes the slots and build up the grid.
	/// </summary>
	private void initializeSlotGrid(){
		//Build from top left to bootom right
		int counter = 0;
		Vector2 startPos = new Vector2(slotMarginX,-slotMarginY);
		for(int y = 0; y < this.slotsY; y++){
			for(int x = 0; x < this.slotsX; x++){
				//Instanciate a new tile
				//GameObject newTile = (GameObject)Instantiate(this.slotPrefapp, new Vector3(0,0,0), Quaternion.identity);

				GameObject newTile = createSlot();

				newTile.GetComponent<Slot>().init(counter++, "Slot " + counter, startPos, slotSize, _InfoPanel);
                newTile.GetComponent<Slot>().GetComponent<Image>().sprite = this.slotBackground;
				this._Slots.Add(newTile);

				//Set new Startpos
				startPos.x = startPos.x + slotSize.x + slotMarginX;
			}
			startPos.x = slotMarginX;
			startPos.y = startPos.y - slotSize.y - slotMarginY;
		}
	}

	/// <summary>
	/// Adds an item to the inventory.
	/// </summary>
	/// <param name="item">Item to add</param>
	public void addItem(GameObject item){

	}
	/// <summary>
	/// Adds multible Items to inventory.
	/// </summary>
	/// <param name="items">Items to add</param>
	public void addItem(List<GameObject> items){

		//Search a free slot for each item
		foreach(GameObject slot in _Slots){

			if(items.Count == 0)
				break;

			if(!slot.GetComponent<Slot>().isFree())
				continue;

			//Add item
			slot.GetComponent<Slot>().setItem(items[0]);
			//slot.getUIPanel().GetComponent<Image>().sprite = items[0].getTexture();
			items.RemoveAt(0);
		}

		//Group stackables into one item
		groupStackables();

		//Update stackInfos
		updateStackInfo();
	}

	public void groupStackables(){

		GameObject stackableSlot = null;
		foreach(GameObject slot in _Slots){

			GameObject peekItem = slot.GetComponent<Slot>().peekItem();
			if(peekItem == null || !peekItem.GetComponent<Item>().isStackable())
				continue;

			if(stackableSlot == null){
				stackableSlot = slot;
			} else {
				//Encrease Stack and empty item
				stackableSlot.GetComponent<Slot>().stackIncrease();
				GameObject item = slot.GetComponent<Slot>().getItem();
			    GameObject.Destroy(item);
			}
		}
	}

	public void updateStackInfo(){
		foreach(GameObject slot in _Slots){
			int stackValue = slot.GetComponent<Slot>().getItemCount();
			Text stackInfoText = slot.transform.FindChild("StackInfo").GetComponent<Text>();
			if(stackValue > 1){
				stackInfoText.text = stackValue.ToString();
			}else{
				stackInfoText.text = "";
			}
		}
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
		foreach(GameObject slot in _Slots){
			slot.GetComponent<Slot>().transform.parent = _Canvas.transform.Find("InventoryBackground/SlotContent/ScrollContent").transform;
			slot.GetComponent<Slot>().GetComponent<RectTransform>().anchoredPosition = new Vector2(slot.GetComponent<Slot>().getStartPos().x, slot.GetComponent<Slot>().getStartPos().y);
		}
	}

	/// <summary>
	/// Creates the inventory canvas.
	/// </summary>
	/// <param name="width">Width of the new inventory</param>
	/// <param name="height">Height of the new inventory</param>
	public void createInventoryCanvas(float width, float height){
		Debug.Log("createInventoryCanvas()");

		//create canvascontainer
		this._CanvasContainer = new GameObject("InventoryCanvasContainer");
		this._CanvasContainer.layer = 5;

		//create canvas
		this._Canvas = new GameObject("Canvas");
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
		//TODO: create a working Scrollbar from script
		//GameObject scrollBar = createSlider("ScrollBar", slotContent, 5);

		//Create Info panel
		this._InfoPanel = new GameObject("InfoPanel");
		this._InfoPanel.transform.parent = this._Canvas.transform;
		this._InfoPanel.layer = 5;
		this._InfoPanel.AddComponent<RectTransform>();
		this._InfoPanel.AddComponent<TextResizer>();
		this._InfoPanel.AddComponent<Image>();
		//Add infopanel text gameobject
		GameObject InfoPanelText = new GameObject("Text");
		InfoPanelText.transform.parent = this._InfoPanel.transform;
		InfoPanelText.layer = 5;
		InfoPanelText.AddComponent<RectTransform>();
		InfoPanelText.AddComponent<Text>();


		//Configure Background
		background.GetComponent<Image>().sprite = this.inventoryBackground;
		//Position x-Right / y-Center of screen
		background.GetComponent<RectTransform>().anchorMin = new Vector2(1f,0.5f);
		background.GetComponent<RectTransform>().anchorMax = new Vector2(1f,0.5f);
		//Calculate background size
		background.GetComponent<RectTransform>().sizeDelta = calculateInventoryCanvasSize();
		//Move 200px to left
		background.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1 * ((background.GetComponent<RectTransform>().sizeDelta.x / 2) + 20), 0);

		//Configure SlotContent
		slotContent.GetComponent<RectTransform>().sizeDelta = new Vector2(
			this._Canvas.transform.Find("InventoryBackground").GetComponent<RectTransform>().sizeDelta.x - 20,
			this._Canvas.transform.Find("InventoryBackground").GetComponent<RectTransform>().sizeDelta.y - 20
		);

		//Configure ScrollRect
		slotContent.GetComponent<ScrollRect>().horizontal = false;
		slotContent.GetComponent<ScrollRect>().vertical = true;
		slotContent.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Clamped;
		slotContent.GetComponent<ScrollRect>().content = scrollContent.GetComponent<RectTransform>();

		//Configure ScrollContent
		scrollContent.GetComponent<Image>().color = new Color(0.1f,0.1f,0.1f,0.5f);
		scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(
			this._Canvas.transform.Find("InventoryBackground").GetComponent<RectTransform>().sizeDelta.x - 20,
			600.0f
		);
		scrollContent.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,1.0f);
		scrollContent.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,1.0f);
		scrollContent.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1.0f);
		scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

		//Configure InfoPanel
		RectTransform backgroundRect = background.GetComponent<RectTransform>();
		this._InfoPanel.GetComponent<Image>().sprite = this.inventoryBackground;
		this._InfoPanel.GetComponent<RectTransform>().anchorMin = new Vector2(1f,0.5f);
		this._InfoPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1f,0.5f);
		this._InfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(backgroundRect.sizeDelta.x, backgroundRect.sizeDelta.y / 3);
		this._InfoPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(backgroundRect.anchoredPosition.x - backgroundRect.sizeDelta.x, backgroundRect.anchoredPosition.y + (backgroundRect.anchoredPosition.y - this._InfoPanel.GetComponent<RectTransform>().sizeDelta.y));
		//this._InfoPanel.SetActive(false); //hide per default

		//Configure InfoPanelText
		InfoPanelText.GetComponent<RectTransform>().sizeDelta = new Vector2(this._InfoPanel.GetComponent<RectTransform>().sizeDelta.x - 40, this._InfoPanel.GetComponent<RectTransform>().sizeDelta.y - 40);
		Font infoFont = (Font)Resources.Load("Fonts/arial");
		if(infoFont == null){
			Debug.Log("Font not found!");
			return;
		}
		InfoPanelText.GetComponent<Text>().font = infoFont;
		InfoPanelText.GetComponent<Text>().color = Color.black;
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

	public GameObject createSlot(){

		GameObject newPanel = new GameObject();
		newPanel.AddComponent<RectTransform>();
		newPanel.AddComponent<CanvasRenderer>();
		newPanel.AddComponent<Image>();
		newPanel.AddComponent<Slot>();

		GameObject stackInfo = new GameObject("StackInfo");
		stackInfo.transform.parent = newPanel.transform;
		stackInfo.AddComponent<RectTransform>();
		stackInfo.AddComponent<Text>();

		//Add Values to components
		RectTransform rect = newPanel.GetComponent<RectTransform>();

		rect.anchorMin = new Vector2(0.0f, 1.0f);
		rect.anchorMax = new Vector2(0.0f, 1.0f);
		rect.pivot = new Vector2(0.0f, 1.0f);

		//Config Stack info
		RectTransform rectStackInfo = stackInfo.GetComponent<RectTransform>();
		rectStackInfo.anchorMin = new Vector2(1.0f, 1.0f);
		rectStackInfo.anchorMax = new Vector2(1.0f, 1.0f);
		rectStackInfo.pivot = new Vector2(0.5f, 0.5f);
		rectStackInfo.anchoredPosition = new Vector2(-18.0f, -10.0f);
		//Font
		stackInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(30,22);
		Font infoFont = (Font)Resources.Load("Fonts/arial");
		stackInfo.GetComponent<Text>().font = infoFont;
		stackInfo.GetComponent<Text>().fontSize = 18;
		stackInfo.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
		stackInfo.GetComponent<Text>().color = Color.black;

		return newPanel;
	}

	/// <summary>
	/// Calculates the size of the inventory canvas.
	/// </summary>
	/// <returns>The inventory canvas size.</returns>
	public Vector2 calculateInventoryCanvasSize(){
		Vector2 rectDelta = new Vector2(0,0);

		//X
		rectDelta.x = slotMarginX * (slotsX + 1) + slotSize.x * slotsX + 20;
		//Y
		rectDelta.y = slotMarginY * (slotsY + 1) + slotSize.y * slotsY;

		Debug.Log(rectDelta);

		return rectDelta;
	}

	private void updateCanvas(){
		//Add 
	}
	#endregion
}
