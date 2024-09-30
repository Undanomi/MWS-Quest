using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class LoginManager : MonoBehaviour
{
    [Header("ユーザ名入力フィールド")] public TMP_InputField usernameField;
    [Header("パスワード入力フィールド")] public TMP_InputField passwordField;
    [Header("ログインボタン")] public Button loginButton;
    [Header("メッセージ")] public TMP_Text message;
    
    private SoundManager _soundManager;
    
    private void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }
    public void OnLoginButtonClicked()
    {
        
        string username = usernameField.text;
        string password = passwordField.text;
        
        // 本番では認証処理をサーバーにリクエストする
        // StartCoroutine(LoginRequest(username, password));
        
        // 仮処理として、ユーザ名とパスワードが一致していればログイン成功とする
        if (username == "admin" && password == "password")
        {
            // ログイン成功時にはシナリオ選択画面に遷移
            StartCoroutine(LoginSuccess(username));
        }
        else
        {
            // ログイン失敗時の処理
            _soundManager.PlaySE(_soundManager.cancelSound);
            message.text = "ユーザ名またはパスワードが違います";
        }
    }
    
    private IEnumerator LoginSuccess(string username)
    {
        _soundManager.PlaySE(_soundManager.decisionSound);
        message.text = $"<color=green>ログイン成功: {username}さんこんにちは </color>";
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("ScenarioSelect");
    }

    private IEnumerator LoginRequest(string username, string password)
    {
        // こんな感じで本来はUnityWebRequestを使ってサーバーにリクエストを送る
        // UnityWebRequest request = new UnityWebRequest("http://localhost:3000/login", "POST");
        // ここでいろいろリクエスト内容を設定する
        // ...
        // リクエスト送信、レスポンス待ち
        //yield return request.SendWebRequest();
        
        // レスポンス処理
        // ...
        
        return null;
    }
   
    /// <summary>
    /// ユーザ登録画面に遷移する
    /// </summary>
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }
}
