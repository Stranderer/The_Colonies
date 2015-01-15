using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawRay(transform.FindChild("Main Camera").transform.position, transform.FindChild("Main Camera").transform.forward * 10, Color.green);

		RaycastHit hit;
		Vector3 fwd = transform.FindChild("Main Camera").transform.forward;
		if (Physics.Raycast(transform.FindChild("Main Camera").transform.position, fwd, out hit, 10)){
			Debug.Log("There is something in front of the object!");
			Debug.Log(hit.collider.transform.name);
		}
	}
}
