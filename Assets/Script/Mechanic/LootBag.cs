using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public float dropForce;
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    Loot GetdroppedItem(){
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach(Loot item in lootList){
            if(randomNumber <= item.dropChance){
                possibleItems.Add(item);
            }
        }
        if(possibleItems.Count > 0){
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("no item drop");
        return null;
    }
    public void InstantiateLoot(Vector3 spawnPosition){
        Loot droppedItem = GetdroppedItem();
        if(droppedItem != null){
            GameObject lootGameObject = Instantiate(droppedItemPrefab,spawnPosition,Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

            // Vector2 dropDirection = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
            // lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection*dropForce, ForceMode2D.Impulse);
            CollectGems collectGems = lootGameObject.GetComponent<CollectGems>();
            if(collectGems != null){
                collectGems.gemLoot = droppedItem;
            }
        }
    }
}