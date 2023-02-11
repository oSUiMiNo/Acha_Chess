using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdType
{
    public readonly static string BannerDefault = "BannerDefault";
    public readonly static string InterstitialOnRoad = "InterstitialOnRoad";
}


public abstract class NonGenericAdCompoBase : SealableMonoBehaviourMyExtention
{
    public abstract void LoadAd();
    public abstract void DestroyAd();
    public abstract void ShowAd();
    public abstract void HideAd();
}

/// <summary>
/// 【使い方】
/// 広告用のコンポーネントに継承させる。
/// その際型引数に、そのコンポーネントで扱いたい広告タイプ (GoogleMobileAds.Api が提供している「BunnerView」や「InterstitialAd」等) を入れておく。
/// </summary>
public abstract class AdCompo<ViewType> : NonGenericAdCompoBase
where ViewType : class
{
    static ViewType view = DefaultAdView.Empty as ViewType;
    public ViewType View
    {
        get
        {
            return GetOrCreateAndRoadView<ViewType>();
        }
        set { }
    }
    public bool IsCreated
    {
        get { return (view != DefaultAdView.Empty as ViewType && view != null); }
    }

    public bool IsLoaded = false;



    protected InheritViewType GetOrCreateAndRoadView<InheritViewType>()
        where InheritViewType : class, ViewType
    {
        if (IsCreated)
        {
            // 基底クラスから呼ばれた後に継承先から呼ばれるとエラーになる。先に継承先から呼ぶ
            if (!typeof(InheritViewType).IsAssignableFrom(view.GetType()))
            {
                Debug.LogErrorFormat("{1}が{0}を継承していません", typeof(ViewType), view.GetType());
            }
        }
        else
        {
            AdRequest request = new AdRequest.Builder().Build(); //空の広告リクエストを作る

            ///<summary>
            /// 今の実装だと、Viewのインスタンスを仮で一回作んないといけない。
            /// なので、Viewを派生クラスで作らなくてもここでデフォルトのViewが入るってことにしといて、
            /// 位置などをカスタマイズしたければ CustomizeAd() をオーバーライドして実装するってことで。
            /// </summary>
            if (typeof(InheritViewType) == typeof(BannerView))
            {
                BannerView bannerView = DefaultAdView.Banner;
                view = bannerView as InheritViewType;
            }
            else
            if (typeof(InheritViewType) == typeof(InterstitialAd))
            {
                InterstitialAd interstitialView = DefaultAdView.Interstitial;
                view = interstitialView as InheritViewType;
            }
            else
            {
                view = DefaultAdView.Empty as InheritViewType;
            }
        }

        return view as InheritViewType;
    }


    protected sealed override void Start()
    {
        SubStart();
        CustomizeAd();
        SetCallBacks();
        LateSubStart();
    }
    protected virtual void SubStart() { }
    protected virtual void LateSubStart() { }




    protected virtual void CustomizeAd() { }
    protected virtual void SetCallBacks() { }


    public override void LoadAd() { StartCoroutine(LoadAd_Co()); }
    protected abstract IEnumerator LoadAd_Co();


    public override void DestroyAd() { StartCoroutine(DestroyAd_Co()); }
    protected abstract IEnumerator DestroyAd_Co();


    public override void ShowAd() { StartCoroutine(ShowAd_Co()); }
    protected virtual IEnumerator ShowAd_Co() { yield return null; }


    public override void HideAd() { StartCoroutine(HideAd_Co()); }
    protected virtual IEnumerator HideAd_Co() { yield return null; }
}






public class DefaultAdView
{
    public class EmptyView : Singleton<EmptyView>
    {
        public event EventHandler<EventArgs> OnAdLoaded;
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;
        public event EventHandler<EventArgs> OnAdOpening;
        public event EventHandler<EventArgs> OnAdClosed;
        public event EventHandler<EventArgs> OnAdLeavingApplication;
        public event EventHandler<AdValueEventArgs> OnPaidEvent;
        public void LoadAd(AdRequest request) { }
        public void Destroy() { }
        public void Show() { }
        public void Hide() { }
    }
    public static EmptyView Empty
    {
        get
        {
            return EmptyView.Ins;
        }
    }


