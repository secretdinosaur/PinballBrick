using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Redcode.Pools
{
    [Serializable]
    internal struct PoolData
    {
        [SerializeField]
        private string _name;

        public string Name => _name;

        [SerializeField]
        private Component _component;

        public Component Component => _component;

        [SerializeField]
        [Min(0)]
        private int _count;

        public int Count => _count;

        [SerializeField]
        private Transform _container;

        public Transform Container => _container;

        [SerializeField]
        private bool _nonLazy;

        public bool NonLazy => _nonLazy;
    }

    /// <summary>
    /// Pool manager. You can set options for it in editor and then use in game. <br/>
    /// It creates specified pools in Awake method, which then you can find with <b>GetPool</b> methods and call its methods.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<PoolData> _pools;

        private readonly List<IPool<Component>> _poolsObjects = new();

        private void Awake()
        {
            var namesGroups = _pools.Select(p => p.Name).GroupBy(n => n).Where(g => g.Count() > 1);

            if (namesGroups.Count() > 0)
                throw new Exception($"Pool Manager already contains pool with name \"{namesGroups.First().Select(g => g).First()}\"");

            var poolsType = typeof(List<IPool<Component>>);
            var poolsAddMethod = poolsType.GetMethod("Add");
            var genericPoolType = typeof(Pool<>);

            foreach (var poolData in _pools)
            {
                var poolType = genericPoolType.MakeGenericType(poolData.Component.GetType());
                var createMethod = poolType.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic);

                var pool = createMethod.Invoke(null, new object[] { poolData.Component, poolData.Count, poolData.Container });

                if (poolData.NonLazy)
                {
                    var nonLazyMethod = poolType.GetMethod("NonLazy");
                    nonLazyMethod.Invoke(pool, null);
                }

                poolsAddMethod.Invoke(_poolsObjects, new object[] { pool });
            }
        }

        public IPool<T> GetPool<T>() where T : Component => (IPool<T>)_poolsObjects.Find(p => p.Source is T);
        public T GetFromPool<T>() where T : Component => GetPool<T>().Get();
        public void TakeToPool<T>(Component component) where T : Component => GetPool<T>().Take(component);
    }
}
