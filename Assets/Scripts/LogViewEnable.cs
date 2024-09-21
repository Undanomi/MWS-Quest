using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LogViewEnable : MonoBehaviour
    {
        private Boolean isLogViewEnable = false;
        KeyCode keyCode = KeyCode.L;
        public GameObject logAnalysisSystem;
        public GameObject lineView;
        public GameObject optionsListView;
        public String previousSearchWord = "";
        private GameObject searchField;
        private GameObject searchButton;

        private void Start()
        {
            searchField = GameObject.Find("SearchField");
            searchButton = GameObject.Find("SearchButton");
            logAnalysisSystem.SetActive(isLogViewEnable);
            lineView.SetActive(!isLogViewEnable);
            optionsListView.SetActive(!isLogViewEnable);
        }

        //Lキーが押されたら，LogAnalysisSystemオブジェクトのすべての要素を可視化
        void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                // 次の状態に遷移
                isLogViewEnable = !isLogViewEnable;
                
                if (!isLogViewEnable)
                {
                    //SearchFieldから検索ワードを取得
                    previousSearchWord = searchField.GetComponent<TMPro.TMP_InputField>().text;
                    Debug.Log("previousSearchWord: " + previousSearchWord);
                }
                
                //ダイアログの表示・非表示
                logAnalysisSystem.SetActive(isLogViewEnable);
                lineView.SetActive(!isLogViewEnable);
                optionsListView.SetActive(!isLogViewEnable);
                keyCode = isLogViewEnable ? KeyCode.Escape : KeyCode.L;
                
                if (isLogViewEnable){
                    //未確定のテキストを削除
                    searchField.GetComponent<TMPro.TMP_InputField>().text = "";
                    //SearchFieldに検索ワードを設定
                    searchField.GetComponent<TMPro.TMP_InputField>().text = previousSearchWord;
                    // SearchButtonをクリック
                    searchButton.GetComponent<Button>().onClick.Invoke();
                    // 現在のフォーカスを解除
                    EventSystem.current.SetSelectedGameObject(null);
                    // SearchFieldにフォーカスを当てる
                    EventSystem.current.SetSelectedGameObject(searchField.GetComponent<TMPro.TMP_InputField>().gameObject);
                    // Select()だけだとカーソルが表示されないのでActivateInputField()を呼ぶ
                    EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
                }
            }
        }
        
    }
}