using UnityEngine;
using System.Collections;

public class koiScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shift(float translation)
    {
        gameObject.transform.Translate(new Vector3(translation, 0, 0));
    }
}
