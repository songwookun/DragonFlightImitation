using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EnemyPrefabMapping
{
    public int id;
    public GameObject prefab;
}

public class EnemyData
{
    public int id;
    public string prefabName;
    public int hp;
}

public class SpawnManager : MonoBehaviour
{
    public bool EnableSpawn = true;
    public TextAsset enemyDataCSV;
    public List<EnemyPrefabMapping> enemyPrefabs;
    private Dictionary<int, GameObject> enemyPrefabDict;
    private List<EnemyData> enemyList = new List<EnemyData>();

    private float spawnY = 5.5f;
    private float spawnInterval = 5f;
    private bool bossSpawned = false;

    void Start()
    {
        LoadEnemyDataFromCSV();
        BuildEnemyPrefabDictionary();
        StartCoroutine(WaveSpawner());
    }

    void BuildEnemyPrefabDictionary()
    {
        enemyPrefabDict = new Dictionary<int, GameObject>();
        foreach (var entry in enemyPrefabs)
        {
            if (!enemyPrefabDict.ContainsKey(entry.id))
            {
                enemyPrefabDict.Add(entry.id, entry.prefab);
            }
        }
    }

    IEnumerator WaveSpawner()
    {
        while (true)
        {
            if (EnableSpawn && !GameManager.Instance.IsGameOver)
            {
                float time = Time.time;
                float dynamicSpeed = 1.0f + (time / 60f);

                List<int> idsToSpawn = new List<int>();

                if (time < 10f)
                {
                    idsToSpawn.AddRange(Enumerable.Repeat(0, 5));
                }
                else if (time < 60f)
                {
                    idsToSpawn.AddRange(Enumerable.Repeat(0, 4));
                    idsToSpawn.Add(1);
                }
                else if (time < 90f)
                {
                    idsToSpawn.AddRange(Enumerable.Repeat(0, 3));
                    idsToSpawn.Add(1);
                    idsToSpawn.Add(2);
                }
                else if (!bossSpawned)
                {
                    idsToSpawn.Add(3); 
                    bossSpawned = true;
                }

                List<float> spawnX = new List<float> { -2f, -1f, 0f, 1f, 2f };
                Shuffle(spawnX);

                for (int i = 0; i < idsToSpawn.Count; i++)
                {
                    int id = idsToSpawn[i];
                    Vector3 spawnPos = (id == 3)
                        ? new Vector3(0f, 2.5f, 0f)
                        : new Vector3(spawnX[Mathf.Min(i, spawnX.Count - 1)], spawnY, 0);

                    if (enemyPrefabDict.TryGetValue(id, out GameObject prefab))
                    {
                        GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);

                        var data = enemyList.Find(e => e.id == id);
                        var comp = newEnemy.GetComponent<Enemy>();
                        if (comp != null && data != null)
                        {
                            bool isBoss = (id == 3);
                            comp.Init(data.hp, dynamicSpeed, isBoss);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    void LoadEnemyDataFromCSV()
    {
        if (enemyDataCSV == null)
        {
            Debug.LogError("CSV 파일이 연결되지 않았습니다.");
            return;
        }

        string[] lines = enemyDataCSV.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');
            if (values.Length < 3) continue;

            for (int j = 0; j < values.Length; j++) values[j] = values[j].Trim();

            int id, hp;
            if (!int.TryParse(values[0], out id)) continue;
            string prefabName = values[1];
            if (!int.TryParse(values[2], out hp)) continue;

            EnemyData data = new EnemyData();
            data.id = id;
            data.prefabName = prefabName;
            data.hp = hp;

            enemyList.Add(data);
        }
    }
}
