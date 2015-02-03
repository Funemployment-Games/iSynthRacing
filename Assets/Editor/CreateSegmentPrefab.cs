using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateSegmentPrefab : MonoBehaviour
{
	// Validate the menu item.
    // The item will be disabled if no transform is selected.
    [MenuItem ("TrackEditor/Create Segment Prefab")]
    static bool CreateSegment ()
	{
		if(Selection.activeGameObject == null) return false;
		GameObject newSegment = Track.ConvertToSegmentPrefab(Selection.activeGameObject, Selection.activeGameObject.transform.position, Selection.activeGameObject.transform.position);
		
		if(newSegment == null) return true;
		
		string prefabName = Selection.activeGameObject.name + "-segment_prefab";
		string path = "Prefabs/Segments/";
		
		while(EditorUtility.FindAsset(path+prefabName+".prefab", typeof(Transform)) != null)
		{
			prefabName += "1";
		}
		
		Object newPrefab = EditorUtility.CreateEmptyPrefab("Assets/"+path+prefabName+".prefab");
		ReplacePrefabOptions options = new ReplacePrefabOptions();
		EditorUtility.ReplacePrefab(newSegment, newPrefab, options);
		
		DestroyImmediate(newSegment);
		
		EditorUtility.InstantiatePrefab(newPrefab);
		
		EditorUtility.DisplayDialog("Segment built successfully","Remember to set start and end positions and applying changes to prefab!", "OK");
		
		return true;
    }
}
