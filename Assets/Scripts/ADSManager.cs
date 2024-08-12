using GoogleMobileAds;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using UnityEngine;

public class ADSManager : MonoBehaviour
{
    
    private static bool isInit = false;
    private static int nowLoses = 0;

#if UNITY_ANDROID
  private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif

private InterstitialAd _interstitialAd;

public void Avake(){
  if(PlayerPrefs.GetString("NoAds") == "Yes")
    Destroy(gameObject);
}
    public void Start()
    {
        if(!isInit){
        MobileAds.Initialize((InitializationStatus initStatus) => {});
        isInit = true;
        }
        LoadInterstitialAd();
    }
    public void LoadInterstitialAd()
  {
      // Clean up the old ad before loading a new one.
      if (_interstitialAd != null)
      {
            _interstitialAd.Destroy();
            _interstitialAd = null;
      }

      Debug.Log("Loading the interstitial ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();

      // send the request to load the ad.
      InterstitialAd.Load(_adUnitId, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              _interstitialAd = ad;
              
          });
          
          // Raised when the ad closed full screen content.
          _interstitialAd.OnAdFullScreenContentClosed += () =>
          {
            Debug.Log("Interstitial Ad full screen content closed.");

          // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
          };
          // Raised when the ad failed to open full screen content.
          _interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
          {
          Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);

          // Reload the ad so that we can show another as soon as possible.
          LoadInterstitialAd();
          };
        }
  public void ShowInterstitialAd()
{
    if (_interstitialAd != null && _interstitialAd.CanShowAd())
    {
        Debug.Log("Showing interstitial ad.");
        _interstitialAd.Show();
    }
    else
    {
        Debug.LogError("Interstitial ad is not ready yet.");
    }
}
public void Update(){
        if(GameController.countLooses %3 == 0 && GameController.countLooses != 0 && GameController.countLooses != nowLoses) {
            nowLoses = GameController.countLooses;
            ShowInterstitialAd();
        }
    }
}

