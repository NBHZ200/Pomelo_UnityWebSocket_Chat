using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 延迟器，作用相当于原来 HeartBeatService 里边的那个 Timer
/// </summary>
public class Delayer : MonoBehaviour {

	public static Delayer Instance { get; private set; }
    private void Awake()
    {
        Instance = this.GetComponent<Delayer>();
    }
    Action action;
    /// <summary>
    /// 延迟执行
    /// </summary>
    /// <param name="second">延迟时间。单位：秒</param>
    /// <param name="action">函数</param>
    public void WaitAndInvoke(float second, Action action)
    {
        this.action = action;
        InvokeRepeating("WaitInvoke", 0, second);
    }
    /// <summary>
    /// 延迟执行实体函数
    /// </summary>
    private void WaitInvoke()
    {
        action.Invoke();
    }

    /// <summary>
    /// 停止延迟发送
    /// </summary>
    public void Stop()
    {
        StopCoroutine("WaitInvoke");
    }
}
