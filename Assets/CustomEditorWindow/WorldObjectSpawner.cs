#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class WorldObjectSpawner : EditorWindow
{
    string objectBaseName = "";
    int objectID = 1;
    float objectScale = 1;
    float spawnRadius = 0.5f;

    GameObject hidingSpotPrefab;
    GameObject barrierPrefab;
    GameObject hazardPrefab;
    GameObject SceneTransitionerPrefab;

    [Header("Doodads")]
    GameObject HangingCorpseUpPrefab;
    GameObject HangingCorpseDownPrefab;

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
        hidingSpotPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/HidingSpots/HidingSpot.prefab", typeof(GameObject));
        barrierPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/Barriers/Barrier.prefab", typeof(GameObject));
        hazardPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/Hazards/Hazard.prefab", typeof(GameObject));
        SceneTransitionerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/SceneTransitioners/SceneTransitioner.prefab", typeof(GameObject));
        HangingCorpseUpPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/Doodads/HangingCorpse_Chain_Up.prefab", typeof(GameObject));
        HangingCorpseDownPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/Doodads/HangingCorpse_Chain_Down.prefab", typeof(GameObject));


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
        else if (GUILayout.Button("Scene Transitioner"))
        {
            SpawnSceneTransitioner();
        }
        else if (GUILayout.Button("Hanging Corpse Up"))
        {
            SpawnHangingCorpseUp();
        }
        else if (GUILayout.Button("Hanging Corpse Down"))
        {
            SpawnHangingCorpseDown();
        }
    }

    private void SpawnHidingSpot()
    {
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
    private void SpawnSceneTransitioner()
    {
        if (SceneTransitionerPrefab == null)
        {
            Debug.LogError("Scene Transitioner needs a prefab assigned.");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(SceneTransitionerPrefab, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + " (" + objectID + ")";

        objectID++;
    }

    private void SpawnHangingCorpseUp()
    {
        if(HangingCorpseUpPrefab == null)
        {
            Debug.LogError("Hanging Corpse Up needs a prefab assigned.");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(HangingCorpseUpPrefab, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + " (" + objectID + ")";

        objectID++;
    }

    private void SpawnHangingCorpseDown()
    {
        if (HangingCorpseDownPrefab == null)
        {
            Debug.LogError("Hanging Corpse Down needs a prefab assigned.");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Please enter a base name for the object.");
            return;
        }

        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newObject = Instantiate(HangingCorpseDownPrefab, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + " (" + objectID + ")";

        objectID++;
    }
}
#endif
