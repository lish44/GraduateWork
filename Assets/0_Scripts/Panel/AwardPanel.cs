using System.Collections;
using System.Collections.Generic;
using System.Text;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class AwardPanel : PanelBase {
    public AwardData data;
    public bool VectoryOrDefeat;
    StringBuilder sb = new StringBuilder ();
    public E_Gmae_Event_Type event_Type;
    /// <summary>
    /// 用来点击任意位置后关闭奖励界面
    /// </summary>
    public Image closeBg;
    public override void Show () {
        UIMgr.Instance.AddCustomEventListner (closeBg, UnityEngine.EventSystems.EventTriggerType.PointerDown, (dataBase) => {
            UIMgr.Instance.ClosePanel (this.name);
        });

        // 根据胜负 放对应动画和音效
        if (VectoryOrDefeat) {
            PlayerMgr.Instance.player.playerFSM.ChangeState (E_FSM_State_Type.PlayerVictory);
            AudioMgr.Instance.Play ("Yes", false, 0.7f);
            AudioMgr.Instance.Play ("Victory", false, 0.7f);
        } else {

            AudioMgr.Instance.Play ("Defeat", false, 0.4f);
            PlayerMgr.Instance.player.playerFSM.ChangeState (E_FSM_State_Type.PlayerDefeat);
        }

        // 设置标题
        
        string titleContent = string.Empty;
        if (VectoryOrDefeat)
        {
            if (event_Type == E_Gmae_Event_Type.Qa) titleContent = "获得奖励";

            else titleContent = "战斗胜利";
        }
        else
        {
            if (event_Type == E_Gmae_Event_Type.Qa) titleContent = "回答错误";

            else titleContent = "战斗失败";
        }
        GetControl<Text>("TitleContnet").text = titleContent;

        SettingAwardContent ();
    }

    public void SettingAwardContent () {
        string atk = string.Format (
            "<color=#bf616a>攻击力：{0}</color>\n防御力：<color=#a3be8c>{1}</color>\n学识点：<color=#8497bd>{2}</color>\n金币：<color=#ebcb8b>{3}</color>\n血量：<color=red>{4}</color>\n",
            data.ATK.ToString (),
            data.DEF.ToString (),
            data.LER.ToString (),
            data.Money.ToString (),
            data.HP.ToString ()
        );
        string content = string.Format ("");
        GetControl<Text> ("AwardContent").text = atk;
    }

    // 关闭面板后 发放对应奖励处罚 
    public override void Hied () {
        PlayerMgr.Instance.SetAwardOrDefeat (data);
        // StartCoroutine (PlayerMgr.Instance.ChangeIdle ());
        PlayerMgr.Instance.OnHandleChangeIdle (0.5f);
        
    }

}