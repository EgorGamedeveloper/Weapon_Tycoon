using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    public int money;

} 

public class StatsManager : MonoBehaviour
{

    public Stats playerStats;

    StatsManager Instance;



    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
