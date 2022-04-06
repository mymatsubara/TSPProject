using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManagement))]
public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static InventoryManagement Inventory { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManagement>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
        _startSequence.Add(Inventory);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }

        yield return null;


        do {
            int statedCount = _startSequence.FindAll(s => s.status == ManagerStatus.Started).Count;
            Debug.Log($"Progress {statedCount}/{_startSequence.Count}");

            yield return null;            
        } while (!_startSequence.TrueForAll(s => s.status == ManagerStatus.Started)) ;
        Debug.Log("All managers started up");
    }
}
