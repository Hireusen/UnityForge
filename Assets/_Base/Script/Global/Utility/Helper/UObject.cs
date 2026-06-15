using UnityEngine;

/// <summary>
/// 방어 코드가 포함된 게임 오브젝트를 다루는 유틸리티 클래스입니다.
/// </summary>
public static class UObject
{
    #region ─────────────────────────▶ 게임 오브젝트 ◀─────────────────────────
    /// <summary>
    /// 캔버스 간 이동할 때 오프셋이 어긋나는 것을 해결하기 위해 오프셋을 리셋합니다.
    /// </summary>
    /// <param name="rectTr">리셋할 렉트 트랜스폼 컴포넌트</param>
    public static void ResetRect(RectTransform rectTr)
    {
        if (rectTr != null) {
            rectTr.localScale = Vector3.one;
            rectTr.offsetMin = Vector2.zero;
            rectTr.offsetMax = Vector2.zero;
        }
    }

    /// <summary>
    /// 빈 게임 오브젝트를 생성합니다.
    /// </summary>
    /// <param name="name">이름</param>
    /// <param name="parent">부모 오브젝트</param>
    /// <param name="worldPosStays">월드 좌표 유지 여부</param>
    public static GameObject Create
        (string name, Transform parent = null, bool worldPosStays = false)
    {
        GameObject go = new GameObject(name);
        if (parent != null) {
            go.transform.SetParent(parent, worldPosStays);
        }
        return go;
    }

    /// <summary>
    /// 프리팹을 생성합니다.
    /// </summary>
    /// <param name="prefab">프리펩 게임 오브젝트</param>
    /// <param name="parent">부모 오브젝트</param>
    /// <param name="worldPosStays">월드 좌표 유지 여부</param>
    public static GameObject Spawn
        (GameObject prefab, Transform parent = null, bool worldPosStays = false)
    {
        if (prefab == null) return null;

        return UnityEngine.Object.Instantiate(prefab, parent, worldPosStays);
    }

    /// <summary>
    /// 프리팹을 생성합니다.
    /// </summary>
    /// <param name="prefab">프리펩 게임 오브젝트</param>
    /// <param name="position">좌표</param>
    /// <param name="rotation">회전</param>
    /// <param name="parent">부모 오브젝트</param>
    public static GameObject Spawn
        (GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (prefab == null) return null;

        return UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
    }

