using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Segment : MonoBehaviour
{
	public Transform m_startPoint;
	public Transform m_endPoint;

	float m_zLength;
	
	ArrayList m_obstacles;
	
	public ObstacleEntry[] m_obstaclePrefabs;
	
	// DEBUG
		 bool m_disableObstacles = false;
	
	// Use this for initialization
	public void InitObstacles ()
	{
		m_obstacles = new ArrayList();
		foreach (Obstacle segment in gameObject.GetComponentsInChildren(typeof(Obstacle)))
		{
			m_obstacles.Add(segment);
		}
		
		m_zLength = Mathf.Abs(m_endPoint.position.z - m_startPoint.position.z);
		
		foreach (Obstacle obstacle in m_obstacles)
		{
			obstacle.percentAlongSegment = Mathf.Abs(obstacle.transform.position.z-m_startPoint.transform.position.z)/m_zLength;
			obstacle.percentAlongSegment *= 100;

			if(m_disableObstacles) obstacle.gameObject.SetActiveRecursively(false);
		}
		
		SortObstacles();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!Application.isPlaying)
		{
			UpdateEditor();
		}
	}
	
	public Vector3 GetPathPositionAt(float percent)
	{
		// linear interpolation between start and end for now, works only for straight segments
		
		Vector3 p = Vector3.Lerp(m_startPoint.position, m_endPoint.position, percent/100f);

		return p;
	}
	
	public void SetPos(Vector3 startPos)
	{
		Vector3 newPos = startPos - m_startPoint.localPosition*transform.localScale.x;
		
		transform.position = newPos;
	}
	
	public void HideObstacles()
	{
		foreach (GameObject m_obstacle in m_obstacles)
		{
			m_obstacle.active = false;
		}
	}
	
		
	void SortObstacles()
	{
		int i;
		int j;
		Obstacle temp;
			
		for( i = (m_obstacles.Count - 1); i >= 0; i-- )
		{
			for( j = 1; j <= i; j++ )
			{
				Obstacle o1 = (Obstacle) m_obstacles[j-1];
				Obstacle o2 = (Obstacle) m_obstacles[j];
				
				if( o1.percentAlongSegment > o2.percentAlongSegment )
				{
					temp = (Obstacle)m_obstacles[j-1];
					m_obstacles[j-1] = m_obstacles[j];
					m_obstacles[j] = temp;
				}
			}
		}
	}
	
	public Obstacle GetNextObstacle(float playerPerc)
	{
		
		for(int i=0; i<m_obstacles.Count; i++)
		{
			if(playerPerc < ((Obstacle)m_obstacles[i]).percentAlongSegment)
				return ((Obstacle)m_obstacles[i]);
		}
		
		return null;
	}
	
	public float length
	{
		get{
			return (m_startPoint.position - m_endPoint.position).magnitude;
		}
	}
	
	public bool m_update = false;
	
	void UpdateEditor()
	{
		if(m_update)
		{
			DestroyTempObstacles();
			
			PlaceObstacles();
		}
	}
	
	public void DestroyTempObstacles()
	{
		if(m_obstacles != null && m_obstacles.Count > 0)
		{
			foreach (Obstacle obstacle in m_obstacles)
			{
				DestroyImmediate(obstacle.gameObject);
			}
		}
		Component[] obs = GetComponentsInChildren(typeof(Obstacle), true);
		foreach (Component ob in obs)
		{
			DestroyImmediate(ob.gameObject);
		}
	}
	
	public static string obstacleHolderName = "Obstacles";
	
	public void PlaceObstacles()
	{
		// create holder gameobject if it doesn't exist
		m_obstacleHolder = GameObject.Find(gameObject.name+"/"+obstacleHolderName);
		if(m_obstacleHolder == null)
		{
			m_obstacleHolder = new GameObject(obstacleHolderName);
			m_obstacleHolder.transform.parent = transform;
			m_obstacleHolder.transform.position = transform.position;
		}
		
		m_obstacles = new ArrayList();
		for(int i = 0; i < m_obstaclePrefabs.Length; i++)
		{
			GameObject obstacleGO = (GameObject) Instantiate(m_obstaclePrefabs[i].prefab.gameObject, transform.position, Quaternion.identity);
			obstacleGO.transform.parent = m_obstacleHolder.transform;
			obstacleGO.transform.localPosition = m_obstaclePrefabs[i].position;
			
			m_obstacles.Add(obstacleGO.GetComponent(typeof(Obstacle)));
		}
	}
	
	GameObject m_obstacleHolder;
	
	GameObject m_geometryHolder;
	
	public void Create()
	{
		m_geometryHolder = new GameObject("Geometry");
		m_geometryHolder.transform.parent = transform;
		m_geometryHolder.transform.position = transform.position;
		
		GameObject markers = new GameObject("Markers");
		markers.transform.parent = transform;
		markers.transform.position = transform.position;
		
		m_obstacleHolder = new GameObject(obstacleHolderName);
		m_obstacleHolder.transform.parent = transform;
		m_obstacleHolder.transform.position = transform.position;
		
		m_startPoint = new GameObject("1-Start").transform;
		m_startPoint.transform.parent = markers.transform;
		m_endPoint = new GameObject("2-End").transform;
		m_endPoint.transform.parent = markers.transform;
		
		m_obstacles = new ArrayList();
	}
	
	public void SetStartPoint(Vector3 startPoint)
	{
		m_startPoint.transform.localPosition = startPoint;
	}
	public void SetEndPoint(Vector3 endPoint)
	{
		m_endPoint.transform.localPosition = endPoint;
	}
	
	public Vector3 GetStartPoint()
	{
		return m_startPoint.position;
	}
	
	public Vector3 GetEndPoint()
	{
		return m_endPoint.position;
	}
	
	public void AddGeometry(GameObject newObject, Vector3 localPos)
	{
		newObject.transform.parent = m_geometryHolder.transform;
		
		newObject.transform.localPosition = localPos;
	}
	
	public void AddObstacle(GameObject newObstacleGO, Vector3 localPos)
	{
		newObstacleGO.transform.parent = m_obstacleHolder.transform;
		
		newObstacleGO.transform.localPosition = localPos;
		
		Obstacle newObstacle = (Obstacle) newObstacleGO.GetComponent(typeof(Obstacle));
		
		m_obstacles.Add(newObstacle);

		/*
		m_obstaclePrefabs = new ObstacleEntry[m_obstacles.Count];
		for(int i = 0; i < m_obstaclePrefabs.Length; i++)
		{
			Obstacle obstacle = (Obstacle) m_obstacles[i];
			m_obstaclePrefabs[i] = new ObstacleEntry(obstacle, obstacle.transform.localPosition);
		}
		*/
	}
	
	public Renderer[] GetAllGeometry()
	{
		Component[] rends = GetComponentsInChildren(typeof(Renderer));
		
		ArrayList ret = new ArrayList();
		for (int i = 0; i < rends.Length; i++)
		{
			if(rends[i].gameObject.GetComponent(typeof(ObstaclePart)) != null) continue;
			ret.Add((Renderer)rends[i]);
		}
		
		return ((Renderer[])ret.ToArray(typeof(Renderer)));
	}
}

[System.Serializable]
public class ObstacleEntry
{
    public Obstacle prefab;
    public Vector3 position;
	
	public ObstacleEntry (Obstacle pref, Vector3 pos)
	{
		prefab = pref;
		position = pos;
	}
}
