using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour 
{
	public GameObject objectBlinkText;
	private Text m_blinkTextLabel;
	public float BlinkSpeed;

	// Use this for initialization
	void Start () 
	{
		m_blinkTextLabel = objectBlinkText.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{

		float fAlphaValue = Mathf.Round(Mathf.PingPong(Time.time * BlinkSpeed, 1.0f));
		Color textColor = m_blinkTextLabel.material.color;
		textColor.a = fAlphaValue;
		m_blinkTextLabel.material.color = textColor;
	}

	public void OnPressedStart()
	{
		//SoundManager.PlayGUISound(SoundEvent.ButtonPressedGeneric);

		m_blinkTextLabel.text = "Loading...";
		
		MenuManager.LoadMainMenu(false);
	}
}
