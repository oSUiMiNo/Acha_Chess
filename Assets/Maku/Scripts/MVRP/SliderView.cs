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

            //�m�[�}����Savable�ŁA��������̃R���X�g���N�^���g���Ă�ꍇ�́A�f�t�H���g�R���X�g���N�^������Ƃ��ĂˁB

            #region �f�[�^
            [SerializeField] public float value = 0;
            #endregion
        }


        [SerializeField] private Slider slider;
        Data_Slider data = new Data_Slider();

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
                .Subscribe(value => { OnValueChange(value, floatReactiveProperty); })
                .AddTo(this);

            data.Load();
            slider.value = data.value;

            Canvas_Setting.Compo.OnHideWindow += data.Save;
        }

        /// <summary>
        /// Slider�̒l�ύX���̏���
        /// </summary>
        /// <param name="value">Slider�̒l</param>
        /// <param name="floatReactiveProperty">�l���X�V��������RP</param>
        /// <param name="valueText">�X�V����e�L�X�g</param>
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


