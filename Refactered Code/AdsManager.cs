using UnityEngine;
using UnityEngine.Advertisements;

namespace Domain
{
    public class AdsManager : MonoBehaviour
    {
        private const string PlayStoreId = "3707345";
        private const string InterstitialAd = "video";
        [SerializeField] private bool isTestAds;
        private int _counter;

        private void Start() 
        {
            Advertisement.Initialize(PlayStoreId, isTestAds);
        }    

        public void PlayInitializeAds()
        {
            _counter++;
            if (_counter % 3 != 0) return;
            if (!Advertisement.IsReady(InterstitialAd)) return;
            Advertisement.Show(InterstitialAd);
        }
    }
}
