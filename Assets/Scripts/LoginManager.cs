using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("ユーザ名入力フィールド")] public TMP_InputField usernameField;
    [Header("パスワード入力フィールド")] public TMP_InputField passwordField;
    [Header("ログインボタン")] public Button loginButton;
    [Header("エラーメッセージ")] public TMP_Text errorMessage;
    
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
            SceneManager.LoadScene("ScenarioSelect");
        }
        else
        {
            // ログイン失敗時の処理
            errorMessage.text = "ユーザ名またはパスワードが違います";
        }
        
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
