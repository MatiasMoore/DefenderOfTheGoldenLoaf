using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SlowArea : MonoBehaviour
{
    [SerializeField]
    private float _multiplier = 0.5f;
    [SerializeField]
    private LayerMask _slowLayer;
    [SerializeField]
    private bool _isActive = false;

    private Timer _timer;

    private Dictionary<Collider2D, float> _frogsSpeed = new Dictionary<Collider2D, float>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Frog"))
        {
            _frogsSpeed.Add(collision, collision.gameObject.GetComponent<ObjectMovement>().GetMaxSpeed());
            collision.gameObject.GetComponent<ObjectMovement>().SetMaxSpeed(collision.gameObject.GetComponent<ObjectMovement>().GetMaxSpeed() * _multiplier);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isActive)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Frog"))
        {
            if (_frogsSpeed.TryGetValue(collision, out float speed))
            {
                collision.gameObject.GetComponent<ObjectMovement>().SetMaxSpeed(speed);
                _frogsSpeed.Remove(collision);
            }
        }
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        if (_isActive)
        {
            return;
        }
        Debug.Log("Slow Area activate");
        _isActive = true;           
    }

    [ContextMenu("Deactivate")]
    public void Deactivate()
    {
        if (!_isActive)
        {
            return;
        }

        _isActive = false;
        foreach (var frog in _frogsSpeed)
        {
            frog.Key.GetComponent<ObjectMovement>().SetMaxSpeed(frog.Value);
        }
        _frogsSpeed.Clear();

        Destroy(gameObject);
    }

    public void ActivateForSeconds(float seconds)
    {
        _timer = new Timer(seconds);
        _timer.OnTimerDone += Deactivate;
        _timer.StartTimer();
        Activate();
    }

    private void Update()
    {
        if (_timer != null)
        {
            _timer.Tick();
        }
    }

}
