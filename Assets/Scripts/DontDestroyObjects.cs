using UnityEngine;
using System.Collections;

public class DontDestroyObjects : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(this);
	}
}
