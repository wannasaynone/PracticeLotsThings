using System.Collections.Generic;
using UnityEngine;

namespace PracticeLotsThings.Manager
{
    using View = View.View;
    public abstract class Manager
    {
        private static List<View> m_views = new List<View>();

        public static void RegisterView(View view)
        {
            if (m_views.Contains(view))
            {
                Debug.LogWarningFormat("{0} was registered", view.GetType().Name);
                return;
            }

            m_views.Add(view);
        }

        public static void UnregisterView(View view)
        {
            if (!m_views.Contains(view))
            {
                Debug.LogWarningFormat("{0} was not registered but is trying to unregister", view.GetType().Name);
                return;
            }

            m_views.Remove(view);
        }

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

        protected List<View> GetViews<T>() where T : View
        {
            List<View> _views = new List<View>();

            for (int i = 0; i < m_views.Count; i++)
            {
                if (m_views[i] is T)
                {
                    _views.Add(m_views[i]);
                }
            }
            return _views;
        }
    }
}

