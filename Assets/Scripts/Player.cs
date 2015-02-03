using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public MeshRenderer m_geometry;
	
	Vector3 m_pathPosition = Vector3.zero;
	
	Vector3 m_targetOffset;
	
	Vector3 m_currentOffset;
	Vector3 m_prevOffset;
	
	Vector3 m_offsetDelta;
	
	public Vector2 m_maxOffsetFromPath = new Vector2(5f, 5f);
	
	bool m_controlsEnabled = true;
	
	public float m_accelerationBias = 1f;
	
	public float m_maxSwayAngle = 10f;
	public float m_maxTiltAngle = 10f;
	public float m_handlingSpeedBias = 1f;
	
	public float m_normalSpeed = 2f;
	public float m_obstacleSpeed = 0.7f;
	public float m_tiltingSpeed = 1.8f;
	public float m_rollingSpeedFactor = 1.5f;
	
	float m_accelSpeed = 1.6f;
	float m_deaccelSpeed = 10f;
	
	float m_currentSpeed = 0f;
	float m_targetSpeed = 0f;
	
	float m_speedBeforeHit = 0f;
	
	// value that will slow the ship down when turning
	float m_airDrag = 0f;
	
	// other player components
	PlayerBoost m_playerBoost;
	PlayerTilt m_playerTilt;
	PlayerRoll m_playerRoll;
	PlayerRetroBoost m_playerRetroBoost;
	PlayerCollision m_playerCollision;
	
	ChaseCamera m_camera;
	
	Vector3 m_targetRot;
	
	int m_xMoveDirection = 1;

	// Use this for initialization
	void Awake ()
	{
		m_geometry.material.mainTexture = MenuManager.GetShipTexture(GameMetrics.selectedShip, GameMetrics.selectedTexture);
	}
	
	void Start()
	{
		SoundManager.SetPlayerReference(this);
		MusicManager.SetPlayerReference(this);
	}
	
	public void Init()
	{

		m_playerBoost = (PlayerBoost) GetComponent(typeof(PlayerBoost));
		m_playerTilt = (PlayerTilt) GetComponent(typeof(PlayerTilt));
		m_playerRoll = (PlayerRoll) GetComponent(typeof(PlayerRoll));
		m_playerRetroBoost = (PlayerRetroBoost) GetComponent(typeof(PlayerRetroBoost));
		m_playerCollision  = (PlayerCollision) GetComponent(typeof(PlayerCollision));
		
		m_playerBoost.Init();
		m_playerTilt.Init();
		m_playerRoll.Init();
		m_playerRetroBoost.Init();
		
		m_currentOffset = new Vector3(0.5f, 0.5f, 0f);
		m_targetOffset = m_currentOffset;
		m_offsetDelta = Vector3.zero;
		m_prevOffset = Vector3.zero;
		
		m_targetSpeed = m_normalSpeed;
		m_currentSpeed = 0f;
		
		m_targetRot = Vector3.forward;
		
		SegmentManager.Init();
		
		m_pathPosition = SegmentManager.instance.GetNextPosition(0f);
		transform.position = m_pathPosition;
		
		transform.rotation = Quaternion.identity;
		
		if(!m_camera)
			m_camera = (ChaseCamera) GameObject.FindObjectOfType(typeof(ChaseCamera));
		
		m_camera.Init(this);
		
		m_camera.UpdateCamera();
	}
	
	// Update is called once per frame
	void Update ()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(this.name, gameObject.name, "Update");
		if(!GameManager.instance) return;
	//	pm.CallIsFinished();
		if(GameManager.gameState == GameState.Running)
		{
			UpdateSpeed();
			
			
			UpdatePosition();
			
			UpdateOffset();
			
		}
		
		if(GameManager.gameState == GameState.Results)
		{
			if(m_currentSpeed > 1.01f || m_currentSpeed < 0.99f)
			{
				m_currentSpeed = Mathf.Lerp(m_currentSpeed, 1f, 1f*Time.deltaTime);
			}
			
			UpdatePosition();
			UpdateOffset();
		}
		
		if(GameManager.gameState == GameState.Countdown)
		{
			UpdateOffset();
		}
	}
	
	void UpdateSpeed()
	{
		float m_changeSpeed;
		
		float speedGain = 0f;
		if(m_controlsEnabled)
		{
			if(currentSpeed < 400)
			{
				speedGain = 2f * m_accelerationBias;
			}
			else
			{
				speedGain = 0.5f * m_accelerationBias;
			}
			m_targetSpeed += speedGain/30f * Time.deltaTime;
		}
	
		if(m_targetSpeed > m_currentSpeed)
			m_changeSpeed = m_accelSpeed;
		else
			m_changeSpeed = m_deaccelSpeed;
		
		m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_targetSpeed, m_changeSpeed*Time.deltaTime);
		
		GameMetrics.UpdateSpeed(currentSpeed);
		//m_currentSpeed = m_targetSpeed;
		//print("speed updated "+m_currentSpeed);
	}
	
	void UpdatePosition()
	{
		float travelSpeed = m_currentSpeed;
		
		if(retroBoostComponent.isActive)
		{
			travelSpeed /=2;
		}
		
		m_pathPosition = SegmentManager.instance.GetNextPosition(travelSpeed);
	}
	
	public void UpdateOffset()
	{
		transform.position = m_pathPosition + m_currentOffset;
		
		m_offsetDelta = (m_currentOffset - m_prevOffset);
		
		m_prevOffset = m_currentOffset;
		
		m_camera.UpdateCamera();
		
		UpdateRotation2();
	}
	
	void UpdateTransparency()
	{
		
		float maxY = 3f;
		float minY = -1f;
		
		float midpoint = (maxY+minY)/2;
		float halflen = (maxY-minY)/2;
		float offsety = Mathf.Abs(m_currentOffset.y - midpoint);
		float perc = offsety/Mathf.Abs(halflen);
		perc*=perc;
		perc = Mathf.Clamp(perc, 0.2f, 1f);
		
		Color col = m_geometry.material.color;
		col.a = perc;
		m_geometry.material.color = col;
	//	print(perc);
	}
	
	void FixedUpdate()
	{
	}
	
	void UpdateRotation2()
	{
		if(rollComponent.isActive || tiltComponent.isActive)
		{
			return;
		}
		
		Vector3 delta = m_currentOffset - m_targetOffset;
		
		float maxdeltax = 20;
		float maxdeltay = 5;
		
		//dead zone
		float mindeltax = 3f;
		float mindeltay = 2f;
		
		if(Mathf.Abs(delta.x) < mindeltax)
			delta.x = 0;
		else
			delta.x -= mindeltax * Mathf.Sign(delta.x);
		if(Mathf.Abs(delta.y) < mindeltay)
			delta.y = 0;
		else
			delta.y -= mindeltay * Mathf.Sign(delta.y);
		
		float percx = Mathf.InverseLerp(-maxdeltax, maxdeltax, delta.x);
		float percy = Mathf.InverseLerp(-maxdeltay, maxdeltay, delta.y);
		
		m_targetRot.z = Mathf.Lerp(-m_maxSwayAngle, m_maxSwayAngle, percx);
		m_targetRot.x = Mathf.Lerp(-m_maxTiltAngle, m_maxTiltAngle, percy);
		m_targetRot.y = -Mathf.Lerp(-10, 10, percx);
		
		Quaternion targetRot = Quaternion.identity;
		
		targetRot.eulerAngles = m_targetRot;
		
		//transform.localRotation = targetRot;
		transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, 8f * Time.deltaTime);
	}
	/*
	void UpdateRotation()
	{
		if(rollComponent.isActive || tiltComponent.isActive)
		{
			return;
		}
		
		Quaternion targetRot = Quaternion.identity;
		
		targetRot.eulerAngles = m_targetRot;
		
		transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, 8f * Time.deltaTime);
		
		// check dead zone before updating target rotation
		float dead_zone_x = 0.4f;
		float dead_zone_y = 0.2f;
		
	//	print(m_positionDelta.x);
		Vector3 delta = m_offsetDelta;
		
		delta.x = Mathf.Clamp(delta.x, -3f, 3f);
		delta.y = Mathf.Clamp(delta.y, -1f, 1f);
		
		if(Mathf.Abs(delta.y) < dead_zone_y)
			delta.y = 0;
		else
			delta.y -= dead_zone_y * Mathf.Sign(delta.y);
		
		if(Mathf.Abs(delta.x) < dead_zone_x)
			delta.x = 0;
		else
			delta.x -= dead_zone_x * Mathf.Sign(delta.x);
		
		print(delta.y);
		
		float max = m_maxSwayAngle;
		m_targetRot = new Vector3(-delta.y*30, delta.x*1.5f, -delta.x*20f);
		m_targetRot *= m_swayAngleBias;
		
		m_targetRot.x = Mathf.Clamp(m_targetRot.x, -max, max);
		m_targetRot.y = Mathf.Clamp(m_targetRot.y, -max, max);
		m_targetRot.z = Mathf.Clamp(m_targetRot.z, -max, max);
	}*/
	
	public void SetOffset(Vector2 offsetPercent)
	{
		m_targetOffset.x = Mathf.Lerp(-m_maxOffsetFromPath.x, m_maxOffsetFromPath.x, offsetPercent.x);
		m_targetOffset.y = Mathf.Lerp(-m_maxOffsetFromPath.y, m_maxOffsetFromPath.y, offsetPercent.y);
		//print(Vector3.Distance(m_currentOffset, m_targetOffset));
		
		float lerpSpeed = 4f;
		
		lerpSpeed *= m_handlingSpeedBias;
		
		if(Vector3.Distance(m_currentOffset, m_targetOffset) > 0.2f)
			m_currentOffset =  Vector3.Lerp(m_currentOffset, m_targetOffset, lerpSpeed*Time.deltaTime);
		
		if(m_targetOffset.x > m_currentOffset.x)
			m_xMoveDirection = -1;
		else
			m_xMoveDirection = 1;
		
	//	m_currentOffset = m_targetOffset;
	}
	
	public void DisableControls()
	{
		SoundManager.PlaySoundEvent(SoundEvent.ObstacleHit, transform.position);
		
		m_controlsEnabled = false;
		
		m_speedBeforeHit = m_targetSpeed;
		
		if(m_speedBeforeHit < 1) m_speedBeforeHit = 1;
		
		m_targetSpeed = m_obstacleSpeed;
	}
	
	public void EnableControls()
	{
		m_controlsEnabled = true;
		
		m_targetSpeed = m_speedBeforeHit*0.8f;
	}
	
	public void OnTiltActivate()
	{

	}
	
	public void OnTiltDeactivate()
	{

	}
	
	public void OnRollActivate()
	{
		SoundManager.PlaySoundEvent(SoundEvent.BarrelRoll, transform.position);
		
		if(currentSpeed >= 450) return;
		
		if(collisionComponent.pickedUpPickup)
		{
			m_targetSpeed += 0.1f;
		}
		else
		{
			m_targetSpeed += 0.05f;
		}
	}
	
	public void OnRollDeactivate()
	{
		
	}
	
	public void OnRetroBoostActivate()
	{
		SoundManager.PlaySoundEvent(SoundEvent.RetroBoost, transform.position);
	}
	
	public void OnRetroBoostDeactivate()
	{
		
	}
	
	
	public void OnBoostActivate()
	{
		if(tiltComponent.isActive)
		{
			tiltComponent.Deactivate();
		}
	}
	
	public void OnBoostDeactivate()
	{
		
	}
	
	public void OnPickupPicked()
	{
		GameManager.AddTime(1f);
		
		float speedMod = 0f;
		
		if(currentSpeed >= 450) return;
		
		if(rollComponent.isActive)
		{
			speedMod = 0.2f;
		}
		else if(retroBoostComponent.isActive)
		{
			retroBoostComponent.Deactivate();
			speedMod = 0.2f;
		}
		else
		{
			speedMod = 0.1f;
		}
		
		if(currentSpeed < 400)
		{
			m_targetSpeed += speedMod;
		}
	}
	
	public void ForceTargetOffsetFromPath(Vector3 newOffset)
	{
	//	m_targetOffset = newOffset;
	}
	
	public Vector3 pathPosition{
		get{
			return m_pathPosition;
		}
	}
	
	public Vector3 offsetFromPath{
		get{
			return m_currentOffset;
		}
	}
	
	public PlayerBoost boostComponent{
		get{
			return m_playerBoost;
		}
	}
	
	public PlayerTilt tiltComponent{
		get{
			return m_playerTilt;
		}
	}
	
	public PlayerRoll rollComponent{
		get{
			return m_playerRoll;
		}
	}
	
	public PlayerRetroBoost retroBoostComponent{
		get{
			return m_playerRetroBoost;
		}
	}
	
	public PlayerCollision collisionComponent{
		get{
			return m_playerCollision;
		}
	}
	
	public float currentSpeed{
		get{
			return ((int)((realSpeed/m_normalSpeed)*100));
		}
	}
	
	public float normalizedSpeed100400{
		get{
			return Mathf.InverseLerp(100, 400, currentSpeed);
		}
	}
	
	public float normalizedSpeed0400{
		get{
			return Mathf.InverseLerp(0, 400, currentSpeed);
		}
	}
	
	float realSpeed{
		get{
			return (m_currentSpeed * boostComponent.boostValue) - m_airDrag;
		}
	}
	
	public int xMoveDirection{
		get{
			return m_xMoveDirection;
		}
	}
}
