using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private List<GameObject> Nodes;

    bool win = false;

    public string textureName = "_MainTex";
    public Vector2 uvAnimationRate = new Vector2(0.5f, 0.0f);

    Vector2 uvOffset = Vector2.zero;

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
            Debug.Log("Winner winner, chicken dinner");
            foreach (GameObject node in Nodes)
            {
                StartNodeScript script = node.GetComponent<StartNodeScript>();
                script.ResetWinState();
            }
        }

       // UpdateTexture();
	}

    void UpdateTexture()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        Renderer renderer = gameObject.GetComponent<Renderer>();

        renderer.materials[0].SetTextureOffset(textureName, uvOffset);
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
