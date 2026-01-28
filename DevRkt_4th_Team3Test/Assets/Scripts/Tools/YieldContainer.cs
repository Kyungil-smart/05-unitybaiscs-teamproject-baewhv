using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldContainer
{
    public static readonly WaitForFixedUpdate WFFU = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> _WaitForSecondsDict = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_WaitForSecondsDict.ContainsKey(seconds))
        {
            _WaitForSecondsDict.Add(seconds, new WaitForSeconds(seconds));
        }
        return _WaitForSecondsDict[seconds];
    }
}
