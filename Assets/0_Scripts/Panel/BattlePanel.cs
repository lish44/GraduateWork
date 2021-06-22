using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FW;
using UnityEngine.UI;
using DG.Tweening;

public class BattlePanel : PanelBase
{
    public int rank = 1;

    public override void Show()
    {
        SetInfo();
        var p = GetControl<Image>("PlayerPabel_M");
        p.transform.DOLocalMoveX(0, 3).From(-2);
        ChangeTextContent("Lv_M", "Lv." + rank);

        if (PlayerMgr.Instance.GetPlayerInfo().role_Type == E_Role_Type.Explorer)
        {
            GetControl<Image>("PHead_M").sprite = ResMgr.Instance.Load<Sprite>("Ty");
        }
        else
        {
            GetControl<Image>("PHead_M").sprite = ResMgr.Instance.Load<Sprite>("Kaya");
            
        }

        string mosterName = "m" + rank;
        GetControl<Image>("MHead_M").sprite = ResMgr.Instance.Load<Sprite>(mosterName);

    }

    protected override void OnClick(string _widgetName)
    {
        switch (_widgetName)
        {
            case "Battle_M":
                Battle();
                break;
            case "Cancle_M":
                Close();
                break;

        }
    }

    public void Battle()
    {
        
        var winner = GameEventMgr.Instance.Battle(PlayerMgr.Instance.GetPlayerInfo(), GameEventMgr.Instance.GetMoster(rank));
        AwardData award;
        if (winner == E_Winner.player)
        {
            award = GameEventMgr.Instance.GetAward(true, rank);
            //PlayerMgr.Instance.SetAwardOrDefeat(award);
        }
        else
        {
            //award = GameEventMgr.Instance.GetAward(false, rank);
            award = new AwardData() { HP = -GameEventMgr.Instance.GetMoster(rank).Damage };
        }
        UIMgr.Instance.ShowPanel<AwardPanel>("AwardPanel", E_UI_layer.Top, (panel) =>
        {
            panel.closeBg = panel.GetComponent<Image>();
            panel.VectoryOrDefeat = (winner == E_Winner.player);
            panel.data = award;
            panel.event_Type = E_Gmae_Event_Type.Battle;
        });
        UIMgr.Instance.ClosePanel(this.name);

    }

    public void Close()
    {
        UIMgr.Instance.ClosePanel(this.name);
        AwardData awardData = GameEventMgr.Instance.GetAward(false,rank);
        AwardData newdata = new AwardData()
        {
            ATK = awardData.ATK / 2,
            DEF = awardData.DEF / 2,
            LER = awardData.LER / 2,
            Money = awardData.Money / 2,
            HP = awardData.HP / 2
            
        };
       
        UIMgr.Instance.ShowPanel<AwardPanel>("AwardPanel", E_UI_layer.Top, (panel) =>
        {
            panel.closeBg = panel.GetComponent<Image>();
            panel.VectoryOrDefeat = false;
            panel.data = newdata;
            panel.event_Type = E_Gmae_Event_Type.Battle;
        });
    }

    public void SetInfo()
    {
        ChangeTextContent("PlayerAtk_M", PlayerMgr.Instance.GetATK().ToString());
        ChangeTextContent("PlayerDef_M", PlayerMgr.Instance.GetDef().ToString());
        ChangeTextContent("MosterAtk_M", GameEventMgr.Instance.GetMoster(rank).ATK.ToString());
        ChangeTextContent("MosterDef_M", GameEventMgr.Instance.GetMoster(rank).DEF.ToString());
    }
}