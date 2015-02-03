using UnityEngine;
using System.Collections;

public class BoostBar : MonoBehaviour
{
	public Color m_enabledColor;
	public Color m_disabledColor1;
	public Color m_disabledColor2;
	public Color m_inUseColor;
	
	float m_maxWidth;
	float m_minWidth;
	
	bool m_inUse = false;

	// Use this for initialization
	void Start ()
	{
		m_maxWidth = 480;//guiTexture.pixelInset.width;
		m_minWidth = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	Rect pixelInset;
	public void SetBoostBarPercent(float perc)
	{
		
		pixelInset = guiTexture.pixelInset;
		pixelInset.width = Mathf.Lerp(m_minWidth, m_maxWidth, perc);
		pixelInset.x = (m_maxWidth - pixelInset.width)/2;
		guiTexture.pixelInset = pixelInset;
		
		if(!m_inUse)
		{
			if(perc < 0.9f)
				guiTexture.color = Color.Lerp(m_disabledColor1, m_disabledColor2, perc);
			else
				guiTexture.color = m_enabledColor;
		}
	}
	
	public void SetEnabled(bool state)
	{

	}
	
	public void SetInUse(bool inUse)
	{
		m_inUse = inUse;
		if(inUse)
		{
			guiTexture.color = m_inUseColor;
		}
	}
	
	public bool inUse{
		get{
			return m_inUse;
		}
	}
}
