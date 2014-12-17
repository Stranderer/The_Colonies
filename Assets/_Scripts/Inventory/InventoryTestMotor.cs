using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryTestMotor : MonoBehaviour {

	public Sprite[] _itemSprites;

	private List<GameObject> _BaseItems = new List<GameObject>();

	// Use this for initialization
	void Start () {
		//Get Inventory Controller
		InventoryController myInventory = transform.GetComponent<InventoryController>();

		//Create some test items
		GameObject item1 = new GameObject("item1");
		item1.AddComponent<BaseItem>();
		item1.GetComponent<BaseItem>().init(0, "Schwert", "Ein einfaches Schwert", 150, _itemSprites[0], ItemType.Weapon, false);
		_BaseItems.Add(item1);


		/*
		this._BaseItems.Add(new BaseItem(1, "Schwert", "Ein etwas besseres Schwert", 200, _itemSprites[0], ItemType.Weapon, false));
		this._BaseItems.Add(new BaseItem(2, "Rüstung", "Eine einfache Rüstung", 187, _itemSprites[1], ItemType.Weapon, false));
		
		myInventory.addItem(_BaseItems);

		this._BaseItems.Add(new BaseItem(3, "Schwert", "Ein einfaches Schwert", 150, _itemSprites[0], ItemType.Weapon, false));
		this._BaseItems.Add(new BaseItem(4, "Schwert", "Ein etwas besseres Schwert", 200, _itemSprites[0], ItemType.Weapon, false));
		this._BaseItems.Add(new BaseItem(5, "Rüstung", "Eine einfache Rüstung", 187, _itemSprites[1], ItemType.Weapon, false));
		*/
		myInventory.addItem(_BaseItems);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
