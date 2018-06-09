using UnityEngine;
using System.Collections.Generic;

public class UIManager {

    private static List<Page> m_pages = new List<Page>();

    public static void RegisterPage(Page page)
    {
        if(m_pages.Contains(page))
        {
            Debug.LogWarning(string.Format("{0} is registered", page.name));
            return;
        }

        m_pages.Add(page);
    }

    public static Page GetPage<T>() where T : Page
    {
       for(int i = 0; i < m_pages.Count; i++)
       {
           if (m_pages[i].GetType() == typeof(T))
            {
                return m_pages[i];
            }
       }

        return null;
    }

}
