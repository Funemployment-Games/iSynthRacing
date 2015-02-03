using UnityEngine;
using System.Collections;

public class GhostShip : MonoBehaviour
{
	Vector3 readPos;
	Quaternion readRot;

	// Use this for initialization
	void Start ()
	{
		GhostManager.GetStartPosition(out readPos, out readRot);
		transform.position = readPos;
		transform.rotation = readRot;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = readPos;
	//	transform.rotation = readRot;
//		transform.position = Vector3.Lerp(transform.position, readPos, 20f * Time.deltaTime);
		
		transform.rotation = readRot;
	}
	
	void FixedUpdate()
	{
		if(GameManager.gameState == GameState.Running)
		{
			bool res = GhostManager.GetNextPosition(out readPos, out readRot);
			if(!res)
			{
				Destroy(gameObject);
			}
		}
	}
}
