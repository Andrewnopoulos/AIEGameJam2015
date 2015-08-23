using UnityEngine;
using System.Collections;

public enum NodeColour
{
     BLUE = 0, YELLOW = 1, RED = 2, GREEN = 3, PURPLE = 4, ORANGE = 5, WHITE = 6
}

public enum Theme
{
	ZEN = 0, SIMPLE = 1, FOOD = 2
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
    public float RippleMaxScaleValue = 2.0f;
    public float RippleSpeed = 1.0f;

    public int numberOfRipples = 1;

    private int activeRipples = 0;

    public NodeColour nodeColour = NodeColour.WHITE;

    private Color actualNodeColour;

    public bool isStartNode = false;
    public bool isEndNode = false;
    public int WinningRipplesNeeded = 1;

    private float currentCooldown = 0;

    public float winCooldown = 0.2f;
    private float currentWinCooldown = 0.0f;
    private int currentWinCount = 0;

    public bool winState = false;

    public float overallWinstateCooldown = 0.2f;
    private float overallCurrentWinCooldown = 0.0f;

	public Theme currentTheme = Theme.ZEN;

    public GameObject manager;

    public GameObject rippleTemplate;
	public GameObject fishTemplate;

    public bool disabled = false;

    private bool isAdded = false;

    private float winColourCooldown = 1.0f;
    private float currentWinColourCooldown = 0.0f;

    // is activated and cooldown value for each colour
    private bool[] colourActivations = new bool[6];
    private float[] colourCooldowns = new float[6];

    public bool moveAble = false;

    public bool selected = false;

    public float speed = 0.3f;

	// Use this for initialization
	void Start () {
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

        // initialize ripples
        for (int i = 0; i < numberOfRipples; i++)
        {
            GameObject ripple = (GameObject)Instantiate(rippleTemplate, transform.position, transform.rotation);
            ripple.transform.parent = this.gameObject.transform;

            RippleScript script = ripple.GetComponent<RippleScript>();
            script.SetParent(gameObject);
            script.ScalingSpeed = RippleSpeed;
            script.MaxScaleValue = RippleMaxScaleValue;
            script.SetColour(actualNodeColour);
        }

        if (isEndNode)
        {
            // initialize koi
            for (int i = 0; i < WinningRipplesNeeded; i++)
            {
                GameObject koi = (GameObject)Instantiate(fishTemplate, transform.position, transform.rotation);
                koi.transform.parent = this.gameObject.transform;

                FishScript fishy = koi.GetComponent<FishScript>();
                fishy.FishInitialize(Random.Range(0, 360.0f), i % 2 == 0, i * 0.4f);
            }
        }

        //GameObject ripple = (GameObject)Instantiate(rippleTemplate, transform.position, transform.rotation);
        //ripple.transform.parent = this.gameObject.transform;

        //RippleScript script = ripple.GetComponent<RippleScript>();
        //script.lifetime = RippleLifetime;
        //script.ScalingSpeed = RippleSpeed;

        //gameObject.GetComponentsInChildren<RippleScript>();

        //RippleScript script = RippleObject.GetComponent<RippleScript>();
        //script.SetColour(actualNodeColour);

        isAdded = true;

		//ChangeTheme (Theme.FOOD);

    }

	// Update is called once per frame
	void Update () {

        // add to the manager
        if (!isAdded)
        {
            Init();
        }

        currentCooldown -= Time.deltaTime;

        disabled = (activeRipples == numberOfRipples);

        TakeInput();

        UpdateEndNode();

        UpdateColour();
	}
	
