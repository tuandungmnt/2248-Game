using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private string playStoreID = "3707345";
    private string interstitialAd = "video";
    public bool isTestAds;

    private void Start() {
        Advertisement.Initialize(playStoreID, isTestAds);
    }    

    public void PlayInitializeAds() {
        if (!Advertisement.IsReady(interstitialAd)) return;
        Advertisement.Show(interstitialAd);
    }
}
