using UnityEngine;
using System.Collections;

public class EnginePlane : MonoBehaviour
{
	public float m_rotationSpeed = 10f;
	float m_scaleVariation = 0.2f;
	float m_scaleVariationFreq = 10f;
	
	float m_origScale;

	// Use this for initialization
	void Start ()
	{
		m_origScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(0f,0f, m_rotationSpeed * Time.deltaTime);
	
		float scale = m_origScale + Mathf.Sin(Time.time*m_scaleVariationFreq) * m_scaleVariation;
		
		transform.localScale = new Vector3(scale,scale,scale);
	}
}
