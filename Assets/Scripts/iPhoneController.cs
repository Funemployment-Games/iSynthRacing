using UnityEngine;
using System.Collections;

public class iPhoneController : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
	{
		iPhoneSettings.verticalOrientation = false;
		iPhoneSettings.screenCanDarken = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	public static void Init()
	{
		
	}
}

public class iPhoneResolution
{
	public static int x = 480;
	public static int y = 320;
}
