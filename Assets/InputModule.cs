using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour {

	[SerializeField] private string m_keyUp = "w";
	[SerializeField] private string m_keyDown = "s";
	[SerializeField] private string m_keyRight = "d";
	[SerializeField] private string m_keyLeft = "a";

	private float m_direction_Vertical = 0f;
	private float m_direction_Horizontal = 0f;

	private void Update()
	{      
		m_direction_Vertical = (Input.GetKey(m_keyUp) ? 1f : 0f) - (Input.GetKey(m_keyDown) ? 1f : 0f);
		m_direction_Horizontal = (Input.GetKey(m_keyRight) ? 1f : 0f) - (Input.GetKey(m_keyLeft) ? 1f : 0f);
	}

}
