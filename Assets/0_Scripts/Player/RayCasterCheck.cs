using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCasterCheck {
    // 定义三个事件，用来定义不同行为的不同相应
    public event System.Action<Collider> OnRayEnter;
    public event System.Action<Collider> OnRayStay;
    public event System.Action<Collider> OnRayExit;
    // 用来保存上一个射线扫到的物体
    Collider previousCollider;
    public void CastMouseRay () {
        if (EventSystem.current.IsPointerOverGameObject ()) {
            return;
        }
        // 从摄像机向鼠标位置发射一束射线，并返回相关信息
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast (ray, out hitInfo, 100);
        // 处理返回的信息
        CollisionProcess (hitInfo.collider);
    }

    private void CollisionProcess (Collider current) {
        // 执行射线离开物体触发的事件
        // 如果在当前帧中，射线没有碰撞到任何物体，并且在记录中保存到碰撞体不为空，就触发该物体的结束射线碰撞事件
        if (current == null) {
            if (previousCollider != null) {
                OnRayExit?.Invoke (previousCollider);
            }
        }

        // 射线停留在某个物体时触发的事件
        // 如果当前射线碰撞的物体和上一个射线碰撞到物体相同，则触发射线停留事件
        else if (previousCollider == current) {
            OnRayStay?.Invoke (current);
        }

        // 射线更新事件
        // 如果当前射线和上一个射线碰撞的物体不是同一个，则上一个物体触发射线离开事件，当前物体触发射线碰撞事件
        else if (previousCollider != null) {
            OnRayEnter?.Invoke (current);
            OnRayExit?.Invoke (previousCollider);
        }
        // 如果之前帧中没有射线碰撞的物体，当前帧有射线碰撞到物体，则触发此事件。
        else {
            // no collider on last frame
            OnRayEnter?.Invoke (current);
        }

        // 将当前射线碰撞的物体保存为 上一个碰撞的物体
        previousCollider = current;
    }

}