using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yarn.Unity;

public class LogViewController : MonoBehaviour
{
    public bool isLogViewRunning;
    public LogViewController logViewController;
    public DialogueRunner dialogueRunner;
    public ClueViewController clueViewController;
    
    private String _searchWord = "";
    
    private GameObject _lineView;
    private GameObject _optionsListView;
    private GameObject _searchField;
    private GameObject _clearButton;
    private GameObject _searchButton;
    private GameObject _escapeButton;
    private GameObject _logText;
    private KeyCode _keyCode = KeyCode.L;
    private string _logTextString;
    private string _highlightedLogTextString;
    
    private SoundManager _soundManager;

    private void Start()
    {
        isLogViewRunning = false;
        _lineView = dialogueRunner.transform.Find("Canvas/Line View").gameObject;
        _optionsListView = dialogueRunner.transform.Find("Canvas/Options List View").gameObject;
        
        _clearButton = logViewController.transform.Find("Canvas/Panel/ClearButton").gameObject;
        _searchButton = logViewController.transform.Find("Canvas/Panel/SearchButton").gameObject;
        _escapeButton = logViewController.transform.Find("Canvas/Panel/EscapeButton").gameObject;
        _logText = logViewController.transform.Find("Canvas/Panel/Scroll View/Viewport/Content/LogText").gameObject;
        _searchField = logViewController.transform.Find("Canvas/Panel/SearchField").gameObject;
        
        _soundManager = FindObjectOfType<SoundManager>();
        
        TextAsset textAsset = Resources.Load<TextAsset>("log");

        // TextAssetがnullでないことを確認
        if (textAsset != null)
        {
            // テキスト内容をstringとして取得
            _logTextString = textAsset.text;
            _logText.GetComponent<TMPro.TMP_Text>().text = _logTextString;
        }
        else
        {
            Debug.LogError("Text file not found!");
        }

        //ダイアログの表示・非表示
        logViewController.transform.Find("Canvas/Panel").gameObject.SetActive(isLogViewRunning);
        logViewController.transform.Find("Canvas/OpenLogViewButton").gameObject.SetActive(!isLogViewRunning);
        _lineView.SetActive(!isLogViewRunning);
        _optionsListView.SetActive(!isLogViewRunning);
        
        // 現在のフォーカスを解除
        EventSystem.current.SetSelectedGameObject(null);
        // SearchFieldにフォーカスを当てる
        EventSystem.current.SetSelectedGameObject(_searchField.GetComponent<TMPro.TMP_InputField>().gameObject);
        // Select()だけだとカーソルが表示されないのでActivateInputField()を呼ぶ
        EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
        // EscapeButtonにOnClickイベントを追加
        _escapeButton.GetComponent<Button>().onClick.AddListener(OnEscapeButtonClicked);
        // ClearButtonにOnClickイベントを追加
        _clearButton.GetComponent<Button>().onClick.AddListener(OnClearButtonClicked);
        // SearchButtonにOnClickイベントを追加
        _searchButton.GetComponent<Button>().onClick.AddListener(OnSearchButtonClicked);
    }

