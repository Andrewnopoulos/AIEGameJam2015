using UnityEngine;
using System.Collections;

public class ListenToInput : MonoBehaviour {

	private bool isInputTriggered;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton(0)){
			isInputTriggered = true;
		}

		if(isInputTriggered) {

			Color newColour = gameObject.GetComponent<SpriteRenderer>().material.color;
			newColour.a -= Time.deltaTime;
			gameObject.GetComponent<SpriteRenderer>().material.color = newColour;

			if(newColour.a <= 0)
				Destroy(gameObject);
		}
	}
}
