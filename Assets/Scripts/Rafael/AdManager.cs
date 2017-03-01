using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;


public class AdManager : MonoBehaviour
{
    [HeaderAttribute ("Testing")]
    public bool isTesting;

    [HeaderAttribute ("Ad Unit")]
    public string adUnitBannerAndroid;
    public string adUnitBannerIOS;
    //
    BannerView bannerView;

    // Use this for initialization
    void Start ()
    {
        RequestBanner ();
    }

    // Update is called once per frame
    void Update ()
    {
        Screen.fullScreen = false;
    }

    private void RequestBanner ()
    {
#if UNITY_ANDROID
        string adUnitId = adUnitBannerAndroid;
#elif UNITY_IPHONE
        string adUnitId = adUnitBannerIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // bannerView.OnAdOpening += AdOpenedHandler;

        // Create an empty ad request.
        AdRequest request;
        //
        if (isTesting)
        {
			#if UNITY_ANDROID
            request = new AdRequest.Builder ().AddTestDevice (GetAndroidAdMobID ()).Build ();
			#elif UNITY_IPHONE
			request = new AdRequest.Builder ().AddTestDevice (GetIOSAdMobID ()).Build ();
			#endif
        }
        else
        {
            request = new AdRequest.Builder ().Build ();
        }
        // Load the banner with the request.
        bannerView.LoadAd (request);
    }

    // public void AdOpenedHandler(object sender, EventArgs args)
    // {

    // }

#if UNITY_ANDROID
    public static string GetAndroidAdMobID ()
    {
        UnityEngine.AndroidJavaClass up = new UnityEngine.AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        UnityEngine.AndroidJavaObject currentActivity = up.GetStatic<UnityEngine.AndroidJavaObject> ("currentActivity");
        UnityEngine.AndroidJavaObject contentResolver = currentActivity.Call<UnityEngine.AndroidJavaObject> ("getContentResolver");
        UnityEngine.AndroidJavaObject secure = new UnityEngine.AndroidJavaObject ("android.provider.Settings$Secure");
        string deviceID = secure.CallStatic<string> ("getString", contentResolver, "android_id");
        return Md5Sum (deviceID).ToUpper ();
    }
#endif

#if UNITY_IPHONE
    public static string GetIOSAdMobID ()
    {
        return Md5Sum (UnityEngine.iPhone.advertisingIdentifier);
    }
#endif
    public static string Md5Sum (string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
        byte[] bytes = ue.GetBytes (strToEncrypt);

        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
        byte[] hashBytes = md5.ComputeHash (bytes);

        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString (hashBytes[i], 16).PadLeft (2, '0');
        }

        return hashString.PadLeft (32, '0');
    }
}