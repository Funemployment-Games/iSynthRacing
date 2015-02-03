using UnityEngine;
using System.Collections;

public class OptionsDialog : MonoBehaviour
{
	////// singleton
	private static OptionsDialog m_instance;
	
	public static OptionsDialog instance
	{
		get
		{
			return m_instance;
		}
	}
	
	void Awake()
	{
		if( m_instance != null )
		{
			return;
		}
		m_instance = this;
		
		gameObject.SetActiveRecursively(false);
	}
	
	public bool m_enabled = false;
	
	public Vector2 m_position;
	
	public Texture2D m_backgroundTexture;
	
	/// buttons
	///
	// ok button
	public Vector2 m_butOkPos;
	public Texture2D m_butOkText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(!m_enabled) return;
		
		GUI.BeginGroup(new Rect(m_position.x, m_position.y, m_backgroundTexture.width, m_backgroundTexture.height));
		
		GUI.DrawTexture(new Rect(0,0, m_backgroundTexture.width, m_backgroundTexture.height), m_backgroundTexture);
		
		if(GUI.Button(new Rect(m_butOkPos.x, m_butOkPos.y, m_butOkText.width, m_butOkText.height), m_butOkText))
		{
			Disable();
		}
		
		GUI.EndGroup();
	}
	
	public static void Enable()
	{
		if(GameManager.gameState == GameState.Running || GameManager.gameState == GameState.Countdown)
		{
			return;
		}
		print("enable "+Time.time);
		instance.gameObject.SetActiveRecursively(true);
		instance.m_enabled = true;
	}
	
	void Disable()
	{
		print("disable "+Time.time);
		m_enabled = false;
		
		GameManager.SetGameState(GameState.Countdown);
		
		instance.gameObject.SetActiveRecursively(false);
	}
	
	public static bool isActive
	{
		get{
			return instance.m_enabled;
		}
	}
}
