using UnityEngine;
using System.Collections;

public class RippleScript : MonoBehaviour {

    public GameObject parent;

    public float lifetime = 1.0f;

    public float ScalingSpeed = 0.1f;

    public bool active = false;

    float currentLifetime = 0.0f;

    private Color actualRippleColour;

    public NodeColour rippleColour;

    Vector3 startingScale;

	// Use this for initialization
	void Start () {

        startingScale = transform.localScale;

        actualRippleColour = parent.GetComponent<StartNodeScript>().GetNodeColor();
        rippleColour = parent.GetComponent<StartNodeScript>().nodeColour;

	}

    public void SetColour(Color inputColour)
    {
        actualRippleColour = inputColour;
    }

    public NodeColour GetColour()
    {
        return rippleColour;
    }

    public void SetParent(GameObject daddy)
    {
        parent = daddy;
    }
	
    public GameObject GetParent()
    {
        return parent;
    }

	// Update is called once per frame
	void Update () {

        if (active) {
			if (gameObject.GetComponent<Renderer>().enabled == false)
			{
				gameObject.GetComponent<Renderer>().enabled = true;
			}

			currentLifetime += Time.deltaTime;

			transform.localScale += new Vector3 (currentLifetime, currentLifetime, 0) * ScalingSpeed;

			Renderer r = gameObject.GetComponent<Renderer> ();
			r.material.color = new Color (actualRippleColour.r, actualRippleColour.g, actualRippleColour.b, parent.GetComponent<StartNodeScript> ().RippleLifetime - currentLifetime);

			if (currentLifetime > lifetime) {
				DeActivate ();
			}
		} else {
			if (gameObject.GetComponent<Renderer>().enabled == true)
				gameObject.GetComponent<Renderer>().enabled = false;
		}
	}

    public void DeActivate()
    {
        active = false;
        transform.localScale = startingScale;
        currentLifetime = 0.0f;
        parent.GetComponent<StartNodeScript>().DisableRipple();
    }

    public void Activate()
    {
        active = true;
        if (transform.localScale != startingScale)
        {
            transform.localScale = startingScale;
        }
    }
}
