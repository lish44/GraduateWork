/**
2020-8-10 rehma
所有表对应的属性 写在这个类
表名和类命保持一致
*/
using System.Collections;
using System.Collections.Generic;
public class NamePathDataType {
	public string Name { get; set; }
	public string Path { get; set; }
}

public class PrefabDataType {
	public string Id { get; set; }
	public string Name { get; set; }
	public string Path { get; set; }
}

public class RoleBattleType {
	public int Lv { set; get; }
	public int STR { set; get; }
	public int DEX { set; get; }
	public int CON { set; get; }
	public int INT { set; get; }
	public int WIS { set; get; }
	public int CHA { set; get; }
	public int AC { set; get; }
	public int ATK { set; get; }
}

public class RoleBaseAttributeType {
	public int Lv { get; set; }
	public int HP { get; set; }
	public int Exp { get; set; }
	public int Money { get; set; }
	public int Rep { get; set; }
	public int Step { get; set; }
	public IEnumerator<int> GetEnumerator () {
		yield return Lv;
		yield return HP;
		yield return Exp;
		yield return Money;
		yield return Rep;
		yield return Step;
	}
}

public class RoleBaseBattleAttributeType {
	public int STR { set; get; }
	public int DEX { set; get; }
	public int CON { set; get; }
	public int INT { set; get; }
	public int WIS { set; get; }
	public int CHA { set; get; }
	public int AC { set; get; }
	public int ATK { set; get; }

	public IEnumerator<int> GetEnumerator () {
		yield return this.STR;
		yield return this.DEX;
		yield return this.CON;
		yield return this.INT;
		yield return this.WIS;
		yield return this.CHA;
		yield return this.AC;
		yield return this.ATK;
	}
}