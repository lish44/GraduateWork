using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelPanel : FW.PanelBase {

    PlayerInfo playerInfo;

    // 保存游戏人物 用来进行选择时展示
    List<GameObject> playerSelection;

    public override void Show () {
        playerInfo = new PlayerInfo ();
        playerSelection = new List<GameObject> ();
        LoadPrefab ();
    }

    // 加载两个模型选人物时用
    public void LoadPrefab () {
        for (int i = 0; i < 2; i++) {
            GameObject go = null;
            if (i == 0)
                go = ResMgr.Instance.Load<GameObject> ("ty");
            else
                go = ResMgr.Instance.Load<GameObject> ("kaya");
            go.transform.position = new Vector3 (0, -0.5f, -5);
            go.transform.rotation = Quaternion.Euler (0, 180f, 0);
            go.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
            go.SetActive (false);
            var boxcol = go.AddComponent<BoxCollider> ();
            boxcol.size = new Vector3 (1, 2.5f, 1);
            boxcol.center = new Vector3 (0, 1, 0);
            go.AddComponent<SpinWithMouse> ();
            playerSelection.Add (go);
        }

        playerSelection[0].SetActive (true);
    }

    public override void Hied () {
        for (int i = playerSelection.Count - 1; i >= 0; i--) {
            Destroy (playerSelection[i]);
        }
    }

    protected override void Awake () {
        base.Awake ();
    }

    protected override void OnClick (string _widgetName) {
        AudioMgr.Instance.Play("Click");
        switch (_widgetName) {
            case "Ensure_M":
                Ensure ();
                break;
            case "LeftBtn_M":
                LeftBtn ();
                break;
            case "RightBtn_M":
                RightBtn ();
                break;
            case "Back_M":
                Back ();
                break;
        }
    }

    protected override void onEndEdit (string _widgetName, string _content) {
        playerInfo.Name = _content;

        if (playerInfo.Name == "") {
            GetControl<Button> ("Ensure_M").interactable = false;
        } else {

            GetControl<Button> ("Ensure_M").interactable = true;
        }
    }

    bool roleTypeFlag;
    private void ChangeRoleType () {
        string name = roleTypeFlag ? "探险者" : "智者";
        ChangeTextContent ("TypeName_M", name);
        E_Role_Type role_Type = roleTypeFlag ? E_Role_Type.Explorer : E_Role_Type.WiseMan;
        playerInfo.role_Type = role_Type;

        // 模型切换
        for (int i = 0; i < playerSelection.Count; i++) {
            if ((int) role_Type == i) {
                playerSelection[i].SetActive (true);
            } else {
                playerSelection[i].SetActive (false);
            }
        }

        roleTypeFlag = !roleTypeFlag;
    }

    // 确定进入游戏 开始构建地图和事件
    public void Ensure () {
        // 加载基本信息
        var datas = DataMgr.Instance.LoadJsonData<List<CharacterBaseData>> ("CharacterBaseData.json");

        var data = datas.Find (f => { return f.type == playerInfo.role_Type; });
        playerInfo.Money = data.Money;
        playerInfo.LER = data.LER;
        playerInfo.ATK = data.ATK;
        playerInfo.DEF = data.DEF;
        playerInfo.HP = 100;
        ArchiveMgr.Instance.CreateArchive (playerInfo);

        // 加载面板完毕后打开CharacterPanel
        FW.UIMgr.Instance.ShowPanel<LoadingPanel> ("LoadingPanel", E_UI_layer.System, (panel) => {
            panel.NextSceneName = "GameScene";
            panel.speed = 5;
            panel.OpenPanel<CharacterPanel> ("CharacterPanel", E_UI_layer.Mid, (p) => {
                p.info = playerInfo;
            });
        });
        UIMgr.Instance.ClosePanel (this.name);

    }

    // 向左选择角色 
    public void LeftBtn () {
        ChangeRoleType ();
        
    }
    // 向右选择角色 
    public void RightBtn () {
        ChangeRoleType ();
        
    }

    public void Back () {
        UIMgr.Instance.ClosePanel (this.name);
        UIMgr.Instance.ShowPanel<GameStartPanel> ("GameStartPanel");
        
    }

}

/// <summary>
/// 角色出生识的基本信息
/// </summary>
public class CharacterBaseData {
    public E_Role_Type type { set; get; }
    public int Money { set; get; }
    public int ATK { set; get; }
    public int DEF { set; get; }
    public int LER { set; get; }
}