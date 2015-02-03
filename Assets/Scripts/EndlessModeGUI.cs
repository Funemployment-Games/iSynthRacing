using UnityEngine;
using System.Collections;

public class EndlessModeGUI : MonoBehaviour
{
	Player m_player;
	
	public GUIText m_distanceText;
	
	public GUIText m_remainingLivesText;
	
	
	public GUIText m_comboCountText;
	
	BoostBar m_boostBar;

	// Use this for initialization
	void Start ()
	{
		m_player = (Player) FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(GameManager.gameState == GameState.Results)
		{
			return;
		}
		m_distanceText.text = "dist: "+NumberFormat.FloatToString(GameMetrics.totalDistance, 2)+"km";
		
		m_remainingLivesText.text = "shield: "+GameManager.remainingLives;
		if(GameManager.remainingLives == 1)
		{
			m_remainingLivesText.material.color = Color.red;
		}
		
		m_comboCountText.text = "combo "+ComboSystem.instance.currentComboCount.ToString();
		
	}
}
