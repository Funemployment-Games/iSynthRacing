using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	////// singleton
	private static SoundManager m_instance;
	
	public AudioClip[] m_obstacleClearedSounds;
	public AudioClip[] m_pickupPickedSounds;
	public AudioClip[] m_barrelRollSounds;
	public AudioClip[] m_obstacleHitSounds;
	public AudioClip[] m_retroBoostSounds;
	public AudioClip m_buttonPressedGenericSound;
	
	// player
	public AudioClip m_playerEngineSound;
	public float m_playerEngineVolume = 1f;
	public float m_playerEnginePitchAtMinSpeed = 0.8f;
	public float m_playerEnginePitchAtMaxSpeed = 1.5f;
	
	public float m_obstaclePassedPitchVariance = 0.08f;
	public float m_obstaclePitchAtMaxSpeed = 1.4f;
	
	public float m_soundVolume = 1f;
	
	static bool m_enabled = true;
	
	static int m_channels = 6;
	
	AudioSource[] m_sources;
	AudioSource m_playerEngineSource;
	
	Player m_player;
	
	public static SoundManager instance
	{
		get
		{
			return m_instance;
		}
	}
	
	void Awake()
	{
		//	DontDestroyOnLoad(this);
		if( m_instance != null )
		{
			return;
		}
		m_instance = this;
	}

	public static void Shutdown()
	{
		instance.m_player = null;
	}
	
	// Use this for initialization
	void Start ()
	{
		//	DontDestroyOnLoad(this);
		
		GameObject go;
		m_sources = new AudioSource[m_channels];
		for (int i = 0; i < m_channels; i++)
		{
			go = new GameObject("AudioSource"+(i+1));
			go.transform.parent = transform;
			m_sources[i] = (AudioSource) go.AddComponent(typeof(AudioSource));
			m_sources[i].playOnAwake = false;
			m_sources[i].loop = false;
			m_sources[i].volume = m_soundVolume;
			//m_sources[i].min = m_soundVolume;
			//m_sources[i].max = m_soundVolume;
			//m_sources[i].rolloffFactor = 0.01f;
		}
			
		go = new GameObject("AudioSourceEngine");
		go.transform.parent = transform;
		m_playerEngineSource = (AudioSource) go.AddComponent(typeof(AudioSource));
		m_playerEngineSource.playOnAwake = false;
		m_playerEngineSource.clip = m_playerEngineSound;
		m_playerEngineSource.pitch = 0f;
		m_playerEngineSource.loop = true;
		//m_playerEngineSource.min = 0f;
		//m_playerEngineSource.max = m_playerEngineVolume;
		//m_playerEngineSource.rolloffFactor = 0f;
		m_playerEngineSource.volume = m_playerEngineVolume;
	}
	
	// Update is called once per frame
	void Update ()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "Update");
		if(m_player && GameManager.instance)
		{
			float m_targetEnginePitch;
			if(GameManager.gameState == GameState.Countdown || GameManager.gameState == GameState.Paused)
			{
				m_targetEnginePitch = m_playerEnginePitchAtMinSpeed;
			}
			else
			{
				m_targetEnginePitch = Mathf.Lerp(m_playerEnginePitchAtMinSpeed, m_playerEnginePitchAtMaxSpeed, m_player.normalizedSpeed0400);
			}

			m_playerEngineSource.pitch = Mathf.Lerp(m_playerEngineSource.pitch, m_targetEnginePitch, 5f * Time.deltaTime);
			m_playerEngineSource.transform.position = m_player.transform.position;
			
			float targetVol = m_playerEngineVolume;
			if(m_player.retroBoostComponent.isActive)
			{
				targetVol = m_playerEngineVolume*0.1f;
			}
			
			if(Mathf.Abs(targetVol-m_playerEngineSource.volume) > 0.02f)
			{
				m_playerEngineSource.volume = Mathf.Lerp(m_playerEngineSource.volume, targetVol, 2f * Time.deltaTime);
			}
		}
		else
		{
			if(instance.m_playerEngineSource.isPlaying)
			{
				instance.m_playerEngineSource.Stop();
			}
		}
	//	pm.CallIsFinished();
	}
	
	public static void SetPlayerReference(Player player)
	{
		if(player == null) return;
		
		instance.m_player = player;
		
		if(m_enabled && !instance.m_playerEngineSource.isPlaying)
		{
			if(GameManager.instance != null)
			{
				instance.m_playerEngineSource.Play();
			}
		}
	}
	
	public static AudioSource[] sources{
		get{
			return instance.m_sources;
		}
	}
	
	public static AudioClip randomObstacleClearedSound
	{
		get{
			return instance.m_obstacleClearedSounds[Random.Range(0, instance.m_obstacleClearedSounds.Length)];
		}
	}
	
	public static AudioClip randomPickupPickedSound
	{
		get{
			return instance.m_pickupPickedSounds[Random.Range(0, instance.m_pickupPickedSounds.Length)];
		}
	}
	
	public static AudioClip randomObstacleHitSound
	{
		get{
			return instance.m_obstacleHitSounds[Random.Range(0, instance.m_obstacleHitSounds.Length)];
		}
	}
	
	public static AudioClip randomBarrelRollSound
	{
		get{
			return instance.m_barrelRollSounds[Random.Range(0, instance.m_barrelRollSounds.Length)];
		}
	}
	
	public static AudioClip randomRetroBoostSound
	{
		get{
			return instance.m_retroBoostSounds[Random.Range(0, instance.m_retroBoostSounds.Length)];
		}
	}
	
	public static void PlaySoundEvent(SoundEvent type, Vector3 position)
	{
		if(!m_enabled)
		{
			return;
		}
		
		AudioClip clip = null;
		float pitch = 1f;
		
		switch (type)
		{
			case SoundEvent.ObstacleCleared:
				clip = randomObstacleClearedSound;
				float v = instance.m_obstaclePassedPitchVariance;
				pitch = Mathf.Lerp(1f, instance.m_obstaclePitchAtMaxSpeed, instance.m_player.normalizedSpeed100400);
				pitch += Random.Range(-v, v);
				break;
				
			case SoundEvent.PickupPicked:
				clip = randomPickupPickedSound;
				break;
				
			case SoundEvent.ObstacleHit:
				clip = randomObstacleHitSound;
				break;
				
			case SoundEvent.BarrelRoll:
				clip = randomBarrelRollSound;
				break;
				
			case SoundEvent.RetroBoost:
				clip = randomRetroBoostSound;
				break;
		}
		
		if(clip == null)
		{
			return;
		}
		
		//find an empty spot
		AudioSource source = sources[0];
		for (int i = 0; i < sources.Length; i++)
		{
			if(!sources[i].isPlaying)
			{
				source = sources[i];
				break;
			}
		}
			
		source.transform.position = position;
		source.pitch = pitch;
		source.clip = clip;
		source.Play();
	}
	
	public static void PlayGUISound(SoundEvent type)
	{
		if(!m_enabled)
		{
			return;
		}
		
		AudioClip clip = null;
		float volume = instance.m_soundVolume;
		
		switch (type)
		{
			case SoundEvent.ButtonPressedGeneric:
				clip = instance.m_buttonPressedGenericSound;
				break;
		}
		
		if(clip == null)
		{
			return;
		}
		
		instance.audio.transform.position = Camera.main.transform.position;
		instance.audio.clip = clip;
		instance.audio.loop = false;
		instance.audio.Play();
	}
	
	public static void SetEnabled(bool enabled)
	{
		if(m_enabled == enabled) return;
		
		m_enabled = enabled;
		
		if(m_enabled)
		{
			
			if(GameManager.instance != null && !instance.m_playerEngineSource.isPlaying)
			{
				print("play enen");
				instance.m_playerEngineSource.Play();
			}
		}
		else
		{
			if(GameManager.instance != null && instance.m_playerEngineSource.isPlaying)
				instance.m_playerEngineSource.Stop();
		}
	}
	
	public static void PauseAll()
	{
		for (int i = 0; i < sources.Length; i++)
		{
			if(!sources[i].isPlaying)
			{
				sources[i].Stop();
			}
		}
		
		instance.m_playerEngineSource.Pause();
	}
	
	public static void UnPauseAll()
	{
		if(m_enabled)
		{
			instance.m_playerEngineSource.Play();
		}
	}
}

public enum SoundEvent{None, ObstacleCleared, ObstacleHit, BarrelRoll, RetroBoost, PickupPicked, ButtonPressedGeneric}
