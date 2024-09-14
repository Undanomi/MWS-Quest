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
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<CircleCollider2D>();
        rb2d.isKinematic = true;
        col2d.isTrigger = true;
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
    
    // プレイヤーがトリガー範囲に入ったときに呼ばれる関数
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter");
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))  // トリガーに入ったオブジェクトがプレイヤーか確認
        {
            Debug.Log("Player");
            // Yarn Spinner の会話を開始
            if (!dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.StartDialogue(conversationNode);
            }else{
                Debug.Log("Dialogue is already running");
            }
        }
    }
}