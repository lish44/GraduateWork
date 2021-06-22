using System.Collections;
using System.Collections.Generic;
using FW;
using UnityEngine;

public class CreateGrid : MonoBehaviour {

    private void Awake () {
        MapManager.Instance.InitGridToScene ();
        PlayerMgr.Instance.CreatePlayer ();
    }
}