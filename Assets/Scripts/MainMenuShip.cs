using UnityEngine;
using System.Collections;

public class MainMenuShip : MonoBehaviour
{
	public Renderer[] m_shipModels;
	Renderer m_activeShipModel;
	
	bool m_movingOut = false;
	bool m_movingIn = false;
	
	int m_moveDirection = 1;
	
	Vector3 m_originalPos;
	Vector3 m_targetPos;
	Vector3 m_currentPos;

	float m_moveOffset = 18f;
	float m_moveSpeed = 5.6f;
	float m_minDistOut = 0.2f;
	float m_minDistIn = 0.01f;

	// Use this for initialization
	void Start ()
	{
		m_originalPos = transform.position;
		m_currentPos = m_originalPos;
		m_targetPos = m_currentPos;
		
		/*
		foreach (Renderer m_shipModel in m_shipModels)
		{
			Color col = m_shipModel.material.color;
			col.a = 1f;
			m_shipModel.material.color = col;
			m_shipModel.material.shader = Shader.Find ("Diffuse");
		}
		*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!isInTransition) return;
		
		m_currentPos = Vector3.Lerp(m_currentPos, m_targetPos, m_moveSpeed*Time.deltaTime);
		
		float minDist = m_movingIn ? m_minDistIn : m_minDistOut;
			
		if(Vector3.Distance(m_currentPos, m_targetPos) < minDist)
		{
			if(m_movingOut)
			{
				SetSelectedShip(GameMetrics.selectedShip);
				SetMoveIn();
			}
			else if(m_movingIn)
			{
				m_movingIn = false;
			}
		}
		else
		{
			transform.position = m_currentPos;
		}
	}
	
	public void SetNextTexture()
	{
		GameMetrics.selectedTexture++;
		
		SetShipTexture();
	}
	
	public void SetShipTexture()
	{
		
		m_activeShipModel.material.mainTexture = MenuManager.GetShipTexture(GameMetrics.selectedShip, GameMetrics.selectedTexture);
	}
	
	public void SetNextShip()
	{
		m_moveDirection = -1;
		
		SetMoveOut();
	}
	
	void SetMoveOut()
	{
		m_movingIn = false;
		m_movingOut = true;
		
		m_targetPos = m_originalPos + Vector3.right*m_moveOffset*m_moveDirection;
	}
	
	void SetMoveIn()
	{
		m_currentPos = m_originalPos + Vector3.right*m_moveOffset*m_moveDirection;
		m_targetPos = m_originalPos;
			
		m_movingOut = false;
		m_movingIn = true;
	}
	
	public void SetSelectedShip(int index)
	{
		print("select ship "+index);
		for (int i = 0; i < m_shipModels.Length; i++)
		{
			if(index != i)
			{
				m_shipModels[i].gameObject.SetActiveRecursively(false);
				m_shipModels[i].enabled = false;
			}
			else
			{
				m_shipModels[i].gameObject.SetActiveRecursively(true);
				m_shipModels[i].enabled = true;
				m_activeShipModel = m_shipModels[i];
			}
		}
	}
	
	public bool isInTransition{
		get{
			return m_movingIn || m_movingOut;
		}
	}
	
	public int shipCount{
		get{
			return m_shipModels.Length;
		}
	}

}


