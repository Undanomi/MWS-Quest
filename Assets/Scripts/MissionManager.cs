using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class MissionManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public TextMeshProUGUI missionDisplay;
    public CanvasGroup missionCanvas;
    
    private readonly Dictionary<string, bool> _visitedNodes = new Dictionary<string, bool>();
    private InMemoryVariableStorage _variableStorage;
    private readonly string[] _metCountVariableNames = new string[]
    {
        "$AliceMetCount", "$BobMetCount", "$CharlieMetCount", "$DaveMetCount", "$EllenMetCount", "$FrankMetCount"
    };

    private int _missionPhase = -1;
    private readonly List<Mission> _missions = new List<Mission>();
    
    void Start()
    {
        dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
        _variableStorage = dialogueRunner.VariableStorage as InMemoryVariableStorage;
        
        LoadMissionData("Missions");
    }

    private void Update()
    {
        
        switch (_missionPhase)
        {
            case -1:
                if (HasVisitedNode("NarratorGreeting"))
                {
                    _missionPhase++;
                    StartCoroutine(ActivateMissionDisplay());
                }
                break;
                
            case 0:
                int completeCount = CountVariablesGraterThanOrEqualTo(1) + 1;
                _missions[_missionPhase].CurrentProgress = completeCount;
                UpdateMissionDisplay();
                if (completeCount == 7)
                {
                    bool isVisited = HasVisitedNode("VillageChiefFirstQuestion");
                    if (isVisited) _missionPhase++;
                }
                break;
            case 1:
                // アリスに2回会ったかどうかを確認する
                bool aliceMetTwice = GetYarnVariable<float>("$AliceMetCount") >= 2;
                _missions[_missionPhase].CurrentProgress = aliceMetTwice ? 1 : 0;
                UpdateMissionDisplay();
                break;
            default:
                UpdateMissionDisplay();
                break;
        }
    }

    private class Mission
    {
        public string Title { get; set; }
        public int CurrentProgress { get; set; }
        public int TotalProgress { get; set; }
        public Mission(string title, int currentProgress, int totalProgress)
        {
            Title = title;
            CurrentProgress = currentProgress;
            TotalProgress = totalProgress;
        }
    }
    
    private void LoadMissionData(string filename)
    {
        TextAsset missionData = Resources.Load<TextAsset>(filename);
        
        if (missionData == null)
        {
            Debug.LogError("ミッションデータが見つかりませんでした。");
            return;
        }
        
        // テキストを1行分割して、リストに追加する
        using StringReader reader = new StringReader(missionData.text);
        while (reader.ReadLine() is { } line)
        {
            string[] data = line.Split(',');
            Mission mission = new Mission(data[0], int.Parse(data[1]), int.Parse(data[2]));
            _missions.Add(mission);
        }
    }
  
    private IEnumerator ActivateMissionDisplay()
    {
        missionCanvas.gameObject.SetActive(true);
        float fadeTime = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            missionCanvas.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            yield return null;
        }
    }
    /// <summary>
    /// ミッションの進行状況を更新する
    /// </summary>
    private void UpdateMissionDisplay()
    {
        string title = _missions[_missionPhase].Title;
        int currentProgress = _missions[_missionPhase].CurrentProgress;
        int totalProgress = _missions[_missionPhase].TotalProgress;
        string color = currentProgress == totalProgress ? "<color=#2E8B57>" : "<color=#B8860B>";
        missionDisplay.text = 
            $"ミッション{_missionPhase+1} : {title}" +
            $"{color}({currentProgress}/{totalProgress})</color>";
    }
    
    
    /// <summary>
    /// metCount変数が指定した値以上の変数の数を返す
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private int CountVariablesGraterThanOrEqualTo(int value)
    {
        int count = 0;
        foreach (var variableName in _metCountVariableNames)
        {
            if (GetYarnVariable<float>(variableName) >= value)
            {
                count++;
            }
        }
        return count;
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
    
    private void OnNodeComplete(string nodeName)
    {
        _visitedNodes.TryAdd(nodeName, true);
    }
   
    /// <summary>
    /// 特定のノードを訪れたかどうかを確認する
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    private bool HasVisitedNode(string nodeName)
    {
        return _visitedNodes.ContainsKey(nodeName) && _visitedNodes[nodeName];
    }
}
