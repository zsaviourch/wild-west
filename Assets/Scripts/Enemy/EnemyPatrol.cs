using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
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
    }

    [SerializeField] public PatrolPoint[] patrolPointsGroup;
}
