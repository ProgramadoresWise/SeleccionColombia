using UnityEngine;
using System.Collections;
using System;

public class PresentationManager : MonoBehaviour 
{
	public ApplicationChrome.States barState;
	// Use this for initialization
	void Awake () 
	{
		ShowNavbar();
	}

	public void ShowNavbar()
	{
		// Toggles the dimmed out state (where status/navigation content is darker)
		ApplicationChrome.dimmed = false;

		// Set the status/navigation background color (set to 0xff000000 to disable)
		ApplicationChrome.statusBarColor = ApplicationChrome.navigationBarColor = 0xfff0bc10;

		// Makes the status bar and navigation bar visible (default)
		ApplicationChrome.navigationBarState = ApplicationChrome.statusBarState = barState;
	}
}
