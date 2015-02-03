using UnityEngine;
using System.Collections;

public class MainMenu : Menu
{
	string m_levelToLoad;
	
	bool m_loadOnNextUpdate = false;
	float m_timeToLoad;
	
	public GameObject m_instructionsScreen;
	public GUITexture m_calibrationShip;
	public Vector2 m_calibrationShipUpLeft;
	public Vector2 m_calibrationShipDownRight;
	Vector3 m_calibrationShipPos;
	
	static public bool active = false;
	public static int m_racersOnline = 0;
	
	public GUIText m_racersOnlineText;
	
	MainMenuShip m_ship;
	
	void Awake()
	{
		GameMetrics.LoadAchievements();
		GameMetrics.SaveAchievements();
		
		OptionsMenu.LoadPlayerPrefs();
		
		GameMetrics.activeGameMode = GameMode.None;
		GameMetrics.selectedTrack = 0;
	}
	
	protected virtual void Start()
	{
		Object[] objs = GameObject.FindObjectsOfType(typeof(Transform));
		
	//	print("FOUND GAMEOBJECTS ------------------"+objs.Length);
	//	foreach (Transform obj in objs)
	//	{
	//		print(obj.name);
	//	}print("FOUND GAMEOBJECTS ------------------");
		
		MenuManager.DisableLoadingScreen();
		
		m_calibrationShipPos = (m_calibrationShipUpLeft+m_calibrationShipDownRight)/2;
		
		if(m_racersOnlineText)
		{
			if(m_racersOnline == 0)
			{
				m_racersOnline = Random.Range(6000,9000);
			}
			else
			{
				m_racersOnline += Random.Range(-500,400);
			}
			m_racersOnlineText.text = "Racers Online: "+m_racersOnline.ToString();
		}
		
		active = true;
		
	//	SetShipTexture(GameMetrics.selectedShip);
		
		m_ship = (MainMenuShip) GameObject.FindObjectOfType(typeof(MainMenuShip));
		
		m_ship.SetSelectedShip(GameMetrics.selectedShip);
		
		m_ship.SetShipTexture();
		
		
		if(Application.isEditor && MenuManager.SkipMenusAndGoStraightToRaceDebug)
		{
			OnMainMenuPlayButton();
		}
		
		MusicManager.Init();
	}
	
	void OnMainMenuPlayButton()
	{
		//	// PIRACY CHECK
		if(!iPhoneUtils.isApplicationGenuine)
		{
			GameObject go = GameObject.Find("MessageForPirates");
			if(go)
			{
				go.guiText.enabled = true;
			}
			return;
		}
		
		GameMode mode = GameMode.Speed;//(GameMode) Random.Range(1,4);
		
		GameMetrics.activeGameMode = mode;
		GameMetrics.selectedTrack = 0;//Random.Range(1,4);
		
	//	m_levelToLoad = "track_"+GameMetrics.selectedTrack;
		
		m_levelToLoad = "TrackSelectMenu";
		
		MenuManager.EnableLoadingScreen();
		
		m_loadOnNextUpdate = true;
		m_timeToLoad = 0.1f;
	}
	
	void OnMainMenuOptionsButton()
	{
	//	m_loadingScreen.guiTexture.enabled = true;
	
		MenuManager.OpenOptionsMenu();
		
	//	m_loadOnNextUpdate = true;
	}
	
	void OnMainMenuPracticeButton()
	{
		GameMetrics.activeGameMode = GameMode.Speed;
		GameMetrics.selectedTrack = 4;
		
		m_levelToLoad = "track_practice";
		
		MenuManager.EnableLoadingScreen();
		
		m_timeToLoad = 0.1f;
		
		m_loadOnNextUpdate = true;
	}
	
	void OnMainMenuInstructionsButton()
	{
	//	m_instructionsScreen.SetActiveRecursively(!m_instructionsScreen.active);
	}
	
	void OnMainMenuLeftArrowPressed()
	{
	//	if(m_ship.isInTransition || m_ship.shipCount == 0) return;
		GameMetrics.selectedShip--;
		if(GameMetrics.selectedShip >= m_ship.shipCount ) GameMetrics.selectedShip = 0;
		if(GameMetrics.selectedShip < 0 ) GameMetrics.selectedShip = m_ship.shipCount-1;
		m_ship.SetNextShip();

	}
	
	void OnCalibrateButtonPressed()
	{
		InputManager.CalibrateNow();
	}
	
	void OnMainMenuRightArrowPressed()
	{
		m_ship.SetNextTexture();
	}
	
	void SetShipTexture(int index)
	{
//		if(m_shipModel)
//			m_shipModel.material.mainTexture = MenuManager.shipTextures[GameMetrics.selectedShip];
	}
	
	void Update()
	{
		if(m_loadOnNextUpdate)
		{
			m_timeToLoad -= Time.deltaTime;
			if(m_timeToLoad > 0) return;
			
			active = false;
			m_loadOnNextUpdate = false;
			Application.LoadLevel(m_levelToLoad);
			return;
		}
		
		UpdateCalibrationShip();
	}
	
	void UpdateCalibrationShip()
	{
		if(m_calibrationShip)
		{
			float x = InputManager.normalizedRoll;
			float y = InputManager.normalizedTilt;
			
			Vector3 targetShipPos = new Vector3();
			targetShipPos.x = Mathf.Lerp(m_calibrationShipUpLeft.x, m_calibrationShipDownRight.x, x);
			targetShipPos.y = Mathf.Lerp(m_calibrationShipDownRight.y, m_calibrationShipUpLeft.y, y);
			
			if(Vector3.Distance(m_calibrationShipPos, targetShipPos) > 0.0f)
				m_calibrationShipPos =  Vector3.Lerp(m_calibrationShipPos, targetShipPos, 4f*Time.deltaTime);
			
			m_calibrationShipPos.z = m_calibrationShip.transform.localPosition.z;
			
			m_calibrationShip.transform.localPosition = m_calibrationShipPos;
		}
	}
}
