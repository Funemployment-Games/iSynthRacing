using UnityEngine;
using System.Collections;

public class CountdownObject : MonoBehaviour
{
	public GUITexture m_readyTexture;
	public GUITexture m_goTexture;
	public GUITexture m_barTexture;
	
	float m_remainingTime;
	float m_countdownTime = -1;
	
	// how long the countdown is
	float m_countdownStartTime = 3f;
	
	int m_countdownInt = -1;
	
	bool m_active = false;
	
	public bool m_enabled = true;
	
	// Use this for initialization
	void Start ()
	{
		Init();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!m_active) return;
		
		if(OptionsMenu.isActive) return;
		
		if(m_remainingTime <= 0f)
		{
			Finish();
			return;
		}
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(this.name, gameObject.name, "Update");
		m_remainingTime -= Time.deltaTime;
		
		float m_maxWidth = 138;
		
		float perc = 1f-(m_remainingTime/m_countdownTime);
		
		Rect pixelIns = m_barTexture.pixelInset;
		pixelIns.width = Mathf.Lerp(0, m_maxWidth, perc);
		pixelIns.x = (m_maxWidth - pixelIns.width)/2;
		m_barTexture.pixelInset = pixelIns;
		
	//	pm.CallIsFinished();
	}
	
	
	void Finish()
	{
		Invoke("Deactivate", 1f);
		m_goTexture.enabled = true;
		m_active = false;
		GameManager.instance.SetCountdownDone();
	}
	
	void Deactivate()
	{
		gameObject.SetActiveRecursively(false);
		m_active = false;
	}
	
	public void Init()
	{
		if(!m_enabled)
		{
			Finish();
			return;
		}
		
		m_active = true;
		
		m_countdownTime = m_countdownStartTime;
		
		m_remainingTime = m_countdownTime;
		
		gameObject.SetActiveRecursively(true);
		
		m_goTexture.enabled = false;
	}
}
