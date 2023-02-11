using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using System.Linq;

public class AdManager : SingletonCompo<AdManager>
{
    //public override bool IsActive { get; protected set; } = false;

#if   UNITY_EDITOR
    public static AdID adID = AdID_Android.Ins;
#elif UNITY_ANDROID
    public static AdID adID = AdID_Android.Ins;
#elif UNITY_IPHONE
    public static AdID adID = SampleAdID_iOS.Ins;
#endif

    public class AdGObj
    {
        public GameObject gObj;
        public NonGenericAdCompoBase adCompo;

        public AdGObj(GameObject gObj, NonGenericAdCompoBase adCompo)
        {
            this.gObj = gObj;
            this.adCompo = adCompo;
        }
    }
    public static Dictionary<string, AdGObj> AdGObjs = new Dictionary<string, AdGObj>();

    public static AdRequest request = new AdRequest.Builder().Build(); //空の広告リクエストを作る

    protected override void LateSubAwake()
    {
        Debug.Log($"こんぽ！  {AdManager.Compo}");
        InitializeAd();
        CreateAdGObjs();
        Debug.Log($"こんぽ！  {AdManager.Compo}");
    }

    void InitializeAd()
    {
        MobileAds.Initialize(initStatus => { });
    }

    void CreateAdGObjs()
    {
        IEnumerable<Type> adClassesType;
        adClassesType = System.Reflection.Assembly
            .GetAssembly(typeof(NonGenericAdCompoBase))
            .GetTypes()
            .Where(t =>
            {
                return
                    t.IsSubclassOf(typeof(NonGenericAdCompoBase)) &&
                    !t.IsAbstract;
            });

        foreach (var a in adClassesType)
        {   
            GameObject gObj = new GameObject(a.Name);
            gObj.transform.parent = transform;

            object obj = Activator.CreateInstance(a);
            NonGenericAdCompoBase n = (NonGenericAdCompoBase)obj;
            Component compo = CheckComponent(n.GetType(), gObj);
            NonGenericAdCompoBase convartedCompo = (NonGenericAdCompoBase)compo;

            AdGObj adGObj = new AdGObj(gObj, convartedCompo);
            if (AdGObjs.ContainsKey(a.Name))
            {
                AdGObjs[a.Name] = adGObj;
            }
            else
            {
                AdGObjs.Add(a.Name, adGObj);
            }
        }
    }



    public static void LoadAd(string name)
    {
        AdGObjs[name].adCompo.LoadAd();
    }

    public static void DestroyAd(string name)
    {
        AdGObjs[name].adCompo.DestroyAd();
    }

    public static void ShowAd(string name)
    {
        AdGObjs[name].adCompo.ShowAd();
    }

    public static void HideAd(string name)
    {
        AdGObjs[name].adCompo.HideAd();
    }
}