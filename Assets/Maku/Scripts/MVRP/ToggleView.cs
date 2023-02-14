using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ToggleView : MonoBehaviourMyExtention
{
    class Data_Toggle : Savable
    {
        public override List<SaveSystem.IFriendWith_SaveSystem> Instances { get; protected set; } = instances;

        public static List<SaveSystem.IFriendWith_SaveSystem> instances = new();

        //ノーマルのSavableで、引数ありのコンストラクタを使ってる場合は、デフォルトコンストラクタも作っといてね。

        #region データ
        [SerializeField] public bool value = false;
        #endregion
    }




    [SerializeField] private Toggle toggle;
    Data_Toggle data = new Data_Toggle();

    /// <summary>
    /// X軸操作のSlider
    /// 購読機能のみ外部に公開
    /// </summary>
    //public IReadOnlyReactiveProperty<float> SliderValueRP => floatReactiveProperty;
    public BoolReactiveProperty ToggleValueRP => boolReactiveProperty;
    private readonly BoolReactiveProperty boolReactiveProperty = new BoolReactiveProperty(false);

    void Start()
    {
        toggle = GetComponent<Toggle>();

        //Toggleの値の変更を監視
        toggle.OnValueChangedAsObservable()
            .DistinctUntilChanged()
            .Subscribe(value => { OnValueChange(value, boolReactiveProperty); })
            .AddTo(this);

        data.Load();
        Debug.Log($"とぐるびゅーの初期化   {toggle}   {toggle.isOn}   {data.value}");
        toggle.isOn = data.value;

        Debug.Log($"とぐるびゅーの初期化1 {Canvas_Setting.Compo}");
        Canvas_Setting.Compo.OnHideWindow += data.Save;
        Debug.Log($"とぐるびゅーの初期化2");
    }

    /// <summary>
    /// Toggleの値変更時の処理
    /// </summary>
    /// <param name="value">Toggleの値</param>
    /// <param name="boolReactiveProperty">値を更新をしたいRP</param>
    /// <param name="valueText">更新するテキスト</param>
    private void OnValueChange(bool value, BoolReactiveProperty boolReactiveProperty)
    {
        boolReactiveProperty.Value = value;
        data.value = value;
    }

    public void SetSValue(bool isOn)
    {
        if (toggle == null)
        {
            Start();
        }
        toggle.isOn = isOn;
    }
}
