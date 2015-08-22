using UnityEngine;
using System.Collections;

public class ChangeSceneButton : MonoBehaviour {

	public void NextLevelButton(int a_index)
	{
		Application.LoadLevel (a_index);
	}

	public void NextLevelButton(string a_lvlName)
	{
		Application.LoadLevel (a_lvlName);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
