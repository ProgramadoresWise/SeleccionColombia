using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class InAppBrowser : System.Object {

	public struct DisplayOptions {
		public string pageTitle;
		public string backButtonText;
		public string barBackgroundColor;
		public string textColor;
		public bool displayURLAsPageTitle;
	}

	public static void OpenURL(string URL) {
		DisplayOptions displayOptions = new DisplayOptions();
		displayOptions.displayURLAsPageTitle = true;
		OpenURL(URL, displayOptions);
	}

	public static void OpenURL(string URL, DisplayOptions displayOptions) {
		#if UNITY_IOS && !UNITY_EDITOR
			iOSInAppBrowser.OpenURL(URL, displayOptions);
		#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidInAppBrowser.OpenURL(URL, displayOptions); 
		#endif
	}

	#if UNITY_ANDROID && !UNITY_EDITOR
	private class AndroidInAppBrowser {
		public static void OpenURL(string URL, DisplayOptions displayOptions) {
			var helper = new AndroidJavaClass("inappbrowser.kokosoft.com.Helper");
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			helper.CallStatic("OpenInAppBrowser", currentActivity, URL, CreateJavaDisplayOptions(displayOptions));                                 
		}

		private static AndroidJavaObject CreateJavaDisplayOptions(DisplayOptions displayOptions) {
			var ajc = new AndroidJavaObject("inappbrowser.kokosoft.com.DisplayOptions");
			if (displayOptions.pageTitle != null) {
				ajc.Set<string>("pageTitle", displayOptions.pageTitle);
			}

			if (displayOptions.backButtonText != null) {
				ajc.Set<string>("backButtonText", displayOptions.backButtonText);
			}

			if (displayOptions.barBackgroundColor != null) {
				ajc.Set<string>("barBackgroundColor", displayOptions.barBackgroundColor);
			}

			if (displayOptions.textColor != null) {
				ajc.Set<string>("textColor", displayOptions.textColor);
			}

			ajc.Set<bool>("displayURLAsPageTitle", displayOptions.displayURLAsPageTitle);
			return ajc;
		}

	}
	#endif

	#if UNITY_IPHONE && !UNITY_EDITOR
	private class iOSInAppBrowser {

		[DllImport ("__Internal")]
		private static extern void _OpenInAppBrowser(string URL, DisplayOptions displayOptions);

		public static void OpenURL(string URL, DisplayOptions displayOptions) {
			_OpenInAppBrowser(URL, displayOptions);
		}
	}
	#endif
}
