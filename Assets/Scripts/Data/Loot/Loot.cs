using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Loot/New Loot")]
public class Loot : ScriptableObject
{
   public List<GameObject> lootList = new List<GameObject>();
}
