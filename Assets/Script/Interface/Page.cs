using UnityEngine;

public abstract class Page : MonoBehaviour {

    protected virtual void Awake()
    {
        Manager.RegisterPage(this);
    }

    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Manager.UnregisterPage(this);
    }

}
