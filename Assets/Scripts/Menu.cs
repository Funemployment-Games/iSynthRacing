using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	protected GameObject m_background;
	
	protected virtual void Start()
	{
		m_background = GameObject.Find("Background");
		MenuManager.DisableLoadingScreen();
	}
}
