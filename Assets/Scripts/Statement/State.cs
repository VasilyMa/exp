using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public abstract class State : MonoBehaviour
    {
        protected static State _instance;
        public static State Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<State>();
                }
                return _instance;
            }
        }

        protected List<SourceCanvas> _canvases;

        public virtual void Awake()
        {
            InitCanvas();
        }
        public abstract void Start();
        public abstract void Update();
        public abstract void FixedUpdate();
        public virtual void OnDestroy()
        {
            _canvases?.ForEach(_canvas => _canvas.Dispose());
        }
        protected virtual void InitCanvas()
        {
            _canvases = new List<SourceCanvas>();

            var canveses = FindObjectsOfType<SourceCanvas>();

            for (int i = 0; i < canveses.Length; i++)
            {
                _canvases.Add(canveses[i]);
            } 

            _canvases.ForEach(canvas => canvas.Init());
        }

        public virtual T InvokeCanvas<T>() where T : SourceCanvas
        {
            SourceCanvas returnedCanvas = null;

            foreach (var canvas in _canvases)
            {
                if (canvas is T returned)
                {
                    returnedCanvas = returned;
                }
                else
                {
                    canvas.CloseCanvas();
                }
            }

            returnedCanvas.InvokeCanvas();

            return returnedCanvas as T;
        }

        public virtual T InvokeCanvas<T, M>() where T : SourceCanvas where M : SourceCanvas
        {
            SourceCanvas returnedCanvas = null;

            foreach (var canvas in _canvases)
            {
                if (canvas is T returned)
                {
                    returnedCanvas = returned;
                }
                else if (canvas is M)
                {
                    continue;
                }
                else
                {
                    canvas.CloseCanvas();
                }
            }

            returnedCanvas.InvokeCanvas();

            return returnedCanvas as T;
        }

        public virtual T GetCanvas<T>() where T : SourceCanvas
        {
            foreach (var canvas in _canvases)
            {
                if (canvas is T returned)
                {
                    return returned as T;
                }
            }

            return null;
        }

        public virtual bool IsInitedCanvses()
        {
            return _canvases != null && !_canvases.Any(canvas => !canvas.isInited);  // Проверяем, инициализированы ли все канвасы
        }
        protected virtual void DisposeCanvas()
        {
            _canvases.ForEach(canvas => canvas.Dispose());
        }

        public virtual Coroutine RunCoroutine(IEnumerator coroutine, Action callback = null)
        {
            if (Instance != null)
            {
                return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
            }
            else
            {
                return null;
            }
        }
        public virtual Coroutine RunCoroutine(IEnumerator coroutine, params Action[] callback)
        {
            if (Instance != null)
            {
                return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
            }
            else
            {
                return null;
            }
        }
        protected virtual IEnumerator CoroutineWrapper(IEnumerator coroutine, Action callback = null)
        {
            yield return StartCoroutine(coroutine);

            callback?.Invoke();
        }
        protected virtual IEnumerator CoroutineWrapper(IEnumerator coroutine, params Action[] callback)
        {
            yield return StartCoroutine(coroutine);

            for (int i = 0; i < callback.Length; i++)
            {
                callback[i]?.Invoke();
            }
        }

        public void RequestLoadingScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}