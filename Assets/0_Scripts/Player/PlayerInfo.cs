/// <summary>
/// 角色的基础信息
/// </summary>
public class PlayerInfo {
    public E_Role_Type role_Type;
    public string Name { set; get; }
    public int HP { set; get; }
    public int Money { set; get; }
    public int ATK { set; get; }
    public int DEF { set; get; }
    public int LER { set; get; } //学识

    // 坐标 
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
}