using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Core;
using Core.Combat;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PowerupCycle : MonoBehaviour
{
    public FiniteTimer powerUpTimer = new FiniteTimer(0, 36);
    private Transform player;

    [SerializeField] Arena arena;
    [SerializeField] private float minArenaCenterDist = 1f;
    [SerializeField] private ImageFillBar progressBar;

    [SerializeField]private int curPowerUpIndex = 0;
    [SerializeField]private List<PowerupCycleSlot> cycle;

    [SerializeField]private float boostPerKill = .5f;
    [SerializeField]private float boostPerHypno = .5f;
    [SerializeField]private float boostPerWave = 3;
    
    

    public Vector3 getPowerupSpawnPos()
    {
        Vector3 dirFromPlayerToCenter = arena.transform.position - player.position;
        float playerToArenaCenterDist = dirFromPlayerToCenter.magnitude;

        Vector2 spawnCornerDir;
        if (playerToArenaCenterDist < minArenaCenterDist)
        {
            spawnCornerDir = new Vector2(
                (Random.Range(0, 2) != 0 ? -1 : 1),
                (Random.Range(0, 2) != 0 ? -1 : 1)
            );
        }
        else
        {
            spawnCornerDir = new Vector2(
                (dirFromPlayerToCenter.x > 0 ? 1 : -1),
                (dirFromPlayerToCenter.y > 0 ? 1 : -1)
            );
        }

        return arena.getCornerInDirection(spawnCornerDir);
    }


    void spawnPowerUp(Powerup prefab)
    {
        GameplayPoolManager.Instance.powerUpPool
            .get(prefab)
            .transform.position = getPowerupSpawnPos();
    }

    public void updateTimer(float amount)
    {
        powerUpTimer.updateTimer(amount);
        progressBar.updateFromRatio(powerUpTimer.Ratio);

        if (curPowerUpIndex < cycle.Count
            && getCurThreshold() <= powerUpTimer.Ratio)
        {
            cycle[curPowerUpIndex].activate();
            spawnPowerUp(cycle[curPowerUpIndex].prefab);
            //deactivate next one
            curPowerUpIndex = (curPowerUpIndex + 1) % cycle.Count;
            if (curPowerUpIndex < cycle.Count)
            {
                cycle[curPowerUpIndex].deactivate();
            }
        }
        
        
        
        if(powerUpTimer.isComplete)
            powerUpTimer.resetAndKeepExtra();
    }

    private float getCurThreshold()
    {
        return ((float)(curPowerUpIndex + 1)) / cycle.Count;
    }


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;

        curPowerUpIndex = 0;
    }


    // Update is called once per frame
    void Update()
    {
        updateTimer(Time.deltaTime);   
    }
    
    [Serializable]
    public class PowerupCycleSlot
    {
        public Powerup prefab;
        public Image icon;
        public Color activeColor;
        public Color inactiveColor;
        public void activate()
        {
            icon.color = activeColor;
        }
        
        public void deactivate()
        {
            icon.color = inactiveColor;
        }
    }
}