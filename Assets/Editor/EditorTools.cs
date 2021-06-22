using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEngine;
public class EditorTools : Editor {

    [MenuItem ("Tools/GenerateSpriteConfig")]
    public static void GenerateSpriteConfig () {
        CreateConfig ("Sprite", new string[] { "Assets" }, "SpriteCfg");
    }

    [MenuItem ("Tools/GenerateAudioConfig")]
    public static void GenerateAudioConfig () {
        CreateConfig ("AudioClip", new string[] { "Assets" }, "AudioCfg");
    }

    [MenuItem ("Tools/GeneratePrefabConfig")]
    public static void GeneratePrefabConfig () {
        CreatePrefab ("Prefab", new string[] { "Assets" }, "PrefabCfg");
    }

    /// <summary>
    /// 资源路径map成 json -> name : path
    /// </summary>
    /// <param name="_type">资源类型：prefab MP3 Sprite...</param>
    /// <param name="_SearchScope">搜索范围：Assets/Resources</param>
    /// <param name="_OutFileName">输出的文件名</param>
    /// <param name="_OutPath">输出路径默认: Assets/StreamingAssets/</param>
    public static void CreateConfig (string _type, string[] _SearchScope, string _OutFileName, string _OutPath = "Assets/StreamingAssets/") {
        //1. 找resource下所有 asset 列子找prefab
        //两个重载 1选择器需要加载的类型资源 2. 待搜索文件夹
        //GUID: unity下资源编号
        var GUID = AssetDatabase.FindAssets ("t:" + _type, _SearchScope);
        if (GUID.Length == 0) {
            Debug.Log ("error!!");
            return;
        }
        //转成资源的完整路径 从Asset开始
        int len = GUID.Length;
        string[] res = new string[len];
        string[] json = new string[len + 2];
        json[0] = "[";
        json[len + 1] = "]";
        for (int i = 0; i < len; i++) {
            res[i] = AssetDatabase.GUIDToAssetPath (GUID[i]);
            //2. 生成对应关系 名称 = 路径
            string fileName = Path.GetFileNameWithoutExtension (res[i]);
            // 去后缀名字
            string filePath = res[i].Replace ("Assets/Resources/", string.Empty);
            int startIndex = filePath.LastIndexOf (".");
            filePath = filePath.Remove (startIndex, filePath.Length - startIndex);
            // string filePath = res[i].Replace ("Assets/Resources", string.Empty).Replace ("." + _type, string.Empty);

            // res[i] = fileName + "=" + filePath;
            var c = i == len - 1 ? "" : ",";
            json[i + 1] = string.Format ("{0}\"Name\":\"{1}\"{2}\"Path\":\"{3}\"{4}{5}", "{", fileName, ",", filePath, "}", c);

        }
        //写入文件
        File.WriteAllLines (_OutPath + _OutFileName + ".txt", json);
        AssetDatabase.Refresh ();
        Debug.Log ("succeed!!! Path is ：" + _OutPath + _OutFileName);
    }

    // prefab路径生成 json格式
    public static void CreatePrefab (string _type, string[] _SearchScope, string _OutFileName, string _OutPath = "Assets/StreamingAssets/") {
        var GUID = AssetDatabase.FindAssets ("t:" + _type, _SearchScope);
        int len = GUID.Length;
        if (len == 0) {
            Debug.Log ("error!!");
            return;
        }
        var res = new string[len];
        List<string> list = new List<string> () { "[" };
        for (int i = 0; i < len; ++i) {
            PrefabDataType data = new PrefabDataType ();
            data.Id = i.ToString ();
            res[i] = AssetDatabase.GUIDToAssetPath (GUID[i]);
            data.Name = Path.GetFileNameWithoutExtension (res[i]);
            // 去后缀名字
            string filePath = res[i].Replace ("Assets/Resources/", string.Empty);
            int startIndex = filePath.LastIndexOf (".");
            data.Path = filePath.Remove (startIndex, filePath.Length - startIndex);

            string str = JsonMapper.ToJson (data);
            if (i != len - 1)
                list.Add (str + ",");
            else
                list.Add (str);
        }
        list.Add ("]");
        File.WriteAllLines (_OutPath + _OutFileName + ".txt", list.ToArray ());
        AssetDatabase.Refresh ();
        Debug.Log ("succeed!!! Path is ：" + _OutPath + _OutFileName);

    }

    // 所有配置表路径
    [MenuItem ("Tools/GenerateAllConfigfile")]
    public static void GenerateAllConfigfile () {

        var path = Application.dataPath + "/streamingAssets";
        if (Directory.Exists (path)) {
            DirectoryInfo direction = new DirectoryInfo (path);
            // 
            FileInfo[] files = direction.GetFiles ("*.txt", SearchOption.AllDirectories);

            List<string> l = new List<string> ();
            NamePathDataType data = new NamePathDataType ();
            for (int i = 0; i < files.Length; i++) {
                if (files[i].Name.EndsWith (".meta")) {
                    continue;
                }

                l.Add (files[i].Name);
                data.Name = files[i].Name;
                data.Path = files[i].DirectoryName;
                // Debug.Log ("Name : " + files[i].Name);
                // Debug.Log ("FullName : " + files[i].FullName);
                // Debug.Log ("DirectoryName : " + files[i].DirectoryName);
            }
            File.WriteAllLines ("Assets/StreamingAssets/AllConfigrationFile.txt", l.ToArray ());
            AssetDatabase.Refresh ();
        }
    }

    // // 读配置表生成对应类
    // public static void Gen (string _configName, string _outName) {
    //     string filename = _outName;
    //     string path = FW.Tools.FileOperation.GetConfigFilePath (_configName);

    //     try {
    //         string line;
    //         using (System.IO.StreamReader sr = new System.IO.StreamReader (path, Encoding.Default)) {
    //             // 设置生成位置
    //             string tarDir = Application.dataPath + "/0_Scripts/";
    //             if (!Directory.Exists (tarDir)) {
    //                 Debug.Log ("路径不存在");
    //                 return;
    //             }
    //             string filePath = tarDir + filename + ".cs";
    //             List<string> content = new List<string> ();
    //             content.Add ("public class " + filename + " {\n");
    //             line = sr.ReadLine ();
    //             line = sr.ReadLine ();
    //             string[] vs = line.Split (',');
    //             for (int i = 0, j = vs.Length - 1; i < j; ++i) {
    //                 int start = vs[i].IndexOf ('"') + 1;
    //                 int end = vs[i].IndexOf ('"', vs[i].IndexOf ('"') + 1) - start;
    //                 string typeName = vs[i].Substring (start, end);
    //                 content.Add ("\tpublic " + "int " + typeName + " { set; get; }");
    //             }
    //             sr.Close ();
    //             content.Add ("\n}");
    //             File.WriteAllLines (filePath, content);
    //             Debug.Log ("succeed!!! Path is ：" + filename);
    //         }
    //     } catch {
    //         Debug.Log ("转化错误");
    //     }
    // }

}

