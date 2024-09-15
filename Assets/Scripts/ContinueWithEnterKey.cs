using UnityEngine;
using UnityEngine.UI;

public class ContinueOnEnter : MonoBehaviour
{
    public Button continueButton;  // ここにContinueボタンのButtonコンポーネントをアタッチ

    void Start()
    {
        if (continueButton == null)
        {
            Debug.LogError("Continue Button is not assigned!");
            return;
        }

        // ボタンのクリックイベントにEnterキーの入力を追加
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    void Update()
    {
        // Enterキーが押されたときにボタンをクリック
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (continueButton != null)
            {
                continueButton.onClick.Invoke();
            }
        }
    }

    void OnContinueClicked()
    {
        // ここにボタンがクリックされたときの処理を書く
        Debug.Log("Continue button clicked!");
    }
}