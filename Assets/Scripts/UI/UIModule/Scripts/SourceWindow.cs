using UnityEngine;

public abstract class SourceWindow : MonoBehaviour
{
    protected SourcePanel _panel;

    public virtual SourceWindow Init(SourcePanel panel)
    {
        _panel = panel;

        return this;
    }

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }

    public abstract void Dispose();
}
