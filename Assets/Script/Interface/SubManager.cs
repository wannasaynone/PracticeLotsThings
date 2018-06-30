using System.Collections.Generic;
using UnityEngine;

public abstract class SubManager {

    private static Dictionary<Page, List<SubManager>> m_pageToSubManagers;

    public SubManager(Page page)
    {
        if(page == null)
        {
            Debug.LogError("SubManager MUST set a page into. SubManager=" + ToString() + " page is null");
            return;
        }

        if(m_pageToSubManagers == null)
        {
            m_pageToSubManagers = new Dictionary<Page, List<SubManager>>();
        }

        if(m_pageToSubManagers.ContainsKey(page))
        {
            if(m_pageToSubManagers[page] == null)
            {
                m_pageToSubManagers[page] = new List<SubManager>();
            }
            m_pageToSubManagers[page].Add(this);
        }
        else
        {
            m_pageToSubManagers.Add(page, new List<SubManager>() { this });
        }
    }

    public static List<string> GetAllSubManagerNameInPage<T>() where T : Page
    {
        List<Page> keys = new List<Page>(m_pageToSubManagers.Keys);
        for(int _keyIndex = 0; _keyIndex < keys.Count; _keyIndex++)
        {
            if(keys[_keyIndex].GetType() == typeof(T))
            {
                List<string> result = new List<string>();
                for(int _sunManagerIndex = 0; _sunManagerIndex < m_pageToSubManagers[keys[_keyIndex]].Count; _sunManagerIndex++)
                {
                    result.Add(m_pageToSubManagers[keys[_keyIndex]][_sunManagerIndex].GetType().Name);
                }
                return result;
            }
        }
        return null;
    }

    public abstract void Update();
}
