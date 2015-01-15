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
		GameObject item;

		ItemType[] types = new ItemType[3];
		types[0] = ItemType.Weapon;
		types[1] = ItemType.Wearable;
		types[2] = ItemType.Consumable;
		string[] typeNames = new string[3];
		typeNames[0] = "Schwert";
		typeNames[1] = "Rüstung";
		typeNames[2] = "Trank";
		string[] typeDescr = new string[5];
		typeDescr[0] = "Eine kurze Beschreibung.";
		typeDescr[1] = "Eine normal lange Beschreibung mit einem Detail.";
		typeDescr[2] = "Eine ziemlich lange Beschreibung mit mehreren Details, welche nicht weiter beschrieben werden.";
		typeDescr[3] = "Eine lange Beschreibung mit vielen Details, welche alle genau und ausführlich beschreiben werden. Daher reicht hier ein einnzelner Satz nicht. So, dies sollte genug sein.";
		typeDescr[4] = "Eine richtig lange und ausgedehnte Beschreibung mit extrem vielen Details, welche zu allem überfluss noch Zeilenummbrüche enthält." + System.Environment.NewLine + "Nach so einem Umbruch kann man noch lange weiter schreiben, aber ich lasse das jetz mal sein. Wäre ja auch ganz schön langweilig, so ein langer nichts sagender Text.";


		for(int i = 0; i < Random.Range (1, 31); i++){

			int randType = Random.Range(0,3);

			item = new GameObject("Item " + i);
			item.AddComponent<Item>();
			item.GetComponent<Item>().init(i, typeNames[randType], typeDescr[Random.Range(0,5)], Random.Range(50,251), _itemSprites[randType], types[randType], (types[randType] == ItemType.Consumable)? true : false);
			_BaseItems.Add(item);
		}

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
