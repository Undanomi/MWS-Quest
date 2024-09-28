using System;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;


public class StartDialogueButtonController : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public LogViewController logViewController;
    public ClueViewController clueViewController;
    private bool _isAvailable = true;
    
    private void Update()
    {
        if(!_isAvailable)
        {
           return;
        }
        if(dialogueRunner.IsDialogueRunning || 
           logViewController.isLogViewRunning || 
           clueViewController.isClueViewRunning)
        {
           dialogueRunner.transform.Find("Canvas/StartDialogueButton").gameObject.SetActive(false);
        }
        if (!dialogueRunner.IsDialogueRunning && 
            !logViewController.isLogViewRunning && 
            !clueViewController.isClueViewRunning)
        {
            dialogueRunner.transform.Find("Canvas/StartDialogueButton").gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// DialogueButtonの表示を切り替える
    /// </summary>
    /// <param name="isAvailable"></param>
    public void SetStartDialogueButtonAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
        dialogueRunner.transform.Find("Canvas/StartDialogueButton").gameObject.SetActive(isAvailable);
    }
}
