using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour
{
	Player m_player;
	
	public GUIText m_speedText;
	public GUIText m_comboCountText;
	public GUIText m_boostText;
	
	public BoostBar m_boostBar;

	// Use this for initialization
	void Start ()
	{
		m_player = (Player) FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_speedText.text = "Speed: "+m_player.currentSpeed.ToString();
		m_comboCountText.text = "Combo count: "+ComboSystem.instance.currentComboCount.ToString();
		m_boostText.text = "Boost: "+m_player.boostComponent.boostValue.ToString();
		
		if(m_boostBar)
		{
			m_boostBar.SetBoostBarPercent(m_player.boostComponent.normalizedBoostReserve);
			
			m_boostBar.SetInUse(m_player.boostComponent.isActive);
		}
		
	}
}
