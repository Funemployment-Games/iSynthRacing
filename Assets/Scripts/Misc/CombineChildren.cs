using UnityEngine;
using System.Collections;
/*
 Attach this script as a parent to some game objects. The script will then combine the meshes at startup.
 This is useful as a performance optimization since it is faster to render one big mesh than many small meshes. See the docs on graphics performance optimization for more info.
 
 Different materials will cause multiple meshes to be created, thus it is useful to share as many textures/material as you can.
 */

[AddComponentMenu("Mesh/Combine Children")]
[ExecuteInEditMode]
public class CombineChildren : MonoBehaviour {
	
	/// Usually rendering with triangle strips is faster.
	/// However when combining objects with very low triangle counts, it can be faster to use triangles.
	/// Best is to try out which value is faster in practice.

	public bool m_update = false;
	public bool generateTriangleStrips = true;
	public float distanceThreshold = 1000;
	public bool disableObjects = false; // if false, only mesh renderer will be disabled, keeping colliders, etc active
	
	Hashtable materialToMesh= new Hashtable();
	Hashtable instanceToPosition= new Hashtable();
	Vector3 combinedMeshPosition;
	Component[] allfilters;
	
//	public Material m_obstacleMaterial;
//	public Material m_tunnelMaterial;
//	public Track m_track;
	
	/// This option has a far longer preprocessing time at startup but leads to better runtime performance.
	void Awake()
	{
		if(Application.isPlaying)
		{
		//	Execute();
		}
	}
	
	GameObject parent_go;
	
	void Execute ()
	{
		parent_go = new GameObject(gameObject.name+"-Combined meshes");
		parent_go.transform.parent = transform.parent;
		
		allfilters  = GetComponentsInChildren(typeof(MeshFilter));
		
		SortFilters();
		
		if(distanceThreshold == 0) distanceThreshold = Mathf.Infinity;
		
		Vector3 groupStartPos = allfilters[0].transform.position;
		ArrayList filtersInGroup = new ArrayList();
		for (int i = 0; i < allfilters.Length; i++)
		{
			Component filter = allfilters[i];
			
			float distToStart = filter.transform.position.z - groupStartPos.z;
			
			filtersInGroup.Add(filter);
			if(distToStart > distanceThreshold || i == allfilters.Length-1)
			{
				Component[] filters = (Component[]) filtersInGroup.ToArray(typeof(Component));
				ComputeMaterialsToMesh(filters);
				CombineMeshes();
				if(i+1 < allfilters.Length)
				{
					filtersInGroup.Clear();
					groupStartPos = allfilters[i+1].transform.position;
				}
			}
		}
	}
	
	
	void ComputeMaterialsToMesh(Component[] filters)
	{
		materialToMesh= new Hashtable();
		instanceToPosition= new Hashtable();
		Matrix4x4 myTransform = transform.worldToLocalMatrix;
		
		for (int i=0;i<filters.Length;i++)
		{
			MeshFilter filter = (MeshFilter)filters[i];
			Renderer curRenderer  = filters[i].renderer;
			MeshCombineUtility.MeshInstance instance = new MeshCombineUtility.MeshInstance ();
			instance.mesh = filter.sharedMesh;
			if (curRenderer != null && curRenderer.enabled && instance.mesh != null)
			{
				instance.transform = myTransform * filter.transform.localToWorldMatrix;
				
				Material[] materials = curRenderer.sharedMaterials;
				for (int m=0;m<materials.Length;m++)
				{
					instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);
					
					ArrayList objects = (ArrayList)materialToMesh[materials[m]];
					if (objects != null)
					{
						objects.Add(instance);
						if(!instanceToPosition.ContainsKey(instance))
							instanceToPosition.Add(instance, filter.transform);
					}
					else
					{
						objects = new ArrayList ();
						objects.Add(instance);
						if(!instanceToPosition.ContainsKey(instance))
							instanceToPosition.Add(instance, filter.transform);
						materialToMesh.Add(materials[m], objects);
					}
				}
				
				if(disableObjects)
				{
					curRenderer.gameObject.active = false;
				}
				else
				{
					curRenderer.enabled = false;
					Animation comp = (Animation)curRenderer.gameObject.GetComponent(typeof(Animation));
					if(comp)
					{
						Object.DestroyImmediate(comp);
					}
				}
			}
		}
	}
	
	void Update()
	{
		if(m_update)
		{
			Execute();
			
			m_update = false;
		}
	}
	
	void CombineMeshes()
	{
		foreach (DictionaryEntry de  in materialToMesh)
		{
			ArrayList elements = (ArrayList)de.Value;
			MeshCombineUtility.MeshInstance[] instances = (MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MeshCombineUtility.MeshInstance));
			
			GameObject go = new GameObject("Combined mesh ");
			go.transform.parent = parent_go.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localPosition = Vector3.zero;
			go.AddComponent(typeof(MeshFilter));
			go.AddComponent("MeshRenderer");
			go.renderer.sharedMaterial = (Material)de.Key;
			if(go.renderer.sharedMaterial == null || go.renderer.sharedMaterial.name == null)
			{
				DestroyImmediate(go);
				continue;
			}
			go.name += " - "+go.renderer.sharedMaterial.name;
			
			MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
			
			Vector3 centroid = GetAveragePosition(instances);
			go.transform.position = centroid;
			for (int i = 0; i < instances.Length; i++)
			{
				instances[i].transform = go.transform.worldToLocalMatrix * ((Transform)instanceToPosition[instances[i]]).localToWorldMatrix;
			}
			
			filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
		}
	}
	
	Vector3 GetAveragePosition(MeshCombineUtility.MeshInstance[] instances)
	{
		Vector3 pos_acum = Vector3.zero;
		int count = 0;
		foreach (MeshCombineUtility.MeshInstance instance in instances)
		{
			Transform pos = (Transform) instanceToPosition[instance];
			pos_acum += pos.position;
			count++;
		}
		
		return pos_acum/count;
		
	}
	
	void SortFilters()
	{
		int i;
		int j;
		Component temp;
		
		for( i = (allfilters.Length - 1); i >= 0; i-- )
		{
			for( j = 1; j <= i; j++ )
			{
				Component o1 = (Component) allfilters[j-1];
				Component o2 = (Component) allfilters[j];
				
				if( o1.transform.position.z > o2.transform.position.z )
				{
					temp = (Component)allfilters[j-1];
					allfilters[j-1] = allfilters[j];
					allfilters[j] = temp;
				}
			}
		}
	}
}
