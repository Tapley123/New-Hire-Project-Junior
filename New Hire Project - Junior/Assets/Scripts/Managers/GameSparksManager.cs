using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksManager : MonoBehaviour
{
    private static GameSparksManager instance = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject); //if there already is a GameSparksManager then destroy this one
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
