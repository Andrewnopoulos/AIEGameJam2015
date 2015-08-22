using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private List<GameObject> Nodes;

    bool win = false;

	public float scrollSpeed;

	public Canvas canvas;

	// Use this for initialization
	void Start () {
	    Nodes = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        win = true;
	    foreach (GameObject node in Nodes)
        {
            StartNodeScript script = node.GetComponent<StartNodeScript>();
            if (script.isEndNode && !script.winState)
            {
                win = false;
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

        UpdateTexture();
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
