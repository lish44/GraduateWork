using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath {
    List<Vector3> pathList;
    // 缓存上一次显示的格子坐标
    List<Vector3> preCachePathList;
    public FindPath () {
        pathList = new List<Vector3> ();
        preCachePathList = new List<Vector3> ();
    }
    public List<Vector3> Find (Vector3 _v3) {
        pathList.Clear ();
        // 精度可能丢失
        Vector2Int tmpPos = new Vector2Int ((int) _v3.x, (int) _v3.z);
        Vector3 tmpv3 = Vector3.zero;
        for (int i = 0, j = DIR.Length; i < j; ++i) {
            // 往 上 下 左 右 左上 右下 六个方向搜 
            var explorePos = tmpPos + DIR[i];
            // 判断是否在有效的地图中没有就跳过
            if (!MapManager.Instance.ContainsPostion (explorePos)) continue;
            GridInfo gridInfo = MapManager.Instance.GetGridInfo (explorePos);
            // 判断格子是否显现和可行走
            if (!gridInfo.IsShow || !gridInfo.IsWalk) continue;
            tmpv3 = new Vector3 ((float) gridInfo.x, (float) gridInfo.y, (float) gridInfo.z);
            pathList.Add (tmpv3);
        }
        return pathList;
    }

    public void HighLightArriveGrid (Vector3 _v3) {
        // 如果是待机状态才可以高亮
        if (PlayerMgr.Instance.player.playerFSM.GetCurState () == E_FSM_State_Type.PlayerLook) {
            var paths = Find(_v3);
            preCachePathList = paths;
            foreach (var item in paths) {
                var ho = MapManager.Instance.GetHighlightableObj (item);
                ho.ConstantParams (Color.green);
                ho.ConstantOn ();
            }
        }
    }

    public void EliminateArriveGrid () {
        if (preCachePathList == null) return;
        foreach (var item in preCachePathList) {
            var ho = MapManager.Instance.GetHighlightableObj (item);
          
            ho.ConstantOff ();
        }
        preCachePathList.Clear ();
    }
    Vector2Int[] DIR = {
        Vector2Int.left,
        new Vector2Int (-1, 1),
        Vector2Int.up,
        Vector2Int.right,
        new Vector2Int (1, -1),
        Vector2Int.down
    };
}