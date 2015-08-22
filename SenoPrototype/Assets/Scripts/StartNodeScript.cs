using UnityEngine;
using System.Collections;

public enum NodeColour
{
     BLUE = 0, YELLOW = 1, RED = 2, GREEN = 3, PURPLE = 4, ORANGE = 5, WHITE = 6
}

public class StartNodeScript : MonoBehaviour {

    static Color NodeWhite = Color.white;

    static Color NodeYellow = Color.yellow * 0.8f;
    static Color NodeRed = Color.red * 0.8f;
    static Color NodeBlue = Color.blue * 0.8f;

    static Color NodeGreen = Color.green * 0.8f;
    static Color NodePurple = new Color(0.6f, 0.0f, 1.0f) * 0.9f;
    static Color NodeOrange = new Color(1.0f, 0.6f, 0.0f) * 0.9f;

    public float RippleCooldown = 2.5f;
    public float RippleLifetime = 1.0f;
    public float RippleSpeed = 1.0f;

    public NodeColour nodeColour = NodeColour.WHITE;

    private Color actualNodeColour;

    public bool isStartNode = false;
    public bool isEndNode = false;
    public int WinningRipplesNeeded = 1;

    public GameObject RippleObject;

    private float currentCooldown = 0;

    public float winCooldown = 0.2f;
    private float currentWinCooldown = 0.0f;
    private int currentWinCount = 0;

    public bool winState = false;

    public float overallWinstateCooldown = 0.2f;
    private float overallCurrentWinCooldown = 0.0f;

    public GameObject manager;

    public bool disabled = false;

    private bool isAdded = false;

    private float winColourCooldown = 1.0f;
    private float currentWinColourCooldown = 0.0f;

    // is activated and cooldown value for each colour
    private bool[] colourActivations = new bool[6];
    private float[] colourCooldowns = new float[6];

	// Use this for initialization
	void Start () {
        RippleScript script = RippleObject.GetComponent<RippleScript>();
        script.lifetime = RippleLifetime;
        script.ScalingSpeed = RippleSpeed;

        for (int i = 0; i < 6; i++)
        {
            colourActivations[i] = false;
            colourCooldowns[i] = 0.0f;
        }
	}

    void Init()
    {
        GameManager managerScript = manager.GetComponent<GameManager>();
        managerScript.AddNode(gameObject);

        switch (nodeColour)
        {
            case NodeColour.WHITE:
                actualNodeColour = NodeWhite;
                break;
            case NodeColour.RED:
                actualNodeColour = NodeRed;
                break;
            case NodeColour.BLUE:
                actualNodeColour = NodeBlue;
                break;
            case NodeColour.YELLOW:
                actualNodeColour = NodeYellow;
                break;
            case NodeColour.PURPLE:
                actualNodeColour = NodePurple;
                break;
            case NodeColour.GREEN:
                actualNodeColour = NodeGreen;
                break;
            case NodeColour.ORANGE:
                actualNodeColour = NodeOrange;
                break;
        }

        RippleScript script = RippleObject.GetComponent<RippleScript>();
        script.SetColour(actualNodeColour);

        isAdded = true;
    }
	
	// Update is called once per frame
	void Update () {

        // add to the manager
        if (!isAdded)
        {
            Init();
        }

        currentCooldown -= Time.deltaTime;

        disabled = currentCooldown > 0;

        TakeInput();

        UpdateEndNode();

        UpdateColour();
	}

