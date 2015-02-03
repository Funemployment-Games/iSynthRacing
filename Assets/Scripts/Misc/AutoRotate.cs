using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour
{
	public Vector3 m_rotation = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(m_rotation*Time.deltaTime);
	}
}
