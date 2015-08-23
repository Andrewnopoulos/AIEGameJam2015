using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private List<GameObject> Nodes;

    bool win = false;

	public float scrollSpeed;

	public Canvas canvas;

    private bool movingANode;

	// Use this for initialization
	void Start () {
	    Nodes = new List<GameObject>();
	}

    public bool IsMovingANode()
    {
        return movingANode;
    }
	
	// Update is called once per frame
	void Update () {
        win = true;

        movingANode = false;

	    foreach (GameObject node in Nodes)
        {
            StartNodeScript script = node.GetComponent<StartNodeScript>();
            if (script.isEndNode && !script.winState)
            {
                win = false;
            }

            if (script.selected)
            {
                movingANode = true;
            }
        }

        if (win)
        {
            //Debug.Log("Winner winner, chicken dinner");
            foreach (GameObject node in Nodes)
            {
                StartNodeScript script = node.GetComponent<StartNodeScript>();
                script.ResetWinState();
            }

			canvas.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.enabled = true;
        }

        //UpdateTexture();
	}

    void UpdateTexture()
    {
		float offset = Time.time * scrollSpeed;
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, offset/2);
    }

    public void AddNode(GameObject node)
    {
        if (!Nodes.Contains(node))
        {
            Nodes.Add(node);
        }
    }

    public bool RemoveNode(GameObject node)
    {
        if (Nodes.Contains(node))
        {
            Nodes.Remove(node);
            return true;
        }
        return false;
    }
}
