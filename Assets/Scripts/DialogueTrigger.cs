using System;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class NPCTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;  // Yarn Spinner の DialogueRunner を参照
    public string conversationNode = "StartConversation";  // Yarn の会話のノード名
    private Rigidbody2D rb2d;
    private CircleCollider2D col2d;
    private bool isPlayerInRange = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<CircleCollider2D>();
        rb2d.isKinematic = true;
        col2d.isTrigger = true;
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void Update()
    {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && 
                dialogueRunner.GetComponent<LogView>().isLogViewEnable == false)
            {
                if (!dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.StartDialogue(conversationNode);
                }
            }
    }

    // プレイヤーがトリガー範囲に入ったときに呼ばれる関数
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // トリガーに入ったオブジェクトがプレイヤーか確認
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // トリガーから出たオブジェクトがプレイヤーか確認
        {
            isPlayerInRange = false;
        }
    }
}