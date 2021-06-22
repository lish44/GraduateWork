using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCell : PanelBase {
    public E_Game_Event_Ans ans;

    public override void Show () {

    }

    protected override void OnToggleChanged (string _widgetName, bool isSel) {
        if (isSel)
            Evencenter.Instance.EventTrigger<E_Game_Event_Ans> (EventName.GAME_EVENT_ANS, ans);
    }

    public void SetSeltionTitle (string content) {
        GetControl<Text> ("Label_M").text = content;
    }

}