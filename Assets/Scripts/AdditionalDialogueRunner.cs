using UnityEngine;
using Yarn.Unity;


public class AdditionalDialogueRunner : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public LogViewController logViewController;
    public ClueViewController clueViewController;
    
    void Update()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            logViewController.gameObject.SetActive(false);
            clueViewController.gameObject.SetActive(false);
        }
        if(!dialogueRunner.IsDialogueRunning)
        {
            logViewController.gameObject.SetActive(true);
            clueViewController.gameObject.SetActive(true);
        }
    }
}
