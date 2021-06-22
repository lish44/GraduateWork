using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FW;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanel : FW.PanelBase {
    public string NextSceneName { get; set; } // 需要跳转的场景
    public float speed { get; set; }

    private float m_progressValue;

    private Slider m_slider;
    protected override void Awake () {
        base.Awake ();

        Evencenter.Instance.AddEventListener<float> (EventName.LOADING, OnProgressValue);
    }

    public override void Show () {
        base.Show ();
        AudioMgr.Instance.Stop ();
    }

    void OnProgressValue (float _progressValue) {
        m_progressValue = _progressValue;
    }

    void Start () {
        m_slider = GetControl<Slider> ("Slider_M");
        m_slider.value = 0;

        if (NextSceneName != null)
            Jump ();

        FW.MonoMgr.Instance.AddUpdateListener (CalculateProgressUpdate);
    }

    void Jump () {
        FW.ScenesMgr.Instance.LoadSceneAsyn (NextSceneName, () => {
            FW.UIMgr.Instance.ClosePanel (this.name);

        });
    }
    public void OpenPanel<T> (string _NextPanelName, E_UI_layer _layer = E_UI_layer.Mid, UnityAction<T> _callback = null) where T : FW.PanelBase {
        FW.UIMgr.Instance.ShowPanel<T> (_NextPanelName, _layer, _callback);
    }

    private void CalculateProgressUpdate () {
        if (m_progressValue >= .9f)
            m_progressValue = 1f;

        if (m_progressValue != m_slider.value) {
            m_slider.value = Mathf.Lerp (m_slider.value, m_progressValue, Time.deltaTime * speed);
            if (Mathf.Abs (m_slider.value - m_progressValue) < .01f)
                m_slider.value = m_progressValue;
        }

        ChangeTextContent ("Progress_M", ((int) (m_slider.value * 100)).ToString () + "%");
        if ((int) (m_slider.value * 100) == 100) {
            FW.Evencenter.Instance.EventTrigger<bool> (EventName.LOADINGFINISH, true);
        }
    }

    void OnDestroy () {

        FW.MonoMgr.Instance.RemoveUpdateListener (CalculateProgressUpdate);
        FW.Evencenter.Instance.RemoveEventListener<float> (EventName.LOADING, OnProgressValue);
    }
}