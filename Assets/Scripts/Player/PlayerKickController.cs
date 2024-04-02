using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class PlayerKickController : MonoBehaviour
{
    [Header("Kick params")]
    [SerializeField, Range(0, 179)]
    private float _fovAngle;
    [SerializeField, Range(0, 10)]
    private float _distance;
    [SerializeField, Range(0,3)]
    private float _kickForce;
    [SerializeField, Min(0)]
    private int _staminaCost;
    [SerializeField, Min(0)]
    private float _cooldownTime;
    [SerializeField, Min(0)]
    private float _pushTime;

    [Header("Configuration")]
    [SerializeField]
    private Animator _playerAnimator;
    [SerializeField]
    private Animator _smokeAnimator;
    [SerializeField]
    private GameObject _smokeHolder;
    [SerializeField]
    private Stamina _stamina;
    [SerializeField]
    private GameObject _arrowHolder;
    [SerializeField]
    private LayerMask _frogMask;
    [SerializeField]
    private LayerMask _wallMask;

    private bool _isCooldown = false;

    public void Init()
    {
        PlayerControls.Instance.AttackKickEvent += Kick;
    }

    private void Update()
    {
        RotateGameObjectTo(PlayerControls.Instance.getTouchWorldPosition2d(), _arrowHolder);
    }

    private void Kick(Vector2 direction)
    {
        if (_isCooldown)
        {
            return;
        }

        if (!_stamina.DecreaseStamina(_staminaCost))
        {
            return;
        }

        RotateGameObjectTo(direction, _smokeHolder);

        _playerAnimator.SetTrigger("Kick");
        _smokeAnimator.SetTrigger("Kick");

        _isCooldown = true;
        StartCoroutine(Cooldown());

        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.kick);

        foreach (var collider in GetFrogsInKickArea())
        {
            Debug.Log($"Kick to {collider.gameObject.name}");
            Push(collider);
        }

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        _isCooldown = false;
        yield break;
    }

    private void Push(GameObject collider)
    {

        MovingAIStateManager movingAIStateManager = collider.GetComponent<MovingAIStateManager>();
        Vector2 direction = (collider.transform.position - transform.position).normalized;
        if (movingAIStateManager != null)
        {
            movingAIStateManager.StopForSeconds(_pushTime);
        }

        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * _kickForce, ForceMode2D.Impulse);

        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFX.frog);
    }

    private void RotateGameObjectTo(Vector2 pos, GameObject gameObj)
    {
        Vector2 toPos = pos - (Vector2)transform.position;

        var desiredQuat = Quaternion.LookRotation(toPos);

        desiredQuat *= Quaternion.AngleAxis(-90, Vector3.up);

        gameObj.transform.rotation = desiredQuat;
    }

    private Collider2D[] GetAllFrogsInDistance(float distance)

    {
        Vector2 lowerLeftCorner = new Vector2(transform.position.x - distance, transform.position.y - distance);
        Vector2 upperRightCorner = new Vector2(transform.position.x + distance, transform.position.y + distance);
        return Physics2D.OverlapAreaAll(lowerLeftCorner, upperRightCorner, _frogMask);
    }

    private bool CanSeeFrog(GameObject frog)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, frog.transform.position - transform.position, Vector2.Distance(transform.position, frog.transform.position), _wallMask);
        return hit.collider == null;
    }

    private List<GameObject> GetFrogsInKickArea()
    {
        
        List<GameObject> frogs = new List<GameObject>();
        Vector2 watchDirection = PlayerControls.Instance.getTouchWorldPosition2d() - (Vector2)transform.position;
        foreach (var frog in GetAllFrogsInDistance( _distance))
        {
            Vector2 toFrog = frog.transform.position - transform.position;
            float angle = Vector2.Angle(watchDirection, toFrog);      

            if (angle <= (_fovAngle / 2f) && CanSeeFrog(frog.gameObject))
            {
                frogs.Add(frog.gameObject);      
            }             
        }
        return frogs;
    }

    private void OnDrawGizmos()
    {
        Vector2 watchDirection;
        if (Application.isPlaying)
        {
            watchDirection = PlayerControls.Instance.getTouchWorldPosition2d() - (Vector2)transform.position;     
            watchDirection.Normalize();
        }
        else
        {
            watchDirection = Vector2.right;
        }

        float halfFov = _fovAngle / 2f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.forward);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.forward);
     
        Vector2 leftRayDirection = leftRayRotation * watchDirection;
        Vector2 rightRayDirection = rightRayRotation * watchDirection;
        
        float angleRad = Mathf.Deg2Rad * (90 - halfFov);
        float boundaryDistance = _distance / Mathf.Sin(angleRad);

        Vector2 leftBoundary = (Vector2)transform.position + leftRayDirection * boundaryDistance;
        Vector2 rightBoundary = (Vector2)transform.position + rightRayDirection * boundaryDistance;

        DebugDraw.DrawLine(transform.position, leftBoundary, Color.red);
        DebugDraw.DrawLine(transform.position, rightBoundary, Color.red);
        DebugDraw.DrawLine(leftBoundary, rightBoundary, Color.red);

    }
}
