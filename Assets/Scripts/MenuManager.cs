using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	////// singleton
	private static MenuManager m_instance;
	
	public OptionsMenu m_optionsMenu;
	
	public LoadingScreen m_loadingScreen;
	
	public static bool firstLoadMainMenu = true;
	
	public GameObject[] m_shipPrefabs;
	public GameObject[] m_shipGhostPrefabs;
	
	public Texture2D[] m_ship1Textures;
	public Texture2D[] m_ship2Textures;
	public Texture2D[] m_ship3Textures;
	public Texture2D[] m_ship4Textures;
	
	public static bool SkipMenusAndGoStraightToRaceDebug = false;
	
	bool m_loadLevelOnNextFrame = false;
	string m_levelName;
	
	public static MenuManager instance
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

	public static Texture2D GetShipTexture(int ship, int textureIndex)
	{
		Texture2D[] textures = null;
		if(ship == 0)
			textures = instance.m_ship1Textures;
		if(ship == 1)
			textures = instance.m_ship2Textures;
		if(ship == 2)
			textures = instance.m_ship3Textures;
		if(ship == 3)
			textures = instance.m_ship4Textures;
		
		if(textures != null)
		{
			int i = textureIndex;
			if(textureIndex >= textures.Length) i = 0;
			GameMetrics.selectedTexture = i;
			return textures[i];
		}
		
		return instance.m_ship1Textures[0];
	}
	
	void Start()
	{
		GameMetrics.selectedShip = 3;
	}
	
	public static GameObject[] shipPrefabs
	{
		get{
			return instance.m_shipPrefabs;
		}
	}
	
	public static GameObject[] shipGhostPrefabs
	{
		get{
			return instance.m_shipGhostPrefabs;
		}
	}
	
	public static void SendMessage(string message)
	{
		Menu menu = (Menu) FindObjectOfType(typeof(Menu));
		menu.gameObject.SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
	
	public static void OpenOptionsMenu()
	{
		instance.m_optionsMenu.OpenMenu();
		
		if(GameManager.instance != null)
		{
			MusicManager.PauseAll();
			SoundManager.PauseAll();
		}
			
	}
	
	public static void CloseOptionsMenu()
	{
		instance.m_optionsMenu.CloseMenu();
		
		if(GameManager.instance != null)
		{
			MusicManager.UnPauseAll();
			SoundManager.UnPauseAll();
		}
	}
	
	public static void EnableLoadingScreen()
	{
		instance.m_loadingScreen.Enable();
	}
	
	public static void DisableLoadingScreen()
	{
		instance.m_loadingScreen.Disable();
	}
	
	public static void LoadMainMenu(bool showLoading)
	{
		InputManager.Shutdown();
		MusicManager.Shutdown();
		SoundManager.Shutdown();
		
		Player player = (Player)FindObjectOfType(typeof(Player));
		if(player)
		{
			Destroy(player.gameObject);
			player = null;
		}
		
		Track track = (Track)FindObjectOfType(typeof(Track));
		if(track)
		{
			Destroy(track.gameObject);
			track = null;
		}
		
		GameManager gameman = (GameManager)FindObjectOfType(typeof(GameManager));
		if(gameman)
		{
			Destroy(gameman.gameObject);
			gameman = null;
		}
		
		SegmentManager segman = (SegmentManager)FindObjectOfType(typeof(SegmentManager));
		if(segman)
		{
			DestroyImmediate(segman.gameObject, true);
			segman = null;
		}
		
		GameMetrics.activeGameMode = GameMode.None;
		GameMetrics.selectedTrack = 0;
		
		if(showLoading)
		{
			EnableLoadingScreen();
		}
		
		System.GC.Collect();
		
		instance.m_loadLevelOnNextFrame = true;
	}
	
	
	void Update()
	{
		if(m_loadLevelOnNextFrame)
		{
			m_loadLevelOnNextFrame = false;
			
			Application.LoadLevel("mainmenu");
		}
	}
}
