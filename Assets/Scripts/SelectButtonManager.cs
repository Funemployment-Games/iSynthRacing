using UnityEngine;
using System.Collections;

public class SelectButtonManager : MonoBehaviour
{
	protected int m_selectedButtonIndex;
	protected SelectButton m_selectedButton;
	
	ArrayList m_selectButtons;

	// Use this for initialization
	protected virtual void Start ()
	{
	//	SelectButton(1);
	}
	
	public void RegisterButton(SelectButton button)
	{
		if(m_selectButtons == null) m_selectButtons = new ArrayList();
		m_selectButtons.Add(button);
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		
	}
	
	public virtual void SelectButton(int number)
	{
		foreach (SelectButton m_button in m_selectButtons)
		{
			if(m_button.m_trackNumber == number)
			{
				m_selectedButtonIndex = number;
				m_selectedButton = m_button;
				m_button.Activate();
				
			}
			else
			{
				m_button.Deactivate();
			}
		}
	}
	
	
}
