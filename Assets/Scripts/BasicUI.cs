using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{    
    private void OnGUI()
    {
        int posX = 10;
        int posY = 80;
        int width = 100;
        int height = 30;
        int buffer = 10;

        List<string> itemList = Managers.Inventory.GetItemList();
        if (itemList.Count == 0)
        {
            GUI.Box(new Rect(posX, posY, width, height), "No Items");
        }

        foreach (string item in itemList)
        {
            int count = Managers.Inventory.GetItemCount(item);
            Texture2D image = Resources.Load<Texture2D>("Icons/" + item);
            if (GUI.Button(new Rect(posX, posY, width, height), new GUIContent($"({count})", image)))
            {
                Managers.Inventory.Equip(item);
            }
            posX += width + buffer;
        }


        string equiped = Managers.Inventory.equiped;
        if (equiped == null)
        {
            GUI.Box(new Rect(10, posY + height + buffer, width, height), new GUIContent("Nothing equiped"));
        } else
        {
            Texture2D image = Resources.Load<Texture2D>("Icons/" + equiped);
            if(GUI.Button(new Rect(10, posY + height + buffer, width, height), new GUIContent($"({equiped})", image)))
            {
                Managers.Inventory.Consume(equiped);
            }
        }
    }
}
