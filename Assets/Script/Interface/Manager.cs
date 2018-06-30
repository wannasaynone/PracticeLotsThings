using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager {

    private static List<Page> m_pages = new List<Page>();
    private List<SubManager> m_subManagers = new List<SubManager>();

#if UNITY_EDITOR
    public Manager()
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        for (int i = 0; i < stackTrace.GetFrames().Length; i++)
        {
            if (stackTrace.GetFrames()[i].GetMethod().Name == "Awake")
            {
                Debug.LogError("Manager CAN'T be created in any Awake() function");
            }
        }
    }
#endif

    protected void RegisterSubManager(SubManager subManager)
    {
        if (!m_subManagers.Contains(subManager))
        {
            m_subManagers.Add(subManager);
        }
        else
        {
            Debug.LogWarning(string.Format("sub manager {0} is registered", subManager));
        }
    }

    protected void UnregisterSubManager(SubManager subManager)
    {
        if (m_subManagers.Contains(subManager))
        {
            m_subManagers.Remove(subManager);
        }
    }

    protected T GetSubManager<T>() where T : SubManager
    {
        for (int i = 0; i < m_subManagers.Count; i++)
        {
            if (m_subManagers[i].GetType() == typeof(T))
            {
                return (T)m_subManagers[i];
            }
        }

        return default(T);
    }

    public static void RegisterPage(Page page)
    {
        if (m_pages.Contains(page))
        {
            Debug.LogWarning(string.Format("page {0} is registered", page.name));
            return;
        }

        m_pages.Add(page);
    }

    public static void UnregisterPage(Page page)
    {
        if (m_pages.Contains(page))
        {
            m_pages.Remove(page);
        }
    }

    public T GetPage<T>() where T : Page
    {
        for (int i = 0; i < m_pages.Count; i++)
        {
            if (m_pages[i].GetType() == typeof(T))
            {
                return (T)m_pages[i];
            }
        }

        return default(T);
    }

    public virtual void Update()
    {
        for (int i = 0; i < m_subManagers.Count; i++)
        {
            m_subManagers[i].Update();
        }
    }
}
