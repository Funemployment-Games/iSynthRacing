using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Player))]
public class PlayerCollision : MonoBehaviour
{
	Player m_player;
	
	public float m_damageTime = 0.6f;
	
	public float m_boostTime = 0.3f;
	
	public Color m_damageColor = Color.yellow;
	
	public Color m_boostColor = Color.green;
	
	Color m_origColor;
	
	float m_damageStartTime;
	
	float m_boostStartTime;
	
	bool m_damageOn = false;
	
	bool m_boostOn = false;
	
	GameObject m_obstacle = null;
	
	// force applied to the ship when hitting certain kind of obstacles (e.g. repel from the wall
	Vector3 m_obstacleForce;
	
	// time to apply the obstacle force
	float m_obstacleForceTime;
	
	// are we currently applying force
	bool m_applyObstacleForce = false;
	
	Renderer m_renderer;
	

	// Use this for initialization
	void Start ()
	{
		m_renderer = (Renderer) GameObject.Find(gameObject.name+"/Geometry").GetComponent(typeof(Renderer));
		
		m_player = (Player) GetComponent(typeof(Player));
		
		m_origColor = m_renderer.material.color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_damageOn)
		{
			if(Time.time > m_damageStartTime + m_damageTime)
			{
				m_damageOn = false;
				ChangeColor(m_origColor);
				m_player.EnableControls();
			}
		}
		
		if(m_boostOn)
		{
			if(Time.time > m_boostStartTime + m_boostTime)
			{
				m_boostOn = false;
				ChangeColor(m_origColor);
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		//print("pickup ");
		if(GameManager.gameState != GameState.Running)
		{
			return;
		}
		
		if(GameManager.currentSpeedLevel == 9)
		{
			return;
		}
		
//		print("Trigger enter "+other.gameObject.name+" "+other.gameObject.layer +" "+LayerMask.NameToLayer("Obstacle"));
		if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
		{
			if(m_damageOn)
			{
				return;
			}
			
			m_damageStartTime = Time.time;
			
			m_damageOn = true;
			
			m_boostOn = false;
			
			ChangeColor(m_damageColor);
			
			m_player.DisableControls();
			
			ComboSystem.instance.ObstacleHit();
			
			GameManager.OnObstacleHit();
			
			if(m_player.boostComponent.isActive)
				m_player.boostComponent.SetBoostState(false);
			
			if(m_player.retroBoostComponent.isActive)
				m_player.retroBoostComponent.Deactivate();
		}
		
		if(other.gameObject.layer == LayerMask.NameToLayer("Pickup"))
		{
			if(m_damageOn)
			{
				m_damageOn = false;
				m_player.EnableControls();
			}
			
			m_boostOn = true;
			m_boostStartTime = Time.time;
			ChangeColor(m_boostColor);
		
			m_player.OnPickupPicked();
			
			SoundManager.PlaySoundEvent(SoundEvent.PickupPicked, transform.position);
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("BonusWalls"))
		{
			print("Bonus STAY "+other.gameObject.name+" "+Time.time);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
	//	print("Trigger exit "+other.gameObject.name);
		if(other.gameObject.layer == LayerMask.NameToLayer("BonusWalls"))
		{
			
		}
	}
	
	void ChangeColor(Color newCol)
	{
		m_renderer.material.color = newCol;
	}
	
	public bool pickedUpPickup{
		get{
			return m_boostOn;
		}
	}
}
