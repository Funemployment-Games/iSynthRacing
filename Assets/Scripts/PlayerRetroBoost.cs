using UnityEngine;
using System.Collections;

public class PlayerRetroBoost : MonoBehaviour
{
	bool m_active;
	
	Player m_player;
	
	float m_maxReserve = 10f;
	float m_currentReserve;
	
	float m_reserveFillSpeed = 3f;
	float m_reserveDepletionSpeed = 4f;
	float m_currentReserveDepletionSpeed = 4f;
	
	void Start()
	{
		m_player = (Player) GetComponent(typeof(Player));
		
		Init();
	}
	
	void Update()
	{
		if(m_active)
		{
			HandleActive();
		}
		else
		{
			HandleNotActive();
		}
	}
	
	public void Init()
	{
		m_currentReserve = 0f;
	}
	
	public void Activate()
	{
		if(normalizedReserve < 0.6f)
		{
			return;
		}

		m_active = true;
		
		m_player.OnRetroBoostActivate();
	}
	
	public void Deactivate()
	{
		m_active = false;
		
		m_player.OnRetroBoostDeactivate();
	}

	
	void HandleActive()
	{
		m_currentReserveDepletionSpeed = m_reserveDepletionSpeed-(GameManager.currentSpeedLevel*0.2f);
		m_currentReserve -= m_currentReserveDepletionSpeed*Time.deltaTime;
		
		if(m_currentReserve <= 0f)
		{
			m_currentReserve = 0f;
			Deactivate();
		}
	}
	
	void HandleNotActive()
	{
		if(!GameManager.instance) return;
		
		if(GameManager.gameState == GameState.Running)
		{
			m_currentReserve += m_reserveFillSpeed*Time.deltaTime;
		}
		
		m_currentReserve = Mathf.Clamp(m_currentReserve, 0f, m_maxReserve);
	}
	
	public bool CanBeActivated()
	{
		bool ok = true;
		
		ok &= normalizedReserve > 0.9f;
		
		return ok;
	}
	
	public bool isActive{
		get{
			return m_active;
		}
	}
	
	public float normalizedReserve{
		get{
			return (m_currentReserve/m_maxReserve);
		}
	}
}
