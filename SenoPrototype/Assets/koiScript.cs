using UnityEngine;
using System.Collections;

public class koiScript : MonoBehaviour {

    public float rotationAngle = 90;

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

    public void TurnAround()
    {
        gameObject.transform.Rotate(new Vector3(0, 1, 0), rotationAngle);
    }
}
