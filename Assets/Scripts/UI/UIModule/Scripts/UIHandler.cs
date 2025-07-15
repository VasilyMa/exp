using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{ 
    protected List<SourceCanvas> _canvases;

    public void Init()
    {
        _canvases = new List<SourceCanvas>();

        var canveses = GetComponentsInChildren<SourceCanvas>();

        for (int i = 0; i < canveses.Length; i++)
        {
            _canvases.Add(canveses[i]);
        }

        _canvases.ForEach(canvas => canvas.Init());
    }

    public void Dispose()
    {
        _canvases.ForEach(_canvas => _canvas.Dispose());
    }

    public virtual bool InvokeCanvas<T>(out T canvas) where T : SourceCanvas
    {
        canvas = null;

        foreach (var c in _canvases)
        {
            if (c is T returned)
            {
                canvas = returned;
            }
            else
            {
                c.CloseCanvas();
            }
        }

        canvas.InvokeCanvas();

        return canvas != null;
    }

    public virtual bool CloseCanvas<T>(out T canvas) where T : SourceCanvas
    {
        canvas = null;
         
        foreach (var c in _canvases)
        {
            if (c is T closed)
            {
                canvas = closed;
            } 
        }

        canvas.CloseCanvas();

        return canvas != null;
    }

    public virtual bool TryGetCanvas<T>(out T canvas) where T : SourceCanvas
    {
        canvas = null;

        foreach (var c in _canvases)
        {
            if (c is T returned)
            {
                canvas = returned; 
                break;
            }
        }

        return canvas != null;
    } 
}
