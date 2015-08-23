using UnityEngine;
using System.Collections;

public class FishScript : MonoBehaviour {

	public bool clockwise = true;
	public float speed = 50.0f;

	// Use this for initialization
	void Start () {
	
	}

    public void FishInitialize(float initialAngleOffset, bool swimmingClockwise, float additionalRadius)
    {
        gameObject.transform.Rotate(new Vector3(0, 0, 1), initialAngleOffset);
        clockwise = swimmingClockwise;

        //gameObject.transform.localScale = new Vector3(transform.localScale.x + additionalRadius, transform.localScale.y, transform.localScale.z);
        koiScript fishTransform = gameObject.GetComponentInChildren<koiScript>();
        fishTransform.Shift(additionalRadius);
        if (clockwise)
        {
            fishTransform.TurnAround();
        }
    }
	
	// Update is called once per frame
	void Update () {
        //gameObject.transform.Rotate(new Vector3(0, 0, 1), speed * Time.deltaTime * (clockwise ? 1 : -1) );

        koiScript fishTransform = gameObject.GetComponentInChildren<koiScript>();
        fishTransform.TurnAround();
	}
}
