using UnityEngine;

public abstract class View : MonoBehaviour {

    protected virtual void Awake()
    {
        Manager.RegisterView(this);
    }

    protected virtual void OnDestroy()
    {
        Manager.UnregisterView(this);
    }

}
