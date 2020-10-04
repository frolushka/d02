using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthPoints;

    [SerializeField] private UnityEvent onTakeDamage;
    [SerializeField] private UnityEvent onDeath;
    
    private float _currentHealthPoints;

    private void Start()
    {
        _currentHealthPoints = healthPoints;
    }

    public void TakeDamage(float damage)
    {
        _currentHealthPoints -= damage;
        Debug.Log($"{name} [{_currentHealthPoints}/{healthPoints}] HP has been attacked");
        
        onTakeDamage?.Invoke();
        
        if (_currentHealthPoints <= 0)
        {
            onDeath?.Invoke();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
