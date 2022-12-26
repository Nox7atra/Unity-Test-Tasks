using UnityEngine;

public class Task1_Solution : MonoBehaviour
{
    [SerializeField] private Task1_ThrowRigidBody _taskObject;
    [SerializeField] private float _maxHeight = 6f;
    [SerializeField] private float _distance = 6f;
    private void OnEnable()
    {
        var g = Mathf.Abs(Physics2D.gravity.y);
        var vY = Mathf.Sqrt(2 * _maxHeight * g);
        var t = vY / g * 2;
        var vX = _distance / t;
        _taskObject.SetForce(new Vector2(vX, vY));
    }
}
