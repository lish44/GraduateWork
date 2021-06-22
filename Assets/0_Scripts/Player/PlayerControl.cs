using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl {

    //Player player;
    RayCasterCheck rayCasterCheck;

    public PlayerControl (Player _player) {
        //this.player = _player;
        rayCasterCheck = new RayCasterCheck ();
    }

    public void Starts () {

        rayCasterCheck.OnRayEnter += Enter;
        rayCasterCheck.OnRayStay += Stay;
    }

    public void Updates () {
        HitObj ();
        rayCasterCheck.CastMouseRay ();
    }

    public void Enter (Collider _col) {
        Debug.Log (_col.name);
    }
    public void Stay (Collider _col) {
        Debug.Log (_col.name);
    }
    public void HitObj () {
        if (Input.GetMouseButtonDown (0)) {
            
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                if (hit.collider.tag.Equals ("Player")) {
                    return;
                }
                Vector3 pos = new Vector3 (hit.transform.position.x - .3f, hit.transform.position.y + .5f, hit.transform.position.z - .3f);

                if (PlayerMgr.Instance.player.playerFSM.GetCurState () == E_FSM_State_Type.PlayerLook) {
                    var curGridPos = new Vector3 (PlayerMgr.Instance.player.transform.position.x + .3f, PlayerMgr.Instance.player.transform.position.y - .5f, PlayerMgr.Instance.player.transform.position.z + .3f);

                    if (MapManager.Instance.FindPath (curGridPos).Find (g => { return g == hit.transform.position; }) != Vector3.zero) {

                        PlayerMgr.Instance.player.Jump (pos, 1f);
                    }
                }
            }
        }
    }
}