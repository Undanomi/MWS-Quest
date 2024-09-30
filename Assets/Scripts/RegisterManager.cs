using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [Header("ユーザ名入力フィールド")] public TMP_InputField usernameField;
    [Header("パスワード入力フィールド")] public TMP_InputField passwordField;
    [Header("確認用パスワード入力フィールド")] public TMP_InputField confirmPasswordField;
    [Header("登録ボタン")] public Button registerButton;
    [Header("メッセージ")] public TMP_Text message;
    
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        
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
            confirmPasswordField.text != null &&
            usernameField.text != "" && 
            passwordField.text != "" && 
            confirmPasswordField.text != ""
            )
        {
            OnRegisterButtonClicked();
        }
    }
    
    public void OnRegisterButtonClicked()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;
        
        // 本番ではユーザ登録処理をサーバーにリクエストする
        // StartCoroutine(RegisterRequest(username, password));
        
        // 仮処理として、ユーザ名とパスワードが入力されており、パスワードと確認用パスワードが一致していれば登録成功とする
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && password == confirmPassword)
        {
            // ユーザ登録成功時の処理
            StartCoroutine(RegisterSuccess(username));
        }
        else if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // ユーザ登録失敗時の処理
            RegisterFailed("ユーザ名またはパスワードが未入力です");
        }
        else
        {
            // ユーザ登録失敗時の処理
            RegisterFailed("パスワードと確認用パスワードが一致しません");
        }
    }

    private IEnumerator RegisterSuccess(string username)
    {
        _soundManager.PlaySE(_soundManager.decisionSound);
        message.text = $"<color=green>ユーザ登録成功: {username}さんこんにちは </color>";
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("ScenarioSelect");
    }
    
    private void RegisterFailed(string errorMassage)
    {
        _soundManager.PlaySE(_soundManager.cancelSound);
        message.text = errorMassage;
    }
    
    /// <summary>
    /// ログイン画面に遷移する
    /// </summary>
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

}
