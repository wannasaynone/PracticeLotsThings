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

    public abstract void Update();

    public static List<string> GetAllSubManagerNameInPage<T>() where T : Page
    {
        List<Page> keys = new List<Page>(m_pageToSubManagers.Keys);
        for(int i = 0; i < keys.Count; i++)
        {
            if(keys[i].GetType() == typeof(T))
            {
                List<string> result = new List<string>();
                for(int j = 0; j < m_pageToSubManagers[keys[i]].Count; j++)
                {
                    result.Add(m_pageToSubManagers[keys[i]][j].GetType().Name);
                }
                return result;
            }
        }
        return null;
    }
}
