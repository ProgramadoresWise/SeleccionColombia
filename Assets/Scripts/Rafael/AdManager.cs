using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using MaterialUI;


public class AdManager : MonoBehaviour
{
    [HeaderAttribute ("Testing")]
    public bool isTesting;

    [HeaderAttribute ("Ad Unit")]
    public string adUnitBannerAndroidTop;
    public string adUnitBannerAndroidBottom;
    public string adUnitBannerTopIOS;
    public string adUnitBannerBottomIOS;

    [HeaderAttribute("Enable/Disable")]
    public TabView TabViewRegistro;
    public GameObject goCarga, goNavMenu, goWelcome;
    bool canShow = false;
    public static bool snackShow;
    //
    public BannerView bannerViewBottom;
    public BannerView bannerViewTop;

    // Use this for initialization
    void Start ()
    {
        RequestBannerBottom ();
        //RequestBannerTop ();
    }

    // Update is called once per frame
    void Update ()
    {
        if(TabViewRegistro.gameObject.activeInHierarchy)
        {
            if(TabViewRegistro.currentPage == 5)
            {
                bannerViewBottom.Hide();
            }
            else
            {
                bannerViewBottom.Show();
            }
        }

        if(goCarga.activeInHierarchy || goNavMenu.activeInHierarchy || goWelcome.activeInHierarchy)
        {
            bannerViewBottom.Hide();
            canShow = true;
        }
        else if(canShow)
        {
            bannerViewBottom.Show();
            canShow = false;
        }
    }

    private void RequestBannerBottom ()
    {
#if UNITY_ANDROID
        string adUnitId = adUnitBannerAndroidBottom;
#elif UNITY_IPHONE
        string adUnitId = adUnitBannerIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create a 320x50 banner at the top of the screen.
        bannerViewBottom = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

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
        bannerViewBottom.LoadAd (request);
    }

    private void RequestBannerTop ()
    {
#if UNITY_ANDROID
        string adUnitId = adUnitBannerAndroidTop;
#elif UNITY_IPHONE
        string adUnitId = adUnitBannerIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create a 320x50 banner at the top of the screen.
        bannerViewTop = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Top);

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
        bannerViewTop.LoadAd (request);
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