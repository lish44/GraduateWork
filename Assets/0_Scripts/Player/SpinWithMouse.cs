using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWithMouse : MonoBehaviour {
    private bool isClick = false; //是否按下
    private Vector3 nowPos; //当前鼠标的位置
    private Vector3 oldPos; //上一帧鼠标的位置
    public float length = 5;

    private void OnMouseUp () {
        isClick = false;
    }

    private void OnMouseDown () {
        isClick = true;
    }

    private void Update () {
        nowPos = Input.mousePosition;
        if (isClick) //鼠标按下不松手
        {
            Vector3 offset = nowPos - oldPos; //偏移量
            //x轴 > y轴，并且x的长度大于一定的距离，才可以旋转
            if (Mathf.Abs (offset.x) > Mathf.Abs (offset.y) && Mathf.Abs (offset.x) > length) {
                //进行旋转
                transform.Rotate (Vector3.up, -offset.x); //参数：1、沿Y轴旋转，2、旋转角度
            }
        }
        oldPos = Input.mousePosition;
    }

}