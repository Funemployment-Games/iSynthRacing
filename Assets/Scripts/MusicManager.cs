using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	////// singleton
	private static MusicManager m_instance;
	
	public static MusicManager instance
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
	
	///
	//
	static bool m_enabled = true;
	
	public AudioClip[] m_clips;
	
	int m_bpm = 120;
	
	
	float m_beatStartOffset = 0f;
	
	public float m_volume = 0.5f;
	
	float m_lastBeatTime = 0f;
	
	float m_timeBetweenBeats;
	
	AudioSource m_source;
	
	float m_playStartTime;
	float m_elapsedTime;
	
	Renderer[] m_trackRenderers;
	Color m_originalTrackColor;
	
	public static bool pauseOnNextFrame = false;
	
	Player m_player;
	
	public static void Init()
	{
		AudioClip clip = null;
		if(instance.m_clips[Application.loadedLevel-1] != null)
		{
			clip = instance.m_clips[Application.loadedLevel-1];
		}
		
		instance.m_source.Stop();
		
		if(clip != null)
		{
			instance.m_source.clip = clip;
			if(m_enabled)
			{
				instance.m_source.Play();
			}
		}
		else
		{
			instance.m_source.clip = null;
		}
	}
	
	public static void Shutdown()
	{
		StopAll();
		instance.m_player = null;
	}
	
	void Update()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "Update");
		if(m_player != null)
		{
			float targetVol = m_volume;
			if(m_player.retroBoostComponent && m_player.retroBoostComponent.isActive)
			{
				targetVol = m_volume * 0.1f;
			}
			
			if(Mathf.Abs(targetVol-m_source.volume) > 0.02f)
			{
				m_source.volume = Mathf.Lerp(m_source.volume, targetVol, 2f * Time.deltaTime);
			}
		}
	//	pm.CallIsFinished();
	}
	
	public static void SetPlayerReference(Player player)
	{
		instance.m_player = player;
	}
	
	// Use this for initialization
	void Start ()
	{
		if(m_source == null)
		{
			GameObject sourceGO = new GameObject("MusicAudioSource");
			
			sourceGO.transform.parent = transform;
			m_source = (AudioSource) sourceGO.AddComponent(typeof(AudioSource));
			m_source.playOnAwake = false;
			m_source.loop = true;
			m_source.clip = null;
			m_source.volume = m_volume;
		}
	}
	
	public static void PauseAll()
	{
		print("pause "+Time.time);
		if(instance.m_source && instance.m_source.isPlaying && instance.m_source.clip != null)
			instance.m_source.Pause();
	}
	
	public static void UnPauseAll()
	{
		if(!m_enabled)
		{
			return;
		}
		
		print("un pause "+Time.time);
		if(instance.m_source && !instance.m_source.isPlaying && instance.m_source.clip != null)
			instance.m_source.Play();
	}
	
	public static void StopAll()
	{
		print("stop "+Time.time);
		if(instance.m_source)
		{
			instance.m_source.Stop();
		}
	}
	
	public static void SetEnabled(bool enabled)
	{
		if(enabled == m_enabled)
			return;
		
		m_enabled = enabled;
		
		if(!m_enabled && instance.m_source.isPlaying)
		{
			instance.m_source.Pause();
		}
		
		print("dds "+m_enabled+" "+instance.m_source.isPlaying+" "+(instance.m_source.clip==null));
		if(m_enabled && !instance.m_source.isPlaying && instance.m_source.clip != null)
		{
			print("playy");
			instance.m_source.Play();
		}
	}
}
