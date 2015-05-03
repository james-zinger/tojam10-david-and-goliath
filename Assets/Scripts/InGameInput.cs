﻿using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class InGameInput : MonoBehaviour
{
	public EventSystem es;
	
	private AudioSource onClick;
	private AudioSource onBack;

	private int winSel = 0;

	Cube cube;
	private InGameUI UIcontrols;
	// Use this for initialization
	void Start () {

		Transform t = transform.FindChild( "OnClick" );
		if ( t != null )
		{
			onClick = t.audio;
		}
		t = transform.FindChild ("OnBack");
		if (t != null) 
		{
			onBack = t.audio;
		}

		cube = FindObjectOfType<Cube>();
		UIcontrols = FindObjectOfType<InGameUI>();
 //		EventSystem.current.SetSelectedGameObject( UIcontrols.gameOptions[ opSel ].gameObject, new BaseEventData( EventSystem.current ) );
	}
	
	// Update is called once per frame
	void Update () {
		Xbox360GamepadState.Instance.UpdateState ();
		if (cube.HasFinished == true) {
			UIcontrols.winScreen.gameObject.SetActive(true);
			if (UIcontrols.winScreen.gameObject.activeSelf)
			{
				Time.timeScale = 0;
				// When menu is active the following is allowed
				if (Xbox360GamepadState.Instance.AxisJustPastThreshold(Xbox.Axis.LAnalogX, 0.5f) || Input.GetKeyDown(KeyCode.D)) {
					winSel +=1; // Increment the array by 1 as you move down the list
					// This statement allows for returning back to top option once you reach the bottom of the list
					if (UIcontrols.winOptions.Length == winSel)
					{
						// Sets the variable for the array back to 0 to continue moving down the list
						winSel = 0;
					}
					// This line is what allows for the selection of buttons through key input and so on
					es.SetSelectedGameObject(UIcontrols.winOptions[winSel].gameObject, new BaseEventData(es));
				}
				// This statement does the same as above but for upward action
				if (Xbox360GamepadState.Instance.AxisJustPastThreshold(Xbox.Axis.LAnalogX, -0.5f) || Input.GetKeyDown(KeyCode.A)) {
					winSel -=1;
					if (winSel < 0)
					{
						winSel = 1;
					}
					es.SetSelectedGameObject(UIcontrols.winOptions[winSel].gameObject, new BaseEventData(es));
				}
				if((winSel == 0 && Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) || (winSel == 0 && Input.GetKeyDown(KeyCode.KeypadEnter))) {
					onClick.Play();
					Time.timeScale = 0;
					UIcontrols.winScreen.gameObject.SetActive(false);
					Application.LoadLevel(0);
				}
				if((winSel == 1 && Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) || (winSel == 1 && Input.GetKeyDown(KeyCode.KeypadEnter))) {
					onClick.Play();
					Time.timeScale = 0;
					UIcontrols.winScreen.gameObject.SetActive(false);
					var levelstring = Cube.LevelString;
					var levelNum = levelstring[ levelstring.Length - 1 ];
					Cube.LevelString = string.Format("Level {0}", levelNum ); 
					Application.LoadLevel (1);
				}
			}
			return;
		}
		//if (UIcontrols.pauseMenu.gameObject.activeSelf == true) {
		//	// When menu is active the following is allowed
		//	if (Xbox360GamepadState.Instance.AxisJustPastThreshold(Xbox.Axis.LAnalogY, -0.5f) || Input.GetKeyDown(KeyCode.S)) {
		//		opSel +=1; // Increment the array by 1 as you move down the list
		//		// This statement allows for returning back to top option once you reach the bottom of the list
		//		if (UIcontrols.gameOptions.Length == opSel)
		//		{
		//			// Sets the variable for the array back to 0 to continue moving down the list
		//			opSel = 0;
		//		}
		//		// This line is what allows for the selection of buttons through key input and so on
		//		es.SetSelectedGameObject(UIcontrols.gameOptions[opSel].gameObject, new BaseEventData(es));
		//	}
		//	// This statement does the same as above but for upward action
		//	if (Xbox360GamepadState.Instance.AxisJustPastThreshold(Xbox.Axis.LAnalogY, 0.5f) || Input.GetKeyDown(KeyCode.W)) {
		//		opSel -=1;
		//		if (opSel < 0)
		//		{
		//			opSel = 1;
		//		}
		//		es.SetSelectedGameObject(UIcontrols.gameOptions[opSel].gameObject, new BaseEventData(es));
		//	}
		//	if ((opSel == 0 && Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) || (opSel == 0 && Input.GetKeyDown(KeyCode.A))) {
		//		onClick.Play();
		//		UIcontrols.pauseMenu.gameObject.SetActive (false);
		//		UIcontrols.inGameHowTo.gameObject.SetActive(true);
		//	}
		//	else if ((opSel == 1 && Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) || (opSel == 1 && Input.GetKeyDown(KeyCode.A))) {
		//		onClick.Play();
		//		Application.LoadLevel(0);
		//	}
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.Start) || Input.GetKeyDown(KeyCode.M)) {
		//		UIcontrols.pauseMenu.Toggle();
		//		Time.timeScale = 0;
		//	}
		//	if(Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.Back) || Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.Start)|| Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.B)|| Input.GetKeyDown(KeyCode.Space)) {
		//		UIcontrols.pauseMenu.Toggle();
		//		opSel = 0;
		//		es.SetSelectedGameObject(UIcontrols.gameOptions[opSel].gameObject, new BaseEventData(es));
		//		Time.timeScale = 1;
		//	}
		//	return;
		//}

		//if (cube.HasFinished == true) {
		//	UIcontrols.winScreen.gameObject.SetActive(true);
		//	if (UIcontrols.winScreen.gameObject.activeSelf)
		//	{
		//		Time.timeScale = 0;
		//		// When menu is active the following is allowed
		//		if (Xbox360GamepadState.Instance.AxisJustPastThreshold(Xbox.Axis.LAnalogY, -0.5f) || Input.GetKeyDown(KeyCode.S)) {
		//			winSel +=1; // Increment the array by 1 as you move down the list
		//			// This statement allows for returning back to top option once you reach the bottom of the list
		//			if (UIcontrols.winOptions.Length == winSel)
		//			{
		//				// Sets the variable for the array back to 0 to continue moving down the list
		//				winSel = 0;
		//			}
		//			// This line is what allows for the selection of buttons through key input and so on
		//			es.SetSelectedGameObject(UIcontrols.winOptions[winSel].gameObject, new BaseEventData(es));
		//		}
		//		// This statement does the same as above but for upward action
		//		if (Xbox360GamepadState.Instance.AxisJustPastThreshold(Xbox.Axis.LAnalogY, 0.5f) || Input.GetKeyDown(KeyCode.W)) {
		//			winSel -=1;
		//			if (winSel < 0)
		//			{
		//				winSel = 1;
		//			}
		//			es.SetSelectedGameObject(UIcontrols.winOptions[winSel].gameObject, new BaseEventData(es));
		//		}
		//		if ((winSel == 0 && Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) || (winSel == 0 && Input.GetKeyDown(KeyCode.A))) {
		//			onClick.Play();
		//			Time.timeScale = 0;
		//			UIcontrols.winScreen.gameObject.SetActive (false);
		//			cube.MasterReset();
		//		}

		//		if((winSel == 1 && Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) || (winSel == 1 && Input.GetKeyDown(KeyCode.Space))) {
		//			onClick.Play();
		//			Time.timeScale = 0;
		//			UIcontrols.winScreen.gameObject.SetActive(false);
		//			Application.LoadLevel(0);
		//		}
		//	}
		//	return;
		//}
		
		//if (UIcontrols.pauseMenu.gameObject.activeSelf == false) {
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.Start) || Input.GetKeyDown(KeyCode.M)) {
		//		onClick.Play();
		//		UIcontrols.pauseMenu.gameObject.SetActive(true);
		//		Time.timeScale = 0;
		//	}
		//}
		
		//if (UIcontrols.inGameHowTo.gameObject.activeSelf == true) {
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.B) || Input.GetKeyDown (KeyCode.Space)) {
		//		onBack.Play();
		//		UIcontrols.inGameHowTo.gameObject.SetActive (false);
		//		UIcontrols.pauseMenu.gameObject.SetActive(true);
		//		opSel = 0;
		//		es.SetSelectedGameObject(UIcontrols.gameOptions[opSel].gameObject, new BaseEventData(es));
		//	}
		//}

		//if (UIcontrols.pauseMenu.gameObject.activeSelf == false) {
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.X)) {
		//		cube.RotateX ();
		//	}
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.A)) {
		//		cube.RotateY ();
		//	}
			
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.B)) {
		//		cube.RotateZ ();
		//	}
			
		//	if (Xbox360GamepadState.Instance.IsButtonDown (Xbox.Button.Y)) {
		//		cube.StartMovingGoat ();
		//	}
		//	return;
		//}
	}
}