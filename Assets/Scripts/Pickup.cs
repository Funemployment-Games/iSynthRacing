using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
	public GameObject m_pickupEffect;
	
	BoxCollider m_collider;
	
	Vector3 m_colliderSize = new Vector3(1.2f, 1.2f, 5f);
	
	// Use this for initialization
	void Start ()
	{
		m_collider = (BoxCollider)GetComponentInChildren(typeof(BoxCollider));
		m_collider.gameObject.layer = LayerMask.NameToLayer("Pickup");
		m_collider.isTrigger = true;
		m_collider.size = m_colliderSize;
		
		if(m_pickupEffect)
		{
			m_pickupEffect.SetActiveRecursively(false);
		}
	}
	
	void OnTriggerEnter()
	{
		if(GameManager.gameState != GameState.Running)
			return;
		
		GameManager.OnPickupPicked();
	}
}
