using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class Track : MonoBehaviour
{
	public SegmentGroup[] m_segmentGroups;
	
	ArrayList m_segments;

	public bool m_update = false;
	
	bool m_convertToSegmentPrefab = false;
	
	ArrayList m_combinedObstacles;
	ArrayList m_combinedTunnels;
	public Material m_obstacleMaterial;
	public Material m_tunnelMaterial;
	
	void GetSegments()
	{
		m_segments = new ArrayList();
		foreach (Segment segment in gameObject.GetComponentsInChildren(typeof(Segment)))
		{
			m_segments.Add(segment);
		}
		
	}
	
	void Awake()
	{
		// combine child meshes
		
		// get references to all obstacle and tunnels combined meshes
		if(Application.isPlaying)
		{
			Component[] renderers = GetComponentsInChildren(typeof(Renderer));
			foreach (Renderer renderer in renderers)
			{
				if(renderer.material.name.StartsWith(m_obstacleMaterial.name))
				{
					AddObstacleCombinedMesh(renderer.gameObject);
				}
				
				if(renderer.material.name.StartsWith(m_tunnelMaterial.name))
				{
					AddTunnelCombinedMesh(renderer.gameObject);
				}
			}
		}
	}
	
	void Start()
	{
	}
	
	public void Init()
	{
		if(m_segments == null || m_segments.Count == 0)
		{
			GetSegments();
		}
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		if(!GameManager.instance) return;
		
		if(!Application.isPlaying)
		{
			UpdateEditor();
		}
	}

	Vector3 m_nextSegmentPosition;
	Quaternion m_nextSegmentDir;
	
	void UpdateEditor()
	{
		if(m_update)
		{
			DestroyTempSegments();
			
			BuildTrack();
			
			m_update = false;
		}
		
		if(m_convertToSegmentPrefab)
		{
			ConvertToSegmentPrefab();
			m_convertToSegmentPrefab = false;
		}
	}
	
	public void DestroyTempSegments()
	{
		if(m_segments != null && m_segments.Count > 0)
		{
			foreach (Segment m_segment in m_segments)
			{
				DestroyImmediate(m_segment.gameObject);
			}
		}
		
		Component[] segs = GetComponentsInChildren(typeof(Segment), true);
		foreach (Component seg in segs)
		{
			DestroyImmediate(seg.gameObject);
		}
	}
	
	string segmentHolderName = "Segments";
	
	public ArrayList BuildTrack()
	{
		// create holder gameobject if it doesn't exist
		
		GameObject segmentHolder = GameObject.Find(gameObject.name+"/"+segmentHolderName);
		if(segmentHolder == null)
		{
			segmentHolder = new GameObject(segmentHolderName);
			segmentHolder.transform.parent = transform;
			segmentHolder.transform.position = transform.position;
		}
		
		m_segments = new ArrayList();
		//	return;
		m_nextSegmentPosition = transform.position;
		m_nextSegmentDir = Quaternion.identity;
		int i = 0;
		foreach (SegmentGroup m_segmentGroup in m_segmentGroups)
		{
			if(m_segmentGroup.prefab == null) continue;
			for(int j = 0; j < m_segmentGroup.amount; j++)
			{
				GameObject newSegmentObj = (GameObject) Instantiate(m_segmentGroup.prefab.gameObject, Vector3.zero, m_nextSegmentDir);
				newSegmentObj.name = "Segment "+i;
				newSegmentObj.SetActiveRecursively(true);
				newSegmentObj.transform.parent = segmentHolder.transform;
				
				Segment newSegment = (Segment) newSegmentObj.GetComponent(typeof(Segment));
				
				newSegment.SetPos(m_nextSegmentPosition);
				
				m_nextSegmentPosition = newSegment.m_endPoint.position;
				m_nextSegmentDir = Quaternion.LookRotation(newSegment.m_endPoint.position-newSegment.m_startPoint.position);
				
				newSegment.InitObstacles();
				
				m_segments.Add(newSegment);
				i++;
			}
		}
		
		return m_segments;
	}
	
	public Renderer[] GetAllGeometry()
	{
		ArrayList rendList = new ArrayList();
		
		foreach (Segment segment in segments)
		{
			foreach (Renderer rend in segment.GetAllGeometry())
			{
				rendList.Add(rend);
			}
		}
		
		return ((Renderer[])rendList.ToArray(typeof(Renderer)));
	}
	
	public Segment[] segments{
		get{
			if(m_segments == null)  GetSegments();
			return (Segment[])m_segments.ToArray(typeof(Segment));
		}
	}
	
	public int segmentCount{
		get{
			return m_segments.Count;
		}
	}
	
	public void AddObstacleCombinedMesh(GameObject obstacleGroup)
	{
		if(m_combinedObstacles == null)
		{
			m_combinedObstacles = new ArrayList();
		}
//		print("add obstacl mesh "+m_combinedObstacles.Count);
		Renderer rend = (MeshRenderer)obstacleGroup.GetComponent(typeof(MeshRenderer));
		if(rend)
		{
			m_combinedObstacles.Add(rend);
		}
	}
	
	public void AddTunnelCombinedMesh(GameObject tunnels)
	{
		if(m_combinedTunnels == null)
		{
			m_combinedTunnels = new ArrayList();
		}
//		print("add tunnel mesh "+m_combinedTunnels.Count);
		Renderer rend = (MeshRenderer)tunnels.GetComponent(typeof(MeshRenderer));
		if(rend)
		{
			m_combinedTunnels.Add(rend);
		}
		
	}
	
	public void HideObstacles()
	{
		foreach (MeshRenderer obstacle in m_combinedObstacles)
		{
			obstacle.gameObject.active = false;
		}
	}
	
	public void UnhideObstacles()
	{
		foreach (MeshRenderer obstacle in m_combinedObstacles)
		{
			obstacle.gameObject.active = true;
		}
	}
	
	public void SetObstaclesColor(Color color)
	{
		if(m_combinedObstacles == null) return;
		foreach (MeshRenderer rend in m_combinedObstacles)
		{
			rend.material.color = color;
		}
	}
	
	public void SetTunnelColor(Color color)
	{
		if(m_combinedTunnels == null)
		{
			Debug.LogError("ERROR WITH TUNNEL COMBINED MESH");
			return;
		}
		
		foreach (MeshRenderer rend in m_combinedTunnels)
		{
			rend.material.color = color;
		}
	}
	
	
	///
	///
	///
	///
	///
	public static GameObject ConvertToSegmentPrefab(GameObject parentObject, Vector3 startPoint, Vector3 endPoint)
	{
		
		// create our new segment object
		GameObject newSegmentGO = new GameObject("NewSegment");
		Segment newSegment = (Segment) newSegmentGO.AddComponent(typeof(Segment));
		newSegment.Create();
		
		newSegment.transform.position += Vector3.up * 50;
		
//		string segmentPath = gameObject.name+"/"+segmentHolderName+"/"+segment.gameObject.name;
		
		//	GameObject geometry = GameObject.Find(segmentPath+"/Geometry");
		Component[] transforms = parentObject.GetComponentsInChildren(typeof(Renderer), true);
		Component[] obstacles = parentObject.GetComponentsInChildren(typeof(Obstacle), true);
		
		if(transforms.Length == 0 && obstacles.Length == 0) return null;
		
		foreach (Renderer tr in transforms)
		{
			if(tr.gameObject.GetComponent(typeof(ObstaclePart)) != null)
			{
				continue;
			}
			
			GameObject newTr = (GameObject) Instantiate(tr.gameObject);
			
			newSegment.AddGeometry(newTr, tr.transform.position-parentObject.transform.position);
		}
		
		//	GameObject obstaclesHolder = GameObject.Find(segmentPath+"/"+Segment.obstacleHolderName);
		
		foreach (Obstacle ob in obstacles)
		{
			GameObject newOb = (GameObject) Instantiate(ob.gameObject);
			
			newSegment.AddObstacle(newOb, ob.transform.position-parentObject.transform.position);
		}

		newSegment.SetStartPoint(startPoint-parentObject.transform.position);
		
		newSegment.SetEndPoint(endPoint-parentObject.transform.position);
		
		return newSegmentGO;
	}
	
	
	public void ConvertToSegmentPrefab()
	{
		// make sure we have the latest references
		GetSegments();
		
		Segment firstSegment = (Segment) m_segments[0];
		Segment lastSegment = (Segment) m_segments[m_segments.Count-1];

		ConvertToSegmentPrefab(gameObject, firstSegment.GetStartPoint(), lastSegment.GetStartPoint());
	}
}

[System.Serializable]
public class SegmentGroup
{
    public Segment prefab;
    public int amount;
}
