using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LogView : MonoBehaviour
    {
        public Boolean isLogViewEnable = false;
        KeyCode keyCode = KeyCode.L;
        public GameObject logAnalysisSystem;
        public GameObject lineView;
        public GameObject optionsListView;
        public String searchWord = "";
        private GameObject _searchField;
        private GameObject _clearButton;
        private GameObject _searchButton;
        private GameObject _escapeButton;
        private GameObject _logText;
        private string _logTextString;
        private string _highlightedLogTextString;

        private void Start()
        {
            _searchField = GameObject.Find("SearchField");
            _clearButton = GameObject.Find("ClearButton");
            _searchButton = GameObject.Find("SearchButton");
            _escapeButton = GameObject.Find("EscapeButton");
            _logText = GameObject.Find("Scroll View/Viewport/Content/LogText");
            _logTextString = _logText.GetComponent<TMPro.TMP_Text>().text;

            logAnalysisSystem.SetActive(isLogViewEnable);
            lineView.SetActive(!isLogViewEnable);
            optionsListView.SetActive(!isLogViewEnable);
            // 現在のフォーカスを解除
            EventSystem.current.SetSelectedGameObject(null);
            // SearchFieldにフォーカスを当てる
            EventSystem.current.SetSelectedGameObject(_searchField.GetComponent<TMPro.TMP_InputField>().gameObject);
            // Select()だけだとカーソルが表示されないのでActivateInputField()を呼ぶ
            EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
            // EscapeButtonにOnClickイベントを追加
            _escapeButton.GetComponent<Button>().onClick.AddListener(onEscapeButtonClicked);
            // ClearButtonにOnClickイベントを追加
            _clearButton.GetComponent<Button>().onClick.AddListener(onClearButtonClicked);
            // SearchButtonにOnClickイベントを追加
            _searchButton.GetComponent<Button>().onClick.AddListener(onSearchButtonClicked);
        }

        //Lキーが押されたら，LogAnalysisSystemオブジェクトのすべての要素を可視化
        void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                SwitchActivatingLogView();
            }

            if (_searchField.GetComponent<TMPro.TMP_InputField>().text == "")
            {
                _logText.GetComponent<TMPro.TMP_Text>().text = _logTextString;
            }

            if (Input.GetKeyDown(KeyCode.Return) && isLogViewEnable)
            {
                _searchButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        void onEscapeButtonClicked()
        {
            Debug.Log("EscapeButtonClicked");
            SwitchActivatingLogView();
        }

        void onClearButtonClicked()
        {
            Debug.Log("ClearButtonClicked");
            _searchField.GetComponent<TMPro.TMP_InputField>().text = "";
        }

        void onSearchButtonClicked()
        {
            Debug.Log("SearchButtonClicked");
            SearchWord();
        }

        void SwitchActivatingLogView()
        {
            // 次の状態に遷移
            isLogViewEnable = !isLogViewEnable;

            if (!isLogViewEnable)
            {
                //SearchFieldを一旦確定
                _searchField.GetComponent<TMPro.TMP_InputField>().DeactivateInputField();
                //SearchFieldから検索ワードを取得
                searchWord = _searchField.GetComponent<TMPro.TMP_InputField>().text;
                Debug.Log("previousSearchWord: " + searchWord);
                //文字化けしてたら削除
                if (searchWord.Contains("\u001b"))
                {
                    searchWord = "";
                }
            }

            //ダイアログの表示・非表示
            logAnalysisSystem.SetActive(isLogViewEnable);
            lineView.SetActive(!isLogViewEnable);
            optionsListView.SetActive(!isLogViewEnable);
            keyCode = isLogViewEnable ? KeyCode.Escape : KeyCode.L;

            if (isLogViewEnable)
            {
                //未確定のテキストを削除
                _searchField.GetComponent<TMPro.TMP_InputField>().text = "";
                //SearchFieldに検索ワードを設定
                _searchField.GetComponent<TMPro.TMP_InputField>().text = searchWord;
                if (searchWord != null && searchWord != "")
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

        void SearchWord()
        {
            // SearchFieldから検索ワードを取得
            searchWord = _searchField.GetComponent<TMPro.TMP_InputField>().text;
            Debug.Log("previousSearchWord: " + searchWord);

            // 文字化けしてたら削除
            if (searchWord.Contains("\u001b"))
            {
                searchWord = "";
            }

            // 検索ワードが空の場合は処理をスキップ
            if (string.IsNullOrEmpty(searchWord))
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
                        string tag = _logTextString.Substring(i, tagEnd - i + 1);
                        tagPositions.Add((tag, plainTextIndex)); // プレーンテキスト内の位置を記録
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
                Regex regex = new Regex(searchWord);
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
                            string tag = highlightedPlainText.Substring(i, tagEnd - i + 1);
                            if (tag.StartsWith("<mark") || tag == "</mark>")
                            {
                                _highlightedLogTextString += tag; // <mark> タグをそのまま追加
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
}