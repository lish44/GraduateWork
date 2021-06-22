using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickEvent : MonoBehaviour {
    public void Selection () {
        FW.AudioMgr.Instance.Play ("Selection");
    }

    public void Ensure () {

        FW.AudioMgr.Instance.Play ("Ensure");
    }

    public void Pk()
    {
        FW.AudioMgr.Instance.Play("Pk");
    }
    public void Xiu()
    {
        FW.AudioMgr.Instance.Play("Xiu");
    }
}