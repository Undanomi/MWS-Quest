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

        private void Start()
        {
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
                    previousSearchWord = GameObject.Find("SearchField").GetComponent<TMPro.TMP_InputField>().text;
                    Debug.Log("previousSearchWord: " + previousSearchWord);
                }
                
                //ダイアログの表示・非表示
                logAnalysisSystem.SetActive(isLogViewEnable);
                lineView.SetActive(!isLogViewEnable);
                optionsListView.SetActive(!isLogViewEnable);
                keyCode = isLogViewEnable ? KeyCode.Escape : KeyCode.L;
                
                if (isLogViewEnable){
                    //未確定のテキストを削除
                    GameObject.Find("SearchField").GetComponent<TMPro.TMP_InputField>().text = "";
                    //SearchFieldに検索ワードを設定
                    GameObject.Find("SearchField").GetComponent<TMPro.TMP_InputField>().text = previousSearchWord;
                    // SearchButtonをクリック
                    GameObject.Find("SearchButton").GetComponent<Button>().onClick.Invoke();
                    // SearchFieldにフォーカスを当てる
                    GameObject.Find("SearchField").GetComponent<TMPro.TMP_InputField>().Select();
                }
            }
        }
        
    }
}