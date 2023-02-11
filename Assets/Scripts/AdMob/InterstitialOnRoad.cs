using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class InterstitialOnRoad : AdCompo<InterstitialAd>
{
    protected override void SetCallBacks()
    {
        View.OnAdLoaded += (object sender, EventArgs args) => IsLoaded = true;
        View.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs args) => View.Destroy();
        View.OnAdClosed += (object sender, EventArgs args) => IsLoaded = false;
    }



    protected override IEnumerator LoadAd_Co()
    {
        yield return new WaitForSeconds(0.5f);
        if (!IsLoaded)
        {
            View.LoadAd(AdManager.request);
        }
    }



    protected override IEnumerator DestroyAd_Co()
    {
        yield return new WaitUntil(() => IsLoaded);
        View.Destroy();
    }


    protected override IEnumerator ShowAd_Co()
    {
        yield return new WaitUntil(() => IsLoaded);
        yield return new WaitForSeconds(0.5f);
        View.Show();
    }
}



