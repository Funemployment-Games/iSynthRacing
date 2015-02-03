using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	static InputManager m_singleton;
	
	public static InputManager instance
	{
		get{
			return m_singleton;
		}
	}
	
	void Awake()
	{
		if( m_singleton != null )
		{
			return;
		}
		m_singleton = this;
		
	//	DontDestroyOnLoad(this);
	}
	
	Player m_player;
	
	public bool m_enableMouseOnEditor = false;
	
	Vector2 m_currentDelta;
	UtilAverage m_mouseDeltaAverage;
	UtilAverage m_acceleratorAverage;
	
	// final values for roll and tilt
	float m_normalizedRoll = 0.5f;
	float m_normalizedTilt = 0.5f;
	
	// Use this for initialization
	void Start ()
	{
		m_mouseDeltaAverage = new UtilAverage(6, Vector2.zero);
		m_acceleratorAverage = new UtilAverage(20, Vector3.zero);
		
		m_normalizedRoll = 0.5f;
		m_normalizedTilt = 0.5f;
	}
	
	public static void Init()
	{
		instance.m_player = (Player)FindObjectOfType(typeof(Player));
	}
	
	public static void Shutdown()
	{
		instance.m_player = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "Update");
		if(Application.isEditor && m_enableMouseOnEditor)
		{
			UpdateMouse();
		}

		else if(GameManager.instance != null) // in track
		{
			UpdateAccelerometer2();
			
			if(GameManager.gameState == GameState.Running ||
			   GameManager.gameState == GameState.Results ||
			   GameManager.gameState == GameState.Countdown)
			{
				if(m_player != null)
					m_player.SetOffset(new Vector2(m_normalizedRoll, m_normalizedTilt));
			}
		}
		else //in menu
		{
			if(OptionsMenu.active || MainMenu.active)
			{
				UpdateAccelerometer2();
			}
		}
	//	pm.CallIsFinished();
	}
	
	
	Vector3 m_prevAccelerator;
	/// <summary>
	/// Method UpdateOffset
	/// </summary>
	void UpdateAccelerometer()
	{
		print("upda ac");
		Vector3 accelerator  = iPhoneInput.acceleration;
		
		float xChange = (accelerator.y - m_prevAccelerator.y);
		float yChange = (accelerator.x - m_prevAccelerator.x);
	
		float xsensitivity = 70f;
		float ysensitivity = 50f;
		
		m_currentDelta.x = -xChange*xsensitivity*Time.deltaTime;
		m_currentDelta.y = -yChange*ysensitivity*Time.deltaTime;
		
		m_currentDelta = m_mouseDeltaAverage.Update(m_currentDelta);
		
		m_prevAccelerator = accelerator;
	}
	
	Vector3 accelerator;
	float m_tiltCenter = -0.60f;
	float m_tiltRange;
	float m_rollCenter = 0f;
	float m_rollRange;
	
	void UpdateAccelerometer2()
	{
		accelerator  = Vector3.Lerp(accelerator, iPhoneInput.acceleration, 7f *Time.deltaTime);
		
		float deadZone = 0.0f;
		//print(accelerator);
		float tilt = m_tiltCenter - accelerator.x;
		if(Mathf.Abs(tilt) < deadZone) tilt = 0.5f;

		if(OptionsMenu.invertY) tilt = -tilt;
			
		m_normalizedTilt = Mathf.InverseLerp(-m_tiltRange, m_tiltRange, tilt);

		float roll = m_rollCenter - accelerator.y;
		if(Mathf.Abs(roll) < roll) roll = 0.5f;
		
		if(OptionsMenu.invertX) roll = -roll;
		
		m_normalizedRoll = Mathf.InverseLerp(-m_rollRange, m_rollRange, roll);
	}
	
	void UpdateMouse()
	{
		float sensitivity = 5f;
		m_currentDelta.x += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
		m_currentDelta.y += Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
		m_currentDelta.x = Mathf.Clamp01(m_currentDelta.x);
		m_currentDelta.y = Mathf.Clamp01(m_currentDelta.y);
		
		if(m_player)
			m_player.SetOffset(m_currentDelta);
	}
	
	int m_prevTouchCount = 0;
	
	public static float normalizedRoll{
		get{
			return instance.m_normalizedRoll;
		}
	}
	
	public static float normalizedTilt{
		get{
			return instance.m_normalizedTilt;
		}
	}
	
	public static void SetAccelerometerSensitivity(SensitivitySetting sensitivity)
	{
		if(sensitivity == SensitivitySetting.High)
		{
			instance.m_rollRange = 0.1f;
			instance.m_tiltRange = 0.08f;
		}
		else if(sensitivity == SensitivitySetting.Medium)
		{
			instance.m_rollRange = 0.2f;
			instance.m_tiltRange = 0.15f;
		}
		else if(sensitivity == SensitivitySetting.Low)
		{
			instance.m_rollRange = 0.3f;
			instance.m_tiltRange = 0.2f;
		}
	}
	
	public static void SetCenter(float rollCenter, float tiltCenter)
	{
		instance.m_rollCenter = rollCenter;
		instance.m_tiltCenter = tiltCenter;
	}
	
	public static void CalibrateNow()
	{
		instance.m_rollCenter = iPhoneInput.acceleration.y;
		instance.m_tiltCenter = iPhoneInput.acceleration.x;
	}
	
	public static float centerRoll{
		get{
			return instance.m_rollCenter;
		}
	}
	public static float centerTilt{
		get{
			return instance.m_tiltCenter;
		}
	}
}
