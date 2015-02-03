using UnityEngine;
using System.Collections;

public class GrandPrixModeResultsScreen : MonoBehaviour
{
	public GUIText m_totalTime;
	public GUIText m_averageLap;
	public GUIText m_averageSpeed;
//	public GUIText m_topSpeed;

	// Use this for initialization
	void Start ()
	{
		m_totalTime.text = NumberFormat.FloatToString(GameMetrics.totalTime,2);
		
		m_averageLap.text = NumberFormat.FloatToString(GameMetrics.averageLap, 2);
		
		m_averageSpeed.text = NumberFormat.FloatToString(GameMetrics.avgSpeed,2);
		
	//	m_topSpeed.text = GameMetrics.maxSpeed.ToString();
	
		int new_achievement = GameMetrics.SetAchievement(GameMetrics.selectedTrack, (int)GameMetrics.activeGameMode, GameMetrics.averageLap);
		
		AchievementBar achiev_bar = (AchievementBar)GetComponentInChildren(typeof(AchievementBar));
		if(achiev_bar)
			achiev_bar.SetAchievementCount(new_achievement);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
