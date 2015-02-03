using UnityEngine;
using System.Collections;

public class RetroBoostButton : MonoBehaviour
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
		if(Input.GetKeyDown("q"))
			OnMouseDown();
		
		if(Input.GetKeyUp("q"))
			OnMouseUp();
	}
	
	void OnMouseDown()
	{
		if(GameManager.gameState != GameState.Running)
		{
			return;
		}
		m_player.retroBoostComponent.Activate();
		
		guiTexture.texture = m_pressedTexture;
	}
	
	void OnMouseUp()
	{
	//	m_player.retroBoostComponent.Deactivate();
		
		guiTexture.texture = m_normalTexture;
	}
}
