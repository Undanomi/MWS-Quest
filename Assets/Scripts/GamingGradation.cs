using UnityEngine;
using UnityEngine.UI;  // UIコンポーネントを扱うために必要

public class GamingGradation : MonoBehaviour
{
    // Image コンポーネントをパブリックで宣言してエディタから指定できるようにする
    public Image targetImage;

    // H の変化速度
    public float hueSpeed = 0.1f;

    // 現在の H 値を保持するための変数
    private float currentHue;

    // アルファ値（透明度）を保持
    private float alphaValue;

    void Start()
    {
        // 初期色を取得
        if (targetImage != null)
        {
            Color initialColor = targetImage.color;

            // 初期色からRGBをHSVに変換し、H値を取得
            Color.RGBToHSV(initialColor, out currentHue, out _, out _);

            // アルファ値（透明度）を保持
            alphaValue = initialColor.a;
        }
    }

    void Update()
    {
        // H の値を時間経過に基づいて増加させる
        currentHue += hueSpeed * Time.deltaTime;

        // H の値を 0〜1 の範囲に制限
        if (currentHue > 1f)
        {
            currentHue -= 1f;
        }

        // HSVから新しい色を生成し、保持していたアルファ値を適用
        if (targetImage != null)
        {
            Color newColor = Color.HSVToRGB(currentHue, 1, 1);

            // アルファ値を保持したまま新しい色を適用
            newColor.a = alphaValue;
            targetImage.color = newColor;
        }
    }
}