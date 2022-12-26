using System.Collections;
using UnityEngine;

public class Task1_ThrowRigidBody : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Vector2 _force;

    private bool _isFlying;
    private Vector2 _startPos;

    public void SetForce(Vector2 force)
    {
        _force = force;
    }
    private IEnumerator Start()
    {
        _startPos = transform.position;
        yield return new WaitForSeconds(1f);
        SendCircle();
        Debug.Log("Press Space to Relaunch Circle");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendCircle();
        }
    }

    private void SendCircle()
    {
        if(_isFlying) return;
        _isFlying = true;
        _rigidbody2D.AddForce(_force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        _isFlying = false;
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.angularVelocity = 0;
        if (col.transform.CompareTag("Finish"))
        {
            Debug.Log("Puzzle Solved");
        }
        else
        {
            Debug.Log("Try again");
            transform.position = _startPos;
        }
    }
}