    void TakeInput()
    {
        if (isStartNode)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        RaycastHit hit = new RaycastHit();

                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject == gameObject)
                            {
                                ActivateRipple();
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        ActivateRipple();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updating of end node parameters.
    /// </summary>
    void UpdateEndNode()
    {
        if (isEndNode)
        {
            if (currentWinCount > 0)
            {
                currentWinCooldown -= Time.deltaTime;
            }

            if (currentWinCooldown < 0)
            {
                currentWinCooldown = 0;
                currentWinCount = 0;
            }

            if (overallCurrentWinCooldown < 0 && nodeColour == NodeColour.WHITE)
            {
                winState = false;
                overallCurrentWinCooldown = 0;
            }

            if (winState)
            {
                overallCurrentWinCooldown -= Time.deltaTime;
            }

            for (int i = 0; i < 6; i++)
            {

                if (colourActivations[i])
                {
                    colourCooldowns[i] -= Time.deltaTime;

                    if (colourCooldowns[i] < 0)
                    {
                        colourActivations[i] = false;
                        colourCooldowns[i] = 0.0f;
                    }
                }
            }

            switch(nodeColour)
            {
                case NodeColour.RED:
                    if(colourActivations[(int)NodeColour.RED])
                    {
                        winState = true;
                    }
                    break;
                case NodeColour.BLUE:
                    if (colourActivations[(int)NodeColour.BLUE])
                    {
                        winState = true;
                    }
                    break;
                case NodeColour.YELLOW:
                    if (colourActivations[(int)NodeColour.YELLOW])
                    {
                        winState = true;
                    }
                    break;
                case NodeColour.PURPLE:
                    if (colourActivations[(int)NodeColour.RED] && colourActivations[(int)NodeColour.BLUE])
                    {
                        winState = true;
                    }
                    break;
                case NodeColour.GREEN:
                    if (colourActivations[(int)NodeColour.YELLOW] && colourActivations[(int)NodeColour.BLUE])
                    {
                        winState = true;
                    }
                    break;
                case NodeColour.ORANGE:
                    if (colourActivations[(int)NodeColour.RED] && colourActivations[(int)NodeColour.YELLOW])
                    {
                        winState = true;
                    }
                    break;
            }

        }
    }

    public Color GetNodeColor()
    {
        return actualNodeColour;
    }

    void UpdateColour()
    {
        Renderer r = gameObject.GetComponent<Renderer>();

        if (currentWinColourCooldown > 0)
        {
            currentWinColourCooldown -= Time.deltaTime;

            r.material.color = actualNodeColour + actualNodeColour * currentWinColourCooldown * 3;

            if (currentWinColourCooldown < 0)
            {
                currentWinColourCooldown = 0.0f;
            }
        }
        else
        {
            if (disabled)
            {
                //r.material.color = Color.gray;
                r.material.color = actualNodeColour * 0.5f;
            }
            else
            {
                r.material.color = actualNodeColour;
            }

            if (winState)
            {
                if (isEndNode)
                {
                    currentWinColourCooldown = winColourCooldown;
                    r.material.color = actualNodeColour * 3;
                }
            }
        }
    }
       
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ripple" && !isStartNode)
        {
            ActivateRipple(other.gameObject.GetComponent<RippleScript>().GetColour());
        }
    }

    public void ActivateRipple()
    {
        ActivateRipple(NodeColour.WHITE);
    }

    public void ActivateRipple(NodeColour rippleInputColour)
    {
        if (isEndNode)
        {
            if (nodeColour == NodeColour.WHITE)
            {
                IncrementWinningRippleHits();
            }
            else
            {
                ColouredEndInput(rippleInputColour);
            }
        }
        else if (currentCooldown < 0 && !disabled)
        {
            if (rippleInputColour == nodeColour || rippleInputColour == NodeColour.WHITE)
            {
                currentCooldown = RippleCooldown;
                RippleScript script = RippleObject.GetComponent<RippleScript>();
                script.Activate();
                currentCooldown = RippleCooldown;
            }
        }
    }

    public void ColouredEndInput(NodeColour rippleInputColour)
    {
        if (rippleInputColour != NodeColour.WHITE)
        {
            if (!colourActivations[(int)rippleInputColour]) // if it's not active
            {
                colourActivations[(int)rippleInputColour] = true;
                colourCooldowns[(int)rippleInputColour] = winCooldown;
            }
        }
    }

    public void ResetWinState()
    {
        winState = false;
    }

    private void IncrementWinningRippleHits()
    {
        if (currentWinCount == 0)
        {
            currentWinCooldown = winCooldown;
        }

        currentWinCount++;

        if (currentWinCount == WinningRipplesNeeded)
        {
            winState = true;
            overallCurrentWinCooldown = overallWinstateCooldown;
        }
    }

}
