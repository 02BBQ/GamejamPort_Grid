using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public interface IPoolable
{
    public void OnSpawned();
    public void OnDespawned();
}

public static class PoolManager
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        Application.quitting += () => { OnExiting?.Invoke(); };
        SceneManager.sceneUnloaded += _ => { OnSceneUnloaded?.Invoke(); };
    }

    private static readonly Dictionary<GameObject, Pool> Pools = new();

    private static event Action OnExiting;
    private static event Action OnSceneUnloaded;

    /// <summary>
    ///     오브젝트를 풀에서 가져옵니다.
    /// </summary>
    /// <param name="prefab">프리팹</param>
    /// <param name="parent">부모</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <returns>풀링된 오브젝트</returns>
    public static T Get<T>(T prefab, Transform parent = null) where T : Object
    {
        var pool = Pool<T>.Instance;
        return pool.Get(prefab, parent);
    }

    /// <summary>
    ///     오브젝트를 풀에서 가져옵니다.
    /// </summary>
    /// <param name="prefab">프리팹</param>
    /// <param name="position">위치</param>
    /// <param name="rotation">회전</param>
    /// <param name="parent">부모</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <returns>풀링된 오브젝트</returns>
    public static T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
    {
        var pool = Pool<T>.Instance;
        var obj = pool.Get(prefab, parent);

        var gameObject = GetGameObject(obj);
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    /// <summary>
    ///     오브젝트를 풀에 반환합니다.
    /// </summary>
    /// <param name="obj">반환할 오브젝트</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    public static void Release<T>(T obj) where T : Object
    {
        var gameObject = GetGameObject(obj);

        if (!Pools.TryGetValue(gameObject, out var pool))
        {
            Pool<GameObject>.Instance.Release(gameObject);
            return;
        }

        pool.Release(gameObject);
    }

    /// <summary>
    ///     오브젝트를 풀에 미리 생성합니다.
    /// </summary>
    /// <param name="prefab">프리팹</param>
    /// <param name="count">생성할 개수</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    public static void Preload<T>(T prefab, int count) where T : Object
    {
        var pool = Pool<T>.Instance;
        for (var i = 0; i < count; i++)
        {
            var obj = pool.Get(prefab);
            pool.Release(obj);
        }
    }

    /// <summary>
    ///     수동으로 모든 풀을 비웁니다.
    /// </summary>
    public static void Clear()
    {
        foreach (var pool in Pools.Values)
        {
            pool.ClearPool();
        }

        Pools.Clear();
    }

    private static GameObject GetGameObject<T>(T prefab) where T : Object
    {
        return prefab as GameObject ?? (prefab as Component)?.gameObject;
    }

    private abstract class Pool
    {
        public abstract void Release(GameObject gameObject);
        public abstract void ClearPool();
    }

    private class Pool<T> : Pool where T : Object
    {
        private readonly Dictionary<GameObject, T> _prefabs = new();
        private readonly Dictionary<GameObject, T> _components = new();
        private readonly Dictionary<T, Stack<T>> _stacks = new();
        private readonly Dictionary<GameObject, IPoolable[]> _poolables = new();

        static Pool()
        {
            OnExiting += Instance.ClearPool;
            OnSceneUnloaded += Instance.ClearPool;
            Debug.Log("PoolManager Initialized - " + typeof(T));
        }

        public static Pool<T> Instance { get; } = new();

        public T Get(T prefab, Transform parent = null)
        {
            if (!_stacks.TryGetValue(prefab, out var stack))
            {
                stack = new Stack<T>();
                _stacks.Add(prefab, stack);
            }

            T obj;
            GameObject gameObject;

            if (stack.Count > 0)
            {
                obj = stack.Pop();
                gameObject = GetGameObject(obj);

                if (GetGameObject(prefab).activeSelf)
                    gameObject.SetActive(true);
            }
            else
            {
                obj = Object.Instantiate(prefab, parent);
                gameObject = GetGameObject(obj);
                _prefabs.Add(gameObject, prefab);
                _components.Add(gameObject, obj);
                _poolables.Add(gameObject, gameObject.GetComponentsInChildren<IPoolable>(true));
                Pools.Add(gameObject, this);
            }

            gameObject.transform.SetParent(parent, false);
            gameObject.hideFlags = HideFlags.None;

            foreach (var poolable in _poolables[gameObject])
            {
                poolable.OnSpawned();
            }

            return obj;
        }

        public void Release(T obj)
        {
            Release(GetGameObject(obj));
        }

        public override void Release(GameObject gameObject)
        {
            if (!_prefabs.TryGetValue(gameObject, out var prefab))
            {
                Object.Destroy(gameObject);
                return;
            }

            if (_poolables.TryGetValue(gameObject, out var poolables))
            {
                foreach (var poolable in poolables)
                {
                    poolable.OnDespawned();
                }
            }

            if (gameObject != null) gameObject.SetActive(false);
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            _stacks[prefab].Push(_components[gameObject]);
        }

        public override void ClearPool()
        {
            foreach (var obj in _stacks.Values.SelectMany(stack => stack))
            {
                Object.Destroy(GetGameObject(obj));
            }

            _stacks.Clear();
            _prefabs.Clear();
            _poolables.Clear();
        }
    }
}