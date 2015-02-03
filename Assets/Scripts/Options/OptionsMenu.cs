using UnityEngine;
using System.Collections;

public class OptionsMenu : Menu
{
	////// singleton
	private static OptionsMenu m_instance;
	
	public static OptionsMenu instance
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
		
	//	DontDestroyOnLoad(this);
	}
	
	///////
	/// ///
	///
	///
	///
	
	public static bool active = false;
	public static bool isActive{
		get{
			return active;
		}
	}
	
	///
	///
	static OnOffSetting m_invertX = OnOffSetting.Off;
	static OnOffSetting m_invertY = OnOffSetting.Off;
	
	static OnOffSetting m_music = OnOffSetting.Off;
	static OnOffSetting m_sound = OnOffSetting.Off;
	
	static SensitivitySetting m_sensitivity = SensitivitySetting.None;
	///
	///
	
	public GUITexture m_calibrationShip;
//	public Vector2 m_calibrationShipCenter;
	public Vector2 m_calibrationShipUpLeft;
	public Vector2 m_calibrationShipDownRight;
	Vector3 m_calibrationShipPos;
	
	public GameObject m_creditsScreen;
	
	void Start()
	{
		Init();
	}
	
	public static void LoadPlayerPrefs()
	{
		m_invertX = (OnOffSetting) PlayerPrefs.GetInt("InvertX", 2);
		m_invertY = (OnOffSetting) PlayerPrefs.GetInt("InvertY", 2);
		m_music = (OnOffSetting) PlayerPrefs.GetInt("MusicOn", 1);
		m_sound = (OnOffSetting) PlayerPrefs.GetInt("SoundOn", 1);
		m_sensitivity = (SensitivitySetting) PlayerPrefs.GetInt("Sensitivity", 2);
		float centerRoll = PlayerPrefs.GetFloat("CenterRoll", 0f);
		float centerTilt = PlayerPrefs.GetFloat("CenterTilt", -0.60f);
		
	//	print("load cent "+centerRoll+" "+centerTilt);
		InputManager.SetCenter(centerRoll, centerTilt);
		InputManager.SetAccelerometerSensitivity(m_sensitivity);
		
		bool music_enabled = m_music == OnOffSetting.On;
		MusicManager.SetEnabled(music_enabled);
		
		bool sound_enabled = m_sound == OnOffSetting.On;
		SoundManager.SetEnabled(sound_enabled);
	}
	
	static void SavePlayerPrefs()
	{
	//	print("save sense x "+(int)m_sensitivity);
		PlayerPrefs.SetInt("InvertX", (int)m_invertX);
		PlayerPrefs.SetInt("InvertY", (int)m_invertY);
		PlayerPrefs.SetInt("MusicOn", (int)m_music);
		PlayerPrefs.SetInt("SoundOn", (int)m_sound);
		PlayerPrefs.SetInt("Sensitivity", (int)m_sensitivity);
		PlayerPrefs.SetFloat("CenterRoll", InputManager.centerRoll);
		PlayerPrefs.SetFloat("CenterTilt", InputManager.centerTilt);
	//	print("save cent "+InputManager.centerRoll+" "+InputManager.centerTilt);
	}
	
	void Init()
	{
		// start in the middle
		m_calibrationShipPos = Vector3.Lerp(m_calibrationShipDownRight, m_calibrationShipUpLeft, 0.5f);
	}
	
	void Update()
	{
		if(!active) return;
		
		float x = InputManager.normalizedRoll;
		float y = InputManager.normalizedTilt;
	//	print("upd calib "+Time.time+" "+x+" "+y);
		Vector3 targetShipPos = new Vector3();
		targetShipPos.x = Mathf.Lerp(m_calibrationShipUpLeft.x, m_calibrationShipDownRight.x, x);
		targetShipPos.y = Mathf.Lerp(m_calibrationShipDownRight.y, m_calibrationShipUpLeft.y, y);
		
		if(Vector3.Distance(m_calibrationShipPos, targetShipPos) > 0.0f)
			m_calibrationShipPos =  Vector3.Lerp(m_calibrationShipPos, targetShipPos, 4f*Time.deltaTime);
		
		m_calibrationShipPos.z = 1000;
		
		m_calibrationShip.transform.localPosition = m_calibrationShipPos;
	}
	
	
	public static void SetSensitivity(SensitivitySetting new_sensitivity)
	{
		m_sensitivity = new_sensitivity;
		InputManager.SetAccelerometerSensitivity(new_sensitivity);
	}
	
	public static void SetInvertX(OnOffSetting onoff)
	{
		m_invertX = onoff;
	}
	public static void SetInvertY(OnOffSetting onoff)
	{
		m_invertY = onoff;
	}
	
	public static bool invertX{
		get{
			return m_invertX == OnOffSetting.On;
		}
	}
	public static bool invertY{
		get{
			return m_invertY == OnOffSetting.On;
		}
	}
	
	public static bool musicOn{
		get{
			return m_music == OnOffSetting.On;
		}
	}
	
	public static bool soundOn{
		get{
			return m_sound == OnOffSetting.On;
		}
	}
	
	public static int sensitivity{
		get{
			return (int) m_sensitivity;
		}
	}
	
	public static void SetMusic(OnOffSetting onoff)
	{
		m_music = onoff;
		bool music_enabled = m_music == OnOffSetting.On;
		MusicManager.SetEnabled(music_enabled);
	}
	
	public static void SetSound(OnOffSetting onoff)
	{
		m_sound = onoff;
		bool sound_enabled = m_sound == OnOffSetting.On;
		SoundManager.SetEnabled(sound_enabled);
	}
	
	public static void CalibrateNow()
	{
		InputManager.CalibrateNow();
	}
	
	public void OpenMenu()
	{
		LoadPlayerPrefs();
		
		gameObject.SetActiveRecursively(true);
		active = true;
		
		ShowCredits(false);
	}
	
	public void CloseMenu()
	{
		SavePlayerPrefs();
		
		gameObject.SetActiveRecursively(false);
		
		active = false;
	}
	
	public static void ShowCredits(bool show)
	{
		if(instance.m_creditsScreen)
		{
			instance.m_creditsScreen.SetActiveRecursively(show);
		}
	}
}

public enum SensitivitySetting{None, High, Medium, Low}
public enum OnOffSetting{None, On, Off}
