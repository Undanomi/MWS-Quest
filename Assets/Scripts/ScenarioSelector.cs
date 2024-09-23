using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] scenarioButtons;
    void Start()
    {
        for (int i = 0; i < scenarioButtons.Length; i++)
        {
            int index = i; // ラムダ式内での参照用
            scenarioButtons[i].onClick.AddListener(() => LoadScenario(index + 1));
        }
    }
    
    private void LoadScenario(int scenarioId)
    {
        // 2桁の0埋めしたシナリオ名を作成
        string sceneName = "Scenario" + scenarioId.ToString("D2");
        SceneManager.LoadScene(sceneName);
    }
}
