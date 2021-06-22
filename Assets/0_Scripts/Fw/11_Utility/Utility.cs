﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

/// <summary>
/// 通用工具类
/// </summary>
namespace FW {
    public static class Utility {

        // 文件操作
        public struct FileOperation {

            /// <summary>
            /// 读取配置表所有内容
            /// </summary>
            /// <param name="_name">表名</param>
            /// <returns></returns>
            public static string GetConfigFileAllContent (string _name) {
                return File.ReadAllText (GetConfigFilePath (_name));
            }

            /// <summary>
            /// 获取配置表路径 - - txt文件 需要解析
            /// </summary>
            /// <param name="_name">表名</param>
            /// <returns>路径</returns>
            public static string GetConfigFilePath (string _name) {
                return Application.streamingAssetsPath + "/" + _name;
            }

            /// <summary>
            /// 从配置文件中一行一行读取供 handler操作
            /// </summary>
            /// <param name="_handler">解析操作的回调函数 每次对一行进行操作</param>
            public static void ReaderConfigFileToLine (string _fileName, System.Action<string> _handler) {
                var allContent = GetConfigFileAllContent (_fileName);
                using (StringReader reader = new StringReader (allContent)) {
                    string line = reader.ReadLine ();
                    while (line != null) {
                        _handler (line);
                        line = reader.ReadLine ();
                    }
                    // reader.Dispose(); // 用了using会自动释放退出
                } // 字符串读取器
            }

            /// <summary>
            /// 获取jsondata
            /// </summary>
            /// <param name="_configFileName">配置文件名</param>
            /// <returns>返回jsonData数据结构</returns>
            public static JsonData GetJsonData (string _configFileName) {
                //1. 用IO 把文本里面的json加载出来用 string 保存
                string path = GetConfigFilePath (_configFileName);
                StreamReader json = null;
                try {
                    json = File.OpenText (path);
                } catch (FileNotFoundException) {
                    Debug.Log (_configFileName + " : 文件不存在");
                    return null;
                }
                string input_json = json.ReadToEnd ();
                json.Close ();
                //2. 通过LitJson的提供的接口 把前面的string转化成JsonData对象
                try {
                    JsonReader jsonReader = new JsonReader (input_json);
                    JsonData data = JsonMapper.ToObject (jsonReader);
                    return data;
                } catch {
                    // Debug.Log (_configFileName + " : 不是json格式");
                    return null;
                }
            }

        }

        public struct TransformOperation {

            /// <summary>
            /// 设置子物体到父对象上
            /// </summary>
            public static void SetParent (Transform _childrenTrans, Transform _fatherTrans) {
                _childrenTrans.transform.SetParent (_fatherTrans);
                _childrenTrans.transform.localPosition = Vector3.zero;
                _childrenTrans.transform.localRotation = Quaternion.identity;
                _childrenTrans.transform.localScale = Vector3.one;

                var rect = _childrenTrans.transform as RectTransform;
                if (rect != null) {
                    rect.anchoredPosition = Vector3.zero;
                    rect.localRotation = Quaternion.identity;
                    rect.localScale = Vector3.one;
                    rect.offsetMax = Vector2.zero;
                    rect.offsetMin = Vector2.zero;
                }
            }

            /// <summary>
            /// Transform 重置Position、Rotation、Scale
            /// </summary>
            public static void ResetTransform (Transform trans) {
                trans.localRotation = Quaternion.identity;
                trans.localPosition = Vector3.zero;
                trans.localScale = Vector3.one;
            }

            /// <summary>
            /// 设置UI的大小 忽略Anchors是否重合
            /// </summary>
            public static void SetRectTransformSize (RectTransform trans, Vector2 newSize) {
                Vector2 oldSize = trans.rect.size;
                Vector2 deltaSize = newSize - oldSize;
                trans.offsetMin = trans.offsetMin - new Vector2 (deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
                trans.offsetMax = trans.offsetMax + new Vector2 (deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
            }

            public enum FacingDirection {
                UP = 270,
                DOWN = 90,
                LEFT = 180,
                RIGHT = 0
            }
            /// <summary>
            /// 取得兩點 Quaternion
            /// </summary>
            /// <param name="startingPosition">開始位置</param>
            /// <param name="targetPosition">目標位置</param>
            /// <param name="facing">方向</param>
            public static Quaternion LookAt2D (Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing) {
                Vector2 direction = targetPosition - startingPosition;
                float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
                angle -= (float) facing;

                return Quaternion.AngleAxis (angle, Vector3.forward);
            }

            /// <summary>
            /// 取得兩點 角度
            /// </summary>
            /// <param name="startingPosition">開始位置</param>
            /// <param name="targetPosition">目標位置</param>
            /// <param name="facing">方向</param>
            public static Vector3 LookAt2DAngle (Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing) {
                return LookAt2D (startingPosition, targetPosition, facing).eulerAngles;
            }
        }

        /// <summary>
        /// 单位坐标转换
        /// </summary>
        public struct ConvertUnit {
            public static Vector2Int V2 (Transform _t, float _val = 1f) {
                return new Vector2Int (
                    Mathf.RoundToInt ((_t.position.x / _val)),
                    Mathf.RoundToInt ((_t.position.z / _val))
                );
            }
            public static Vector2Int V2 (Vector3 _t, float _val = 1f) {
                return new Vector2Int (
                    Mathf.RoundToInt ((_t.x / _val)),
                    Mathf.RoundToInt ((_t.z / _val))
                );
            }
            public static Vector3Int V3 (Transform _t, float _val) {
                return new Vector3Int (
                    Mathf.RoundToInt ((_t.position.x / _val)),
                    Mathf.RoundToInt ((_t.position.y / _val)),
                    Mathf.RoundToInt ((_t.position.z / _val))
                );
            }
        }

    }
}