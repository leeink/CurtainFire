using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public static string nextScene;
    
    public Image loadingTipImage;
    
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Button startButton;
    [SerializeField] private Button gameTipScript;
    [SerializeField] private Text tipText;

    [SerializeField] private Sprite[] tipSprite;
    public LocalizeStringEvent localizeStringEvent;
    private int _tipIndex = 0;
    private IDictionary<int, string> _tipScript = new Dictionary<int, string>()
    {
        { 0, "Kill the enemy and Level up your space" },
        { 1, "As your space levels up, enemies also become stronger." },
        { 2, "When you reach a certain level, a powerful enemy will appear." },
        { 3, "Repair your space and become stronger by picking up randomly appearing items." },
        { 4, "Leveling up allows you to perform stronger attacks." }
    };
    
    public LocalizedString[] localizedTips = new LocalizedString[5];
    
    private AsyncOperation _op;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Scenes/LoadingScene");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tipText.text = _tipScript[_tipIndex];
        startButton.onClick.AddListener(OnClickGameStart);
        gameTipScript.onClick.AddListener(NextScript);
        //Task.Run(OtherDataLoad);
        StartCoroutine(LoadSceneProgress());
    }

    IEnumerator LoadSceneProgress()
    {
        _op = SceneManager.LoadSceneAsync(nextScene);
        _op!.allowSceneActivation = false;
        
        float time = 0f;
        
        while (!_op.isDone)
        {
            yield return null;

            if (_op.progress < 0.9f)
            {
                loadingBar.value = _op.progress;
            }
            else
            {
                time += Time.unscaledDeltaTime;
                loadingBar.value = Mathf.Lerp(0.9f, 1f, time);

                if (loadingBar.value >= 1f)
                {
                    yield return new WaitForSeconds(5f);    // Fake loading time
                    OnStartUI();
                    yield break;
                }
            }
        }
    }

    private void OnStartUI()
    {
        if (_op != null && !_op.isDone)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    private void OnClickGameStart()
    {
        _op.allowSceneActivation = true;
    }

    private void NextScript()
    {
        _tipIndex = (_tipIndex + 1) % _tipScript.Count;
        loadingTipImage.sprite = tipSprite[_tipIndex];
        //tipText.text = _tipScript[_tipIndex];
        localizeStringEvent.StringReference = localizedTips[_tipIndex];
    }
}
