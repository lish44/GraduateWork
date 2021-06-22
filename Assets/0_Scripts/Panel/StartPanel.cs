using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始加载面板 跳转场景 
/// </summary>
public class StartPanel : FW.PanelBase {
    private float m_progressValue;
    public string NextSceneName;
    public float Speed;
    public Slider m_slider;

    protected override void Awake () {
        base.Awake ();
        FW.Evencenter.Instance.AddEventListener<float> (EventName.LOADING, OnProgressValue);
    }

    void OnProgressValue (float _progressValue) {
        m_progressValue = _progressValue;
    }

    private void Start () {
        m_slider = GetControl<Slider> ("Slider_M");

        FW.ScenesMgr.Instance.LoadSceneAsyn (NextSceneName, () => {
            FW.UIMgr.Instance.ClosePanel (this.name);
            // FW.UIMgr.Instance.ShowPanel<MainMenuPanel> ("MainMenuPanel");
        });
    }

    private void Update () {
        CalculateProgress ();
    }

    void CalculateProgress () {

        if (m_progressValue >= .9f)
            m_progressValue = 1f;

        if (m_progressValue != m_slider.value) {
            m_slider.value = Mathf.Lerp (m_slider.value, m_progressValue, Time.deltaTime * Speed);
            if (Mathf.Abs (m_slider.value - m_progressValue) < .01f)
                m_slider.value = m_progressValue;
        }

        ChangeTextContent ("Progress_M", ((int) (m_slider.value * 100)).ToString () + "%");
        if ((int) (m_slider.value * 100) == 100) {
            FW.Evencenter.Instance.EventTrigger<bool> (EventName.LOADINGFINISH, true);
        }
    }
}