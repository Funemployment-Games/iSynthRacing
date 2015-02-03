using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Button : MonoBehaviour
{
	public Texture2D texture;
	public Texture2D texturePressed;

	public string actionName;
	
	protected bool pressed = false;
	bool released = false;
	
	float timer;
	
	public float UpdateTimer(float delta)
	{
		timer -= delta;
		return timer;
	}
	
	public void Reset()
	{
		timer = 0f;
		pressed = false;
	}
	
	protected virtual void Start()
	{
		
	}
	
	protected virtual void Update()
	{

	}
	
	protected virtual void OnMouseUp()
	{
		print("mouse up");
		Release();
	}
	
	protected virtual void OnMouseDown()
	{
		print("mouse down");
		Press();
	}
	
	public void Press()
	{
		pressed = true;
		SoundManager.PlayGUISound(SoundEvent.ButtonPressedGeneric);
		if(texturePressed)
			guiTexture.texture = texturePressed;
	}
	
	void Release()
	{
		released = true;
		pressed = false;
		timer = 1f;
		Activate();
	}
	
	protected virtual void Activate()
	{
		released = false;
		guiTexture.texture = texture;
		if(actionName.Length > 0)
			MenuManager.SendMessage(actionName);
	}
}
