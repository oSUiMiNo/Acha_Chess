using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MVP.View
{
    public class SliderView : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;

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
                .Subscribe(value => { OnValueChange(value, floatReactiveProperty, text); })
                .AddTo(this);
        }

        /// <summary>
        /// Sliderの値変更時の処理
        /// </summary>
        /// <param name="value">Sliderの値</param>
        /// <param name="floatReactiveProperty">値を更新をしたいRP</param>
        /// <param name="valueText">更新するテキスト</param>
        private void OnValueChange(float value, FloatReactiveProperty floatReactiveProperty, TextMeshProUGUI valueText)
        {
            //値の整形
            //var arrangeValue = Mathf.Floor((value - 0.5f) * 100) / 100 * 360;
            //値の更新
            //floatReactiveProperty.Value = arrangeValue;
            //テキストに値を反映
            //valueText.text = arrangeValue.ToString();
            floatReactiveProperty.Value = value;
        }

        public void SetSValue(float volume)
        {
            slider.value = volume;
        }
    }
}


