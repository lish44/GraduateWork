using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FW;
using LitJson;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 地图的创建以及生成mesh的类      
/// </summary>
public class MapBuilders
{
    //public--

    //prive--
    int width, height;
    float offset;

    /// <summary>
    /// 场景中的grid坐标映射到对应的GridInfo 当相于addcompent
    /// </summary>
    Dictionary<Vector2Int, GridInfo> mapInfoDic;
    Dictionary<Vector3, HighlightableObject> hoDic;

    public MapBuilders(int _w, int _h, float _offset)
    {
        InitData(_w, _h, _offset);

        // CreateGridPostion ();
    }

    private void InitData(int _w, int _h, float _offset)
    {
        this.width = _w;
        this.height = _h;
        this.offset = _offset;

        mapInfoDic = new Dictionary<Vector2Int, GridInfo>();
        hoDic = new Dictionary<Vector3, HighlightableObject>();
    }

    // 创建地图信息 要写入配置表中去
    public List<GridInfo> CreateMapInfo()
    {
        List<GridInfo> gridInfoList = new List<GridInfo>();
        List<Vector3> pos = CreateGridPostion();
        int[] eventid = RandomGameEvent();
        for (int i = 0; i < pos.Count; ++i)
        {
            GridInfo gridInfo = new GridInfo();
            // 坐标存储
            gridInfo.x = pos[i].x;
            gridInfo.y = pos[i].y;
            gridInfo.z = pos[i].z;
            // 探索
            gridInfo.IsSearch = i == 0 ? true : false;
            // 显现
            gridInfo.IsShow = i < 3 ? true : false;

            // 都有路可走
            if (i < 6 || i > 93)
            {
                gridInfo.IsWalk = true;
                gridInfo.GridType = (E_Grid_Type)Random.Range(0, 6); //6~8号不可走地形
            }
            else
            {
                // 20分之一几率的格子不可走
                if (Random.Range(0, 10) == 0)
                {
                    gridInfo.IsWalk = false;
                    gridInfo.GridType = (E_Grid_Type)Random.Range(6, 9); //6~8号不可走地形
                }
                else
                {
                    gridInfo.IsWalk = true;
                    gridInfo.GridType = (E_Grid_Type)Random.Range(0, 6);
                }
            }

            // 事件
            gridInfo.EventId = eventid[i];
            //gridInfo.MosterRank = 1;
            // 怪物生成
            if (i < 30)
            {
                gridInfo.MosterRank = Random.Range(1, 4);
            }
            else if (i <= 30 && i < 70)
            {
                gridInfo.MosterRank = Random.Range(3, 7);
            }
            else
            {
                gridInfo.MosterRank = Random.Range(6, 10);
            }

            gridInfoList.Add(gridInfo);
        }
        // 最后一个的胜利
        gridInfoList[99].IsSearch = true;
        return gridInfoList;
    }

    private int[] RandomGameEvent()
    {
        int[] arr = new int[100];
        for (int i = 0; i < 100; ++i)
        {
            arr[i] = i;
        }
        int len = arr.Length;
        while (len > 0)
        {
            var index = Random.Range(0, len);
            var tmp = arr[index];
            arr[index] = arr[len - 1];
            arr[len - 1] = tmp;
            len--;
        }

        return arr;
    }

    // 创建grid的坐标准备写进存档的 
    private List<Vector3> CreateGridPostion()
    {
        List<Vector3> postionList = new List<Vector3>();
        // 得到全部grid的坐标 全部以v2int 存 方便计算
        List<Vector2Int> mapPoints = CalculateCoordinates(width, height);

        // 把二维变成三维
        Queue<int> queue = CalculateGridCount(10); // -> 12321
        int gridIndex = 0;
        int high = 1;
        while (queue.Count > 0)
        {
            int len = queue.Dequeue();
            for (int i = 0; i < len; ++i)
            {
                // 计算了高度 
                Vector3 girdPos = new Vector3(mapPoints[gridIndex].x * offset, high, mapPoints[gridIndex].y * offset);
                // Vector3 girdPos = new Vector3 (mapPoints[gridIndex].x * offset, (high - 3) + Random.Range (0f, .3f), mapPoints[gridIndex].y * offset);
                postionList.Add(girdPos);
                gridIndex++;
            }
            high++;
        }
        return postionList;
    }

    // 计算每层格子的个数
    private Queue<int> CalculateGridCount(int _high)
    {
        Queue<int> q = new Queue<int>();
        for (int k = 1; k <= _high; k++) q.Enqueue(k);
        for (int m = _high - 1; m > 0; m--) q.Enqueue(m);
        return q;
    }

    public void Mapping(List<GridInfo> _grids)
    {
        var mapPoints = _grids.GetEnumerator();
        while (mapPoints.MoveNext())
        {
            // 操作的地图格子的坐标  忽略y轴
            var gridPos = new Vector2Int((int)mapPoints.Current.x, (int)mapPoints.Current.z);
            if (!mapInfoDic.ContainsKey(gridPos))
                mapInfoDic.Add(gridPos, mapPoints.Current);
            else
            {
                mapInfoDic[gridPos] = mapPoints.Current;
            }
        }

    }

