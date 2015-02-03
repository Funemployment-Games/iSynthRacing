using UnityEngine;
using System.Collections;

public class EndlessModeResultsScreen : MonoBehaviour
{
	public GUIText m_totalTime;
	public GUIText m_totalDistance;
	public GUIText m_maxCombo;

	// Use this for initialization
	void Start ()
	{
		m_totalTime.text = NumberFormat.FloatToString(GameMetrics.totalTime, 2);
		
		m_maxCombo.text = GameMetrics.maxCombo.ToString();
		
		m_totalDistance.text = NumberFormat.FloatToString(GameMetrics.totalDistance, 2);
		
		int new_achievement =GameMetrics.SetAchievement(GameMetrics.selectedTrack, (int)GameMetrics.activeGameMode, GameMetrics.totalDistance);
		
		
		AchievementBar achiev_bar = (AchievementBar)GetComponentInChildren(typeof(AchievementBar));
		if(achiev_bar)
			achiev_bar.SetAchievementCount(new_achievement);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class NumberFormat
{
	public static string FloatToString(float number, int decimalCount)
	{
		int factor = (int) Mathf.Pow(10, decimalCount);
		int int_num = ((int)(number*factor));
		float num = ((float)int_num)/(float)factor;
		
		string format = "00.";
		for (int i = 0; i < decimalCount; i++)
		{
			format+="0";
		}
		return num.ToString(format);
	}
}
