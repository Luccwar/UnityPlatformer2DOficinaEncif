using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject prefab;
    public int score = 1;

    void Awake(){
        prefab = Resources.Load("Prefabs/Collectible/Collected") as GameObject;
    }

    void OnDestroy() {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
