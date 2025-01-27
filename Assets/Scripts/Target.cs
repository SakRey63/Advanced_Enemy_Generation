using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform[] _movePositions;
    [SerializeField] private float _speed;
    
    private int _currentPosition = 0;
    private float _epsilon = 0.1f;
    private Vector3 _currentTargetPosition;
    
    private void Start()
    {
        UpdateCurrentTargetPosition();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTargetPosition, _speed * Time.deltaTime);

        if ((transform.position - _currentTargetPosition).sqrMagnitude < _epsilon)
        {
            _currentPosition = ++_currentPosition % _movePositions.Length;
            
            UpdateCurrentTargetPosition();
        }
    }

    private void UpdateCurrentTargetPosition()
    {
        _currentTargetPosition = _movePositions[_currentPosition].position;
        _currentTargetPosition.y = transform.position.y;
        
        gameObject.transform.LookAt(_currentTargetPosition);
    }
}
