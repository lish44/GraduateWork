using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 相当于PlayerMoter
/// </summary>
public class Player : MonoBehaviour {

    PlayerControl playerControl; // 用来控制点击设计

    public FSM playerFSM;

    private void Awake () {
        playerControl = new PlayerControl (this);
        FW.MonoMgr.Instance.AddStartListener (playerControl.Starts);
    }

    private void Start () {

        playerFSM = new FSM ();

        var idle = new PlayerIdle (E_FSM_State_Type.PlayerIdle, this);
        var victory = new PlayerVictory (E_FSM_State_Type.PlayerVictory, this);
        var jump = new PlayerJump (E_FSM_State_Type.PlayerJump, this);
        var look = new PlayerLook (E_FSM_State_Type.PlayerLook, this);
        var defeat = new PlayerDefeat (E_FSM_State_Type.PlayerDefeat, this);
        var Dead = new PlayerDead (E_FSM_State_Type.PlayerDead, this);

        playerFSM.RegistState (idle);
        playerFSM.RegistState (victory);
        playerFSM.RegistState (jump);
        playerFSM.RegistState (look);
        playerFSM.RegistState (defeat);
        playerFSM.RegistState (Dead);

        playerFSM.Go (idle);

        FW.MonoMgr.Instance.AddUpdateListener (playerControl.Updates);
    }

    public void Update () {
        playerFSM.UpdateState ();
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //playerFSM.ChangeState(E_FSM_State_Type.PlayerDead);
        //    PlayerMgr.Instance.SetAwardOrDefeat(new AwardData() { ATK = 100 });
        //    //FW.Evencenter.Instance.EventTrigger<bool>(EventName.GAME_OVER, false);
        //}
    }

    public void Jump (Vector3 target, float power) {
        playerFSM.ChangeState (E_FSM_State_Type.PlayerJump);
        transform.DOJump (target, power, 1, 1).SetDelay<Tween> (.5f).SetEase<Tween> (Ease.InSine).OnComplete<Tween> (JumpedCallBack);
    }

    Vector3 top = new Vector3 (9, 19, 9);
    public void JumpedCallBack () {
        playerFSM.ChangeState (E_FSM_State_Type.PlayerIdle);
        // 因为在PlayerControl 里面算了角色位置偏移 这里格子的位置要加回来
        Vector3 curGridPos = new Vector3 (transform.position.x + .3f, transform.position.y - .5f, transform.position.z + .3f);
        MapManager.Instance.ExploreAroundGrid (curGridPos);
        PlayerMgr.Instance.SetPostion (transform.position);
        GridInfo gridInfo = MapManager.Instance.GetGridInfo (curGridPos);
        GameEventMgr.Instance.OnHandleGameEvent (gridInfo);
        //  是终点
        if (curGridPos == top) {
            playerFSM.ChangeState (E_FSM_State_Type.PlayerVictory);
            FW.Evencenter.Instance.EventTrigger<bool> (EventName.GAME_OVER, true);
        }
    }

    public void Rotate (float angle) {

        transform.DORotate (new Vector3 (0, angle, 0), 0.1f);
    }

    private void OnDestroy () {
        FW.MonoMgr.Instance.RemoveStartListener (playerControl.Starts);
    }
}

class PlayerIdle : StateTemplates<Player> {

    public PlayerIdle (E_FSM_State_Type _state, Player _p) : base (_state, _p) { }

    public override void OnEnter (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Happy", true);
    }

    public override void OnStay (params object[] args) { }

    public override void OnExit (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Happy", false);
    }

}

class PlayerVictory : StateTemplates<Player> {
    public PlayerVictory (E_FSM_State_Type _state, Player _p) : base (_state, _p) { }

    public override void OnEnter (params object[] args) {

        owner.GetComponentInChildren<Animator> ().SetBool ("Victory", true);
    }

    public override void OnStay (params object[] args) { }

    public override void OnExit (params object[] args) {

        owner.GetComponentInChildren<Animator> ().SetBool ("Victory", false);
    }

}

class PlayerJump : StateTemplates<Player> {

    public PlayerJump (E_FSM_State_Type _state, Player _p) : base (_state, _p) { }

    public override void OnEnter (params object[] args) {

        owner.GetComponentInChildren<Animator> ().SetBool ("Jumping", true);
    }

    public override void OnStay (params object[] args) { }

    public override void OnExit (params object[] args) {

        owner.GetComponentInChildren<Animator> ().SetBool ("Jumping", false);
    }
}

class PlayerLook : StateTemplates<Player> {
    public PlayerLook (E_FSM_State_Type _state, Player _p) : base (_state, _p) { }

    public override void OnEnter (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Look", true);
    }

    public override void OnExit (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Look", false);
        MapManager.Instance.EliminateArriveGrid ();
    }

}
class PlayerDefeat : StateTemplates<Player> {
    public PlayerDefeat (E_FSM_State_Type _state, Player _p) : base (_state, _p) { }

    public override void OnEnter (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Defeat", true);
    }

    public override void OnExit (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Defeat", false);
    }

}
class PlayerDead : StateTemplates<Player> {
    public PlayerDead (E_FSM_State_Type _state, Player _p) : base (_state, _p) { }

    public override void OnEnter (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Die", true);
        FW.AudioMgr.Instance.Stop ();
    }

    public override void OnExit (params object[] args) {
        owner.GetComponentInChildren<Animator> ().SetBool ("Die", false);
    }

}