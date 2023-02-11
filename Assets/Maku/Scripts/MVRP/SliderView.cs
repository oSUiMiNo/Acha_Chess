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
        /// X�������Slider
        /// �w�ǋ@�\�̂݊O���Ɍ��J
        /// </summary>
        //public IReadOnlyReactiveProperty<float> SliderValueRP => floatReactiveProperty;
        public FloatReactiveProperty SliderValueRP => floatReactiveProperty;
        private readonly FloatReactiveProperty floatReactiveProperty = new FloatReactiveProperty();

        void Start()
        {
            slider = GetComponent<Slider>();

            //Slider�̒l�̕ύX���Ď�
            slider.OnValueChangedAsObservable()
                .DistinctUntilChanged()
                .Subscribe(value => { OnValueChange(value, floatReactiveProperty, text); })
                .AddTo(this);
        }

        /// <summary>
        /// Slider�̒l�ύX���̏���
        /// </summary>
        /// <param name="value">Slider�̒l</param>
        /// <param name="floatReactiveProperty">�l���X�V��������RP</param>
        /// <param name="valueText">�X�V����e�L�X�g</param>
        private void OnValueChange(float value, FloatReactiveProperty floatReactiveProperty, TextMeshProUGUI valueText)
        {
            //�l�̐��`
            //var arrangeValue = Mathf.Floor((value - 0.5f) * 100) / 100 * 360;
            //�l�̍X�V
            //floatReactiveProperty.Value = arrangeValue;
            //�e�L�X�g�ɒl�𔽉f
            //valueText.text = arrangeValue.ToString();
            floatReactiveProperty.Value = value;
        }

        public void SetSValue(float volume)
        {
            slider.value = volume;
        }
    }
}


