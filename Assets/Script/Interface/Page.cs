using UnityEngine;

public abstract class Page : MonoBehaviour {

    protected virtual void Awake()
    {
        UIManager.RegisterPage(this);
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
        UIManager.UnregisterPage(this);
    }

}
