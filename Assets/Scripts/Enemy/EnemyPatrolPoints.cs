using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolPoints : MonoBehaviour
{
    [System.Serializable]
    public enum EnemyType
    {
        Golem,
        Lamia,
        IcyGoat,
        Tikbalang,
        Wendigo
    }

    [System.Serializable]
    public struct PatrolPoint
    {
       [SerializeField] public EnemyType enemyType;
       [SerializeField] public Transform[] patrolPoints;
       [System.NonSerialized] public bool used;
    }

    [SerializeField] public PatrolPoint[] patrolPointsGroup;

    private void Awake()
    {

    }

    private EnemyPatrolPoints() { }

    public Transform[] GetUnusedPatrolPoints(EnemyType enemyType)
    {
        for (int i = 0; i < patrolPointsGroup.Length; i++)
        {
            if (patrolPointsGroup[i].enemyType == enemyType && !patrolPointsGroup[i].used)
            {
                patrolPointsGroup[i].used = true;
                return patrolPointsGroup[i].patrolPoints;
            }
        }

        return null;
    }
}
