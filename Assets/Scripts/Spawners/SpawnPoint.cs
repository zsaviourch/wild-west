using UnityEngine;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
    [System.Serializable]
    public class CharacterPrefab
    {
        public SpawnType type;
        public GameObject[] prefabs;
    }

        public enum SpawnType
    {
        Player_Weston,
        Enemy_Golem,
        Enemy_Lamia,
        Enemy_IcyGoat,
        Enemy_Tikbalang,
        Enemy_Wendigo,
        Civilian_Gangster,
        Civilian_Marev,
        Civilian_Sarah,
        Civilian_Nick,
        Civilian_Joel,
        Civilian_Nicole,
        Civilian_Frank,
        Civilian_Samantha,
        Civilian_Carlos,
        Civilian_Hector,
        Civilian_Tony,
        Civilian_Luna,
        Civilian_Mary,
        Civilian_Katherine,
        Civilian_Lufacon,
        Boss_Wendigo
    }

    [Header("Spawn Settings")]
    public SpawnType spawnType;

    [Header("Prefab Authoring")]
    public CharacterPrefab[] prefabList;

    private Dictionary<SpawnType, GameObject[]> prefabMap = new Dictionary<SpawnType, GameObject[]>();

    private void Awake()
    {
        foreach (CharacterPrefab prefab in prefabList)
        {
            if (prefabMap.ContainsKey(prefab.type))
            {
                Debug.LogWarning($"Duplicate SpawnType: {prefab.type}");
                continue;
            }
            prefabMap.Add(prefab.type, prefab.prefabs);
        }
    }

    //private void Start()
    //{
    //    Spawn();
    //}

    public GameObject Spawn()
    {
        if (prefabList == null || prefabList.Length == 0)
        {
            Debug.LogError("No prefabs assigned to SpawnPoint");
            return null;
        }

        if (!prefabMap.ContainsKey(spawnType))
        {
            Debug.LogError($"No prefab assigned for SpawnType {spawnType}");
            return null;
        }

        GameObject[] prefabs = prefabMap[spawnType];
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError($"No prefabs assigned for SpawnType {spawnType}");
            return null;
        }

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        return Instantiate(prefab, transform.position, Quaternion.identity, transform);
    }
}
