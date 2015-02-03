using UnityEngine;
using System.Collections;

public class PlayerCycleColors : MonoBehaviour
{
	public Color[] m_colors;
	
	public float m_speed = 1f;
	
	float m_currPerc = 0f;
	int m_colorIndex = 0;
	int m_nextColorIndex = 0;
	
	Color m_currColor;
	
	/// stuff to affect with this color
	public Light m_playerLight;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update()
	{
		UpdateColor();
		
		m_playerLight.color = m_currColor;
	}
	
	// Update is called once per frame
	void UpdateColor ()
	{
		if(m_colors.Length == 0) return;
	
		m_currPerc += m_speed * Time.deltaTime;
		
		if(m_currPerc >= 1f)
		{
			m_currPerc = 0f;
			
			if(++m_colorIndex >= m_colors.Length)
			{
				m_colorIndex = 0;
			}
		}
		
		m_nextColorIndex = m_colorIndex + 1;
		
		if(m_nextColorIndex >= m_colors.Length)
			m_nextColorIndex = 0;
		
		m_currColor = Color.Lerp(m_colors[m_colorIndex], m_colors[m_nextColorIndex], m_currPerc);
	}
}
