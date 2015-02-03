using UnityEngine;
using System.Collections;

public class TrackSelectButtonManager : SelectButtonManager
{
	public GUITexture m_trackDetails;
	
	public GUIText m_topSpeedRecord;
	public GUIText m_distanceRecord;
	public GUIText m_fastestLapRecord;
	
	// Use this for initialization
	void Start ()
	{
		base.Start();
		
		SelectButton(1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update();
	}
	
	public override void SelectButton(int number)
	{
		base.SelectButton(number);
		
		if(m_selectedButton.trackDetailsTexture)
			m_trackDetails.texture = m_selectedButton.trackDetailsTexture;
		
		GameMetrics.selectedTrack = number;
		
		UpdateTrackRecords();
	}
	
	public int selectedTrackNumber{
		get{
			return m_selectedButtonIndex;
		}
	}
	
	void UpdateTrackRecords()
	{
		////////
		//records
		
		m_topSpeedRecord.text = ((int)GameMetrics.GetRecordSpeed()).ToString();
		m_topSpeedRecord.text = "Top Speed: "+ m_topSpeedRecord.text +" kph";
		
		m_fastestLapRecord.text = NumberFormat.FloatToString(GameMetrics.GetRecordLap(),2);
		m_fastestLapRecord.text = "Fastest Lap: "+ m_fastestLapRecord.text +" s";
		
		m_distanceRecord.text = NumberFormat.FloatToString(GameMetrics.GetRecordDistance(),2);
		m_distanceRecord.text = "Distance: "+ m_distanceRecord.text +" km";
	}
}
