using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using MVP.View;
//using MVP.Model;
//using MVP.Presenter;


public class AudioName
{
    public static string BGM = "BGM";
    public static string SE0 = "SoundEffect0";
    public static string SE1 = "SoundEffect1";
}
public class SliderName
{
    public static string BGM = "Slider_BgmVolume";
    public static string SE0 = "Slider_SeVolume";
    public static string SE1 = "Slider_SeVolume";
}

public class AudioUnit : MyExtention
{
    [NonSerialized] public AudioSource source;  //モデル
    [NonSerialized] public SliderView sliderView;  //ビュー

    public AudioUnit(string sourceName, string sliderName)
    {
        source = LoadPrefab(sourceName).GetComponent<AudioSource>();
        source.transform.parent = AudioManager.Compo.transform;
     
        sliderView = GameObject.Find(sliderName).transform.Find("Slider").GetComponent<SliderView>();
        DebugView.Log($"1   {sliderView}");

        // Sliderの値の更新を監視
        sliderView.SliderValueRP
        .Subscribe(x =>
        {
            source.volume = x;
        });
    }

    public void PlayOneShot()
    {
        source.PlayOneShot(source.clip);
    }
}




public class AudioManager : SingletonCompo<AudioManager>
{
    public static Dictionary<string, AudioUnit> Units = new Dictionary<string, AudioUnit>();
    public event System.Action OnInitialized;

    protected override void Start()
    {
        DebugView.Log("ゆにっとつくるーーーーー");
        Units.Add(AudioName.BGM, new AudioUnit(AudioName.BGM, SliderName.BGM));
        DebugView.Log("ゆにっとつくるーーーーー1");
        Units.Add(AudioName.SE0, new AudioUnit(AudioName.SE0, SliderName.SE0));
        DebugView.Log("ゆにっとつくるーーーーー2");
        Units.Add(AudioName.SE1, new AudioUnit(AudioName.SE1, SliderName.SE1));
        OnInitialized?.Invoke();
    }
}