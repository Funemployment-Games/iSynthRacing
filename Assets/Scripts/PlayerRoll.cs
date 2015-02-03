using UnityEngine;
using System.Collections;

public class PlayerRoll : MonoBehaviour
{
	bool m_active;
	
	float m_startTurnSpeed = 600f;
	
	float m_turnSpeed;
	
	int m_turnDirection = 1;
	
	Quaternion m_targetRot;
	
	Quaternion m_originalRot;
	
	Quaternion m_targetRotOver;
	
	float m_rotatedAngle;
	
	float m_bounceAngle = 20f;
	
	bool m_bounced = false;
	
	Player m_player;
	
	int m_step = 0;
	
	void Start()
	{
		m_player = (Player) GetComponent(typeof(Player));
		
		m_originalRot = transform.rotation;
	}
	
	public void Init()
	{
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
	
	public void Activate()
	{
		if(m_active) return;
		
		if(m_player.tiltComponent.isActive) return;
		
		m_active = true;
		
		m_turnDirection = m_player.xMoveDirection;
		
		m_rotatedAngle = 0;
		
		m_bounced = true;
		
		m_turnSpeed = m_startTurnSpeed;
		
		m_step = 1;
		
		m_player.OnRollActivate();
	}
	
	public void Deactivate()
	{
		m_active = false;
		
		m_player.OnRollDeactivate();
	}

	
	void HandleActive()
	{
		if(m_step == 1)
		{
			float rot = m_turnSpeed * 2f * Time.deltaTime * m_turnDirection;
			m_turnSpeed *= 1-(Time.deltaTime*5);
			transform.Rotate(Vector3.forward * rot, Space.Self);
			m_rotatedAngle += Mathf.Abs(rot);
			
			if(m_rotatedAngle >= 180f)
			{
				m_step = 2;
				m_turnSpeed = m_startTurnSpeed;
			}
			
		}
		else if (m_step == 2)
		{
			float rot = m_turnSpeed * 1.2f * Time.deltaTime * m_turnDirection;
			transform.Rotate(Vector3.forward * rot, Space.Self);
			m_rotatedAngle += Mathf.Abs(rot);
			
			if(m_rotatedAngle >= 360 + m_bounceAngle)
			{
				m_step = 3;
				m_rotatedAngle = 0;
				m_turnSpeed = m_startTurnSpeed;
			}
		}
		else if (m_step == 3)
		{
			float rot = m_turnSpeed * 0.3f * Time.deltaTime * -m_turnDirection;
			m_turnSpeed *= 1-(Time.deltaTime*2);
			transform.Rotate(Vector3.forward * rot, Space.Self);
			m_rotatedAngle += Mathf.Abs(rot);
			
			if(m_rotatedAngle >= m_bounceAngle)
			{
				Deactivate();
			}
		}
	}
	
	void HandleNotActive()
	{
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
