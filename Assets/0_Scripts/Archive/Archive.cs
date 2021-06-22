using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archive {
    // 地图信息
    public List<GridInfo> gridInfo;
    // 角色信息
    public PlayerInfo playerInfo;
    //设置信息
    // public SettingData settingData;

    public Archive () {
        gridInfo = new List<GridInfo> ();
    }
}