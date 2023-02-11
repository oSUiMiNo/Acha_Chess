using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumePresenter : Singleton<AudioVolumePresenter>
{
    //public class VolumeData : SavableSingleton<VolumeData>
    //{
    //    public override List<SaveSystem.IFriendWith_SaveSystem> Instances { get; protected set; } = instances;
    //    public static List<SaveSystem.IFriendWith_SaveSystem> instances = new();

    //    #region ÉfÅ[É^
    //    [SerializeField] public float BGM = 1;
    //    [SerializeField] public float SE0 = 1;
    //    #endregion
    //}


    //public void Init()
    //{
    //    VolumeData.Ins.Load();
    //    foreach(var a in AudioManager.units)
    //    {
    //        Reflect_Volume_To_Audio(a.Value);
    //    }
    //}


    //public void DynamicSet(string audioName)
    //{
    //    Reflect_Slider_To_Volume();
    //    Reflect_Volume_To_Audio(AudioManager.units[audioName]);
    //    Save();
    //}


    //public void Reflect_Slider_To_Volume()
    //{
    //    volume_BGM = GameObject.Find("BgmVolume").transform.Find("Slider").GetComponent<Slider>().value;
    //    volume_SE = GameObject.Find("SeVolume").transform.Find("Slider").GetComponent<Slider>().value;
    //}

    ////void Reflect_Volume_To_Audio(AudioSource source, float volume)
    ////{
    ////    AudioManager.Sources[AudioName.BGM].volume = volume_BGM;
    ////    AudioManager.Sources[AudioName.SE0].volume = volume_SE;
    ////    AudioManager.Sources[AudioName.SE1].volume = volume_SE;
    ////}

    //void Reflect_Volume_To_Audio(AudioUnit unit)
    //{
    //    unit.source.volume = unit.volumeData;
    //}
}
    