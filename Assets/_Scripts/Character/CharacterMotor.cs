using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMotor : MonoBehaviour {

	public Text infoText;

	// Use this for initialization
	void Start () {
		infoText = transform.Find("GUICanvas/ActionInfo").gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawRay(transform.FindChild("Main Camera").transform.position, transform.FindChild("Main Camera").transform.forward * 10, Color.green);

		RaycastHit hit;
		Vector3 fwd = transform.FindChild("Main Camera").transform.forward;
		if (Physics.Raycast(transform.FindChild("Main Camera").transform.position, fwd, out hit, 5)){
			Debug.Log("There is something in front of the object!");
			Debug.Log(hit.collider.transform.name);


			infoText.text = "F";
			infoText.color = new Color(255,255,255,1);

		}else{
			infoText.color = new Color(255, 255, 255, 0);
		}
	}
}
