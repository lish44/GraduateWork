using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FW;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour {


    void Update () {
        transform.LookAt (Camera.main.transform);
    }

   
    public LayerMask layer;

    private void OnMouseDown () {
        Ray ray = new Ray (transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 3, layer)) {
         
            if (PlayerMgr.Instance.player.playerFSM.GetCurState () == E_FSM_State_Type.PlayerIdle) {
                PlayerMgr.Instance.player.playerFSM.ChangeState (E_FSM_State_Type.PlayerLook);
                MapManager.Instance.HighLightArriveGrid (hit.transform.position);
            } else if (PlayerMgr.Instance.player.playerFSM.GetCurState () == E_FSM_State_Type.PlayerLook) {
                PlayerMgr.Instance.player.playerFSM.ChangeState (E_FSM_State_Type.PlayerIdle);
            }

            
        }
    }
}