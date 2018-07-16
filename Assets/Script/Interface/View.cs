using UnityEngine;

public abstract class View : MonoBehaviour {

    private void Awake()
    {
        Manager.RegisterView(this);
    }

    protected virtual void OnDestroy()
    {
        Manager.UnregisterView(this);
    }

}
