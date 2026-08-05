using UnityEngine;
using System.Collections.Generic;

public class SpawnTigger : MonoBehaviour
{
    [System.Serializable]
    public class EnemyArea
    {
        public string areaName;
        public Transform areaCenter;
        public GameObject enemyPrefab;
        public List<Transform> spawnPoints;
    }
    
    [Header("Area")]
    [SerializeField] private List<EnemyArea> enemyAreas = new List<EnemyArea>();
    
    [Header("PlayerTarget")]
    [SerializeField] private Transform playerTarget;  
    
    [Header("Settings")]
    [SerializeField] private float activationMargin = 0.3f;
    [SerializeField] private float checkInterval = 0.3f;
    
    private Camera cam;
    private float timer;
    private Dictionary<EnemyArea, List<GameObject>> spawnedEnemies = new Dictionary<EnemyArea, List<GameObject>>();
    
    void Start()
    {
        cam = Camera.main;
        
        
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTarget = player.transform;
        }
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0;
        
        foreach (var area in enemyAreas)
        {
            if (area.areaCenter == null) continue;
            
            bool inScreen = IsAreaInScreen(area);
            bool hasSpawned = spawnedEnemies.ContainsKey(area) && spawnedEnemies[area].Count > 0;
            
            if (inScreen && !hasSpawned)
            {
                SpawnAreaEnemies(area);
            }
            else if (!inScreen && hasSpawned)
            {
                DespawnAreaEnemies(area);
            }
        }
    }
    
    bool IsAreaInScreen(EnemyArea area)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(area.areaCenter.position);
        return viewPos.x >= -activationMargin && 
               viewPos.x <= 1 + activationMargin &&
               viewPos.y >= -activationMargin && 
               viewPos.y <= 1 + activationMargin &&
               viewPos.z > 0;
    }
    
    void SpawnAreaEnemies(EnemyArea area)
    {
        if (area.enemyPrefab == null) return;
        
        List<GameObject> enemies = new List<GameObject>();
        
        foreach (var spawnPoint in area.spawnPoints)
        {
            if (spawnPoint == null) continue;
            
            GameObject enemy = Instantiate(
                area.enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
            
            
            AssignTarget(enemy);
            
            enemies.Add(enemy);
        }
        
        spawnedEnemies[area] = enemies;
    }
    
    void DespawnAreaEnemies(EnemyArea area)
    {
        if (!spawnedEnemies.ContainsKey(area)) return;
        
        foreach (var enemy in spawnedEnemies[area])
        {
            if (enemy != null)
                Destroy(enemy);
        }
        
        spawnedEnemies[area].Clear();
    }
    
    
    void AssignTarget(GameObject enemy)
    {
        if (playerTarget == null) return;
        
     
        var targetField = enemy.GetComponent<EnemyMovement>();
        
        if (targetField != null)
        {
           
            targetField.target = playerTarget;
            return;
        }
        
       
        enemy.SendMessage("SetTarget", playerTarget, SendMessageOptions.DontRequireReceiver);
        
       
        var enemyScript = enemy.GetComponent<MonoBehaviour>();
        if (enemyScript != null)
        {
            var field = enemyScript.GetType().GetField("target");
            if (field != null)
            {
                field.SetValue(enemyScript, playerTarget);
            }
            
            var prop = enemyScript.GetType().GetProperty("target");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(enemyScript, playerTarget);
            }
        }
    }
}