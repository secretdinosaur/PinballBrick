using System;
using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;

public class SpecialObjManager : MonoBehaviour
{
    public static SpecialObjManager Instance;
    public PoolManager PoolManager;

    public SpecialObjInfo SpecialObjInfo;

    void Awake()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void CreateSpecialObj(string type)
    {
        SpecialObj specialObj = GetSpecificSpecialObj(type);
    }

    private SpecialObj GetSpecificSpecialObj(string SpecialObjTypeName)
    {
        SpecialObj specialObj = null;
        
        Type specialObjType = Type.GetType(SpecialObjTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(specialObjType);
        specialObj = (SpecialObj)genericMethod.Invoke(PoolManager, null);
        return specialObj;
    }

    public void TakeSpecialObj(SpecialObj specialObj)
    {
        Type specialObjType = Type.GetType(specialObj.SpecialObjType);
        var method = typeof(PoolManager).GetMethod("TakeToPool");
        var genericMethod = method.MakeGenericMethod(specialObjType);
        genericMethod.Invoke(PoolManager, new object[] { specialObj.GetComponent<SpecialObj>() });
    }
}
