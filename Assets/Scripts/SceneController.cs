using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private int enemies = 10;
    private List<GameObject> _enemies;

    // Start is called before the first frame update
    void Start()
    {
        _enemies = new List<GameObject>(enemies);
    }

    // Update is called once per frame
    void Update()
    {   
        for (int i = 0; i < _enemies.Count; i++)
        {            
            if (_enemies[i] == null)
                _enemies[i] = spawnEnemy();
        }

        if (_enemies.Count < enemies)
        {
            _enemies.Add(spawnEnemy());
        }
    }

    private GameObject spawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab) as GameObject;

        enemy.transform.position = new Vector3(4, 4, 5);
        enemy.transform.Rotate(0, Random.Range(0, 360), 0);
        return enemy;
    }
}
