using UnityEngine;
using System.Collections;

public class GhostManager : MonoBehaviour
{

	////// singleton
	private static GhostManager m_instance;
	
	public static GhostManager instance
	{
		get
		{
			if( m_instance == null )
				m_instance = (GhostManager)FindObjectOfType(typeof(GhostManager));
			
			if(m_instance == null)
			{
				GameObject go = new GameObject();
				m_instance = (GhostManager)go.AddComponent(typeof(GhostManager));
			}
			
			if(m_instance == null)
			{
				Debug.LogError("Can't find singleton class");
			}
			return m_instance;
		}
	}
	
	///////
	/// ///
	///
	///
	///
	///
	///
	///
	
	GhostData[] writeData;
	GhostData[] readData;
	
	int currentWriteIndex;
	int currentReadIndex;
	
	int datasize;
	
	public static void Init()
	{
		instance.datasize = 20 * 80; //20fps, 80 secs
		instance.readData = instance.writeData;
		instance.writeData = new GhostData[instance.datasize];
		instance.currentWriteIndex = 0;
	}
	
	public static void SetNextPosition(Vector3 pos, Quaternion rot)
	{
		int i = instance.currentWriteIndex;
		
		if( i > instance.datasize)
		{
			Debug.LogError("data overflow");
			return;
		}
		
		GhostData d = new GhostData();
		d.Init(pos, rot);
		instance.writeData[i] = d;
		
		instance.currentWriteIndex++;
	}
	
	public static bool GetNextPosition(out Vector3 pos, out Quaternion rot)
	{
		int i = instance.currentReadIndex;
		
		if( i > instance.datasize)
		{
			Debug.LogError("read overflow");
			pos = Vector3.zero;
			rot = Quaternion.identity;
			return false;
		}
		
		if(instance.readData[i] == null)
		{
			pos = Vector3.zero;
			rot = Quaternion.identity;
			return false;
		}
		
		pos = instance.readData[i].pos;
		rot = instance.readData[i].rot;
		
		instance.currentReadIndex++;
		
		return true;
	}
	
	public static void GetStartPosition(out Vector3 pos, out Quaternion rot)
	{
		if(instance.readData[0] == null)
		{
			pos = Vector3.zero;
			rot = Quaternion.identity;
			return;
		}
		
		pos = instance.readData[0].pos;
		rot = instance.readData[0].rot;
	}
}

public class GhostData
{
	public void Init(Vector3 p, Quaternion r)
	{
		pos = p;
		rot = r;
	}
	
	public Vector3 pos;
	public Quaternion rot;
}
