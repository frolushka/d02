using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float maxAttackDistance;
    [SerializeField] private float attackDelay;
    
    [SerializeField] private UnityEvent onTargetPositionChanged;
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onAttack;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private Health _target;
    private Vector3 _targetPosition;
    
    private float _currentAttackDelay;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        _currentAttackDelay -= Time.deltaTime;
        
        if (_target
            && (_target.transform.position - transform.position).sqrMagnitude <= maxAttackDistance * maxAttackDistance)
        {
            _animator.SetBool("IsAttacking", true);
            if (_currentAttackDelay <= 0)
            {
                _target.GetComponent<Health>().TakeDamage(damage);
                onAttack?.Invoke();
                _currentAttackDelay = attackDelay;
            }
            return;
        }
        
        _animator.SetBool("IsAttacking", false);
        
        if (_target)
        {
            _targetPosition = _target.transform.position;
        }
        
        var direction = (_targetPosition - transform.position).normalized;
        _animator.SetFloat("X", direction.x);
        _animator.SetFloat("Y", direction.y);

        MoveTo(_targetPosition);
    }

    private void MoveTo(Vector3 position)
    {
        if ((position - transform.position).sqrMagnitude > 0.05f)
        {
            var newPosition = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
            _animator.SetBool("IsMoving", true);
            _rigidbody.MovePosition(newPosition);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    public void UpdateTarget(Health newTarget)
    {
        _target = newTarget;
    }

    public void UpdateTargetPosition(Vector3 newTargetPosition)
    {
        newTargetPosition.z = transform.position.z;
        _target = null;
        _targetPosition = newTargetPosition;
        onTargetPositionChanged?.Invoke();
    }

    public void Select()
    {
        onSelected?.Invoke();   
    }

    public void Die()
    {
        _animator.SetBool("IsDead", true);
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Health>());
        Destroy(this);
    }
}
