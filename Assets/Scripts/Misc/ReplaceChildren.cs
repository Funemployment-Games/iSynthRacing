using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ReplaceChildren : MonoBehaviour
{
	public bool update = false;
	public string NameToBeReplaced;
	public GameObject PrefabToReplaceWith;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(update)
		{
			Execute();
			update = false;
		}
	}
	
	void Execute()
	{
		if(!PrefabToReplaceWith) return;
		
		Component[] children = gameObject.GetComponentsInChildren(typeof(Transform));
		print("dsd "+children.Length);
		for (int i = 0; i < children.Length; i++)
		{
			GameObject newObj = (GameObject)Instantiate(PrefabToReplaceWith);
			newObj.transform.position = children[i].transform.position;
			newObj.transform.parent = transform;
		}
	}
}
