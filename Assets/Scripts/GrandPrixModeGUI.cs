using UnityEngine;
using System.Collections;

public class GrandPrixModeGUI : MonoBehaviour
{
	Player m_player;
	
	public GUIText m_speedText;
	public GUIText m_comboCountText;
	
	public BoostBar m_boostBar;
	
	public GUIText m_lapTime;

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
		m_speedText.text = m_player.currentSpeed.ToString()+"km/h";
		m_comboCountText.text = "combo: "+ComboSystem.instance.currentComboCount.ToString();
		m_lapTime.text = "lap: "+GameManager.lapTime+"s";
		
		m_boostBar.SetBoostBarPercent(m_player.boostComponent.normalizedBoostReserve);
		
		if(m_boostBar.inUse != m_player.boostComponent.isActive)
			m_boostBar.SetInUse(m_player.boostComponent.isActive);
	}
}