    //Lキーが押されたら，LogAnalysisSystemオブジェクトのすべての要素を可視化
    void Update()
    {
        if(dialogueRunner.IsDialogueRunning)
        {
            logViewController.transform.Find("Canvas/OpenLogViewButton").gameObject.SetActive(false);
        }
        if(dialogueRunner.IsDialogueRunning == false && isLogViewRunning == false && clueViewController.isClueViewRunning == false)
        {
            logViewController.transform.Find("Canvas/OpenLogViewButton").gameObject.SetActive(true);
        }
        if (_keyCode == KeyCode.None || 
            dialogueRunner.IsDialogueRunning || 
            clueViewController.isClueViewRunning
            )
        {
            return;
        }
        
        // インジェクション対策で，InputFieldのタグを正規表現で検知してエスケープ（削除ではなくテキストで表示）
        if (isLogViewRunning && _searchField != null && _searchField.GetComponent<TMPro.TMP_InputField>().text.Contains("<"))
        {
            _searchField.GetComponent<TMPro.TMP_InputField>().text =
                Regex.Replace(_searchField.GetComponent<TMPro.TMP_InputField>().text, "<", "");
        }

        if (isLogViewRunning && _searchField != null && _searchField.GetComponent<TMPro.TMP_InputField>().text.Contains(">"))
        {
            _searchField.GetComponent<TMPro.TMP_InputField>().text =
                Regex.Replace(_searchField.GetComponent<TMPro.TMP_InputField>().text, ">", "");
        }
        
        if (isLogViewRunning && _searchField != null && _searchField.GetComponent<TMPro.TMP_InputField>().text.Contains("\n"))
        {
            _searchField.GetComponent<TMPro.TMP_InputField>().text =
                Regex.Replace(_searchField.GetComponent<TMPro.TMP_InputField>().text, "\n", "");
        }
        
        
        if (Input.GetKeyDown(_keyCode))
        {
            if (_keyCode == KeyCode.L)
            {
                _soundManager.PlaySE(_soundManager.seDecision);
            }
            else if (_keyCode == KeyCode.Escape)
            {
                _soundManager.PlaySE(_soundManager.seCancel);
            }
            SwitchLogViewRunning();
        }

        if (isLogViewRunning && _searchField != null && _searchField.GetComponent<TMPro.TMP_InputField>().text == "")
        {
            _logText.GetComponent<TMPro.TMP_Text>().text = _logTextString;
        }

        if (Input.GetKeyDown(KeyCode.Return) && isLogViewRunning)
        {
            _searchButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    void OnEscapeButtonClicked()
    {
        _soundManager.PlaySE(_soundManager.seCancel);
        SwitchLogViewRunning();
    }

    void OnClearButtonClicked()
    {
        _soundManager.PlaySE(_soundManager.seCancel);
        _searchField.GetComponent<TMPro.TMP_InputField>().text = "";
    }

    void OnSearchButtonClicked()
    {
        _soundManager.PlaySE(_soundManager.seCancel);
        SearchWord();
    }

    public void SetLogViewAvailable(bool isAvailable)
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        Debug.Log("LogViewAvailable: " + isAvailable);
        logViewController.transform.Find("Canvas").gameObject.SetActive(isAvailable);
        _keyCode = isAvailable ? KeyCode.L : KeyCode.None;
    }

    private void SwitchLogViewRunning()
    {
        // 次の状態に遷移
        isLogViewRunning = !isLogViewRunning;

        if (!isLogViewRunning)
        {
            //SearchFieldを一旦確定
            _searchField.GetComponent<TMPro.TMP_InputField>().DeactivateInputField();
            //SearchFieldから検索ワードを取得
            _searchWord = _searchField.GetComponent<TMPro.TMP_InputField>().text;
            Debug.Log("previousSearchWord: " + _searchWord);
            //文字化けしてたら削除
            if (_searchWord.Contains("\u001b"))
            {
                _searchWord = "";
            }
        }
        //ダイアログの表示・非表示
        logViewController.transform.Find("Canvas/Panel").gameObject.SetActive(isLogViewRunning);
        logViewController.transform.Find("Canvas/OpenLogViewButton").gameObject.SetActive(!isLogViewRunning);
        clueViewController.transform.Find("Canvas/OpenClueViewButton").gameObject.SetActive(!isLogViewRunning);
        dialogueRunner.transform.Find("Canvas").gameObject.SetActive(!isLogViewRunning);
        clueViewController.gameObject.SetActive(!isLogViewRunning);
        _keyCode = isLogViewRunning ? KeyCode.Escape : KeyCode.L;

        if (isLogViewRunning)
        {
            //SearchFieldに検索ワードを設定
            _searchField.GetComponent<TMPro.TMP_InputField>().text = _searchWord;
            if (!string.IsNullOrEmpty(_searchWord))
            {
                // newLogTextStringを入れておく
                _logText.GetComponent<TMPro.TMP_Text>().text = _highlightedLogTextString;
            }

            // 現在のフォーカスを解除
            EventSystem.current.SetSelectedGameObject(null);
            // SearchFieldにフォーカスを当てる
            EventSystem.current.SetSelectedGameObject(_searchField.GetComponent<TMPro.TMP_InputField>().gameObject);
            // Select()だけだとカーソルが表示されないのでActivateInputField()を呼ぶ
            EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
        }
    }

    private void SearchWord()
    {
        // SearchFieldから検索ワードを取得
        _searchWord = _searchField.GetComponent<TMPro.TMP_InputField>().text;
        Debug.Log("previousSearchWord: " + _searchWord);

        // 文字化けしてたら削除
        if (_searchWord.Contains("\u001b"))
        {
            _searchWord = "";
        }

        // 検索ワードが空の場合は処理をスキップ
        if (string.IsNullOrEmpty(_searchWord))
        {
            _logText.GetComponent<TMPro.TMP_Text>().text = _logTextString;
            return;
        }

        // (1) タグと位置の組をリストで保持
        var tagPositions = new List<(string tag, int position)>();
        string plainText = "";
        int plainTextIndex = 0;

        // 元のテキストからタグを取り除きつつ、タグの位置を記録する
        for (int i = 0; i < _logTextString.Length; i++)
        {
            if (_logTextString[i] == '<') // タグの始まり
            {
                int tagEnd = _logTextString.IndexOf('>', i);
                if (tagEnd > -1)
                {
                    string htmlTag = _logTextString.Substring(i, tagEnd - i + 1);
                    tagPositions.Add((htmlTag, plainTextIndex)); // プレーンテキスト内の位置を記録
                    i = tagEnd; // タグ部分をスキップ
                }
            }
            else
            {
                plainText += _logTextString[i]; // プレーンテキストに文字を追加
                plainTextIndex++;
            }
        }

        try
        {
            // (3) 検索ワードに正規表現を使用してプレーンテキスト部分を検索しハイライト
            Regex regex = new Regex(_searchWord);
            string highlightedPlainText = "";
            // 改行で分割
            var lines = plainText.Split('\n');
            foreach (var line in lines)
            {
                // 検索ワードをハイライト
                string highlightedLine = regex.Replace(line, "<mark=#FFFF0055>$0</mark>");
                highlightedPlainText += highlightedLine + "\n";
            }


            // (4) タグを元の位置に復元しつつ、<mark>タグを無視
            int tagIndex = 0;
            plainTextIndex = 0;
            _highlightedLogTextString = "";

            for (int i = 0; i < highlightedPlainText.Length; i++)
            {
                // プレーンテキスト部分を進めるとき、追加した <mark> タグは無視する
                if (highlightedPlainText[i] == '<')
                {
                    // <mark> タグの開始または終了タグを無視
                    int tagEnd = highlightedPlainText.IndexOf('>', i);
                    if (tagEnd > -1)
                    {
                        string htmlTag = highlightedPlainText.Substring(i, tagEnd - i + 1);
                        if (htmlTag.StartsWith("<mark") || htmlTag == "</mark>")
                        {
                            _highlightedLogTextString += htmlTag; // <mark> タグをそのまま追加
                            i = tagEnd; // タグ全体をスキップ
                            continue;
                        }
                    }
                }

                // タグがある場合、その位置にタグを挿入
                while (tagIndex < tagPositions.Count && plainTextIndex == tagPositions[tagIndex].position)
                {
                    _highlightedLogTextString += tagPositions[tagIndex].tag;
                    tagIndex++;
                }

                // プレーンテキスト部分を追加
                _highlightedLogTextString += highlightedPlainText[i];
                plainTextIndex++;
            }

            // 残っているタグを追加
            while (tagIndex < tagPositions.Count)
            {
                _highlightedLogTextString += tagPositions[tagIndex].tag;
                tagIndex++;
            }

            // (5) <mark=#FFFF0055>が含まれない行を削除
            lines = _highlightedLogTextString.Split('\n');
            _highlightedLogTextString = "";
            foreach (var line in lines)
            {
                if (line.Contains("<mark=#FFFF0055>"))
                {
                    _highlightedLogTextString += line + "\n";
                }
            }

            // テキストを更新
            _logText.GetComponent<TMPro.TMP_Text>().text = _highlightedLogTextString;
            Debug.Log("newLogTextString: " + _highlightedLogTextString);
        }
        catch (RegexMatchTimeoutException e)
        {
            Debug.LogError("正規表現の処理がタイムアウトしました: " + e.Message);
        }
        catch (ArgumentException e)
        {
            Debug.LogError("無効な正規表現パターン: " + e.Message);
        }
    }
}