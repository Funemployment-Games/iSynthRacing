using UnityEngine;
using System.Collections;

//todo: iterate throw multiple prevFrame items in array
//todo: check if pref frame items still exist
//todo: add optional constant check option

public class GUITouchController : MonoBehaviour
{
	
    private bool wasMouseDownInPrevFrame;
	private GUIElement prevFrameElement;
    private GUIElement[] prevFrameElements;
	GUIElement[] guiElements;
	static int MAXTOUCHES = 2;
	bool[] currentTouchStates;
	bool[] prevTouchStates;
    
    GUIElement FindGUIElement(Camera camera, Vector3 mousePosition)
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "FindGUIElement");
		// Is the mouse inside the cameras viewport?
		Rect rect = camera.pixelRect;
		if (!rect.Contains (mousePosition))
			return null;
		
		// Did we hit any gui elements?
		GUILayer layer = (GUILayer)camera.GetComponent (typeof (GUILayer));
		if (!layer)
			return null;

		GUIElement res = layer.HitTest (mousePosition);
		
	//	pm.CallIsFinished();
		
		return res;
    }
    
	
	// Use this for initialization
	void Start ()
	{
	//	DontDestroyOnLoad(this);
		
		prevFrameElements = new GUIElement[MAXTOUCHES];
		guiElements = new GUIElement[MAXTOUCHES];
		currentTouchStates = new bool[MAXTOUCHES];
		prevTouchStates = new bool[MAXTOUCHES];
	}
	
	void OnLevelWasLoaded(int level)
	{
		prevFrameElements = new GUIElement[MAXTOUCHES];
		guiElements = new GUIElement[MAXTOUCHES];
		currentTouchStates = new bool[MAXTOUCHES];
		prevTouchStates = new bool[MAXTOUCHES];
	}
	
    
    // Update is called once per frame
	void Update ()
	{
	//	JCsProfilerMethod pm = JCsProfiler.Instance.StartCallStopWatch(gameObject.name, gameObject.name, "Update");
		
		if(OptionsDialog.instance != null)
		{
			if(OptionsDialog.isActive)
			{
				return;
			}
		}
		
		for(int i = 0; i < MAXTOUCHES; i++)
		{
			currentTouchStates[i] = false;
		}
		
		foreach (iPhoneTouch touchInfo in iPhoneInput.touches)
		{
			currentTouchStates[touchInfo.fingerId] = true;
		}
		
		for(int i = 0; i < MAXTOUCHES; i++)
		{
			if(prevTouchStates[i] == false && currentTouchStates[i] == true)
			{
				// touch on
				GUIElement element = FindGUIElement( Camera.main, iPhoneInput.touches[i].position);
				if (element)
				{
					prevFrameElements[i] = element;
					element.SendMessage("OnMouseDown",null,SendMessageOptions.DontRequireReceiver);
				}
			}
			
			else if(prevTouchStates[i] == true && currentTouchStates[i] == false)
			{
				// touch off
				if(prevFrameElements[i] != null)
				{
					prevFrameElements[i].SendMessage("OnMouseUp",null,SendMessageOptions.DontRequireReceiver);
					prevFrameElements[i] = null;
				}
			}
		}
		
		for(int i = 0; i < MAXTOUCHES; i++)
		{
			prevTouchStates[i] = currentTouchStates[i];
		}

	//	pm.CallIsFinished();
		if(Application.isEditor)
		{
			UpdateEditor();
		}
	}
	
	void UpdateEditor()
	{
		if (Input.GetMouseButtonDown(0))
		{
			GUIElement element = FindGUIElement( Camera.main, Input.mousePosition);
			if (element)
			{
				if(!wasMouseDownInPrevFrame)
				{
					wasMouseDownInPrevFrame = true;
					element.SendMessage("OnMouseDown",null,SendMessageOptions.DontRequireReceiver);
					prevFrameElement = element;
				}
			}
		}
		else if(wasMouseDownInPrevFrame)
		{
			if (!Input.GetMouseButton(0))
			{
				if(prevFrameElement)
				{
					prevFrameElement.SendMessage("OnMouseUp",null,SendMessageOptions.DontRequireReceiver);
				}
				wasMouseDownInPrevFrame = false;
			}
		}
	}
    
}
