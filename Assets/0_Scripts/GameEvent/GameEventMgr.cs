using System.Collections;
using System.Collections.Generic;
using System.IO;
using FW;
using LitJson;
using UnityEngine;

public class GameEventMgr : SingletonBase<GameEventMgr> {
    Dictionary<int, QustionData> qustionDic;
    Dictionary<bool, Dictionary<int, List<AwardData>>> awardDic;
    List<MosterData> mosterList;
    List<AwardDataInfo> awardList;
    List<int[]> probability = new List<int[]> ();

    public override void Init () {
        var qustionDatas = DataMgr.Instance.LoadJsonData<List<QustionData>> ("GameEventQustion.json");
        qustionDic = new Dictionary<int, QustionData> ();
        foreach (var item in qustionDatas) {
            qustionDic.Add (item.id, item);
        }
        mosterList = DataMgr.Instance.LoadJsonData<List<MosterData>> ("Moster.json");
        awardList = DataMgr.Instance.LoadJsonData<List<AwardDataInfo>> ("Award.json");
        var probabilityData = DataMgr.Instance.LoadJsonData<List<ProbabilityData>> ("Probability.json");

        for (int i = 0; i < probabilityData.Count; i++) {
            probability.Add (new int[] { probabilityData[i].ratio1, probabilityData[i].ratio2 });

        }
    }

    // 拿一个问题
    public QustionData GetQustion (int id) {
        if (!qustionDic.ContainsKey (id)) return null;
        return qustionDic[id];
    }

    public AwardData GetAward (bool vd, int rank) {
        var tmpData = awardList.FindAll ((f) => {
            return f.vd == vd && f.rank == rank;
        });
        var Index = UnityEngine.Random.Range (0, tmpData.Count);
        return tmpData != null ? tmpData[Index].data : null;
    }

    public MosterData GetMoster (int rank) {
        return mosterList.Find ((f) => { return f.Rank == rank; });
    }

    public E_Winner Battle (PlayerInfo playerInfo, MosterData mosterData) {
        // 攻击后剩余的伤害
        int pdamage = playerInfo.ATK - mosterData.DEF;
        if (pdamage < 0) pdamage = 0;
        int mdamage = mosterData.ATK - playerInfo.DEF;
        if (mdamage < 0) mdamage = 0;
        bool res = (1000 - mdamage) >= (1000 - pdamage);
        List<E_Winner> winners = new List<E_Winner> () { E_Winner.player, E_Winner.moster };
        if (res) winners.Reverse ();
        return CalculatWeight<E_Winner> (probability[mosterData.Rank - 1], winners.ToArray ());
    }
    public T CalculatWeight<T> (int[] weightList, T[] content) {
        int weightIndex = -1;
        // 权重和
        int weightTotal = 0;
        for (int i = 0; i < weightList.Length; i++) {
            weightTotal += weightList[i];
        }
        int rand = Random.Range (1, weightTotal + 1);
        for (int i = 0; i < weightList.Length; i++) {
            if (rand <= weightList[i]) {
                weightIndex = i;
                break;
            }
            rand -= weightList[i];
        }
        weightIndex = weightIndex != -1 ? weightIndex : weightList.Length - 1;
        return content[weightIndex];
    }

    E_Gmae_Event_Type[] eventType = new E_Gmae_Event_Type[] {
        E_Gmae_Event_Type.Shop,
        E_Gmae_Event_Type.Qa,
        E_Gmae_Event_Type.Battle
    };

    /// <summary>
    /// 到达格子后事件触发
    /// </summary>
    int[] eventProbability = { 10, 45, 45 };
    public void OnHandleGameEvent (GridInfo gridInfo) {
        var happenEvent = CalculatWeight<E_Gmae_Event_Type> (eventProbability, eventType);
        if (gridInfo.IsSearch) return;
        gridInfo.IsSearch = true;
        MonoMgr.Instance.StartCoroutine (EventTrigger (happenEvent, gridInfo));
    }

    private IEnumerator EventTrigger (E_Gmae_Event_Type happenEvent, GridInfo gridInfo) {
        yield return new WaitForSeconds (.4f);
        if (happenEvent == E_Gmae_Event_Type.Qa) {
            UIMgr.Instance.ShowPanel<QustionPanel> ("QustionPanel", E_UI_layer.Top, (panel) => {
                panel.data = GameEventMgr.Instance.GetQustion (gridInfo.EventId);
            });
        } else if (happenEvent == E_Gmae_Event_Type.Shop) {
            UIMgr.Instance.ShowPanel<ShopPanel> ("ShopPanel");
        } else {
            UIMgr.Instance.ShowPanel<BattlePanel> ("BattlePanel", E_UI_layer.Top, (panel) => {
                panel.rank = gridInfo.MosterRank;
            });
        }

        //UIMgr.Instance.ShowPanel<BattlePanel>("BattlePanel", E_UI_layer.Top, (panel) =>
        //{
        //    panel.rank = gridInfo.MosterRank;
        //});

        //UIMgr.Instance.ShowPanel<ShopPanel>("ShopPanel");

    }

}

public class QustionData {
    public int id { set; get; }
    public string qustion { set; get; }
    public List<string> selection { set; get; }
    public int ans { set; get; }
    public int rank { set; get; }
}

public class AwardDataInfo {
    public bool vd { set; get; } // victory or defeat
    public int rank { set; get; }
    public AwardData data { set; get; }
}
public class AwardData {
    public int HP { set; get; }
    public int Money { set; get; }
    public int ATK { set; get; }
    public int DEF { set; get; }
    public int LER { set; get; }
}

public class MosterData {
    public int Rank; //等级
    public int ATK;
    public int DEF;
    public int Damage;
}
public enum E_Winner {
    player,
    moster
}
public class ProbabilityData {
    public int ratio1;
    public int ratio2;
}