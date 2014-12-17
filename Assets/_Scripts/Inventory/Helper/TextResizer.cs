using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextResizer : MonoBehaviour {

	public void resize(){
		this.resize(new Vector2(0.0f, 0.0f));
	}

	public void resize(Vector2 sizeDeltaMax){
		RectTransform rect = gameObject.GetComponent<RectTransform>();
		RectTransform childRect = transform.FindChild("Text").GetComponent<RectTransform>();
		Text childText = transform.FindChild("Text").GetComponent<Text>();

		if(rect == null || childText == null){
			Debug.LogError("Resizer misses Components!");
			return;
		}

		float x = childText.preferredWidth;
		float y = childText.preferredHeight;

		//Set x to max if set 
		if(sizeDeltaMax.x > 0 && x > sizeDeltaMax.x){
			x = sizeDeltaMax.x;
		}

		//set y to max if set
		if(sizeDeltaMax.y > 0 && y > sizeDeltaMax.y){
			y = sizeDeltaMax.y;
		}

		//Set Background 40 px bigger
		rect.sizeDelta = new Vector2( x + 40, y + 40);
		//set text size
		childRect.sizeDelta = new Vector2( x, y);
	}

}