    public static BannerView Banner
    {
        get
        {
            BannerView view = new BannerView(AdManager.adID.Banner, AdSize.Banner, AdPosition.Top);
            view.LoadAd(AdManager.request);
            return view;
        }
    }


    public static InterstitialAd Interstitial
    {
        get
        {
            InterstitialAd view = new InterstitialAd(AdManager.adID.Interstitial);
            view.LoadAd(AdManager.request);
            return view;
        }
    }
}








#region アプリID と 広告ID
public class AdID : Singleton<AdID>
{
    /// <summary>
    /// AdMob管理画面で得たアプリIDと広告IDは、
    /// 商品を完成版としてビルドする際に、Scriptにそれぞれ入力します。
    /// アプリID：ca-app-pub-3327272047661402~9300151549
    /// 広告ID：ca-app-pub-3327272047661402/9805200968
    /// </summary>
    public string AppID;
    public string Banner;
    public string Interstitial;
    public string InterstitialVideo;
    public string Rewarded;
    public string RewardedInterstitial;
    public string NativeAdvanced;
    public string NativeAdvancedVideo;
}

public class SampleAdID_Android : AdID
{
    public static new SampleAdID_Android Ins
    {
        get { return GetOrCreateInstance<SampleAdID_Android>(); }
    }

    public SampleAdID_Android()
    {
        Debug.Log($"{this} のコンストラクタ");
        AppID = "ca-app-pub-3940256099942544~3419835294";
        Banner = "ca-app-pub-3940256099942544/6300978111";
        Interstitial = "ca-app-pub-3940256099942544/1033173712";
        InterstitialVideo = "ca-app-pub-3940256099942544/8691691433";
        Rewarded = "ca-app-pub-3940256099942544/5224354917";
        RewardedInterstitial = "ca-app-pub-3940256099942544/5354046379";
        NativeAdvanced = "ca-app-pub-3940256099942544/2247696110";
        NativeAdvancedVideo = "ca-app-pub-3940256099942544/1044960115";
    }
}

public class SampleAdID_iOS : AdID
{
    public static new SampleAdID_iOS Ins
    {
        get { return GetOrCreateInstance<SampleAdID_iOS>(); }
    }

    public SampleAdID_iOS()
    {
        Debug.Log($"{this} のコンストラクタ");
        AppID = "ca-app-pub-3940256099942544/5662855259";
        Banner = "ca-app-pub-3940256099942544/2934735716";
        Interstitial = "ca-app-pub-3940256099942544/4411468910";
        InterstitialVideo = "ca-app-pub-3940256099942544/5135589807";
        Rewarded = "ca-app-pub-3940256099942544/1712485313";
        RewardedInterstitial = "ca-app-pub-3940256099942544/6978759866";
        NativeAdvanced = "ca-app-pub-3940256099942544/3986624511";
        NativeAdvancedVideo = "ca-app-pub-3940256099942544/2521693316";
    }
}

public class AdID_Android : AdID
{
    public static new AdID_Android Ins
    {
        get { return GetOrCreateInstance<AdID_Android>(); }
    }

    public AdID_Android()
    {
        Debug.Log($"{this} のコンストラクタ");
        AppID = "ca-app-pub-6456681180897600~9283984142";
        Banner = "ca-app-pub-6456681180897600/7096788590";
        Interstitial = "ca-app-pub-6456681180897600/9591089992";
        InterstitialVideo = "";
        Rewarded = "";
        RewardedInterstitial = "";
        NativeAdvanced = "";
        NativeAdvancedVideo = "";
    }
}

public class AdID_iOS : AdID
{
    public static new AdID_iOS Ins
    {
        get { return GetOrCreateInstance<AdID_iOS>(); }
    }

    public AdID_iOS()
    {
        Debug.Log($"{this} のコンストラクタ");
        AppID = "";
        Banner = "";
        Interstitial = "";
        InterstitialVideo = "";
        Rewarded = "";
        RewardedInterstitial = "";
        NativeAdvanced = "";
        NativeAdvancedVideo = "";
    }
}
#endregion

