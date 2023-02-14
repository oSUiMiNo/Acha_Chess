using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace MVP.View
{
    public class SliderView : MonoBehaviourMyExtention
    {
        class Data_Slider : Savable
        {
            public override List<SaveSystem.IFriendWith_SaveSystem> Instances { get; protected set; } = instances;
            public static List<SaveSystem.IFriendWith_SaveSystem> instances = new();

            //ノーマルのSavableで、引数ありのコンストラクタを使ってる場合は、デフォルトコンストラクタも作っといてね。

            #region データ
            [SerializeField] public float value = 0;
            #endregion
        }


        [SerializeField] private Slider slider;
        Data_Slider data = new Data_Slider();

        /// <summary>
        /// X軸操作のSlider
        /// 購読機能のみ外部に公開
        /// </summary>
        //public IReadOnlyReactiveProperty<float> SliderValueRP => floatReactiveProperty;
        public FloatReactiveProperty SliderValueRP => floatReactiveProperty;
        private readonly FloatReactiveProperty floatReactiveProperty = new FloatReactiveProperty();

        void Start()
        {
            slider = GetComponent<Slider>();

            //Sliderの値の変更を監視
            slider.OnValueChangedAsObservable()
                .DistinctUntilChanged()
                .Subscribe(value => { OnValueChange(value, floatReactiveProperty); })
                .AddTo(this);

            data.Load();
            slider.value = data.value;

            Canvas_Setting.Compo.OnHideWindow += data.Save;
        }

        /// <summary>
        /// Sliderの値変更時の処理
        /// </summary>
        /// <param name="value">Sliderの値</param>
        /// <param name="floatReactiveProperty">値を更新をしたいRP</param>
        /// <param name="valueText">更新するテキスト</param>
        private void OnValueChange(float value, FloatReactiveProperty floatReactiveProperty)
        {
            floatReactiveProperty.Value = value;
            data.value = value;
        }

        public void SetSValue(float volume)
        {
            if(slider == null) 
            {
                Start();
            }
            slider.value = volume;
        }
    }
}