	void ChangeTheme(Theme themeType)
	{
		if (currentTheme != themeType)
		{
			currentTheme = themeType;
			
			if (currentTheme == Theme.ZEN)
			{
				gameObject.GetComponentsInChildren<Renderer>()[3].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[4].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[5].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[6].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[7].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[8].enabled = false;
				
				if (isStartNode)
				{
					gameObject.GetComponentsInChildren<Renderer>()[0].enabled = true;
					gameObject.GetComponentsInChildren<Renderer>()[1].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[2].enabled = false;
				}
				else if (isEndNode)
				{
					gameObject.GetComponentsInChildren<Renderer>()[0].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[1].enabled = true;
					gameObject.GetComponentsInChildren<Renderer>()[2].enabled = false;
				}
				else
				{
					gameObject.GetComponentsInChildren<Renderer>()[0].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[1].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[2].enabled = true;
				}

				//activate pond
				manager.GetComponent<Renderer>().enabled = true;
				manager.GetComponentsInParent<Renderer>()[0].enabled = true;
				manager.GetComponentsInParent<Renderer>()[1].enabled = true;

				//disable tablecloth
				
			}
			else if (currentTheme == Theme.SIMPLE)
			{
				gameObject.GetComponentsInChildren<Renderer>()[0].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[1].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[2].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[6].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[7].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[8].enabled = false;
				
				if (isStartNode)
				{
					gameObject.GetComponentsInChildren<Renderer>()[3].enabled = true;
					gameObject.GetComponentsInChildren<Renderer>()[4].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[5].enabled = false;
				}
				else if (isEndNode)
				{
					gameObject.GetComponentsInChildren<Renderer>()[3].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[4].enabled = true;
					gameObject.GetComponentsInChildren<Renderer>()[5].enabled = false;
				}
				else
				{
					gameObject.GetComponentsInChildren<Renderer>()[3].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[4].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[5].enabled = true;
				}

				//disable pond
				manager.GetComponent<Renderer>().enabled = false;
				manager.GetComponentsInParent<Renderer>()[0].enabled = false;
				manager.GetComponentsInParent<Renderer>()[1].enabled = false;

				//disable tablecloth
			}
			else
			{
				//disable zen
				gameObject.GetComponentsInChildren<Renderer>()[0].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[1].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[2].enabled = false;
				//disable simple
				gameObject.GetComponentsInChildren<Renderer>()[3].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[4].enabled = false;
				gameObject.GetComponentsInChildren<Renderer>()[5].enabled = false;
				
				if (isStartNode)
				{
					gameObject.GetComponentsInChildren<Renderer>()[6].enabled = true;
					gameObject.GetComponentsInChildren<Renderer>()[7].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[8].enabled = false;
				}
				else if (isEndNode)
				{
					gameObject.GetComponentsInChildren<Renderer>()[6].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[7].enabled = true;
					gameObject.GetComponentsInChildren<Renderer>()[8].enabled = false;
				}
				else
				{
					gameObject.GetComponentsInChildren<Renderer>()[6].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[7].enabled = false;
					gameObject.GetComponentsInChildren<Renderer>()[8].enabled = true;
				}
				
				//disable pond
				manager.GetComponent<Renderer>().enabled = false;
				manager.GetComponentsInParent<Renderer>()[0].enabled = false;
				manager.GetComponentsInParent<Renderer>()[1].enabled = false;

				//enable tablecloth
			}
		
		}
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

        if (moveAble)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        selected = true;

                        RippleScript[] scripts = gameObject.GetComponentsInChildren<RippleScript>();

                        for (int i = 0; i < scripts.Length; i++)
                        {
                            scripts[i].DeActivate();
                        }
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (selected)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 10.0f;
                    Vector3 touchDeltaPosition = Camera.main.ScreenToWorldPoint(mousePos);
                    gameObject.transform.position = new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                selected = false;
            }
        }
    }

    public void DisableRipple()
    {
        activeRipples--;
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
		if (currentWinColourCooldown > 0) {
			currentWinColourCooldown -= Time.deltaTime;
			for (int i = 0; i < gameObject.GetComponentsInChildren<Renderer>().Length; ++i) {
				gameObject.GetComponentsInChildren<Renderer>()[i].material.color = actualNodeColour + actualNodeColour * currentWinColourCooldown * 3;
			}
			if (currentWinColourCooldown < 0) {
				currentWinColourCooldown = 0.0f;
			}
		} else {
			if (disabled) {
				//r.material.color = Color.gray;
				for (int i = 0; i < gameObject.GetComponentsInChildren<Renderer>().Length; ++i) {
					gameObject.GetComponentsInChildren<Renderer>()[i].material.color = actualNodeColour * 0.5f;
				}
			} else {
				for (int i = 0; i < gameObject.GetComponentsInChildren<Renderer>().Length; ++i) {
					gameObject.GetComponentsInChildren<Renderer>()[i].material.color = actualNodeColour;
				}
			}

			if (winState) {
				if (isEndNode) {
					currentWinColourCooldown = winColourCooldown;
					for (int i = 0; i < gameObject.GetComponentsInChildren<Renderer>().Length; ++i) {
						gameObject.GetComponentsInChildren<Renderer>()[i].material.color = actualNodeColour * 3;
					}
				}
			}
		}
    }
       
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<RippleScript>().GetParent() == gameObject)
        {
            return;
        }

        if (other.gameObject.name == "Ripple(Clone)" && !isStartNode)
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
        // if you're not currently moving a node, then ripples will activate things
        if (!manager.GetComponent<GameManager>().IsMovingANode())
        {
            if (isEndNode)
            {
                gameObject.GetComponent<AudioSource>().Play();

                if (nodeColour == NodeColour.WHITE)
                {
                    IncrementWinningRippleHits();
                }
                else
                {
                    ColouredEndInput(rippleInputColour);
                }
            }
            else if (/*currentCooldown < 0 && */ !disabled)
            {
                if (rippleInputColour == nodeColour || rippleInputColour == NodeColour.WHITE)
                {
                    currentCooldown = RippleCooldown;

                    RippleScript[] scripts = gameObject.GetComponentsInChildren<RippleScript>();

                    for (int i = 0; i < scripts.Length; i++)
                    {
                        if (!scripts[i].active)
                        {
                            //gameObject.GetComponent<AudioSource>().pitch += Random.Range(-0.02f, 0.02f);
                            gameObject.GetComponent<AudioSource>().Play();
                            scripts[i].Activate();
                            activeRipples++;
                            break;
                        }
                    }
                    currentCooldown = RippleCooldown;
                }
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
