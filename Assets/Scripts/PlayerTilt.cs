using UnityEngine;
using System.Collections;

public class PlayerTilt : MonoBehaviour
{
	bool m_active;
	
	float m_turnSpeed = 5f;
	
	int m_turnDirection = 1;
	
	Quaternion m_targetRot;
	
	Quaternion m_originalRot;
	
	Quaternion m_targetRotOver;
	
	bool m_bounced = false;
	
	Player m_player;
	
	float m_activationTime;
	float m_minDuration = 1f;
	
	void Start()
	{
		m_player = (Player) GetComponent(typeof(Player));
		
		m_originalRot = transform.rotation;
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
	}
	
	public void Activate()
	{
		m_active = true;
		
		m_turnDirection = -m_player.xMoveDirection;
		
		m_targetRot = Quaternion.LookRotation(Vector3.forward, Vector3.right*m_turnDirection);
		
		m_targetRotOver = Quaternion.LookRotation(Vector3.forward, (Vector3.right-new Vector3(0,0.3f*m_turnDirection,0) )*m_turnDirection);
		
		m_bounced = false;
		
		m_player.OnTiltActivate();
		
		m_activationTime = Time.time;
	}
	
	void DeactivateNow()
	{
		m_active = false;
		
		m_player.OnTiltDeactivate();
	}
	
	public void Deactivate()
	{
		float pressedTime = (Time.time - m_activationTime);
		if(pressedTime < m_minDuration)
		{
			Invoke("DeactivateNow", m_minDuration-pressedTime);
		}
		else
		{
			DeactivateNow();
		}
	}

	
	void HandleActive()
	{
		if(!m_bounced)
		{
			transform.localRotation = Quaternion.Lerp(transform.rotation, m_targetRotOver, m_turnSpeed * 2 * Time.deltaTime);
			if(Vector3.Distance(transform.localRotation.eulerAngles, m_targetRotOver.eulerAngles) < 5f)
			{
				m_bounced = true;
			}
		}
		else
		{
			transform.localRotation = Quaternion.Lerp(transform.rotation, m_targetRot, m_turnSpeed * Time.deltaTime);
		}
	}
	
	void HandleNotActive()
	{
		if(!m_player.rollComponent.isActive)
		{
	//		transform.localRotation = Quaternion.Lerp(transform.rotation, m_originalRot, m_turnSpeed * Time.deltaTime);
		}
	}
	
	public bool CanBeActivated()
	{
		bool ok = true;
		
		return ok;
	}
	
	public bool isActive{
		get{
			return m_active;
		}
	}
}
