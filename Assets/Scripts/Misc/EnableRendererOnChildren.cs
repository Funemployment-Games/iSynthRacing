using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EnableRendererOnChildren : MonoBehaviour
{
	public bool update = false;

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
		Component[] children = gameObject.GetComponentsInChildren(typeof(MeshFilter));

		for (int i = 0; i < children.Length; i++)
		{
			MeshFilter filter = (MeshFilter) children[i];
			filter.renderer.enabled = true;
		}
	}
}
