using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    
    public Loot(String lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;

    }
}
