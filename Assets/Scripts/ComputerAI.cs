using System;
using System.Linq;
using UnityEngine;

public class ComputerAI : MonoBehaviour
{
    [SerializeField] private float acceptableDistanceToTownhall;
    [SerializeField] private float acceptableDistanceToUnit;
    
    [SerializeField] private Transform myTownhall;
    [SerializeField] private Transform enemyTownhall;
    private void Update()
    {
        if (!enemyTownhall || !myTownhall)
            return;
        
        var allUnits = FindObjectsOfType<Unit>();
        
        var myUnits = allUnits.Where(x => ((1 << x.gameObject.layer) & LayerMask.GetMask("Human")) != 0).ToList();
        var enemyUnits = allUnits.Where(x => ((1 << x.gameObject.layer) & LayerMask.GetMask("Orc")) != 0).ToList();

        foreach (var enemyUnit in enemyUnits)
        {
            if ((enemyUnit.transform.position - myTownhall.position).sqrMagnitude <= acceptableDistanceToTownhall * acceptableDistanceToTownhall
                && enemyUnit.TryGetComponent<Health>(out var health))
            {
                myUnits.ForEach(x => x.UpdateTarget(health));
                return;
            }
        }

        if (enemyTownhall.TryGetComponent<Health>(out var target))
        myUnits.ForEach(x =>
        {
            var closestEnemy = enemyUnits.FirstOrDefault(y => (y.transform.position - x.transform.position).sqrMagnitude <= acceptableDistanceToUnit * acceptableDistanceToUnit);
            if (closestEnemy && closestEnemy.TryGetComponent<Health>(out var enemyTarget))
            {
                x.UpdateTarget(enemyTarget);
            }
            else
            {
                x.UpdateTarget(target);
            }
        });
    }
}