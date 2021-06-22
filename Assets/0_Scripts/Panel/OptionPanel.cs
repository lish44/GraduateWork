using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : PanelBase {

    SettingData data;

    AudioSource bg;
    public override void Show () {
        // data = DataMgr.Instance.settingData;
        bg = AudioMgr.Instance.GetAudioSorce ("BGM1");
    }
    protected override void OnSliderValueChanged (string _widgetName, float _value) {

        bg.volume = _value;

        // data.Bgvalue = _value;
    }
}

public class SettingData {
    public double Bgvalue;
    public double MouseSensitivity;
    public double MouseWheel;

    public SettingData () {
        Bgvalue = 1;
        MouseSensitivity = 5;
        MouseWheel = 5;
    }
}