using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] LayerMask _aimLayerMask;

    Animator _animator;
    BowControl bowControl;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        bowControl = GetComponent<BowControl>();
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
            _animator.SetBool("Aiming", true);
            bowControl.isFiring = true;
        }
        else
        {
            _animator.SetBool("Aiming", false);
            bowControl.isFiring = false;
        }
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
