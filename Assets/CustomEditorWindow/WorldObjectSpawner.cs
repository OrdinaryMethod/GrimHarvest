using UnityEditor;
using UnityEngine;

public class WorldObjectSpawner : EditorWindow
{
    string objectBaseName = "";
    int objectID = 1;
    float objectScale;
    float spawnRadius;

    GameObject hidingSpotPrefab;
    GameObject barrierPrefab;
    GameObject hazardPrefab;

    [MenuItem("Tools/World Object Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WorldObjectSpawner));
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn Settings", EditorStyles.boldLabel);
        objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.5f, 3f);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);

        GUILayout.Label("Objects to Spawn", EditorStyles.boldLabel);
        hidingSpotPrefab = EditorGUILayout.ObjectField("HidingSpot Prefab", hidingSpotPrefab, typeof(GameObject), false) as GameObject;
        barrierPrefab = EditorGUILayout.ObjectField("Barrier Prefab", barrierPrefab, typeof(GameObject), false) as GameObject;
        hazardPrefab = EditorGUILayout.ObjectField("Barrier Prefab", hazardPrefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Spawn Hiding Spot"))
        {
            SpawnHidingSpot();
        }
        else if(GUILayout.Button("Spawn Barrier"))
        {
            SpawnBarrier();
        }
        else if(GUILayout.Button("Spawn Hazard"))
        {
            SpawnHazard();
        }
    }

    private void SpawnHidingSpot()
    {
        if (hidingSpotPrefab == null)
        {
            Debug.LogError("Hiding spot needs a prefab assigned.");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(hidingSpotPrefab, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }

    private void SpawnBarrier()
    {
        if(barrierPrefab == null)
        {
            Debug.LogError("Barrier needs a prefab assigned.");
            return;
        }
        if(objectBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(barrierPrefab, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }

    private void SpawnHazard()
    {
        if (hazardPrefab == null)
        {
            Debug.LogError("Hazard needs a prefab assigned.");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(hazardPrefab, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }
}
