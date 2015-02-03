using UnityEngine;
using System.Collections;

public class SelectButton : MonoBehaviour
{
	public Texture2D texture;
	public Texture2D textureSelected;
	public Texture2D trackDetailsTexture;

	public int m_trackNumber = 1;
	
	bool pressed = false;
	bool selected = false;
	
	public SelectButtonManager m_manager;
	
	void Awake()
	{
		if(m_manager == null)
		{
			m_manager = (SelectButtonManager)FindObjectOfType(typeof(SelectButtonManager));
		}
		m_manager.RegisterButton(this);
	}
	
	public void Reset()
	{
		pressed = false;
		selected = false;
		
		if(texture)
		{
			guiTexture.texture = texture;
			
		}
	}
	
	void Update()
	{
	}
	
	void OnMouseUp()
	{
		print("mouse up");
		Release();
	}
	
	void OnMouseDown()
	{
		print("mouse down");
		Press();
	}
	
	public void Press()
	{
		SoundManager.PlayGUISound(SoundEvent.ButtonPressedGeneric);
		if(textureSelected)
			guiTexture.texture = textureSelected;
	}
	
	void Release()
	{
		pressed = false;
		
		m_manager.SelectButton(m_trackNumber);
	}
	
	public void Activate()
	{
		selected = true;
		if(textureSelected)
			guiTexture.texture = textureSelected;
	}
	
	public void Deactivate()
	{
		selected = false;
		
		Reset();
	}
}
