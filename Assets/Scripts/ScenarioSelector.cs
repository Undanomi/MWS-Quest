using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] scenarioButtons;
    public Image fadeImage;
    
    private SoundManager _soundManager;
    
    void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        
        // シナリオボタンに変数を追加
        for (int i = 0; i < scenarioButtons.Length; i++)
        {
            int index = i; // ラムダ式内での参照用
            scenarioButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }
    private void OnButtonClick(int index)
    {
        foreach (Button button in scenarioButtons)
        {
            button.interactable = false;
        }
        StartCoroutine(LoadScenario(index));
    }
    
    private IEnumerator FadeOut(float delay)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        float elapsedTime = 0f;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / delay);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
    }
    private IEnumerator LoadScenario(int scenarioId)
    {
        // SE再生
        _soundManager.PlaySE(_soundManager.decisionSound);
        // フェードアウトを待つ
        yield return StartCoroutine(FadeOut(1f));
        // 2桁の0埋めしたシナリオ名を作成
        string sceneName = "Scenario" + (scenarioId+1).ToString("D2");
        SceneManager.LoadScene(sceneName);
    }
}
