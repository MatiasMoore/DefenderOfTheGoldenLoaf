using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerKickController : MonoBehaviour
{
    [Header("Kick")]
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
    private Animator _playerAnimator;
    [SerializeField]
    private Animator _smokeAnimator;
    [SerializeField]
    private float _cooldownTime;
    private bool _isCooldown = false;
    [SerializeField]
    private Stamina _stamina;
    [SerializeField]
    private int _staminaCost;
    [SerializeField]
    private GameObject _arrowHolder;
 
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

        RotateGameObjectTo(direction, _kickAreaHolder);

        _playerAnimator.SetTrigger("Kick");
        _smokeAnimator.SetTrigger("Kick");
        //Debug.Log($"Kick to {direction}");
        
        
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = _frogMask;
        contactFilter.useLayerMask = true;

        //var colliders = Physics2D.OverlapAreaAll(_kickArea.bounds.center - _kickArea.bounds.size / 2, _kickArea.bounds.center + _kickArea.bounds.size / 2, _frogMask,0);
        //List<Collider2D> colliders = new List<Collider2D>();
        //var count = Physics2D.OverlapCollider(_kickArea, contactFilter, colliders);
        _isCooldown = true;
        StartCoroutine(Cooldown());


        //count = Physics2D.OverlapCollider(_kickArea, contactFilter, colliders);
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


    }

    private void RotateGameObjectTo(Vector2 pos, GameObject gameObj)
    {
        Vector2 toPos = pos - (Vector2)transform.position;

        var desiredQuat = Quaternion.LookRotation(toPos);

        desiredQuat *= Quaternion.AngleAxis(-90, Vector3.up);

        gameObj.transform.rotation = desiredQuat;
    }

    private List<GameObject> GetAllFrogs()
    {
       
        List<GameObject> frogs = new List<GameObject>();
        foreach (var frog in GameObject.FindGameObjectsWithTag("Frog"))
        {
            frogs.Add(frog);
        }
        return frogs;
        
    }

    public LayerMask _wallMask;
    private bool CanSeeFrog(GameObject frog)
    {
        //wall between player and frog
        RaycastHit2D hit = Physics2D.Raycast(transform.position, frog.transform.position - transform.position, Vector2.Distance(transform.position, frog.transform.position), _wallMask);
        return hit.collider == null;
    }

    public float _dot;
    public float _distance;

    private List<GameObject> GetFrogsInKickArea()
    {
        
        List<GameObject> frogs = new List<GameObject>();
        Vector2 watchDirection = PlayerControls.Instance.getTouchWorldPosition2d() - (Vector2)transform.position;
        foreach (var frog in GetAllFrogs())
        {
            Vector2 toFrog = frog.transform.position - transform.position;
            float dot = Vector2.Dot(toFrog.normalized, watchDirection.normalized);
            float distance = toFrog.magnitude;
            if (dot > _dot && distance < _distance)
            {
                if (CanSeeFrog(frog))
                {
                    frogs.Add(frog);
                }          
            }             
        }
        return frogs;
    }

    private void OnDrawGizmos()
    {
        DebugDraw.DrawSphere(transform.position, _distance, Color.red);
    }
}
