using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;

public class GameOverPanel : PanelBase {

    public bool isWin;

    public override void Show () {
        string content = isWin ? "胜利" : "失败";
        ChangeTextContent ("Content_M", content);
    }
    protected override void OnClick (string _widgetName) {
        switch (_widgetName) {
            case "Exit_M":
                Exit ();
                break;
            case "Again_M":
                Again ();
                break;
        }
    }

    public void Again () {
        ArchiveMgr.Instance.DeleteArchive ();
        UIMgr.Instance.ClosePanel (this.name);
        UIMgr.Instance.ClosePanel ("CharacterPanel");
        GameManager.Instance.LoadNextScene ("MianScene");
    }

    public void Exit () {
        ArchiveMgr.Instance.DeleteArchive ();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif

    }
}