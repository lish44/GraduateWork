/**
    枚举集合
*/

/// <summary>
///  游戏运行状态
/// </summary>
public enum E_GameState {
    Ongoing,
    Stop,
    Over,
    Watch
}

/// <summary>
/// UI 层级
/// </summary>
public enum E_UI_layer {
    Bot,
    Mid,
    Top,
    System,
}

/// <summary>
/// 角色类型 Explorer：探险者 WiseMan：智者
/// </summary>
public enum E_Role_Type {
    Explorer,
    WiseMan
}

/// <summary>
/// 地形类型
/// </summary>
public enum E_Grid_Type {

    /// <summary>
    /// 森林
    /// </summary>
    Forset0,
    Forset1,
    Forset2,

    /// <summary>
    /// 沙漠 
    /// </summary>
    Desert3,
    Desert4,
    Desert5,

    /// <summary>
    /// 险峰 
    /// </summary>
    Perilouspeak6,
    Perilouspeak7,
    Perilouspeak8,
}

/// <summary>
/// 角色状态 
/// </summary>
public enum E_FSM_State_Type {
    PlayerIdle,
    PlayerVictory,
    PlayerJump,
    PlayerLook,
    PlayerDefeat,
    PlayerDead,
    None
}

/// <summary>
/// 事件
/// </summary>
public enum E_Gmae_Event_Type {
    Qa,
    Battle,
    Shop
}

public enum E_Game_Event_Ans {
    A,
    B,
    C,
    D
}