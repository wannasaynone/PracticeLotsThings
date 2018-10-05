using UnityEngine;
using PracticeLotsThings.Manager;

namespace PracticeLotsThings.View
{
    using Manager = Manager.Manager;
    public abstract class View : UnityEngine.MonoBehaviour
    {
        protected virtual void Awake()
        {
            Manager.RegisterView(this);
        }

        protected virtual void OnDestroy()
        {
            Manager.UnregisterView(this);
        }
    }
}

