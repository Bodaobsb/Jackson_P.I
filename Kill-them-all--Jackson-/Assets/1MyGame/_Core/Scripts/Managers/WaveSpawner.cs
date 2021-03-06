﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour {
    
    public enum SpawnState { SPAWNING, WAITING,COUNTING}

    //[System.Serializable]
    /* public class Wave
     {
         
         public string name;  // nome da wave
         [SerializeField] public Transform enemy;
         [SerializeField] public Transform fatEnemy;
         [SerializeField] public Transform slimeEnemy;

         
         public int count = 5;
         public int fatEnemyCount = 0;
         public int slimeEnemyCount = 0;
         [SerializeField] public float rate;   // spawn rate
         int countMultiplier;

         
     }
     */
    Wave wave;
    
    //[SerializeField] int waitingTime;
    public int timeBetweenWaves;
    [SerializeField] GameObject teleport;
    [SerializeField] int countMultiplier;
    [SerializeField] int rangeCountMultiplier;
    //[SerializeField] float damageMultiplier;
    //[SerializeField] float lifeMultiplier;

    [SerializeField] GameObject akPrefab;
    [SerializeField] GameObject rayGunPrefab;
    [SerializeField] GameObject vectorPrefab;
    [SerializeField] GameObject weaponStartPos;
    [SerializeField] GameObject shotgunPrefab;
    int akCount = 1 ;
    int rayCount= 1;
    int shotgunCount = 1;
    int vectorCount = 1;
    int waitingTime;


    int timeCount;
    public Text waveCount;
    public Text currentWave;
    public GameObject teleportAvailable;
    public GameObject cuidado;
    public GameObject kill;
    public GameObject bossPrefab;
  
    
    public Wave[] waves;
    private int nextWave = 0;
    public Transform[] SpawnPosition;
    public Transform bossSpawnPos;

    
    public float waveCountDown = 0f;

    private float SearchCountDown = 1f;
    private SpawnState state = SpawnState.COUNTING;

    
    rock_free_master rock;

    int bossCount;
    bool activeWave;
    bool bossIsAlive;


    public CameraFollow cameraFollow;

    void DisplayCuidado()
    {
        cuidado.SetActive(true);
        Invoke("StopDisplayCuidado", 2f);
    }

    void StopDisplayCuidado()
    {
        cuidado.SetActive(false);
        
        
    }

    void DisplayKill()
    {
        kill.SetActive(true);
        
        Invoke("StopDisplayKill", 1.15f);
    }
    void StopDisplayKill()
    {
        kill.SetActive(false);

    }



    void Start()
    {
        
        wave = GetComponent<Wave>();
        bossCount = 1;
        rock = this.gameObject.GetComponent<rock_free_master>();
        waveCount.text = "";
        waveCountDown = timeBetweenWaves;
        rock.soft = false;
        rock.med = true;
        waitingTime = timeBetweenWaves;
        
    }

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)&& state == SpawnState.COUNTING)
        {
            waveCountDown = 1;
        }
        

        if (nextWave == 1)
        {
            if (akCount == 1)
            {
                //Instantiate(akPrefab, weaponStartPos.transform.position, weaponStartPos.transform.rotation);
                akCount++;
            }
        }

        if (nextWave == 2)
        {
            if (vectorCount == 1)
            {
                rangeCountMultiplier = 2;
                //Instantiate(vectorPrefab, weaponStartPos.transform.position, weaponStartPos.transform.rotation);
                vectorCount++;
            }
        }
        if (nextWave == 3)
        {
            if (shotgunCount == 1)
            {
                //Instantiate(shotgunPrefab, weaponStartPos.transform.position, weaponStartPos.transform.rotation);
                shotgunCount++;
            }
        }

        if (nextWave == 4)
        {
            if (rayCount == 1)
            {
                //Instantiate(rayGunPrefab, weaponStartPos.transform.position, weaponStartPos.transform.rotation);
                rayCount++;
            }

            rock.soft = false;
            rock.med = false;
            rock.forte = true;
        }
        
        currentWave.text = "Current Wave: " + (nextWave +1);
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive() )
            {
      
             WaveCompleted();

            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            teleportAvailable.SetActive(false);
            waveCount.text = "";
            DisplayKill();
            
            if (state !=SpawnState.SPAWNING)
            {
                
                StartCoroutine(SpawnWave2());
            }
        }
        else
        {
            teleport.SetActive(true);
            waveCountDown -= Time.deltaTime;
            if (waveCountDown <0)
            {
                waveCountDown = 0;
            }
            waveCount.text = "Next Wave in   " + Mathf.Floor(waveCountDown)+  "seg  or press N to continue ";
            teleportAvailable.SetActive(true);

        }
    }

    IEnumerator Contagem()
    {
        for (int i = 0; i < waitingTime; i++)
        {
         
            yield return new WaitForSeconds(1);
        }
    }

    void WaveCompleted()
    {
        
        state = SpawnState.COUNTING;
        
        timeCount = 0;
        waveCountDown = timeBetweenWaves;

       
            nextWave++;
        
        
    }

    void WaveCompletada()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;
        
        nextWave++;
    }

    bool EnemyIsAlive()
    {
        

        SearchCountDown -= Time.deltaTime;
        if (SearchCountDown <= 0f)
        {
            SearchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("EnemyChase") == null)
            {
                if ((nextWave + 1) % 3 == 0 &&  !bossIsAlive)
                {
                    bossIsAlive = true;
                    SpawnEnemy(wave.boss, 1);
                    return true;
                }
                return false;
            }          
        }       
        return true;
    }

   IEnumerator SpawnWave2()
    {
        bossIsAlive = false;
        teleport.SetActive(false);
        Debug.Log("Spawining wave");
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy, Random.Range(0, SpawnPosition.Length));

            yield return new WaitForSeconds(1f / wave.rate);

        }
        for (int i = 0; i < wave.rangeCount; i++)
        {
            SpawnEnemy(wave.rangeEnemy, Random.Range(0, SpawnPosition.Length));

            yield return new WaitForSeconds(1f / wave.rate);

        }
        for (int i = 0; i < wave.fatEnemyCount; i++)
        {
            SpawnEnemy(wave.fatEnemy, Random.Range(0, SpawnPosition.Length));

            yield return new WaitForSeconds(1f / wave.rate);

        }

        for (int i = 0; i < wave.slimeEnemyCount; i++)
        {
            SpawnEnemy(wave.slimeEnemy, Random.Range(0, SpawnPosition.Length));

            yield return new WaitForSeconds(1f / wave.rate);

        }
        
        /*
        if ((nextWave +1) % 3 ==0)
        {
            
        }
        */

        MultiplieCount();
        state = SpawnState.WAITING;

    }
    /*
    IEnumerator SpawnWave(Wave _wave)
    {
        teleport.SetActive(false);
        Debug.Log("Spawining wave");
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);

            yield return new WaitForSeconds(1f / _wave.rate);

        }
        for (int i = 0; i < _wave.rangeCount; i++)
        {
            SpawnEnemy(_wave.rangeEnemy);

            yield return new WaitForSeconds(1f / _wave.rate);

        }
        for (int i = 0; i < _wave.fatEnemyCount; i++)
        {
            SpawnEnemy(_wave.fatEnemy);

            yield return new WaitForSeconds(1f / _wave.rate);

        }

        for (int i = 0; i < _wave.slimeEnemyCount; i++)
        {
            SpawnEnemy(_wave.slimeEnemy);

            yield return new WaitForSeconds(1f / _wave.rate);

        }
        MultiplieCount();
        state = SpawnState.WAITING;

        yield break;
    }
    */
    private void MultiplieCount()
    {
        wave.count += (countMultiplier * 2);
        wave.fatEnemyCount += countMultiplier;
        wave.slimeEnemyCount += countMultiplier;
        wave.rangeCount += rangeCountMultiplier;
    }

    void SpawnEnemy(Transform _enemy, int _boss)
    {
        Transform _spawnPos = SpawnPosition[_boss];
        Instantiate(_enemy, _spawnPos.position, _spawnPos.rotation);

    }



    
}
