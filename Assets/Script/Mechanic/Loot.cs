using System;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public int expAmount;
    
    
    public Loot(String lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
