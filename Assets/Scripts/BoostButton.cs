using UnityEngine;
using System.Collections;

public class BoostButton : MonoBehaviour
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
		if(Input.GetKeyDown("b"))
			OnMouseDown();
		
		if(Input.GetKeyUp("b"))
			OnMouseUp();
	}
	
	void OnMouseDown()
	{
		if(GameManager.gameState != GameState.Running)
		{
			return;
		}
		m_player.boostComponent.SetBoostState(true);
		
		guiTexture.texture = m_pressedTexture;
	}
	
	void OnMouseUp()
	{
		m_player.boostComponent.SetBoostState(false);
		
		guiTexture.texture = m_normalTexture;
	}
}
