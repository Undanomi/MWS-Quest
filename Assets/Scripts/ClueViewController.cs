using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Object = System.Object;


public class ClueViewController : MonoBehaviour
{
    public bool isClueViewRunning;
    public LogViewController logViewController;
    public DialogueRunner dialogueRunner;
    public ClueViewController clueViewController;
    public MissionManager missionManager;

    private VariableStorageBehaviour _variableStorage;
    private KeyCode _keyCode = KeyCode.H;
    private GameObject _escapeButton;
    private GameObject _clueText;
    private Npc[] _npcs;
    private Clue[] _clues;
    private int _currentMission = 1;
    
    private SoundManager _soundManager;

    [System.Serializable]
    public class Npc
    {
        public string npc;
        public string display;
        public bool isMet = false;
        public int wholeMetCount = 0; 
    }
    
    [System.Serializable]
    public class Clue
    {
        public string count;
        public string npc;
        public string clue;
    }
    
    [System.Serializable]
    public class ClueList
    {
        public Npc[] npcs;
        public Clue[] clues;
    }
    
    private void Start()
    {
        isClueViewRunning = false;
        _variableStorage = dialogueRunner.VariableStorage;
        
        _soundManager = FindObjectOfType<SoundManager>();
        
        //ダイアログの表示・非表示
        clueViewController.transform.Find("Canvas/Panel").gameObject.SetActive(isClueViewRunning);
        clueViewController.transform.Find("Canvas/OpenClueViewButton").gameObject.SetActive(!isClueViewRunning);
        // EscapeButtonにOnClickイベントを追加
        _escapeButton = clueViewController.transform.Find("Canvas/Panel/EscapeButton").gameObject;
        _clueText = clueViewController.transform.Find("Canvas/Panel/Scroll View/Viewport/Content/ClueText").gameObject;
        _clueText.GetComponent<TMPro.TMP_Text>().text = "村人たちに話を聞いて、手がかりを集めよう。";
        _escapeButton.GetComponent<Button>().onClick.AddListener(OnEscapeButtonClicked);
        InitializeClueList();
    }

    private void Update()
    {
        if(missionManager.GetMissionPhase() > _currentMission)
        {
            _currentMission = missionManager.GetMissionPhase();
            foreach (var npc in _npcs)
            {
                npc.isMet = false;
                Debug.Log(npc.npc + "のisMetをリセットしました。" + npc.isMet);
            }
        }
        if (dialogueRunner.IsDialogueRunning)
        {
            clueViewController.transform.Find("Canvas/OpenClueViewButton").gameObject.SetActive(false);
        }
        if(dialogueRunner.IsDialogueRunning == false && isClueViewRunning == false && logViewController.isLogViewRunning == false)
        {
            clueViewController.transform.Find("Canvas/OpenClueViewButton").gameObject.SetActive(true);
        }
        if (_keyCode == KeyCode.None || 
            dialogueRunner.IsDialogueRunning || 
            logViewController.isLogViewRunning
           )
        {
            return;
        }
        
        if (Input.GetKeyDown(_keyCode))
        {
            if (_keyCode == KeyCode.H)
            {
                _soundManager.PlaySE(_soundManager.decisionSound);
            }
            else if (_keyCode == KeyCode.Escape)
            {
                _soundManager.PlaySE(_soundManager.cancelSound);
            }
            SwitchClueViewRunning();
        }
        AddText();
    }
    
    void OnEscapeButtonClicked()
    {
        _soundManager.PlaySE(_soundManager.cancelSound);
        SwitchClueViewRunning();
    }
    
    public void SwitchClueViewRunning()
    {
        isClueViewRunning = !isClueViewRunning;
        //ダイアログの表示・非表示
        clueViewController.transform.Find("Canvas/Panel").gameObject.SetActive(isClueViewRunning);
        logViewController.transform.Find("Canvas/OpenLogViewButton").gameObject.SetActive(!isClueViewRunning);
        clueViewController.transform.Find("Canvas/OpenClueViewButton").gameObject.SetActive(!isClueViewRunning);
        dialogueRunner.transform.Find("Canvas").gameObject.SetActive(!isClueViewRunning);
        logViewController.gameObject.SetActive(!isClueViewRunning);
        _keyCode = isClueViewRunning ? KeyCode.Escape : KeyCode.H;
    }

    public void SetClueViewAvailable(bool isAvailable)
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        clueViewController.transform.Find("Canvas").gameObject.SetActive(isAvailable);
        _keyCode = isAvailable ? KeyCode.H : KeyCode.None;
    }

    private void InitializeClueList()
    {
        try
        {
            //JsonファイルからClueListを読み込む
            TextAsset json = Resources.Load("clues_from_npc") as TextAsset;
            // jsonには2つの配列が含まれている
            // 1つ目の配列はNPCの名前と表示名の対応表
            // 2つ目の配列はNPCの名前とクエストの内容の対応表
            ClueList clueList = JsonUtility.FromJson<ClueList>(json.text);
            _npcs = clueList.npcs;
            _clues = clueList.clues;
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError("ClueListの読み込みに失敗しました。" + e.Message);
        }
    }
    

    private void AddText()
    {
        foreach (var npc in _npcs)
        {
            float metCount = GetYarnVariable<float>("$" + npc.npc + "MetCount");
            //Yarnのnumberがfloatしかできないので、変な比較をしている
            if (!npc.isMet && metCount > npc.wholeMetCount)
            {
                npc.wholeMetCount = (int)metCount;
                //_npcsのすべてのisMetがfalseであるかどうか
                bool allFalse = true;
                foreach (var n in _npcs)
                {
                    if (n.isMet)
                    {
                        allFalse = false;
                        break;
                    }
                }
                if (allFalse && _currentMission == 1)
                {
                    _clueText.GetComponent<TMPro.TMP_Text>().text = "";
                }
                
                npc.isMet = true;
                Debug.Log("isMetをtrueにしました。" + npc.isMet);
                _clueText.GetComponent<TMPro.TMP_Text>().text += npc.display + "からの情報:\n";
                Debug.Log(npc.npc + "からの情報:");
                foreach (var clue in _clues)
                {
                    if (clue.npc == npc.npc && clue.count == "count" + npc.wholeMetCount)
                    {
                        _clueText.GetComponent<TMPro.TMP_Text>().text += clue.clue + "\n\n";
                    }
                }
            }
        }
        
    }
    
    
    
    /// <summary>
    /// ある変数の値を取得する
    /// </summary>
    /// <param name="variableName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    T GetYarnVariable<T>(string variableName)
    {
        if (typeof(T) == typeof(float))
        {
            if (_variableStorage.TryGetValue(variableName, out float result))
            {
                return (T)(object)result;
            }
        }
        else if (typeof(T) == typeof(string))
        {
            if (_variableStorage.TryGetValue(variableName, out string result))
            {
                return (T)(object)result;
            }
        }
        else if (typeof(T) == typeof(bool))
        {
            if (_variableStorage.TryGetValue(variableName, out bool result))
            {
                return (T)(object)result;
            }
        }
        
        throw new InvalidOperationException($"変数 '{variableName}' は存在しないか、型が一致しません。");
    }
}
