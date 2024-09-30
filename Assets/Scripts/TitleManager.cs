using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button titleButton;
    private const string TitleImagePath = "Images/Title";
    
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        
        // 全画像を読み込む
        Sprite[] titleSprites = Resources.LoadAll<Sprite>(TitleImagePath);
        // ランダムに画像を選択
        Sprite randomSprite = titleSprites[Random.Range(0, titleSprites.Length)];
        // 画像を設定
        titleButton.image.sprite = randomSprite;
        
        titleButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _soundManager.PlaySE(_soundManager.decisionSound);
        SceneManager.LoadScene("Login");
    }
    
    void Update()
    {
        if(Input.anyKey)
        {
            OnClick();
        }
    }
}