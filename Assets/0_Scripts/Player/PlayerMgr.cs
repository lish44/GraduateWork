using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;

public class PlayerMgr : SingletonBase<PlayerMgr> {

    PlayerInfo playerInfo;
    Camera camera;
    GameObject playerGO;

    public Player player;

    public override void Init () {

    }

    public void CreatePlayer () {
        camera = Camera.main;
        playerInfo = ArchiveMgr.Instance.DefaultArchive.playerInfo;
        if (playerInfo.role_Type == E_Role_Type.Explorer)
            playerGO = ResMgr.Instance.Load<GameObject> ("ty");
        else
            playerGO = ResMgr.Instance.Load<GameObject> ("kaya");

        // 设置player初始位置和大小
        if (playerInfo.x == 0 && playerInfo.y == 0 && playerInfo.z == 0) {
            playerGO.transform.position = new Vector3 (-.3f, 1.5f, -.3f);
            SetPostion (playerGO.transform.position);
        } else {
            playerGO.transform.position = new Vector3 ((float) playerInfo.x, (float) playerInfo.y, (float) playerInfo.z);
        }

        playerGO.transform.rotation = Quaternion.Euler (0, 45, 0);
        playerGO.transform.localScale = new Vector3 (.5f, .5f, .5f);

        player = playerGO.AddComponent<Player> ();
        // 添加头像上面的按钮
        var go = ResMgr.Instance.Load<GameObject> ("PlayerHead");
        go.transform.SetParent (playerGO.transform, false);
        MonoMgr.Instance.StartCoroutine (CameraActive ());
    }

    public void Starts () {

    }

    private IEnumerator CameraActive () {
        yield return new WaitForSeconds (1f);
        //camera.GetComponent<CameraDefault> ().enabled = true;
        camera.GetComponent<CameraDefault> ().TargetLookAt = playerGO.transform;
    }

    public void SetPostion (Vector3 playerPos) {
        playerInfo.x = playerPos.x;
        playerInfo.y = playerPos.y;
        playerInfo.z = playerPos.z;
    }

    // 设置状态
    public void SetAwardOrDefeat (AwardData data) {

        playerInfo.HP = Mathf.Clamp (playerInfo.HP += data.HP, 0, 100);
        playerInfo.ATK = Mathf.Clamp (playerInfo.ATK += data.ATK, 0, 999);
        playerInfo.DEF = Mathf.Clamp (playerInfo.DEF += data.DEF, 0, 999);
        playerInfo.LER = Mathf.Clamp (playerInfo.LER += data.LER, 0, int.MaxValue);
        playerInfo.Money = Mathf.Clamp (playerInfo.Money += data.Money, 0, int.MaxValue);

        // 死
        if (playerInfo.HP == 0) {
            player.playerFSM.ChangeState(E_FSM_State_Type.PlayerDead);
            Evencenter.Instance.EventTrigger<bool>(EventName.GAME_OVER,false);
            return;
        }
        // 状态改变
        Evencenter.Instance.EventTrigger<PlayerInfo> (EventName.PLAYER_PROPERTY_CHANGE, playerInfo);
    }

    // 切回待机
    public void OnHandleChangeIdle (float delay) {
        MonoMgr.Instance.StartCoroutine (ChangeIdle (delay));
    }

    public IEnumerator ChangeIdle (float delay) {
        yield return new WaitForSeconds (delay);
        player.playerFSM.ChangeState (E_FSM_State_Type.PlayerIdle);

    }

    public int GetMoney () {
        return playerInfo.Money;
    }
    public int GetATK () {
        return playerInfo.ATK;
    }
    public int GetDef () {
        return playerInfo.DEF;
    }
    public int GetLER () {
        return playerInfo.LER;
    }
    public PlayerInfo GetPlayerInfo () {
        return playerInfo;
    }

}