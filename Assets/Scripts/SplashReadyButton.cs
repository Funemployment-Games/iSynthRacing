using UnityEngine;
using System.Collections;

public class SplashReadyButton : MonoBehaviour
{
	public Texture2D m_onTexture;
	Texture2D m_offTexture;

	// Use this for initialization
	void Start()
	{
		iPhoneSettings.verticalOrientation = false;
		iPhoneSettings.screenCanDarken = false;
		
		Invoke("Blink", 4);
		
		m_offTexture = (Texture2D) guiTexture.texture;
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		
		if(iPhoneInput.touchCount > 0 || Input.GetMouseButtonDown(0))
		{
			OnButtonPressed();
		}
		
		if(Application.isEditor && MenuManager.SkipMenusAndGoStraightToRaceDebug)
		{
			OnButtonPressed();
		}
	}
	
	void OnButtonPressed()
	{
		SoundManager.PlayGUISound(SoundEvent.ButtonPressedGeneric);
		guiTexture.texture = m_onTexture;
		
		GUIText text = (GUIText)m_blinkingThing.GetComponent(typeof(GUIText));
		if(text)
		{
			m_blinkingThing.SetActiveRecursively(true);
			text.text = "Loading...";
		}
		
		MenuManager.LoadMainMenu(false);
	}
	
	public GameObject m_blinkingThing;
	float m_blinkTime = 1f;
	bool m_blinkOn = false;
	
	void Blink()
	{
		if(!m_blinkingThing) return;
		
		m_blinkOn = !m_blinkOn;
		
		float time = m_blinkTime;
		
		if(!m_blinkOn) time/=2;
		
		m_blinkingThing.SetActiveRecursively(m_blinkOn);
		
		Invoke("Blink", time);
		/*
		if(m_blinkOn)
		{
			guiTexture.texture = m_onTexture;
		}
		else
		{
			guiTexture.texture = m_offTexture;
		}*/
	}
}
