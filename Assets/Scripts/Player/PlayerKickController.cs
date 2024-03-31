using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerKickController : MonoBehaviour
{
    [SerializeField]
    private GameObject _kickAreaHolder;
    [SerializeField]
    private Collider2D _kickArea;
    [SerializeField]
    private float _kickForce;
    [SerializeField]
    private LayerMask _frogMask;
    [SerializeField]
    private float _pushTime;
    [SerializeField]
    private Animator _animator;
    
    public void Init()
    {
        PlayerControls.Instance.AttackKickEvent += Kick;
    }

    private void Kick(Vector2 direction)
    {
        _animator.SetTrigger("Kick");
        Debug.Log($"Kick to {direction}");
        RotateKickAreaTo(direction);
        
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = _frogMask;
        List<Collider2D> colliders = new List<Collider2D>();
        int count = _kickArea.OverlapCollider(contactFilter, colliders);
        
        foreach (var collider in colliders)
        {
            Debug.Log($"Kick to {collider.gameObject.name}");
            Push(collider);
        }

    }

    private void Push(Collider2D collider)
    {

        MovingAIStateManager movingAIStateManager = collider.GetComponent<MovingAIStateManager>();
        Vector2 direction = (collider.transform.position - transform.position).normalized;
        if (movingAIStateManager != null)
        {
            movingAIStateManager.StopForSeconds(_pushTime);
        }

        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * _kickForce, ForceMode2D.Impulse);


    }

    private void RotateKickAreaTo(Vector2 pos)
    {
        Vector2 toPos = pos - (Vector2)transform.position;

        var desiredQuat = Quaternion.LookRotation(toPos);

        desiredQuat *= Quaternion.AngleAxis(-90, Vector3.up);

        _kickAreaHolder.transform.rotation = desiredQuat;
    }
}
