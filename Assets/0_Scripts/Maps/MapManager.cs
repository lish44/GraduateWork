using System.Collections;
using System.Collections.Generic;
using FW;
using LitJson;
using UnityEngine;

/// <summary>
/// 地图管理器
/// </summary>
public class MapManager : SingletonBase<MapManager>
{




    public MapBuilders mapBuilders;
    public FindPath findPath;
    public List<GridInfo> grids;
    public override void Init()
    {
        InitData();

    }

    private void InitData()
    {
        mapBuilders = new MapBuilders(10, 10, 1f);
        findPath = new FindPath();
        grids = new List<GridInfo>();
    }

    /// <summary>
    /// 进入游戏主场景后加载存档中的信息并创建
    /// </summary>
    public void InitGridToScene()
    {
        grids.Clear();
        grids = null;
        // 获取存档中的grid信息
        grids = ArchiveMgr.Instance.DefaultArchive.gridInfo;
        // 然后映射
        mapBuilders.Mapping(grids);

        MonoMgr.Instance.StartCoroutine(mapBuilders.Create(grids));

    }

    /// <summary>
    /// 传入当前位置 生成周围的格子 用于回答完成 进行下一轮探索
    /// </summary>
    public void ExploreAroundGrid(Vector3 targetPos)
    {
        var ls = mapBuilders.ReadyShowGrid(targetPos);
        MonoMgr.Instance.StartCoroutine(mapBuilders.CreateGridToScene(ls));
    }

    public bool ContainsPostion(Vector3 _checkPostion)
    {
        return mapBuilders.ContainsPostion(_checkPostion);
    }
    public bool ContainsPostion(Vector2Int _checkPostion)
    {
        return mapBuilders.ContainsPostion(_checkPostion);
    }

    // 根据坐标得到格子身上的具体组件 类似Getcompent
    public GridInfo GetGridInfo(Vector3 _v3)
    {
        return mapBuilders.GetGridInfo(_v3);
    }

    public GridInfo GetGridInfo(Vector2Int _v2)
    {
        return mapBuilders.GetGridInfo(_v2);
    }

    public HighlightableObject GetHighlightableObj(Vector3 _v3)
    {
        return mapBuilders.GetHighlightableObj(_v3);
    }

    public void HighLightArriveGrid(Vector3 _v3)
    {
        findPath.HighLightArriveGrid(_v3);
    }

    public void EliminateArriveGrid()
    {
        findPath.EliminateArriveGrid();
    }

    public List<Vector3> FindPath(Vector3 _v3)
    {
        return findPath.Find(_v3);
    }

}