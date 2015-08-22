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
	
	// Update is called once per frame
	void Update () {

        if (active)
        {
            currentLifetime += Time.deltaTime;

            transform.localScale += new Vector3(currentLifetime, currentLifetime, 0) * ScalingSpeed;

            Renderer r = gameObject.GetComponent<Renderer>();
            r.material.color = new Color(actualRippleColour.r, actualRippleColour.g, actualRippleColour.b, 1.4f - currentLifetime);

            if (currentLifetime > lifetime)
            {
                active = false;
                transform.localScale = startingScale;
                currentLifetime = 0.0f;
            }
        }
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
