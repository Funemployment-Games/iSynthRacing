using UnityEngine;
using System.Collections;

public class AchievementBar : MonoBehaviour
{
	public GUITexture[] m_checkBoxes;

	// Use this for initialization
	void Start ()
	{
		// menu
		if(GameManager.instance == null)
		{
			int a = GameMetrics.GetAchievement(1, (int)GameMetrics.activeGameMode);
			
			SetAchievementCount(a);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		int a = GameMetrics.GetAchievement(GameMetrics.selectedTrack, (int)GameMetrics.activeGameMode);
		SetAchievementCount(a);
	}
	
	public void SetAchievementCount(int count)
	{
		if(count < 0 || count > 5) return;
		
		for (int i = 0; i < m_checkBoxes.Length; i++)
		{
			GUITexture m_checkBoxe = m_checkBoxes[i];
		//	print("sjds "+i);
			if(i < count)
			{
				m_checkBoxe.enabled = true;
			}
			else
			{
				m_checkBoxe.enabled = false;
			}
		}
	}
}
