using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;

public class GameManager : SingletonMono<GameManager> { // 执行优先级 awake -> Init -> start

    private void Awake () { }

    private void Start () {
        FW.Evencenter.Instance.AddEventListener<bool> (EventName.GAME_OVER, OnHandleGameOver);
    }
    public void LoadNextScene (string _nextSceneName) {
        FW.UIMgr.Instance.ShowPanel<LoadingPanel> ("LoadingPanel", E_UI_layer.System, (panel) => {
            panel.NextSceneName = _nextSceneName;
            panel.speed = 4;
            panel.OpenPanel<GameStartPanel> ("GameStartPanel");
        });
    }

    public void OnHandleGameOver (bool isWin) {
        FW.UIMgr.Instance.ShowPanel<GameOverPanel> ("GameOverPanel", E_UI_layer.Top, panel => {
            panel.isWin = isWin;
        });
    }
   

    private void OnApplicationQuit () {
        ArchiveMgr.Instance.SaveArchive ();
    }

}