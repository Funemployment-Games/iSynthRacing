using System.Collections;
using UnityEngine;

public class UtilAverage
{
	// the array of things to average
	ArrayList m_list;
	// the current index in the circular array to overwrite with the new value
	int m_index;
	
	public UtilAverage(int size, Vector3 baseValue)
	{
		m_list = new ArrayList(size);
		for(int i=0;i<size;i++) m_list.Add(baseValue);
	}
	
	public UtilAverage(int size, Vector2 baseValue)
	{
		m_list = new ArrayList(size);
		for(int i=0;i<size;i++) m_list.Add(baseValue);
	}
	
	public UtilAverage(int size, float baseValue)
	{
		m_list = new ArrayList(size);
		for(int i=0;i<size;i++) m_list.Add(baseValue);
	}
	
	public Vector3 Update(Vector3 newValue)
	{
		m_list[m_index] = newValue;
		
		Vector3 avg = Vector3.zero;
		foreach (Vector3 v in m_list)
			avg += v;
		
		if(++m_index >= m_list.Count) m_index = 0;
		
		return avg/m_list.Count;
	}
	
	public Vector2 Update(Vector2 newValue)
	{
		m_list[m_index] = newValue;
		
		Vector2 avg = Vector2.zero;
		foreach (Vector2 v in m_list)
			avg += v;
		
		if(++m_index >= m_list.Count) m_index = 0;
		
		return avg/m_list.Count;
	}
	
	public float Update(float newValue)
	{
		m_list[m_index] = newValue;
		
		float avg = 0f;
		foreach (float v in m_list)
			avg += v;
		
		if(++m_index >= m_list.Count) m_index = 0;
		
		return avg/m_list.Count;
	}
}
