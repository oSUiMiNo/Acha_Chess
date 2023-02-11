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
    public static string BGM = "BgmVolume";
    public static string SE0 = "SeVolume";
    public static string SE1 = "SeVolume";
}

public class AudioUnit : Savable
{
    public override List<SaveSystem.IFriendWith_SaveSystem> Instances { get; protected set; } = instances;
    public static List<SaveSystem.IFriendWith_SaveSystem> instances = new();
    public AudioUnit() { }

    #region データ
    [SerializeField] public float volume;
    #endregion

    [NonSerialized] public AudioSource source;  //モデル
    [NonSerialized] public SliderView sliderView;  //ビュー

    public AudioUnit(string sourceName, string sliderName)
    {
        DebugView.Log($"ゆにっとー0");
        Load();
        DebugView.Log($"ゆにっとー1");
        source = LoadPrefab(sourceName).GetComponent<AudioSource>();
        DebugView.Log($"ゆにっとー2");
        source.transform.parent = AudioManager.Compo.transform;
        source.volume = volume;

        SceneHandler_Setting.Compo.OnInitScene += (() =>
            {
                sliderView = GameObject.Find(sliderName).transform.Find("Slider").GetComponent<SliderView>();
                DebugView.Log($"1   {sliderView}");
                sliderView.SetSValue(volume);

                // Sliderの値の更新を監視
                sliderView.SliderValueRP
                .Subscribe(x =>
                {
                    source.volume = x;
                    volume = x;
                });
            });

        SceneHandler_Setting.Compo.OnExitScene += Save;

        DebugView.Log($"そーーーーす   {source}");
        DebugView.Log($"おんりょう  {source.volume}");
    }

    public void PlayOneShot()
    {
        source.PlayOneShot(source.clip);
    }
}




public class AudioManager : SingletonCompo<AudioManager>
{
    public static Dictionary<string, AudioUnit> Units = new Dictionary<string, AudioUnit>();

    protected override void Start()
    {
        DebugView.Log("ゆにっとつくるーーーーー");
        Units.Add(AudioName.BGM, new AudioUnit(AudioName.BGM, SliderName.BGM));
        Units.Add(AudioName.SE0, new AudioUnit(AudioName.SE0, SliderName.SE0));
        Units.Add(AudioName.SE1, new AudioUnit(AudioName.SE1, SliderName.SE1));
    }
}