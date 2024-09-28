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
        }
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
}