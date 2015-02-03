using UnityEngine;
using System.Collections;

public class ObstaclePart : MonoBehaviour
{
	Color m_touchedColor = Color.red;
	
	Color m_origColor;

	// Use this for initialization
	void Start ()
	{
		if(renderer)
		{
	//		m_origColor = renderer.material.color;
	//		m_touchedColor.a = 0.2f;
		}
		gameObject.layer = LayerMask.NameToLayer("Obstacle");
		collider.isTrigger = true;
	}

	
	void HandleCollision()
	{
		if(renderer)
		{
	//		renderer.material.color = m_touchedColor;
			
			Invoke("ChangeToOriginalColor",1f);
		}
	}
	
	void ChangeToOriginalColor()
	{
		if(renderer)
		{
	//		renderer.material.color = m_origColor;
		}
	}
}