    // 拿到实例化格子到场景中 
    public IEnumerator CreateGridToScene(List<Vector3> _readyList)
    {
        for (int i = 0; i < _readyList.Count; ++i)
        {
            // 创建不同类型格子 mapInfoDic[]
            var gridInfo = GetGridInfo(_readyList[i]);
            var go = FW.ResMgr.Instance.Load<GameObject>(gridInfo.GridType.ToString());
            //如果不可走就不添加碰撞检测
            if ((int)gridInfo.GridType < 7)
                go.AddComponent<BoxCollider>();
            go.transform.position = new Vector3((float)_readyList[i].x, _readyList[i].y - 2, _readyList[i].z);
            var endPos = new Vector3((float)_readyList[i].x, (float)_readyList[i].y, (float)_readyList[i].z);
            // 格子出身的lerp动画
            go.transform.DOMove(endPos, 1);
            var ho = go.AddComponent<HighlightableObject>();
            if (!hoDic.ContainsKey(_readyList[i]))
            {
                hoDic.Add(_readyList[i], ho);
            }
            else
            {
                hoDic[endPos] = ho;
            }
            yield return new WaitForSeconds(.2f); // 每帧 等待一次
        }
    }

    /// <summary>
    /// 创建格子到场景中
    /// </summary>
    public IEnumerator Create(List<GridInfo> _grids)
    {
        // 得到格子的集合
        var gridInfo = _grids.GetEnumerator();
        while (gridInfo.MoveNext())
        {
            if (!gridInfo.Current.IsShow) continue;
            //  创不同的格子
            E_Grid_Type gridType = gridInfo.Current.GridType;
            var go = ResMgr.Instance.Load<GameObject>(gridType.ToString());
            go.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            var endPos = new Vector3((float)gridInfo.Current.x, (float)gridInfo.Current.y, (float)gridInfo.Current.z);
            // 格子出身的lerp动画
            go.transform.DOMove(endPos, 1);

            if ((int)gridInfo.Current.GridType < 7)
                go.AddComponent<BoxCollider>();

            var ho = go.AddComponent<HighlightableObject>();

            if (!hoDic.ContainsKey(endPos))
            {
                hoDic.Add(endPos, ho);
            }
            else
            {
                hoDic[endPos] = ho;
            }
            yield return null; // 每帧 等待一次
        }
    }

    /// <summary>
    /// 计算生成方式以及坐标 BFS算法 时间复杂度O(2n-2) 如 2*2的格子
    /// 遍历完后没找到end就是8次迭代(w*h*2) 找到后就是6次 (w*h*2-2)
    /// </summary>
    private List<Vector2Int> CalculateCoordinates(int _width, int _height)
    {
        Queue<Vector2Int> readySearch = new Queue<Vector2Int>();
        var start = Vector2Int.zero;
        var end = new Vector2Int(_width - 1, _height - 1);
        var alreadySearch = new List<Vector2Int>();
        readySearch.Enqueue(start);
        alreadySearch.Add(start);
        while (readySearch.Count > 0)
        {
            var curSearchCenter = readySearch.Dequeue();
            if (curSearchCenter == end) break;
            for (int i = 0, j = UR.Length; i < j; ++i)
            {
                // 带搜索格子
                var explorePos = curSearchCenter + UR[i];
                // 限制在 w*h 范围内
                if (explorePos.x <= _width - 1 &&
                    explorePos.y <= _height - 1 &&
                    !alreadySearch.Contains(explorePos))
                {
                    readySearch.Enqueue(explorePos);
                    alreadySearch.Add(explorePos);
                }
            }
        }
        return alreadySearch;
    }

    /// <summary>
    /// 输入当前格子坐标 输出以当前格子为点的周围需要显示的格子 然后格子就标记为显示
    /// </summary>
    public List<Vector3> ReadyShowGrid(Vector3 _currentPos)
    {
        List<Vector3> list = new List<Vector3>();
        // 精度可能丢失
        Vector2Int tmpPos = new Vector2Int((int)_currentPos.x, (int)_currentPos.z);
        Vector3 tmpv3 = Vector3.zero;
        for (int i = 0, j = UR.Length; i < j; ++i)
        {
            var explorePos = tmpPos + UR[i];

            if (!mapInfoDic.ContainsKey(explorePos)) continue;
            GridInfo gridInfo = mapInfoDic[explorePos];

            if (gridInfo.IsShow) continue;
            if (explorePos.x <= width - 1 &&
                explorePos.y <= height - 1)
            {
                tmpv3 = new Vector3((float)gridInfo.x, (float)gridInfo.y, (float)gridInfo.z);
                gridInfo.IsShow = true;
            }
            list.Add(tmpv3);

        }
        return list;
    }

    // 查看目标位置是属于整个集合
    public bool ContainsPostion(Vector3 _checkPostion)
    {
        return mapInfoDic.ContainsKey(new Vector2Int((int)_checkPostion.x, (int)_checkPostion.z));
    }
    // 查看目标位置是属于整个集合
    public bool ContainsPostion(Vector2Int _checkPostion)
    {
        return mapInfoDic.ContainsKey(_checkPostion);
    }

    // 根据坐标得到格子身上的具体组件 类似getcompent
    public GridInfo GetGridInfo(Vector3 _v3)
    {
        Vector2Int v2i = new Vector2Int((int)_v3.x, (int)_v3.z);
        if (mapInfoDic.ContainsKey(v2i))
        {
            return mapInfoDic[v2i];
        }
        return null;
    }
    // 根据坐标得到格子身上的具体组件 类似getcompent
    public GridInfo GetGridInfo(Vector2Int _v2)
    {
        if (mapInfoDic.ContainsKey(_v2))
        {
            return mapInfoDic[_v2];
        }
        return null;
    }

    public HighlightableObject GetHighlightableObj(Vector3 _v3)
    {
        return hoDic.ContainsKey(_v3) ? hoDic[_v3] : null;
    }



    Vector2Int[] UR = {
        Vector2Int.up,
        Vector2Int.right,
    };
    Vector2Int[] SIXDIR = {
        Vector2Int.left,
        new Vector2Int (-1, 1),
        Vector2Int.up,
        Vector2Int.right,
        new Vector2Int (1, -1),
        Vector2Int.down
    };

}