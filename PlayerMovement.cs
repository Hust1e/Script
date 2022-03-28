using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] LayerMask _aimLayerMask;

    Animator _animator;
    BowControl _bowControl;

    private bool _rightClick = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _bowControl = GetComponent<BowControl>();
    }
    void Update()
    {
        AimTowardMouse();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 _movement = new Vector3(horizontal, 0f, vertical);

        if(_movement.magnitude > 0f)
        {
            _movement.Normalize();
            _movement *= _speed * Time.deltaTime;
            transform.Translate(_movement, Space.World);
        }

        float velocityZ = Vector3.Dot(_movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(_movement.normalized, transform.right);

        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            _rightClick = true;
            _bowControl.isFiring = true;
        }
        if(Input.GetMouseButtonUp(1))
        {
            _rightClick = false;
            _bowControl.isFiring = false;
        }
        _animator.SetBool("Aiming", _rightClick);
    }
    private void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
        {
            var _direction = hitInfo.point - transform.position;
            _direction.y = 0f;
            _direction.Normalize();
            transform.forward = _direction;
        }
    }
}
