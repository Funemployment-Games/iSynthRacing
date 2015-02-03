using UnityEngine;
using System.Collections;

public class ChaseCamera : MonoBehaviour
{
	Player m_player;
	
	Vector3 m_pathPosition;
	
	public Vector2 m_offsetPercentFromPlayer = new Vector2(1,1);
	
	public Vector3 m_constantOffset = new Vector3(0f, 0f, -10f);
	
	public Vector3 m_maxOffset = new Vector3(100f, 100f, 100f);
	
	Vector3 m_targetOffset;
	
	Vector3 m_currentOffset;
	
	// lens effect when using boost vars
	float m_normalFOV = 60f;
	float m_minFOV = 50f;
	float m_maxFOV = 80f;

	// Use this for initialization
	public void Init (Player player)
	{
		m_player = player;
	}
	
	// Update is called once per frame
	public void UpdateCamera ()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "Update");

		m_pathPosition = m_player.pathPosition;
		
		// constant offsets
		m_pathPosition += m_constantOffset;
		
		// X and Y offsets
		m_targetOffset.x = m_player.offsetFromPath.x * m_offsetPercentFromPlayer.x;
		m_targetOffset.x  = Mathf.Clamp(m_targetOffset.x , -m_maxOffset.x, m_maxOffset.x);
		
		m_targetOffset.y = m_player.offsetFromPath.y * m_offsetPercentFromPlayer.y;
		m_targetOffset.y = Mathf.Clamp(m_targetOffset.y, -m_maxOffset.y, m_maxOffset.y);
		
		float m_adjustSpeed = 10f;
		m_currentOffset = m_targetOffset;
		
		m_pathPosition += Vector3.right * m_currentOffset.x;
		m_pathPosition += Vector3.up * m_currentOffset.y;
	
		transform.position = m_pathPosition;
		
		float fov = m_normalFOV;
		
		float val = 0.1f;
		
		if(m_player.rollComponent.isActive)
		{
			fov = Mathf.Lerp(camera.fieldOfView, m_maxFOV+10, Time.deltaTime * 3f);
		}
		
		
		if(GameManager.gameMode == GameMode.Speed)
		{
			if(m_player.retroBoostComponent.isActive)
			{
				fov = Mathf.Lerp(camera.fieldOfView, m_normalFOV-10, 2f * Time.deltaTime);
			}
			else
			{
				if(m_player.collisionComponent.pickedUpPickup || m_player.rollComponent.isActive)
				{
					fov = Mathf.Lerp(camera.fieldOfView, camera.fieldOfView+2, 10f * Time.deltaTime);
				}
				else
				{
					float minFovSpeed = 100f;
					float maxFovSpeed = 300f;
					
					float speedPerc = Mathf.InverseLerp(minFovSpeed, maxFovSpeed, m_player.currentSpeed);
					
					float targetfov = Mathf.Lerp(m_minFOV, m_maxFOV, speedPerc);
					fov = Mathf.Lerp(camera.fieldOfView, targetfov, 2f * Time.deltaTime);
				}
			}
		}
		
		camera.fieldOfView = fov;
		
	//	pm.CallIsFinished();
	}
}
