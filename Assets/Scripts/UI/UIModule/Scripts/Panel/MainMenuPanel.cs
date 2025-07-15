using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : SourcePanel
{
    [SerializeField] private Button _btnStart;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnExit;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        _btnStart.onClick.AddListener(onStart);
    }

    void onStart()
    {
        UIModule.OpenCanvas<LoadingCanvas>(out var loadingCanvas);

        SceneManager.LoadScene(2);
    }

    void onSettingsOpen()
    {

    }
}
