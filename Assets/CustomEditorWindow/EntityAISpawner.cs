#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EntityAISpawner : EditorWindow
{
    //Entity AI
    string entityBaseName = "";
    int entityID = 1;
    float entityScale = 1;

    //Patrol Point
    string entityPatrolPointName = "EntityPatrolPoint";
    int entityPatrolPointID = 1;
    float patrolPointScale = 1;

    //General values
    float spawnRadius = 0.5f;

    GameObject entityAIPrefab;
    GameObject entityPatrolPointPrefab;

    [MenuItem("Tools/Entity AI Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EntityAISpawner));
    }

    private void OnGUI()
    {
        GUILayout.Label("General Spawn Settings", EditorStyles.boldLabel);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);

        //Entity AI
        GUILayout.Label("Entity AI Spawn Settings", EditorStyles.boldLabel);
        entityBaseName = EditorGUILayout.TextField("Entity Name", entityBaseName);
        entityID = EditorGUILayout.IntField("Object ID", entityID);
        entityScale = EditorGUILayout.Slider("Object Scale", entityScale, 0.5f, 10f);

        if (GUILayout.Button("Spawn Entity AI"))
        {
            SpawnEntityAI();
        }

        //Patrol Points
        GUILayout.Label("Entity Patrol Point Spawn Settings", EditorStyles.boldLabel);
        entityPatrolPointID = EditorGUILayout.IntField("Object ID", entityPatrolPointID);

        GUILayout.Label("Spawn Entities and their Components", EditorStyles.boldLabel);
        entityAIPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/MainEntities/EntityAI.prefab", typeof(GameObject));
        entityPatrolPointPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/MainEntities/EntityPatrolPoint.prefab", typeof(GameObject));

        if(GUILayout.Button("Spawn Entity Patrol Points"))
        {
            SpawnEntityPatrolPoint();
        }
    }

    private void SpawnEntityAI()
    {
        if (entityBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the entity ai.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(entityAIPrefab, spawnPos, Quaternion.identity);
        newObject.name = entityBaseName + " ("+ entityID + ")";
        newObject.transform.localScale = Vector3.one * entityScale;

        entityID++;
    }

    private void SpawnEntityPatrolPoint()
    {
        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(entityPatrolPointPrefab, spawnPos, Quaternion.identity);
        newObject.name = entityPatrolPointName + " ("+ entityPatrolPointID + ")";
        newObject.transform.localScale = Vector3.one * patrolPointScale;

        entityPatrolPointID++;
    }
}
#endif