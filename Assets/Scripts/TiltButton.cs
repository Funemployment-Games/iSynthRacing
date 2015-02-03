using UnityEngine;
using System.Collections;

public class TiltButton : MonoBehaviour
{
	Player m_player;

	public Texture2D m_normalTexture;
	public Texture2D m_pressedTexture;
	// Use this for initialization
	void Start ()
	{
		m_player = (Player) GameObject.FindObjectOfType(typeof(Player));

		guiTexture.texture = m_normalTexture;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown("t"))
			OnMouseDown();
		
		if(Input.GetKeyUp("t"))
			OnMouseUp();
	}
	
	void OnMouseDown()
	{
		if(GameManager.gameState != GameState.Running)
		{
			return;
		}
		if(!m_player.tiltComponent.isActive)
		{
			m_player.tiltComponent.Activate();
		}
		
		guiTexture.texture = m_pressedTexture;
	}
	
	void OnMouseUp()
	{
		m_player.tiltComponent.Deactivate();
		
		guiTexture.texture = m_normalTexture;
	}
}