    /// <summary>
    /// 프리팹을 생성합니다.
    /// </summary>
    /// <param name="prefab">프리펩 게임 오브젝트</param>
    /// <param name="position">좌표</param>
    /// <param name="rotation">회전</param>
    /// <param name="parent">부모 오브젝트</param>
    /// <returns></returns>
    public static T Spawn<T>
        (T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        where T : UnityEngine.Object
    {
        if (prefab == null) return null;

        return UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
    }

    /// <summary>
    /// 게임 오브젝트를 파괴합니다.
    /// </summary>
    /// <param name="go">게임 오브젝트</param>
    /// <param name="delay">시간이 지난 후 파괴(초)</param>
    public static void Destroy(GameObject go, float delay = 0f)
    {
        if (go == null) return;

        UnityEngine.Object.Destroy(go, delay);
    }

    /// <summary>
    /// 게임 오브젝트를 파괴합니다.
    /// </summary>
    /// <param name="component">컴포넌트</param>
    /// <param name="delay">시간이 지난 후 파괴(초)</param>
    public static void Destroy<T>(T component, float delay = 0f) where T : Component
    {
        if (component == null) return;

        UObject.Destroy(component.gameObject, delay);
    }

    /// <summary>
    /// 특정 트랜스폼의 모든 자식을 파괴합니다.
    /// </summary>
    /// <param name="parent">트랜스폼</param>
    public static void DestroyChildren(Transform parent, float delay = 0)
    {
        if (parent == null) return;

        int length = parent.childCount;
        for (int i = length - 1; i >= 0; --i) {
            UObject.Destroy(parent.GetChild(i).gameObject, delay);
        }
    }

    /// <summary>
    /// 게임 오브젝트를 활성화 또는 비활성화합니다.
    /// </summary>
    /// <param name="go">게임 오브젝트</param>
    /// <param name="isActive">활성화 여부</param>
    public static void SetActive(GameObject go, bool isActive)
    {
        if (go == null) return;
        if (go.activeSelf == isActive) return;

        go.SetActive(isActive);
    }

    /// <summary>
    /// 게임 오브젝트의 부모를 설정합니다.
    /// </summary>
    /// <param name="go">게임 오브젝트</param>
    /// <param name="parent">부모 트랜스폼</param>
    public static void SetParent(GameObject go, Transform parent, bool worldPosStays = false)
    {
        if (go == null) return;

        UObject.SetParent(go.transform, parent, worldPosStays);
    }

    /// <summary>
    /// 트랜스폼의 부모를 설정합니다.
    /// </summary>
    /// <param name="tr">트랜스폼</param>
    /// <param name="parent">부모 트랜스폼</param>
    public static void SetParent(Transform tr, Transform parent, bool worldPosStays = false)
    {
        if (tr == null) return;

        tr.SetParent(parent, worldPosStays);
    }

    /// <summary>
    /// 트랜스폼의 좌표, 회전, 크기를 초기화합니다.
    /// </summary>
    /// <param name="tr">트랜스폼</param>
    public static void ResetTransform(Transform tr)
    {
        if (tr == null) return;

        tr.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        tr.localScale = Vector3.one;
    }

    /// <summary>
    /// 씬 전체에서 특정 게임 오브젝트를 찾습니다.
    /// </summary>
    /// <param name="name">게임 오브젝트</param>
    public static GameObject Find(string name)
    {
        return GameObject.Find(name);
    }
    #endregion

    #region ─────────────────────────▶ 컴포넌트 ◀─────────────────────────
    /// <summary>
    /// 컴포넌트를 부착하며 중복으로 붙이지 않습니다.
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    /// <param name="go">게임 오브젝트</param>
    public static T AddComponent<T>(GameObject go) where T : Component
    {
        if (go == null) return null;

        if (!go.TryGetComponent(out T component)) {
            component = go.AddComponent<T>(); // 컴포넌트가 없을 경우 부착
        }
        return component;
    }

    /// <summary>
    /// 게임 오브젝트의 컴포넌트를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    /// <param name="go">게임 오브젝트</param>
    public static T GetComponent<T>(GameObject go) where T : Component
    {
        if (go == null) return null;

        return go.GetComponent<T>();
    }

    /// <summary>
    /// 게임 오브젝트 또는 자식에게서 컴포넌트를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    /// <param name="go">게임 오브젝트</param>
    public static T GetComponentInChildren<T>(GameObject go) where T : Component
    {
        if (go == null) return null;

        return go.GetComponentInChildren<T>();
    }

    /// <summary>
    /// 씬에서 특정 게임 오브젝트의 특정 컴포넌트를 찾습니다.
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    /// <param name="name">게임 오브젝트</param>
    /// <returns>컴포넌트</returns>
    public static T FindComponent<T>(string name) where T : Component
    {
        GameObject go = Find(name);
        if (go == null) return null;

        return go.GetComponentInChildren<T>();
    }

    /// <summary>
    /// 씬에서 특정 게임 오브젝트의 특정 컴포넌트를 찾습니다.
    /// True → 비활성화된 오브젝트도 검색
    /// False → 활성화된 오브젝트만 검색
    /// </summary>
    /// <returns>컴포넌트</returns>
    public static T FindComponent<T>(bool inactive = true) where T : Component
    {
        return Object.FindAnyObjectByType<T>
            (inactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
    }

    /// <summary>
    /// 씬에서 특정 게임 오브젝트의 특정 컴포넌트를 모두 찾습니다.
    /// True → 비활성화된 오브젝트도 검색
    /// False → 활성화된 오브젝트만 검색
    /// </summary>
    /// <returns>컴포넌트</returns>
    public static T[] SearchAnyComponent<T>
        (bool inactive = true, FindObjectsSortMode sortMode = FindObjectsSortMode.None) where T : Component
    {
        return Object.FindObjectsByType<T>
            (inactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, sortMode);
    }

    /// <summary>
    /// 특정 컴포넌트를 파괴합니다.
    /// </summary>
    /// <param name="component">컴포넌트</param>
    /// <param name="delay">시간이 지난 후 파괴(초)</param>
    public static void DestroyComponent<T>(T component, float delay = 0f) where T : Component
    {
        if (component == null) return;

        UnityEngine.Object.Destroy(component, delay);
    }

    /// <summary>
    /// 게임오브젝트에서 특정 컴포넌트를 파괴합니다.
    /// </summary>
    /// <typeparam name="T">컴포넌트</typeparam>
    /// <param name="go">게임 오브젝트</param>
    public static void DestoryComponent<T>(GameObject go) where T : Component
    {
        if (go == null) return;
        if (!go.TryGetComponent(out T component)) return;

        UObject.DestroyComponent(component);
    }
    #endregion
}
