using System.Collections;
using System.Collections.Generic;
using System.Text;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class QustionPanel : PanelBase
{
    // 打开面板时传数据
    public QustionData data;
    private string[] selLetter = { "A", "B", "C", "D" };

    protected override void OnClick(string _widgetName)
    {
        switch (_widgetName)
        {
            case "Help_M":
                Help();
                break;
            case "Quit_M":
                Close();
                break;

        }
    }

    public E_Game_Event_Ans ans;
    public override void Show()
    {
        GetControl<Text>("Qustiontitle_M").text = data.qustion;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.selection.Count; i++)
        {
            sb.Append(selLetter[i]);
            sb.Append(". ");
            sb.Append(data.selection[i]);
            sb.Append("\n");
        }
        GetControl<Text>("Answer_M").text = sb.ToString();
        InitSelBtn();
        Evencenter.Instance.AddEventListener<E_Game_Event_Ans>(EventName.GAME_EVENT_ANS, OnHandleChangeAns);

        //设置答案 并隐藏
        GetControl<Text>("HelpAns_M").text = selLetter[data.ans];
        GetControl<Text>("HelpAns_M").enabled = false;

        //如果没有学士就隐藏按钮
        if (PlayerMgr.Instance.GetLER() == 0)
            GetControl<Button>("Help_M").gameObject.SetActive(false);

    }
    // 选择完成之后会被回调这个函数
    public void OnHandleChangeAns(E_Game_Event_Ans ans)
    {
        bool vd = data.ans == (int)ans;
        UIMgr.Instance.ClosePanel(this.name);
        //jiangli
        AwardData awardData = GameEventMgr.Instance.GetAward(vd, data.rank);
        UIMgr.Instance.ShowPanel<AwardPanel>("AwardPanel", E_UI_layer.Top, (panel) =>
        {
            panel.closeBg = panel.GetComponent<Image>();
            panel.VectoryOrDefeat = vd;
            awardData.LER = 0;
            panel.data = awardData;
            panel.event_Type = E_Gmae_Event_Type.Qa;
        });
    }

    public void InitSelBtn()
    {
        var fatherTG = GetControl<ToggleGroup>("AnsSubRoot_M");
        for (int i = 0; i < data.selection.Count; i++)
        {
            var cell = ResMgr.Instance.Load<GameObject>("ToggleCell");
            cell.GetComponent<ToggleCell>().SetSeltionTitle(selLetter[i]);
            cell.GetComponent<ToggleCell>().ans = (E_Game_Event_Ans)i;
            cell.GetComponent<Toggle>().group = fatherTG;
            Utility.TransformOperation.SetParent(cell.transform, fatherTG.transform);
        }

    }

    public void Help()
    {
        AudioMgr.Instance.Play("Click");
        if (PlayerMgr.Instance.GetLER() == 0) return;
        GetControl<Text>("HelpAns_M").enabled = true;
        PlayerMgr.Instance.SetAwardOrDefeat(new AwardData() { LER = -1 });
    }

    public override void Hied()
    {
        Evencenter.Instance.RemoveEventListener<E_Game_Event_Ans>(EventName.GAME_EVENT_ANS, OnHandleChangeAns);
    }

    public void Close()
    {
        AudioMgr.Instance.Play("Click");
        UIMgr.Instance.ClosePanel(this.name);

        AwardData awardData = GameEventMgr.Instance.GetAward(false, data.rank);
        AwardData newdata = new AwardData()
        {
            ATK = awardData.ATK / 2,
            DEF = awardData.DEF / 2,
            LER = 0,
            Money = awardData.Money / 2,
            HP = awardData.HP / 2
        };

        UIMgr.Instance.ShowPanel<AwardPanel>("AwardPanel", E_UI_layer.Top, (panel) =>
        {
            panel.closeBg = panel.GetComponent<Image>();
            panel.VectoryOrDefeat = false;
            panel.data = newdata;
            panel.event_Type = E_Gmae_Event_Type.Qa;
        });
        Debug.Log("类型是wenda奖励是" + data.rank);
    }

}