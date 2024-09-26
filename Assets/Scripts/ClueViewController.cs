using System;
using UnityEngine;
using Yarn.Unity;


public class ClueViewController : MonoBehaviour
{
    public bool isClueViewRunning;
    public LogViewController logViewController;
    public DialogueRunner dialogueRunner;
    public ClueViewController clueViewController;

    private VariableStorageBehaviour _variableStorage;
    private KeyCode _keyCode = KeyCode.H;
    
    private void Start()
    {
        isClueViewRunning = false;
        _variableStorage = dialogueRunner.VariableStorage;
        clueViewController.transform.Find("Canvas/Panel").gameObject.SetActive(isClueViewRunning);
    }

    private void Update()
    {
        System.Single value;
        _variableStorage.TryGetValue("$AliceMetCount", out value);
        Debug.Log("AliceMetCount: " + value);
        //
        // if (Input.GetKeyDown(_keyCode))
        // {
        //     if (isClueViewRunning)
        //     {
        //         logViewController.SetLogViewAvailable(true);
        //         SetClueViewAvailable(false);
        //     }
        //     else
        //     {
        //         logViewController.SetLogViewAvailable(false);
        //         SetClueViewAvailable(true);
        //     }
        //     isClueViewRunning = !isClueViewRunning;
        // }
    }

    public void SetClueViewAvailable(bool isAvailable)
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        Debug.Log("ClueViewAvailable: " + isAvailable);
        clueViewController.transform.Find("Canvas").gameObject.SetActive(isAvailable);
        _keyCode = isAvailable ? KeyCode.H : KeyCode.None;
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
