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

    public static void UnregisterPage(Page page)
    {
        if (m_pages.Contains(page))
        {
            m_pages.Remove(page);
        }
    }
}
