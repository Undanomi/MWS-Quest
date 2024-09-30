using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{ 
    public Image titleImage;
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
        titleImage.sprite = randomSprite;
    }

    private IEnumerator LoadLoginScene()
    {
        _soundManager.PlaySE(_soundManager.seTitle);
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene("Login");
    }
    
    void Update()
    {
        // 2重で音声が再生されないように
        // キーボードだけ(マウスは除外)が押されたら
        if (Input.anyKeyDown)
        {
            StartCoroutine(LoadLoginScene());
        }
    }
}