using UnityEngine;
using UnityEngine.SceneManagement;

namespace Statement
{
    public class InitState : State
    {
        public static new InitState Instance
        {
            get
            {
                return (InitState)State.Instance;
            }
        }
#if UNITY_EDITOR
        [UnityEngine.SerializeField] UnityEditor.SceneAsset TargetScene;
#endif
        [SerializeField] private string targetSceneName; 
        public override void Awake()
        {
            InitCanvas();
        }
        public override void Start()
        {
            EntityModule.Initialize();

            ConfigModule.Initialize(this, onConfigLoaded);
        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {

        }
        void onConfigLoaded()
        {
            SceneManager.LoadScene(targetSceneName);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (TargetScene)
            {
                targetSceneName = TargetScene.name;
            }
        }
#endif
    }
}