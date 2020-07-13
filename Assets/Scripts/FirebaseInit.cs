using System;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseInit : MonoBehaviour
{
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
}
