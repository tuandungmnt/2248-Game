using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private string playStoreID = "3707345";
    private string interstitialAd = "video";
    public bool isTestAds;
    private int _counter = 0;

    private void Start() {
        Advertisement.Initialize(playStoreID, isTestAds);
    }    

    public void PlayInitializeAds()
    {
        _counter++;
        if (_counter % 3 != 0) return;
        if (!Advertisement.IsReady(interstitialAd)) return;
        Advertisement.Show(interstitialAd);
    }
}
