using UnityEngine;
using System.Collections;

public class GameMetrics
{

	////// singleton
	private static GameMetrics m_instance;
	
	public static GameMetrics instance
	{
		get
		{
			if( m_instance == null )
				m_instance = new GameMetrics();
			
			if(m_instance == null)
			{
				Debug.LogError("Can't find singleton class");
			}
			return m_instance;
		}
	}
	
	///////
	/// ///
	///
	///
	///
	///
	
	public static int selectedShip = 0;
	static int[] selectedTextures = new int[4];
	public static int selectedTexture
	{
		get{
			return selectedTextures[selectedShip];
		}
		set{
			selectedTextures[selectedShip] = value;
		}
	}
	
	GameMode m_activeGameMode;
	
	// achievemnts per track
	static int[][] m_achievements;
	static int[][] m_totalRuns;
	static float[][] m_recordTopSpeed;
	static float[][] m_recordFastestLap;
	static float[][] m_recordDistance;
	
	int m_selectedTrack;
	//
	
	float m_maxSpeed;
	float m_avgSpeed;
	int m_speedUpdateCount;
	
	int m_obstaclesCleared;
	int m_obstaclesHit;
	int m_maxCombo;
	float m_totalTime;
	float m_totalDistance;
	
	
	public static bool newHighSpeed= false;
	
	public static GameMode activeGameMode{
		get{
			return instance.m_activeGameMode;
		}
		set{
			instance.m_activeGameMode = value;
		}
	}
	
	public static int selectedTrack{
		get{
			return instance.m_selectedTrack;
		}
		set{
			instance.m_selectedTrack = value;
		}
	}
	
	
	public static void Init()
	{
		instance.m_maxSpeed = 0f;
		instance.m_avgSpeed = 0f;
		instance.m_speedUpdateCount = 0;
		instance.m_obstaclesCleared = 0;
		instance.m_obstaclesHit = 0;
		instance.m_maxCombo = 0;
		instance.m_totalDistance = 0f;
		instance.m_totalTime = 0f;
	}
	
	public static void UpdateSpeed(float newSpeed)
	{
		if(newSpeed > instance.m_maxSpeed)
		{
			instance.m_maxSpeed = newSpeed;
			newHighSpeed = true;
		}
		instance.m_avgSpeed = (instance.m_totalDistance/GameManager.totalTime);
		
		UpdateDistance(newSpeed*Time.deltaTime);
	}
	
	public static void UpdateDistance(float delta)
	{
		instance.m_totalDistance += delta;
	}
	
	public static void UpdateObstacle(bool hit)
	{
		if(hit)
		{
			instance.m_obstaclesHit++;
		}
		else
		{
			instance.m_obstaclesCleared++;
		}
	}
	
	public static void UpdateCombo(int comboCount)
	{
		if(comboCount > instance.m_maxCombo)
		{
			instance.m_maxCombo = comboCount;
		}
	}
	
	
	public static float maxSpeed{
		get{
			return instance.m_maxSpeed;
		}
	}
	
	public static float avgSpeed{
		get{
			return instance.m_avgSpeed;
		}
	}
	
	public static float averageLap{
		get{
			return GameManager.averageLapTime;
		}
	}
	
	public static float bestLap{
		get{
			return GameManager.bestLapTime;
		}
	}
	
	public static int maxCombo{
		get{
			return instance.m_maxCombo;
		}
	}
	
	public static int obstaclesCleared{
		get{
			return instance.m_obstaclesCleared;
		}
	}
	
	public static int obstaclesHit{
		get{
			return instance.m_obstaclesHit;
		}
	}
	
	public static float totalTime{
		get{
			return GameManager.totalTime;
		}
	}
	
	public static float totalDistance{
		get{
			return (instance.m_totalDistance/1000);
		}
	}
	
	static int maxTracks = 3;
	static int maxModes = 3;
	public static void SaveAchievements()
	{
		for(int track = 1; track <= maxTracks; track++)
		{
			for(int mode = 1; mode <= maxModes; mode++)
			{
				PlayerPrefs.SetInt("Achievement"+track+mode, m_achievements[track][mode]);
				PlayerPrefs.SetFloat("RecordTopSpeed"+track+mode, m_recordTopSpeed[track][mode]);
				PlayerPrefs.SetFloat("RecordFastestLap"+track+mode, m_recordFastestLap[track][mode]);
				PlayerPrefs.SetFloat("RecordDistance"+track+mode, m_recordDistance[track][mode]);
				PlayerPrefs.SetInt("TotalRuns"+track+mode, m_totalRuns[track][mode]);
			}
		}
	}
	
