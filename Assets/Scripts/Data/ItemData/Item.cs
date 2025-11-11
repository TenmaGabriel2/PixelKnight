using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventroy/New Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public int itemID;
    public string itemName;
    public int itemCount;

    [TextArea]
    public string itemInfo;
    
    public Sprite itemIcon;
}
