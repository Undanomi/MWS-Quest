using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class NpcController : MonoBehaviour
{
    private DialogueRunner _dialogueRunner; // Yarn Spinner の DialogueRunner を参照
    private LogViewController _logViewController; // LogViewController を参照
    private ClueViewController _clueViewController; // ClueViewController を参照
    public string conversationNode = "StartConversation"; // Yarn の会話のノード名
    private Rigidbody2D _rb2d;
    private CircleCollider2D _col2d;
    private bool _isPlayerInRange;

    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _col2d = GetComponent<CircleCollider2D>();
        _rb2d.isKinematic = true;
        _col2d.isTrigger = true;
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        _logViewController = FindObjectOfType<LogViewController>();
        _clueViewController = FindObjectOfType<ClueViewController>();
    }

    private void Update()
    {
        if (_isPlayerInRange &&
            Input.GetKeyDown(KeyCode.E) &&
            _logViewController.isLogViewRunning == false &&
            _clueViewController.isClueViewRunning == false &&
            !_dialogueRunner.IsDialogueRunning
           )
        {
            _dialogueRunner.StartDialogue(conversationNode);
        }
    }

    // プレイヤーがトリガー範囲に入ったときに呼ばれる関数
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // トリガーに入ったオブジェクトがプレイヤーか確認
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // トリガーから出たオブジェクトがプレイヤーか確認
        {
            _isPlayerInRange = false;
        }
    }
}