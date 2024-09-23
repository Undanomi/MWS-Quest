using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(CapsuleCollider2D))]
public class ColliderNotes : MonoBehaviour
{
    [HideInInspector] public string circleColliderNote = "会話判定用";
    [HideInInspector] public string capsuleColliderNote = "キャラクター同士のあたり判定用";
}