	public static void LoadAchievements()
	{
		m_achievements = new int[maxTracks+1][];
		m_totalRuns = new int[maxTracks+1][];
		m_recordTopSpeed = new float[maxTracks+1][];
		m_recordFastestLap = new float[maxTracks+1][];
		m_recordDistance = new float[maxTracks+1][];
		
		for (int i = 0; i < m_achievements.Length; i++)
		{
			m_achievements[i] = new int[maxModes+1];
			m_totalRuns[i] = new int[maxModes+1];
			m_recordTopSpeed[i] = new float[maxModes+1];
			m_recordFastestLap[i] = new float[maxModes+1];
			m_recordDistance[i] = new float[maxModes+1];
		}
		//SaveAchievements();
		for(int track = 1; track <= maxTracks; track++)
		{
			for(int mode = 1; mode <= maxModes; mode++)
			{
				m_achievements[track][mode] = PlayerPrefs.GetInt("Achievement"+track+mode, 0);
				m_recordTopSpeed[track][mode] = PlayerPrefs.GetFloat("RecordTopSpeed"+track+mode, 100);
				m_recordFastestLap[track][mode] = PlayerPrefs.GetFloat("RecordFastestLap"+track+mode, 60);
				m_recordDistance[track][mode] = PlayerPrefs.GetFloat("RecordDistance"+track+mode, 1);
				m_totalRuns[track][mode] = PlayerPrefs.GetInt("TotalRuns"+track+mode, 0);
			//	Debug.Log(m_achievements[track][mode]);
			}
		}
	}
	
	public static void SetSpeedResults()
	{
		int track = selectedTrack;
		int mode = (int)activeGameMode;
		if(maxSpeed > m_recordTopSpeed[track][mode])
		{
			m_recordTopSpeed[track][mode] = maxSpeed;
		}
		
		if(bestLap < m_recordFastestLap[track][mode])
		{
			m_recordFastestLap[track][mode] = bestLap;
		}
		
		if(totalDistance > m_recordDistance[track][mode])
		{
			m_recordDistance[track][mode] = totalDistance;
		}
		
		Debug.Log("tot runs "+m_totalRuns[track][mode]);
		m_totalRuns[track][mode]++;
		
		SaveAchievements();
	}
	
	public static float GetRecordSpeed()
	{
		return m_recordTopSpeed[selectedTrack][(int)activeGameMode];
	}
	
	public static float GetRecordLap()
	{
		return m_recordFastestLap[selectedTrack][(int)activeGameMode];
	}
	
	public static float GetRecordDistance()
	{
		return m_recordDistance[selectedTrack][(int)activeGameMode];
	}
	
	public static int GetTotalRuns()
	{
		return m_totalRuns[selectedTrack][(int)activeGameMode];
	}
	
	public static int GetAchievement(int track, int mode)
	{
		return m_achievements[track][mode];
	}
	
	public static int SetAchievement(int track, int mode, float value)
	{
		int earned_achiev = Random.Range(1,6);
		
		earned_achiev = 1;
		
		if(mode == 1) // speed mode, value is max speed
		{
			if(track == 1)
			{
				if(value > 134f) earned_achiev++;
				if(value > 184f) earned_achiev++;
				if(value > 254f) earned_achiev++;
				if(value > 315f) earned_achiev++;
			}
			if(track == 2)
			{
				if(value > 134f) earned_achiev++;
				if(value > 184f) earned_achiev++;
				if(value > 254f) earned_achiev++;
				if(value > 315f) earned_achiev++;
			}
			if(track == 3)
			{
				if(value > 134f) earned_achiev++;
				if(value > 184f) earned_achiev++;
				if(value > 254f) earned_achiev++;
				if(value > 315f) earned_achiev++;
			}
		}
		
		else if(mode == 2) // grand prix mode, value is average lap time
		{
			if(track == 1)
			{
				if(value < 50f) earned_achiev++;
				if(value < 45f) earned_achiev++;
				if(value < 42f) earned_achiev++;
				if(value < 40f) earned_achiev++;
			}
			if(track == 2)
			{
				if(value < 50f) earned_achiev++;
				if(value < 45f) earned_achiev++;
				if(value < 42f) earned_achiev++;
				if(value < 40f) earned_achiev++;
			}
			if(track == 3)
			{
				if(value < 50f) earned_achiev++;
				if(value < 45f) earned_achiev++;
				if(value < 42f) earned_achiev++;
				if(value < 40f) earned_achiev++;
			}
		}
		
		else if(mode == 3) // endless mode, value is total distance
		{
			if(track == 1)
			{
				if(value < 5f) earned_achiev--;
				if(value > 7.5f) earned_achiev++;
				if(value > 12f) earned_achiev++;
				if(value > 17.5f) earned_achiev++;
				if(value > 25f) earned_achiev++;
			}
			if(track == 2)
			{
				if(value < 5f) earned_achiev--;
				if(value > 7.5f) earned_achiev++;
				if(value > 12f) earned_achiev++;
				if(value > 17.5f) earned_achiev++;
				if(value > 25f) earned_achiev++;
			}
			if(track == 3)
			{
				if(value < 5f) earned_achiev--;
				if(value > 7.5f) earned_achiev++;
				if(value > 12f) earned_achiev++;
				if(value > 17.5f) earned_achiev++;
				if(value > 25f) earned_achiev++;
			}
		}
		
		if(earned_achiev > m_achievements[track][mode])
		{
			m_achievements[track][mode] = earned_achiev;
			
			SaveAchievements();
		}
		
		return earned_achiev;
	}
}
