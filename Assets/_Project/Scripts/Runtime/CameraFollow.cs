using _Project.Scripts.Runtime;
using _Project.Scripts.Runtime.Character;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private MainCharacter _mainCharacter;
    [Space]
    [SerializeField] private bool _followByX;
    [SerializeField] private bool _followByY;
    [SerializeField] private bool _followByZ;

    /*private void OnEnable()
    {
        _target = GameObject.FindGameObjectsWithTag("Target")[0].transform;
    }*/

    private void Update()
    {
        var targetPosition = new Vector3(
            _followByX ? _target.position.x : transform.position.x,
            _followByY ? _target.position.y : transform.position.y,
            _followByZ ? _target.position.z : transform.position.z);
        MoveCamera(targetPosition);
    }

    private void MoveCamera(Vector3 targetPosition)
    {
        if (_followByX)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
        }
        if (_followByY)
        {
            if (transform.position.y > targetPosition.y) return;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
        }
        if (_followByZ)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
        }
    }
}
