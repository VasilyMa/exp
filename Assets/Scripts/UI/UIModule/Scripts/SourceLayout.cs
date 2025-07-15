using UnityEngine;

public abstract class SourceLayout : MonoBehaviour
{
    SourcePanel _panel;
    protected SourceSlot[] _slots;

    public virtual SourceLayout Init(SourcePanel panel)
    {
        _panel = panel;

        var slots = GetComponentsInChildren<SourceSlot>(true);
        _slots = new SourceSlot[slots.Length];

        for (int i = 0; i < slots.Length; i++) _slots[i] = slots[i].Init(this);

        return this;
    }

    public virtual void CloseIt<T>() where T : SourceLayout
    {
        _panel.CloseLayout<T>();
    }

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }

    public virtual void Dispose()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].Dispose();
        }
    }
}
