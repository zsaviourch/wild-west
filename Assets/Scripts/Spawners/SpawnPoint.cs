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

        if (spawnType != SpawnType.Player_Weston)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            
            GameObject playerInstantiated = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            if (playerInstantiated.GetComponent<GunManager>())
            {
                playerInstantiated.GetComponent<GunManager>().GunName = GunManager.GunType.OnyxSniper;
                /*                prefab.GetComponent<GunManager>().randomGun = true;
                */
            }

            return playerInstantiated;
        }
        else
        {
            // Check if there is an existing player in the world
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                Debug.Log("A player already exists in the world!");
                players[0].transform.position = transform.position; // Set position of existing player to SpawnPoint position
                return players[0];
            }
            else
            {
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                GameObject newPlayer = Instantiate(prefab, transform.position, Quaternion.identity);
                return newPlayer;
            }
        }
    }
}
