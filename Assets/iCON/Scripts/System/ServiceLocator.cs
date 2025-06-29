using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ServiceLocatorパターンの実装
/// グローバルサービスとローカルサービスを管理する
/// </summary>
[DefaultExecutionOrder(-1000)]
public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator _globalInstance; // グローバルサービスのためのインスタンス
    private static ServiceLocator _localInstance; // ローカルサービスのためのインスタンス
    
    private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    /// <summary>
    /// グローバルサービス
    /// </summary>
    public static ServiceLocator Global
    {
        get
        {
            if (_globalInstance == null) CreateGlobalServiceLocator();
            return _globalInstance;
        }
    }

    /// <summary>
    /// ローカルサービス
    /// </summary>
    public static ServiceLocator Local
    {
        get
        {
            if (_localInstance == null) CreateLocalServiceLocator();
            return _localInstance;
        }
    }

    #region Lifecycle

    private void Awake()
    {
        // インスタンスの重複チェックと適切な割り当て
        if (_globalInstance == null && gameObject.name.Contains("Global"))
        {
            _globalInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_localInstance == null && gameObject.name.Contains("Local"))
        {
            _localInstance = this;
        }
        else if (this != _globalInstance && this != _localInstance)
        {
            Debug.LogWarning("予期しないServiceLocatorインスタンスが検出されました。重複インスタンスを破棄します。");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // シーンインスタンスがDestroyされた場合の処理
        if (this == _localInstance)
        {
            _localInstance = null;
            Debug.Log("ローカルServiceLocatorが破棄されました。");
        }
    }

    #endregion
    
    /// <summary>
    /// 外部からサービスロケータに登録する時に呼ばれるメソッド
    /// </summary>
    public static void Resister<T>(T service, ServiceType serviceType = ServiceType.Global) where T : class
    {
        if (serviceType == ServiceType.Global) Global.ResisterService(service);
        else Local.ResisterService(service);
    }
    
    /// <summary>
    /// サービスの登録を解除する
    /// </summary>
    public static void Unregister<T>(ServiceType serviceType = ServiceType.Global) where T : class
    {
        if (serviceType == ServiceType.Global)
        {
            Global?.UnregisterService<T>();
        }
        else
        {
            Local?.UnregisterService<T>();
        }
    }
    
    /// <summary>
    /// サービスを取得（まずシーン、次にグローバルから検索）
    /// </summary>
    public static T Get<T>() where T : class
    {
        // まずシーンのServiceLocatorから検索
        if (_localInstance != null && _localInstance.TryGetService<T>(out T sceneService))
        {
            return sceneService;
        }

        // 次にグローバルのServiceLocatorから検索
        if (_globalInstance != null && _globalInstance.TryGetService<T>(out T globalService))
        {
            return globalService;
        }

        Debug.LogError($"型 {typeof(T).Name} のサービスがどのServiceLocatorからも見つかりませんでした。");
        return null;
    }
    
    /// <summary>
    /// グローバルサービスを直接取得
    /// </summary>
    public static T GetGlobal<T>() where T : class
    {
        if (_globalInstance != null && _globalInstance.TryGetService<T>(out T service))
        {
            return service;
        }

        Debug.LogError($"型 {typeof(T).Name} のグローバルサービスが見つかりませんでした。");
        return null;
    }

    /// <summary>
    /// ローカルサービスを直接取得
    /// </summary>
    public static T GetLocal<T>() where T : class
    {
        if (_localInstance != null && _localInstance.TryGetService<T>(out T service))
        {
            return service;
        }

        Debug.LogError($"型 {typeof(T).Name} のローカルサービスが見つかりませんでした。");
        return null;
    }
    
    /// <summary>
    /// 指定された型のサービスが登録されているかを確認する
    /// </summary>
    /// <typeparam name="T">確認したいサービスの型</typeparam>
    /// <param name="serviceType">確認するサービスの種類（Global/Local）</param>
    /// <returns>登録されている場合はtrue、されていない場合はfalse</returns>
    public static bool IsRegistered<T>(ServiceType serviceType = ServiceType.Global) where T : class
    {
        if (serviceType == ServiceType.Global)
        {
            return _globalInstance?.IsServiceRegistered<T>() ?? false;
        }
        else
        {
            return _localInstance?.IsServiceRegistered<T>() ?? false;
        }
    }

    /// <summary>
    /// 指定された型のサービスがグローバルに登録されているかを確認する
    /// </summary>
    public static bool IsRegisteredGlobal<T>() where T : class
    {
        return _globalInstance?.IsServiceRegistered<T>() ?? false;
    }

    /// <summary>
    /// 指定された型のサービスがローカルに登録されているかを確認する
    /// </summary>
    public static bool IsRegisteredLocal<T>() where T : class
    {
        return _localInstance?.IsServiceRegistered<T>() ?? false;
    }

    /// <summary>
    /// 指定された型のサービスがどこかに登録されているかを確認する（Local優先でGlobalも検索）
    /// </summary>
    /// <typeparam name="T">確認したいサービスの型</typeparam>
    /// <returns>登録されている場合はtrue、されていない場合はfalse</returns>
    public static bool IsRegisteredAnywhere<T>() where T : class
    {
        // まずローカルを確認
        if (_localInstance?.IsServiceRegistered<T>() == true)
        {
            return true;
        }
    
        // 次にグローバルを確認
        return _globalInstance?.IsServiceRegistered<T>() ?? false;
    }

    /// <summary>
    /// 登録されているサービスをログ出力
    /// </summary>
    public void LogRegisteredServices()
    {
        Debug.Log($"=== {gameObject.name} Services ===");
        if (_services.Count == 0)
        {
            Debug.Log("登録されているサービスはありません。");
        }
        else
        {
            foreach (var kvp in _services)
            {
                Debug.Log($"- {kvp.Key.Name}: {kvp.Value}");
            }
        }
    }

    /// <summary>
    /// 全てのサービスをクリア
    /// </summary>
    public void ClearAllServices()
    {
        _services.Clear();
        Debug.Log($"{gameObject.name} から全てのサービスをクリアしました");
    }

    #region Private Method

    private static void CreateGlobalServiceLocator()
    {
        var go = new GameObject("[Global ServiceLocator]");
        _globalInstance = go.AddComponent<ServiceLocator>();
        DontDestroyOnLoad(go);
    }

    private static void CreateLocalServiceLocator()
    {
        var go = new GameObject("[Local ServiceLocator]");
        _localInstance = go.AddComponent<ServiceLocator>();
    }
    
    /// <summary>
    /// サービスロケーター登録の内部メソッド
    /// </summary>
    private void ResisterService<T>(T service) where T : class
    {
        var type = typeof(T);
        
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"{type.Name} は既にサービスロケーターに登録されています。上書きを行います");
        }

        _services[type] = service;
    }
    
    /// <summary>
    /// サービスの登録を解除する内部メソッド
    /// </summary>
    private void UnregisterService<T>() where T : class
    {
        var type = typeof(T);
        
        if (!_services.Remove(type))
        {
            Debug.LogWarning($"{type.Name} がサービスロケーター内で見つかりませんでした。登録解除できません");
        }
    }
    
    /// <summary>
    /// 指定された型のサービスの取得を試行する
    /// </summary>
    private bool TryGetService<T>(out T service) where T : class
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out object obj))
        {
            service = obj as T;
            return service != null;
        }

        service = null;
        return false;
    }
    
    /// <summary>
    /// 指定された型のサービスの登録を確認する内部メソッド
    /// </summary>
    /// <typeparam name="T">確認したいサービスの型</typeparam>
    private bool IsServiceRegistered<T>() where T : class
    {
        var type = typeof(T);
        return _services.ContainsKey(type);
    }

    #endregion
}