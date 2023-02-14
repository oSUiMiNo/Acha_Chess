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

        //�m�[�}����Savable�ŁA��������̃R���X�g���N�^���g���Ă�ꍇ�́A�f�t�H���g�R���X�g���N�^������Ƃ��ĂˁB

        #region �f�[�^
        [SerializeField] public bool value = false;
        #endregion
    }




    [SerializeField] private Toggle toggle;
    Data_Toggle data = new Data_Toggle();

    /// <summary>
    /// X�������Slider
    /// �w�ǋ@�\�̂݊O���Ɍ��J
    /// </summary>
    //public IReadOnlyReactiveProperty<float> SliderValueRP => floatReactiveProperty;
    public BoolReactiveProperty ToggleValueRP => boolReactiveProperty;
    private readonly BoolReactiveProperty boolReactiveProperty = new BoolReactiveProperty(false);

    void Start()
    {
        toggle = GetComponent<Toggle>();

        //Toggle�̒l�̕ύX���Ď�
        toggle.OnValueChangedAsObservable()
            .DistinctUntilChanged()
            .Subscribe(value => { OnValueChange(value, boolReactiveProperty); })
            .AddTo(this);

        data.Load();
        Debug.Log($"�Ƃ���т�[�̏�����   {toggle}   {toggle.isOn}   {data.value}");
        toggle.isOn = data.value;

        Debug.Log($"�Ƃ���т�[�̏�����1 {Canvas_Setting.Compo}");
        Canvas_Setting.Compo.OnHideWindow += data.Save;
        Debug.Log($"�Ƃ���т�[�̏�����2");
    }

    /// <summary>
    /// Toggle�̒l�ύX���̏���
    /// </summary>
    /// <param name="value">Toggle�̒l</param>
    /// <param name="boolReactiveProperty">�l���X�V��������RP</param>
    /// <param name="valueText">�X�V����e�L�X�g</param>
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
