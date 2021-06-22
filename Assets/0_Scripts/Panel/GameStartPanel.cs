using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : PanelBase {

    protected override void Awake () {
        base.Awake ();
    }

    public override void Show () {
        // 打开主界面 会去硬盘中找存档 如果没有 返回Flase 如果有 就会加载 到默认存档中 然后设置继续游戏按钮是否激活
        if (ArchiveMgr.Instance.LoadArchive ()) {
            GetControl<Button> ("ContinueGame_M").interactable = true;
        } else {
            GetControl<Button> ("ContinueGame_M").interactable = false;
        }
        FW.AudioMgr.Instance.Play ("BGM1", true, 1);
    }

    protected override void OnClick (string _widgetName) {
        switch (_widgetName) {
            case "NewGame_M":
                // StartGame ("GameScene");
                NewGame ();
                break;
            case "ContinueGame_M":
                ContinueGame ();
                break;
            case "Options_M":
                Option ();
                break;
            case "Quit_M":
                Quit ();
                break;
        }
    }

    public void NewGame () {
        FW.UIMgr.Instance.ShowPanel<RoleSelPanel> ("RoleSelPanel", E_UI_layer.Top, (panel) => { });
        UIMgr.Instance.ClosePanel ("GameStartPanel");
    }

    // 继续游戏获取玩家信息 给到CharacterPanel去显示 
    public void ContinueGame () {
        FW.UIMgr.Instance.ShowPanel<LoadingPanel> ("LoadingPanel", E_UI_layer.System, (panel) => {
            panel.NextSceneName = "GameScene";
            panel.speed = 4;
            panel.OpenPanel<CharacterPanel> ("CharacterPanel", E_UI_layer.Mid, (p) => {
                p.info = ArchiveMgr.Instance.DefaultArchive.playerInfo;
            });
        });
        UIMgr.Instance.ClosePanel (this.name);
    }

    public void Option () {
        // FW.UIMgr.Instance.ShowPanel<OptionPanel> ("OptionPanel", E_UI_layer.Top, (panel) => { });
        // UIMgr.Instance.ClosePanel ("GameStartPanel");
    }

    public override void Hied () {
        FW.AudioMgr.Instance.Stop ("BGM1");
    }

    public void Quit () {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}