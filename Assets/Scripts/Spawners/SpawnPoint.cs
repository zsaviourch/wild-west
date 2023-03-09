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

    /*private void Start()
    {
        Spawn();
    }*/

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
            
            GameObject prefabInstantiated = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            /*if (playerInstantiated.GetComponent<GunManager>())
            {
                playerInstantiated.GetComponent<GunManager>().GunName = GunManager.GunType.OnyxSniper;
                *//*                prefab.GetComponent<GunManager>().randomGun = true;
                *//*
            }*/

            return prefabInstantiated;
        }
        else
        {
            // Check if there is an existing player in the world
            var controllers = GameObject.FindObjectsOfType<PlayerController>(true);
            if (controllers.Length > 0)
            {
                Debug.Log("A player already exists in the world!");
                PlayerController playerController = controllers[0];
                playerController.gameObject.transform.position = transform.position; // Set position of existing player to SpawnPoint position
                playerController.UnDie();
                HealthAndEnergy healthScript = playerController.gameObject.GetComponent<HealthAndEnergy>();
                if (healthScript.currentHealth <= 0)
                {
                    healthScript.currentHealth = healthScript.health;
                    healthScript.currentEnergyAmount = healthScript.energyInitialAmount;

                }
                playerController.gameObject.GetComponent<GunManager>().TakeGun(null, true);
                return playerController.gameObject;
            }
            else
            {
                GameObject prefab = prefabs[0];
                GameObject newPlayer = Instantiate(prefab, transform.position, Quaternion.identity);
                if (newPlayer != null)
                {
                    Debug.Log("new player instantiated!");
                }
                return newPlayer;
            }
        }
    }
}
