using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 每个格子的信息 存储在IO上
public class GridInfo {
    // 是否探索过
    public bool IsSearch { get; set; }
    // 是否显现
    public bool IsShow { get; set; }
    // 是否可行走
    public bool IsWalk { get; set; }
    // 坐标 
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }

    public E_Grid_Type GridType { set; get; }
    //事件的类型 1~100
    public int EventId { set; get; }
    // 怪物的等级
    public int MosterRank { set; get; }

}