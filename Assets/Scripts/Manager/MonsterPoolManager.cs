using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterPoolManager : MonoBehaviour
{
    public static MonsterPoolManager Instance;

    [Header("对象池配置")]
    public GameObject monsterPrefab;
    public int maxMonsterCount = 18;
    public int totalPoolSize = 18;
    public int spawnCountPerBatch = 3;
    public float spawnInterval = 5f;
    public List<Transform> spawnPoints;

    //两个池 一个放空闲的 一个放使用中的
    private List<Monster> idleMonsterPool = new List<Monster>();
    private List<Monster> activeMonsterPool = new List<Monster>();


    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ////初始化对象池
        //InitializeMonsterPool();
        ////分批生成
        //spawnCoroutine =StartCoroutine(SpawnMonsterBatchCoroutine());
    }

    #region 1. 对象池初始化
    /// <summary>
    /// 提前创建指定数量的怪物，存入空闲池
    /// </summary>
    private void InitializeMonsterPool()
    {
        for (int i = 0; i < totalPoolSize; i++)
        {
            //实例化怪物
            GameObject monsterObj = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);
            monsterObj.SetActive(false);

            //获取怪物脚本组件
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                idleMonsterPool.Add(monster); //初始全部存入空闲池 用的时候加入使用池
            }
        }
    }
    #endregion

    #region 2. 分批次生成怪物
    private IEnumerator SpawnMonsterBatchCoroutine()
    {
        while (activeMonsterPool.Count < maxMonsterCount)
        {
            int spawnCount = 0; //记录本次实际生成数量

            //循环取出空闲怪物
            for (int i = 0; i < spawnCountPerBatch; i++)
            {
                //检查是否有空闲怪物 没有就退出循环
                if (idleMonsterPool.Count <= 0)
                    break;

                //从空闲池取出第一只怪物
                Monster monsterToSpawn = idleMonsterPool[0];
                if (monsterToSpawn == null)
                {
                    idleMonsterPool.RemoveAt(0);
                    continue;
                }
                idleMonsterPool.RemoveAt(0);

                //随机选择一个生成点
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                monsterToSpawn.transform.position = randomSpawnPoint.position;
                monsterToSpawn.transform.rotation = Quaternion.identity;
                monsterToSpawn.pathPointList.Clear();
                monsterToSpawn.currentPathPointIndex = 0;

                //激活怪物，加入使用中池
                monsterToSpawn.gameObject.SetActive(true);
                activeMonsterPool.Add(monsterToSpawn);

                spawnCount++;
            }
            //生成下一批
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    #endregion

    #region 3. 回收死亡怪物
    /// <summary>
    /// 回收怪物到空闲池
    /// </summary>
    public void RecycleMonster(Monster monster)
    {
        if (monster == null) return;

        //从使用中池移除，加入空闲池
        if (activeMonsterPool.Contains(monster))
        {
            activeMonsterPool.Remove(monster);
        }
        if (!idleMonsterPool.Contains(monster))
        {
            idleMonsterPool.Add(monster);
        }

        //隐藏
        monster.gameObject.SetActive(false);

    }
    #endregion

    #region 4.场景切换相关

    //场景切换时停止协程并清空对象池 防止切换场景时协程报错空引用
    private void OnSceneUnloaded(Scene scene)
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        //销毁所有活跃和空闲的怪物
        foreach (var monster in activeMonsterPool)
        {
            if (monster != null)
            {
                Destroy(monster.gameObject);
            }
        }
        activeMonsterPool.Clear();

        foreach (var monster in idleMonsterPool)
        {
            if (monster != null)
            {
                Destroy(monster.gameObject);
            }
        }
        idleMonsterPool.Clear();
    }
    //场景加载完成时触发
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //进入游戏场景时初始化对象池
        if (scene.name == "GameScene")
        {
            AddSpawnPoint();
            InitializeMonsterPool();
            spawnCoroutine = StartCoroutine(SpawnMonsterBatchCoroutine());
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    private void AddSpawnPoint()
    {
        spawnPoints.Clear();
        GameObject[] nowSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in nowSpawnPoints)
        {
            spawnPoints.Add(point.transform);
        }
        if (spawnPoints.Count == 0)
        {
            AddSpawnPoint();
        } 
        else
        return;
    }
}
