using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{
	public enum LookAxis {X, Y, Z};
	
	public LookAxis m_lookAxis = LookAxis.Z;
	
	public bool m_targetIsCamera;
	
	public Transform m_target;
	
	// how much to rotate along the look axis
	public float m_rotationSpeed;
	

	// Use this for initialization
	void Start ()
	{
		if(m_targetIsCamera)
			m_target = Camera.main.transform;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if(m_target != null)
		{
	//		transform.LookAt(m_target);
			transform.rotation = Quaternion.LookRotation(m_target.position-transform.position, transform.up);
			Vector3 rotAxis = Vector3.forward;
						
			if(m_lookAxis == LookAxis.Y)
			{
				transform.Rotate(Vector3.right, 90);
				rotAxis = Vector3.up;
			}
			
			else if(m_lookAxis == LookAxis.X)
			{
				transform.Rotate(Vector3.up, -90);
				rotAxis = Vector3.right;
			}
			
			if(m_rotationSpeed > 0)
				transform.Rotate(rotAxis*m_rotationSpeed*Time.deltaTime);
		}
	}
	
	void SetTarget(Transform newTarget)
	{
		if(!m_targetIsCamera)
			m_target = newTarget;
	}
}
