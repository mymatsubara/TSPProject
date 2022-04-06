using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryManagement : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private Dictionary<string, int> _items;
    public string equiped { get; private set; }

    public void Startup()
    {
        Debug.Log("Inventory Manager starting...");

        _items = new Dictionary<string, int>();

        status = ManagerStatus.Started;
    }

    private void DisplayItems()
    {
        var sb = new StringBuilder("Items: ");
        foreach (var item in _items)
        {
            sb.Append($"{item.Key}({item.Value}) ");
        }
        Debug.Log(sb.ToString());
    }

    public void AddItem(string name)
    {
        if(_items.ContainsKey(name))
        {
            _items[name]++;
        } else
        {
            _items[name] = 1;
        }

        DisplayItems();
    }

    public List<string> GetItemList()
    {
        return new List<string>(_items.Keys);
    }

    public int GetItemCount(string name)
    {
        int result = 0;
        return _items.TryGetValue(name, out result) ? result : 0;
    }

    public void Equip(string name)
    {
        if (!_items.ContainsKey(name))
        {
            equiped = null;
        } else
        {
            equiped = name;
        }

    }

    public void Consume(string name)
    {
        if (_items.ContainsKey(name))
        {
            int count = _items[name]--;
            if (count == 1)
            {
                _items.Remove(name);
                equiped = null;
            }
        }        
    }
}
