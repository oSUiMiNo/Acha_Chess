using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//public class SingletonCanvas<SingletonType>
//    : SingletonCompo<SingletonType>
//    where SingletonType : SingletonCompo<SingletonType>, new()
//{
//    public Canvas canvas = null;
//    public GraphicRaycaster raycaster = null;
//    protected override void SubLateAwake()
//    {
//        canvas = gameObject.AddComponent<Canvas>();
//        raycaster = gameObject.AddComponent<GraphicRaycaster>();
//        SetRenderMode();
//        SetSortOrder();

//        SubSubLateAwake();
//    }
//    protected virtual void SubSubLateAwake() { }
//    protected virtual void SetRenderMode() { }
//    protected virtual void SetSortOrder() { }
//}



public class Canvas_Setting : MonoBehaviourMyExtention //SingletonCanvas<Canvas_Setting>
{
    GameObject window;
    GameObject button;
    public event System.Action OnShowWindow;
    public event System.Action OnHideWindow;
    Vector3 initialPosition = Vector3.zero;

    #region Åyå¬ï ÉVÉìÉOÉãÉgÉìâªÅz ===================================================================

    public static Canvas_Setting Compo = null;
    void Singletonize()
    {
        if (Compo == null)
        {
            Compo = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion Åyå¬ï ÉVÉìÉOÉãÉgÉìâªÅz ================================================================
    private void Awake()
    {
        Singletonize();

        button = transform.Find("Button").gameObject;
        window = transform.Find("Window").gameObject;

        button.GetComponent<Button>().onClick.AddListener(() => OnShowWindow?.Invoke());
        window.transform.Find("HideButton").GetComponent<Button>().onClick.AddListener(() => OnHideWindow?.Invoke());   
        
        initialPosition = window.transform.position;
        OnShowWindow += () => window.transform.position = initialPosition;
        OnHideWindow += () => window.transform.position = new Vector3(0, 10000, 0);
    }

    private void Start()
    {
        AudioManager.Compo.OnInitialized += () => window.transform.position = new Vector3(0, 10000, 0);
    }
}
