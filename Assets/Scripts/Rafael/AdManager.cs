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
    public string adUnitBannerAndroidBottom2;
    public string adUnitBannerTopIOS;
    public string adUnitBannerBottomIOS;
    public string adUnitBannerBottomIOS2;

    [HeaderAttribute("Enable/Disable")]
    public TabView TabViewRegistro;
    public GameObject goCarga, goNavMenu, goWelcome, goUpdatePolla;
    bool canShow = false;
    public static bool snackShow;
    bool showBottom;
    bool showTop;
    //
    public BannerView bannerViewBottom;
    public BannerView bannerViewTop;
    public BannerView bannerViewBottomMenu;

    // Use this for initialization
    void Start ()
    {
        RequestBanners ();
        //
        // bannerViewBottomMenu.Hide();
        bannerViewTop.Hide();
        showTop = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if(TabViewRegistro.gameObject.activeInHierarchy)
        {
            if(TabViewRegistro.currentPage == 5)
            {
                if(showBottom)
                {
                    bannerViewBottom.Hide();
                    showBottom = false;
                }
            }
            else
            {
                if(!showBottom)
                {
                    bannerViewBottom.Show();
                    showBottom = true;
                }
            }
        }

        if(goCarga.activeInHierarchy || goWelcome.activeInHierarchy || goUpdatePolla.activeInHierarchy)
        {
            if(showBottom)
            {
                bannerViewBottom.Hide();
                showBottom = false;
            }
            canShow = true;  
        }
        else if(goNavMenu.activeInHierarchy)
        {
            if(!showTop)
            {
                bannerViewTop.Show();
                showTop = true;
            }
            canShow = true;
            // bannerViewBottomMenu.Show();
        }
        else if(canShow)
        {
            if(showTop)
            {
                bannerViewTop.Hide();
                showTop = false;
            }
            if(!showBottom)
            {
                bannerViewBottom.Show();
                showBottom = true;
            }
            //bannerViewBottomMenu.Hide();
            canShow = false;
        }
    }

    private void RequestBanners ()
    {
#if UNITY_ANDROID
        string adUnitId = adUnitBannerAndroidBottom;
        string adUnitId1 = adUnitBannerAndroidTop;
        string adUnitId2 = adUnitBannerAndroidBottom2;
#elif UNITY_IPHONE
        string adUnitId = adUnitBannerBottomIOS;
        string adUnitId1 = adUnitBannerTopIOS;
        string adUnitId2 = adUnitBannerBottomIOS2;
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create a 320x50 banner at the top of the screen.
        bannerViewBottom = new BannerView (adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        bannerViewTop = new BannerView (adUnitId1, AdSize.SmartBanner, AdPosition.TopLeft);
        // bannerViewBottom = new BannerView (adUnitId2, AdSize.SmartBanner, AdPosition.BottomLeft);

        // Create an empty ad request.
        AdRequest request;
        AdRequest request1;
        // AdRequest request2;
        //
        if (isTesting)
        {
			#if UNITY_ANDROID
            request = new AdRequest.Builder ().AddTestDevice (GetAndroidAdMobID ()).Build ();
            request1 = new AdRequest.Builder ().AddTestDevice (GetAndroidAdMobID ()).Build ();
            // request2 = new AdRequest.Builder ().AddTestDevice (GetAndroidAdMobID ()).Build ();
			#elif UNITY_IPHONE
			request = new AdRequest.Builder ().AddTestDevice (GetIOSAdMobID ()).Build ();
            request1 = new AdRequest.Builder ().AddTestDevice (GetIOSAdMobID ()).Build ();
            // request2 = new AdRequest.Builder ().AddTestDevice (GetIOSAdMobID ()).Build ();
			#endif
        }
        else
        {
            request = new AdRequest.Builder ().Build ();
            request1 = new AdRequest.Builder ().Build ();
            // request2 = new AdRequest.Builder ().Build ();
        }
        // Load the banner with the request.
        bannerViewBottom.LoadAd (request);
        bannerViewTop.LoadAd (request1);
        // bannerViewBottomMenu.LoadAd (request2);
    }

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