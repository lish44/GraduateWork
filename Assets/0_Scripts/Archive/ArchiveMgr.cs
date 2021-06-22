using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public class ArchiveMgr : SingletonBase<ArchiveMgr> {

    private string archiveConfigFilePath;

    public Archive DefaultArchive { get; set; }

    public override void Init () {
        archiveConfigFilePath = Application.persistentDataPath + "/ArchiveInfo.txt";
    }

    // 存到硬盘上
    public bool SaveArchive () {
        try {
            string json = JsonMapper.ToJson (DefaultArchive);
            // 转义一下
            string archiveInfo = System.Text.RegularExpressions.Regex.Unescape (json);
            // 覆盖写入
            File.WriteAllText (archiveConfigFilePath, archiveInfo);
            Debug.Log("保存成功");
            return true;
        } catch {
            Debug.Log("保存失败");
            return false;
        }
    }
    /// <summary>
    /// 从硬盘里读取 没有就返回false 有就返回true  并把存档加载出来 存到DefaultArchive
    /// </summary>
    public bool LoadArchive () {
        // 如果文件不存在 读取失败
        if (!File.Exists (archiveConfigFilePath)) return false;
        string contents = File.ReadAllText (archiveConfigFilePath);
        // 只要读取到了信息就给 DefaultArchive
        if (!contents.Equals (string.Empty)) {
            DefaultArchive = JsonMapper.ToObject<Archive> (contents);
            return true;
        }
        return false;
    }

    public void UpdateArchive (Archive archive) {
        this.DefaultArchive = archive;
    }

    // 新建存档
    public void CreateArchive (PlayerInfo playerInfo) {

        DefaultArchive = new Archive ();
        List<GridInfo> gridInfo = MapManager.Instance.mapBuilders.CreateMapInfo ();
        DefaultArchive.gridInfo = gridInfo;
        DefaultArchive.playerInfo = playerInfo;

        SaveArchive ();
    }

    public void DeleteArchive () {
        File.Delete (archiveConfigFilePath);
    }

}