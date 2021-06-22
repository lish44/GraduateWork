/// <summary>
/// 消息事件名
/// </summary>
public class EventName {

    /// <summary>
    /// loading界面
    /// </summary>
    public static readonly string LOADING = "Loading";

    /// <summary>
    /// loading界面加载完成
    /// </summary>
    public static readonly string LOADINGFINISH = "LoadingFinish";

    /// <summary>
    /// loading界面加载完成打开一个panel
    /// </summary>
    public static readonly string LOADEDOPENPANEL = "LoadedOpenPanel";

    /// <summary>
    /// 按键按下
    /// </summary>
    public static readonly string KEY_DOWN = "KeyDown";

    /// <summary>
    /// 按键持续按下
    /// </summary>
    public static readonly string KEY = "Key";

    /// <summary>
    /// 按键抬起
    /// </summary>
    public static readonly string KEY_UP = "KeyUp";

    /// <summary>
    /// 鼠标按键按下
    /// </summary>
    public static readonly string MOUSE_DOWN = "MouseDown";

    /// <summary>
    /// 鼠标按键持续按下
    /// </summary>
    public static readonly string MOUSE = "Mouse";

    /// <summary>
    /// 鼠标按键抬起
    /// </summary>
    public static readonly string MOUSE_UP = "MouseUp";

    /// <summary>
    /// 鼠标中间滑动
    /// </summary>
    public static readonly string MOUSE_SCROLLWHEEL = "MouseScrollWheel";

    /// <summary>
    /// 游戏状态
    /// </summary>
    public static readonly string GAME_STATE = "GameState";

    /// <summary>
    /// 游戏结束
    /// </summary>
    public static readonly string GAME_OVER = "GameOver";

    /// <summary>
    /// 改变属性的显示值
    /// </summary>
    public static readonly string CHANGE_PLAYER_ATTRIBUTE_CONTENT = "ChangePlayerAttributeContent";

    /// <summary>
    /// 改变血条或者经验
    /// </summary>
    public static readonly string CHANGE_PLAYER_ATTRIBUTE_HP_OR_EXP = "ChangePlayerAttributeHpOrExp";

    /// <summary>
    /// 最后一个格子生成完成后发出此事件
    /// </summary>
    public static readonly string FINISH_LAST_GRID = "FinishLastGrid";

    /// <summary>
    /// tips确定
    /// </summary>
    public static readonly string EN_SURE = "EnSure";

    /// <summary>
    /// 选择确定
    /// </summary>
    public static readonly string GAME_EVENT_ANS = "GameEventAns";

    /// <summary>
    /// 角色属性值发生变化
    /// </summary>
    public static readonly string PLAYER_PROPERTY_CHANGE = "PlayerPropertyChange";

    /// <summary>
    /// 角色到达方格触发方格事件
    /// </summary>
    public static readonly string GAME_EVENT_TRIGGER = "GameEventTrigger";

}