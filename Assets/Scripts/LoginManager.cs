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
        _soundManager.PlayBGM(_soundManager.bgmLogin, fadeInTime: 0f);
    }
    
    private void Update()
    {
        //インジェクション対策で、タグを無効化。また、改行コードを削除
        if(usernameField.text != null && (usernameField.text.Contains("<") || usernameField.text.Contains(">") || usernameField.text.Contains("\n")))
        {
            usernameField.text = usernameField.text.Replace("<", "");
            usernameField.text = usernameField.text.Replace(">", "");
            usernameField.text = usernameField.text.Replace("\n", "");
        }
        if(passwordField.text != null && (passwordField.text.Contains("<") || passwordField.text.Contains(">") || passwordField.text.Contains("\n")))
        {
            passwordField.text = passwordField.text.Replace("<", "");
            passwordField.text = passwordField.text.Replace(">", "");
            passwordField.text = passwordField.text.Replace("\n", "");
        }
        if (Input.GetKeyDown(KeyCode.Return) &&
            usernameField.text != null &&
            passwordField.text != null &&
            usernameField.text != "" && 
            passwordField.text != ""
            )
        {
            Debug.Log("username: " + usernameField.text);
            OnLoginButtonClicked();
        }
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
            LoginFailed("ユーザ名またはパスワードが間違っています");
        }
    }
    
    private IEnumerator LoginSuccess(string username)
    {
        _soundManager.PlaySE(_soundManager.seScenario);
        message.text = $"<color=green>ログイン成功: {username}さんこんにちは </color>";
        yield return StartCoroutine(_soundManager.StopBGM(fadeOutTime: 1f));
        SceneManager.LoadScene("ScenarioSelect");
    }
    private void LoginFailed(string errorMassage)
    {
        _soundManager.PlaySE(_soundManager.seCancel);
        message.text = errorMassage;
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
