using UnityEngine;
using Yarn.Unity;


public class AdditionalDialogueRunner : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public LogViewController logViewController;
    public ClueViewController clueViewController;
    
    private SoundManager _soundManager;
    
    private void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
    }
    
    void Update()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            logViewController.gameObject.SetActive(false);
            clueViewController.gameObject.SetActive(false);
            // 決定キーが押されたときのSEを再生
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _soundManager.PlaySE(_soundManager.seDecision);
            }
        }

        if (!dialogueRunner.IsDialogueRunning)
        {
            logViewController.gameObject.SetActive(true);
            clueViewController.gameObject.SetActive(true);
        }
    }
}
