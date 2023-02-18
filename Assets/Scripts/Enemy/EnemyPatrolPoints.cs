using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolPoints : MonoBehaviour
{
    private static EnemyPatrolPoints instance = null;

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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private EnemyPatrolPoints() { }

    public static EnemyPatrolPoints Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyPatrolPoints>();

                if (instance == null)
                {
                    Debug.LogError("There is no EnemyPatrolPoints instance in the scene.");
                }
            }

            return instance;
        }
    }

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
