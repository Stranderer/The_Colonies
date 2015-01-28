using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Text infoText;
	public GameObject lastHit = null;

	private Shader shaderNormal;
	private Shader shaderOutline;

	// Use this for initialization
	void Start () {
		infoText = GameObject.Find("GUICanvas/ActionInfo").GetComponent<Text>();
		shaderNormal = Shader.Find( "Custom/DiffuseRimShader" );
		shaderOutline = Shader.Find( "Custom/OutlineShader" );

		Debug.Log(infoText);
	}
	
	// Update is called once per frame
	void Update () {

		//Check if something takeable is in front of Player
		checkTakeable();

	}

	/// <summary>
	/// Checks if something takeable is in front of Player.
	/// </summary>
	private void checkTakeable(){
		Debug.DrawRay(transform.FindChild("Main Camera").transform.position, transform.FindChild("Main Camera").transform.forward * 5, Color.green);
		
		RaycastHit hit;
		Vector3 fwd = transform.FindChild("Main Camera").transform.forward;
		
		if(infoText == null)
			return;
		
		if (Physics.Raycast(transform.FindChild("Main Camera").transform.position, fwd, out hit, 5)){
			//Show info if Takeable
			if(hit.collider.GetComponent<Item>() != null){

				lastHit = hit.collider.gameObject;

				infoText.text = "Take \"" + hit.collider.GetComponent<Item>().getName() + "\"";
				infoText.color = new Color(240,240,240, 0.7f);

				lastHit.renderer.materials[1].shader = shaderOutline;

				//Add item to inventory
				if(Input.GetKeyDown(KeyCode.F)){
					//Create new gui item
					Item worldItem = hit.collider.gameObject.GetComponent<Item>();

					GameObject pickupItem = new GameObject();
					pickupItem.AddComponent<Item>();

					pickupItem.GetComponent<Item>().init(false, worldItem.getId(), worldItem.getName(), worldItem.getDescription(), worldItem.getValue(), worldItem.getTexture(), worldItem.getType(), worldItem.isStackable());

					gameObject.GetComponent<InventoryController>().addItem(pickupItem);

					//Destroy world object
					GameObject.Destroy(hit.collider.gameObject);
				}
			}
		}else{
			infoText.color = new Color(255, 255, 255, 0);

			if(lastHit != null){
				lastHit.renderer.materials[1].shader = shaderNormal;
				lastHit = null;
			}
		}
	}
}
