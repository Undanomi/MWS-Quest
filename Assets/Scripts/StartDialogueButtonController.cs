using System;
using UnityEngine;
using Yarn.Unity;


public class StartDialogueButtonController : MonoBehaviour
{
    private DialogueRunner _dialogueRunner;
    private LogViewController _logViewController;
    private ClueViewController _clueViewController;
    private bool _isAvailable = true;
    
    private void Start()
    {
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        _logViewController = FindObjectOfType<LogViewController>();
        _clueViewController = FindObjectOfType<ClueViewController>();
    }
    
    private void Update()
    {
        if(!_isAvailable)
        {
           return;
        }
        if(_dialogueRunner.IsDialogueRunning || 
           _logViewController.isLogViewRunning || 
           _clueViewController.isClueViewRunning)
        {
           _dialogueRunner.transform.Find("Canvas/StartDialogueButton").gameObject.SetActive(false);
        }
        if (!_dialogueRunner.IsDialogueRunning && 
            !_logViewController.isLogViewRunning && 
            !_clueViewController.isClueViewRunning)
        {
            _dialogueRunner.transform.Find("Canvas/StartDialogueButton").gameObject.SetActive(true);
        }
    }
    
    public void SetStartDialogueButtonAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
        _dialogueRunner.transform.Find("Canvas/StartDialogueButton").gameObject.SetActive(isAvailable);
    }
}
