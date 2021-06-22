using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FW;
using UnityEngine.UI;
using System;

public class ShopPanel : PanelBase
{
    // 购买的东西的下标
    int index = -1;

    public List<ShopData> data;
    public override void Show()
    {

        data = DataMgr.Instance.LoadJsonData<List<ShopData>>("Shop.json");

        if (index == -1)
            GetControl<Button>("Buy_M").interactable = false;

        for (int i = 0; i < data.Count; i++)
        {
            var content = string.Format("价格：<color=#a3be8c>{0}</color> 加成：<color=#b48ead>{1}</color>", data[i].Price, data[i].val);
            ChangeTextContent(i + "_M", content);
        }

    }
    protected override void OnToggleChanged(string _widgetName, bool isSel)
    {
        AudioMgr.Instance.Play("Click");
        switch (_widgetName)
        {
            case "LERCell_M":
                index = 0;
                break;
            case "DEFCell_M":
                index = 1;
                break;
            case "ATKCel_M":
                index = 2;
                break;
            case "HPCell_M":
                index = 3;
                break;
        }

        int money = PlayerMgr.Instance.GetMoney();
        if ((money - data[index].Price) < 0)
        {

            GetControl<Button>("Buy_M").interactable = false;
            ChangeTextContent("BuyContent_M", "金币不足无法购买");

        }
        else
        {
            GetControl<Button>("Buy_M").interactable = true;
            ChangeTextContent("BuyContent_M", "购 买");

        }
    }

    protected override void OnClick(string _widgetName)
    {
        AudioMgr.Instance.Play("Click");
        switch (_widgetName)
        {
            case "Buy_M":
                Buy();
                break;
            case "Quit_M":
                Close();
                break;

        }
    }

    public void Buy()
    {

        AwardData award = new AwardData();

        switch (index)
        {
            case 0:
                award.LER = data[index].val;
                break;
            case 1:
                award.DEF = data[index].val;
                break;
            case 2:
                award.ATK = data[index].val;
                break;
            case 3:
                award.HP = data[index].val;
                break;
        }
        award.Money -= data[index].Price;
        PlayerMgr.Instance.SetAwardOrDefeat(award);
        Close();
    }

    public void Close()
    {
        UIMgr.Instance.ClosePanel(this.name);
    }


}

public class ShopData
{
    public int id;
    public int Price;
    public int val;//加多少
}