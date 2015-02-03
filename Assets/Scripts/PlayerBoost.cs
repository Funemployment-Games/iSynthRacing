using UnityEngine;
using System.Collections;

public class PlayerBoost : MonoBehaviour
{
	public ParticleEmitter m_boostParticles;
	
	float m_maxBoostReserve = 150f;
	
	// the amount of boost we have available for use (size of bar)
	float m_currentBoostReserve = 0f;
	
	// how fast we replenish the boost by default (in units per second)
	float m_boostReplenishSpeed = 15f;
	
	// how fast we deplete the boost when using it (in units per second)
	// note: this is additive with the above value, so each frame we'll add boost then substract it if we're using it
	// this allows us to have a system where we replenish faster than we use it, hence allowing infinite boost.
	float m_boostDepletionSpeed = 30f;
	
	// the minimum boost we have to have in order to be able to activate it
	float m_minBoostForActivation = 50f;
	
	// the actual boost power we are using now. This starts at a value when activating boost, and increases over time
	// making the ship go faster and faster, as long as we have boost reserve
	// 1f is 100% of ship speed, 2 is 200%, etc
	float m_currentBoostValue;
	
	float m_maxBoostValue = 1.5f;
	float m_afterburnerBoostValue = 2f;
	float m_startBoostValue = 1f;
	
	// is the boost currently active
	bool m_boostActive = false;
	
	// the time when we last activated the boost
	float m_lastBoostStartTime = 0f;
	
	float m_boostComboMultiplier;
	
	Player m_player;
		
	// Use this for initialization
	void Start ()
	{
		m_player = (Player) GetComponent(typeof(Player));
		Init();
	}
	
	public void Init()
	{
		m_currentBoostValue = m_startBoostValue;
		m_currentBoostReserve = 0f;
		m_lastBoostStartTime = 0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!GameManager.instance) return;
		
		if(GameManager.gameState != GameState.Running)
		{
			return;
		}
		
		// update combo multiplier
		m_boostComboMultiplier = 1 + (0.04f*ComboSystem.instance.currentComboCount);
		
		if(m_currentBoostReserve < m_maxBoostReserve)
		{
			float boost = m_boostReplenishSpeed * m_boostComboMultiplier * Time.deltaTime;
			
			m_currentBoostReserve += boost;
		}
		
		if(m_boostActive)
		{
			HandleBoostActive();
		}
		else
		{
			HandleBoostNotActive();
		}
	}
	
	public void SetBoostState(bool newBoostState)
	{
		if(newBoostState == true && m_boostActive == false)
		{
			if(!CanBeActivated()) return;
			
			// handle boost turning on
			m_boostActive = true;
			
			m_lastBoostStartTime = Time.time;
			
			m_currentBoostValue = m_startBoostValue;
			
			m_boostParticles.emit = true;
			
			m_player.OnBoostActivate();
		}
		
		if(newBoostState == false && m_boostActive == true)
		{
			// handle boost turning off
			m_boostActive = false;
			
			m_boostParticles.emit = false;
			
			m_player.OnBoostDeactivate();
		}
	}
	
	void HandleBoostActive()
	{
		float boostTime = Time.time - m_lastBoostStartTime;
		
		m_currentBoostValue = m_startBoostValue + boostTime/10f;
		
		m_currentBoostValue = Mathf.Clamp(m_currentBoostValue, m_startBoostValue, m_maxBoostValue);
		
		m_currentBoostReserve -= m_boostDepletionSpeed * Time.deltaTime;
		
		if(m_currentBoostReserve <= 0)
		{
			m_currentBoostReserve = 0f;
			
			SetBoostState(false);
		}
				
		// check for afterburner
		//print((m_boostReplenishSpeed * m_boostComboMultiplier)+" "+m_boostDepletionSpeed);
		if((m_boostReplenishSpeed * m_boostComboMultiplier) > m_boostDepletionSpeed)
		{
			m_currentBoostValue = m_afterburnerBoostValue;
		}
	}
	
	void HandleBoostNotActive()
	{
		if(m_currentBoostValue - m_startBoostValue > 0.001f)
		{
			m_currentBoostValue = Mathf.Lerp(m_currentBoostValue, m_startBoostValue, 3f*Time.deltaTime);
		}
		else m_currentBoostValue = m_startBoostValue;
	}
	
	public bool CanBeActivated()
	{
		bool ok = true;
		
		ok &= m_currentBoostReserve > m_minBoostForActivation;
		
		return ok;
	}
	
	
	/////
	///
	///
	public float boostReserve{
		get{
			return m_currentBoostReserve;
		}
	}
	
	public float boostValue{
		get{
			return m_currentBoostValue;
		}
	}
	
	public float normalizedBoostValue{
		get{
			return (m_currentBoostValue-m_startBoostValue)/(m_maxBoostValue-m_startBoostValue);
		}
	}
	
	public float normalizedBoostReserve{
		get{
			return m_currentBoostReserve/m_maxBoostReserve;
		}
	}
	
	public bool isActive{
		get{
			return m_boostActive;
		}
	}
}
