using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : PanelBase
{

    public PlayerInfo info;
    AudioSource bg;
    CameraDefault cameraDefault;
    public override void Show()
    {

        OnHandlePropertyChange(info);
        SetHead(info);
        Evencenter.Instance.AddEventListener<PlayerInfo>(EventName.PLAYER_PROPERTY_CHANGE, OnHandlePropertyChange);
        AudioMgr.Instance.Play("BGM", true, 0.5f);

        GetControl<Image>("Settingpage_M").gameObject.SetActive(false);
        bg = AudioMgr.Instance.GetAudioSorce("BGM");
       
        

    }
    bool flag = true;
    protected override void OnSliderValueChanged(string _widgetName, float _value)
    {
        if (flag) {
            
            cameraDefault = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraDefault>();
            flag = !flag;
        }
        switch (_widgetName)
        {
            case "BgSlider_M":
                bg.volume = _value;
                break;
            case "Sensitivity_M":
                cameraDefault.X_MouseSensitivity = _value;
                cameraDefault.Y_MouseSensitivity = _value;
                break;

        }

        // data.Bgvalue = _value;
    }

    protected override void OnClick(string _widgetName)
    {
        AudioMgr.Instance.Play("Click");
        switch (_widgetName)
        {
            case "SettingBtn_M":
                OpenSetting();
                break;
            case "Back_M":
                Back();
                break;
            case "Exit_M":
                ExitGame();
                break;
        }
    }
    private void Back()
    {
        ArchiveMgr.Instance.SaveArchive();
        GameManager.Instance.LoadNextScene("MianScene");

        UIMgr.Instance.ClosePanel(this.name);
    }
    public void OpenSetting()
    {
        var active = GetControl<Image>("Settingpage_M").IsActive();
        GetControl<Image>("Settingpage_M").gameObject.SetActive(!active);
    }

    public void SetHead(PlayerInfo info)
    {
        Sprite head;
        if (info.role_Type == E_Role_Type.Explorer)
            head = ResMgr.Instance.Load<Sprite>("Ty");
        else
            head = ResMgr.Instance.Load<Sprite>("Kaya");

        GetControl<Image>("Head_M").sprite = head;
    }

    public void OnHandlePropertyChange(PlayerInfo info)
    {
        GetControl<Text>("Money_M").text = info.Money.ToString();
        GetControl<Text>("LER_M").text = info.LER.ToString();
        GetControl<Text>("ATK_M").text = info.ATK.ToString();
        GetControl<Text>("DEF_M").text = info.DEF.ToString();

        float hp = (float)info.HP / 100f;
        GetControl<Image>("Hp_M").fillAmount = hp;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public override void Hied()
    {

        Evencenter.Instance.RemoveEventListener<PlayerInfo>(EventName.PLAYER_PROPERTY_CHANGE, OnHandlePropertyChange);
    }
}