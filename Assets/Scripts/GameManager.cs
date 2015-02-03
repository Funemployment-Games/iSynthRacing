using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	////// singleton
	private static GameManager m_instance;
	
	public static GameManager instance
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
	}
	
	///////
	/// ///
	///
	///
	
	GameState m_gameState = GameState.None;
	GameMode m_gameMode = GameMode.Speed;
	
	Player m_player;
	
	// obstacle colors per mode
	public Color m_speedModeObstacleColor;
	public Color m_grandprixModeObstacleColor;
	public Color m_endlessModeObstacleColor;
	
	// speed mode
	float m_speedModeTime = 45f;
	//float m_speedModeTime = 10f;
	float m_speedModeTimeLeft;
	
	// endless mode
	uint m_endlessModeStartingLives = 3;
	uint m_endlessModeRemainingLives;
	
	// grand prix
	int m_lapCount = 3;
	
	float m_lapTime = 0f;
	int m_currentLap = 0;
	float m_averageLapTime = 0f;
	float m_bestLapTime = 0f;
	float m_totalTime;
	
	///
	///
	 // results screens prefabs
	public GameObject m_speedModeResultsScreen;
	public GameObject m_grandPrixModeResultsScreen;
	public GameObject m_endlessModeResultsScreen;
	
	///
	///
	// in game hud prefabs
	public GameObject m_speedModeGUI;
	public GameObject m_grandPrixModeGUI;
	public GameObject m_endlessModeGUI;
	
	// countdown object prefab
	public GameObject m_countdownPrefab;
	
	CountdownObject ctdn;
	
	SpeedModeResultsScreen m_resultsScreen;
	
	public static void Reset()
	{
		GameMetrics.Init();
		
		instance.Init();
		
		SegmentManager.activeTrack.UnhideObstacles();
		
		if(instance.m_resultsScreen)
		{
			instance.m_resultsScreen.Deactivate();
		}
		
		// create a ghost ship
	//	GameObject ghostgo = (GameObject)Instantiate(MenuManager.shipGhostPrefabs[GameMetrics.selectedShip]);
	}
	
	void Init()
	{
		if(Application.isEditor && GameMetrics.activeGameMode == GameMode.None)
		{
			GameMetrics.activeGameMode = GameMode.Speed;
			GameMetrics.selectedTrack = 1;
		}
		
		SetGameState(GameState.Loading);
		
		//instance.m_player = (Player)FindObjectOfType(typeof(Player));
		MusicManager.Init();
		
		instance.m_player = (Player)GameObject.FindObjectOfType(typeof(Player));
		if(instance.m_player == null)
		{
			print("creating player");
			GameObject playergo = (GameObject)Instantiate(MenuManager.shipPrefabs[GameMetrics.selectedShip]);
			instance.m_player = (Player)playergo.GetComponent(typeof(Player));
		}
		
		instance.m_player.Init();
		
		gameMode = GameMetrics.activeGameMode;
		
		print(gameMode+ " "+Application.loadedLevelName);
		
		GameMetrics.Init();
		
		ComboSystem.Init();
		
	//	GhostManager.Init();
		
		m_endlessModeRemainingLives = m_endlessModeStartingLives;
		m_lapTime = 0f;
		m_bestLapTime = 1000f;
		m_currentLap = 0;
		
		m_firstService = true;
		
		m_speedModeTimeLeft = m_speedModeTime;
		
		m_targetTunnelColor = speedLevelData[0].tunnelColor;
	}

	// Use this for initialization
	void Start ()
	{
		Init();
		
		InputManager.Init();
		
		// set track's color depending on game mode
		Color ob_color = Color.black;
		GameObject gui_prefab = m_speedModeGUI;
		if(gameMode == GameMode.Speed)
		{
			ob_color = m_speedModeObstacleColor;
			gui_prefab = m_speedModeGUI;
		}
		else if(gameMode == GameMode.Endless)
		{
			ob_color = m_endlessModeObstacleColor;
			gui_prefab = m_endlessModeGUI;
		}
		else if(gameMode == GameMode.GrandPrix)
		{
			ob_color = m_grandprixModeObstacleColor;
			gui_prefab = m_grandPrixModeGUI;
		}
		
		SegmentManager.activeTrack.SetObstaclesColor(ob_color);
		
		if(gui_prefab)
		{
			Instantiate(gui_prefab);
		}
		
		if(!instance.m_resultsScreen)
		{
			CreateResultsScreen();
		}
		
		GameObject ctdnObj = (GameObject) Instantiate(m_countdownPrefab);
		instance.ctdn = (CountdownObject) ctdnObj.GetComponent((typeof(CountdownObject)));
	}

	
	// Update is called once per frame
	void Update ()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "Update");
		if(gameState == GameState.Running)
		{
			m_lapTime += Time.deltaTime;
			
			m_totalTime += Time.deltaTime;
			
			if(gameMode == GameMode.Speed)
			{
				m_speedModeTimeLeft -= Time.deltaTime;
				
				// don't end on practice
				if(m_speedModeTimeLeft <= 0f && GameMetrics.selectedTrack != 4)
				{
					// end track
					SetGameState(GameState.Results);
				}
				
			}
			
			if(gameMode == GameMode.GrandPrix)
			{
			}
			
			if(gameMode == GameMode.Endless)
			{
				m_lapTime += Time.deltaTime;
			}
		}
		
		SetSpeedModeLevelColors();
	//	pm.CallIsFinished();
	}
	
	float m_waitAfterLoadTime = 0.5f;
	
	bool m_firstService = true;
	
	void LateUpdate()
	{
		if(m_waitAfterLoadTime > 0f)
		{
			m_waitAfterLoadTime -= Time.deltaTime;
			
		}
		else if(m_firstService)
		{
			m_firstService = false;
			SetGameState(GameState.Countdown);
			MenuManager.DisableLoadingScreen();
		}
	}
	
	void SetPause(bool pauseMode)
	{
		if(pauseMode == true)
		{
		//	Time.timeScale = 0f;
		}
		else
		{
			//Time.timeScale = 1f;
			
		}
	}
	
	public static void SetGameState(GameState newState)
	{
		instance.m_gameState = newState;
		
		if(newState == GameState.Loading)
		{
		//	instance.SetPause(true);
		}
		
		if(newState == GameState.Paused)
		{
			MusicManager.PauseAll();
			SoundManager.PauseAll();
			instance.SetPause(true);
		}
		
		if(newState == GameState.Running)
		{
			MusicManager.UnPauseAll();
			SoundManager.UnPauseAll();
			instance.SetPause(false);
		}
		
		if(newState == GameState.Countdown)
		{
			if(instance.ctdn)
			{
				instance.ctdn.Init();
			}
			instance.SetPause(false);
		}
		
		if(newState == GameState.Results)
		{
			if(gameMode == GameMode.Speed)
			{
				
			}
			ShowResultsScreen();
		}
	}
	
	public static GameState gameState{
		get{
			return instance.m_gameState;
		}
	}
	
	public static GameMode gameMode{
		get{
			return instance.m_gameMode;
		}
		set{
			instance.m_gameMode = value;
		}
	}
	
	public static Player player{
		get{
			return instance.m_player;
		}
	}
	
	public static float totalTime{
		get{
			return instance.m_totalTime;
		}
	}
	
	public static float lapTime{
		get{
			return instance.m_lapTime;
		}
	}
	
	public static int lapNumber{
		get{
			return instance.m_currentLap;
		}
	}
	
	public static float averageLapTime{
		get{
			return instance.m_averageLapTime;
		}
	}
	
	public static float bestLapTime{
		get{
			return instance.m_bestLapTime;
		}
	}
	
	public static uint remainingLives{
		get{
			return instance.m_endlessModeRemainingLives;
		}
	}
	
	public static uint timeLeft{
		get{
			return (uint)instance.m_speedModeTimeLeft;
		}
	}
	
	public static void AddTime(float val)
	{
		instance.m_speedModeTimeLeft += val;
	}
	
	static void CreateResultsScreen()
	{
		GameObject screenPrefab = null;
		switch (gameMode)
		{
			case GameMode.Endless:
				screenPrefab = instance.m_endlessModeResultsScreen;
				break;
			case GameMode.GrandPrix:
				screenPrefab = instance.m_grandPrixModeResultsScreen;
				break;
			case GameMode.Speed:
				screenPrefab = instance.m_speedModeResultsScreen;
				break;
			default:
				break;
		}
		if(screenPrefab)
		{
			GameObject go = (GameObject) Instantiate(screenPrefab);
			instance.m_resultsScreen = (SpeedModeResultsScreen) go.GetComponent(typeof(SpeedModeResultsScreen));
		}
	}
	
	static void ShowResultsScreen()
	{
		if(instance.m_resultsScreen)
		{
			instance.m_resultsScreen.Activate();
		}
		SegmentManager.activeTrack.HideObstacles();
	}
	
	/// <summary>
	/// Method Instantiate
	/// </summary>
	private static void Instantiate()
	{
		// TODO: Implement this method
	}
	
	/// interfaces
	///
	///
	public void SetCountdownDone()
	{
		SetGameState(GameState.Running);
	}
	
	public void SetGameMode(GameMode newMode)
	{
		m_gameMode = newMode;
	}
	
	public static void OnPickupPicked()
	{
	}
	
	public static void OnObstacleHit()
	{
		if(gameMode == GameMode.Endless)
		{
			instance.m_endlessModeRemainingLives--;
			if(instance.m_endlessModeRemainingLives == 0)
			{
				// game over
				SetGameState(GameState.Results);
			}
		}
	}
	
	public static void OnLap()
	{
		if(gameState!= GameState.Running) return;
		
		instance.m_currentLap++;
		
		if(gameMode == GameMode.GrandPrix && instance.m_currentLap == instance.m_lapCount)
		{
			SetGameState(GameState.Results);
		}
		
		if(instance.m_lapTime < instance.m_bestLapTime)
		{
			instance.m_bestLapTime = instance.m_lapTime;
		}
		
		instance.m_lapTime = 0f;
		instance.m_averageLapTime = instance.m_totalTime/instance.m_currentLap;
	}
	
	public static int currentSpeedLevel{
		get{
			return instance.m_speedLevel;
		}
	}
	
	public SpeedLevelData[] speedLevelData;
	public Color m_retroBoostObstacleColor;
	public Color m_retroBoostTunnelColor;
	Color m_targetObstacleColor;
	Color m_currentObstacleColor;
	Color m_targetTunnelColor;
	Color m_currentTunnelColor;
	
	float m_colorChangeSpeed;
	
	int m_speedLevel = 1;
	
	void SetSpeedModeLevelColors()
	{
	//	return;
		if(speedLevelData.Length == 0) return;

		int i = 0;
		
		SpeedLevelData data = speedLevelData[i];
		while(m_player.currentSpeed > speedLevelData[i].topSpeed)
		{
			i++;
		}
		
		m_speedLevel = i+1;
		
		m_colorChangeSpeed = 1.5f;
		
		m_targetObstacleColor = speedLevelData[i].obstacleColor;
		m_targetTunnelColor = speedLevelData[i].tunnelColor;
		
		m_currentTunnelColor.a = 0.7f;
		
		if(m_player.retroBoostComponent.isActive && m_player.retroBoostComponent.normalizedReserve > 0.3f)
		{
			m_colorChangeSpeed = 3f;
			
			m_targetObstacleColor = m_retroBoostObstacleColor;
			m_targetTunnelColor = m_retroBoostTunnelColor;
		}
		
		if(Mathf.Abs(m_currentObstacleColor.r-m_targetObstacleColor.r) > 0.05f ||
		   Mathf.Abs(m_currentObstacleColor.g-m_targetObstacleColor.g) > 0.05f ||
		   Mathf.Abs(m_currentObstacleColor.b-m_targetObstacleColor.b) > 0.05f)
		{
			m_currentObstacleColor = Color.Lerp(m_currentObstacleColor, m_targetObstacleColor, m_colorChangeSpeed * Time.deltaTime);
			
			SegmentManager.instance.m_track.SetObstaclesColor(m_currentObstacleColor);
			RenderSettings.fogColor = m_currentObstacleColor;
		}
		
		if(Mathf.Abs(m_currentTunnelColor.r-m_targetTunnelColor.r) > 0.05f ||
		   Mathf.Abs(m_currentTunnelColor.g-m_targetTunnelColor.g) > 0.05f ||
		   Mathf.Abs(m_currentTunnelColor.b-m_targetTunnelColor.b) > 0.05f)
		{
			m_currentTunnelColor = Color.Lerp(m_currentTunnelColor, m_targetTunnelColor, m_colorChangeSpeed * Time.deltaTime);
		
			SegmentManager.instance.m_track.SetTunnelColor(m_currentTunnelColor);
		//	RenderSettings.fogColor = m_currentTunnelColor;
		}
		//Camera.main.backgroundColor = m_currentTunnelColor;
		//print(Camera.main.backgroundColor);
		
		float speedPerc = Mathf.InverseLerp(100, 400, m_player.currentSpeed);
		RenderSettings.fogDensity = Mathf.Lerp(0.003f, 0.008f, speedPerc);
	}
	
}

public enum GameState {None, Loading, Paused, Running, Countdown, Results};
public enum GameMode {None, Speed, GrandPrix, Endless};

[System.Serializable]
public class SpeedLevelData
{
	public Color obstacleColor;
	public Color tunnelColor;
	public float topSpeed;
}
