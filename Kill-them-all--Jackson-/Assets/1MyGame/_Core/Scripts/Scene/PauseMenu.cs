﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject restartUI;
    public GameObject inGameOptionsUI;

    public GameObject skillPanel;
    public GameObject hudCanvas;
    public GameObject shopCanvas;
    public GameObject shopConsuCanvas;

    bool isInPanel = false;
    bool isInShop = false;
    GameObject player;
    GameObject playerWeapon;
    PlayerHealth playerHealth;
    bool inOptions;
    Weapon weapon;
    Experience experience;
    PlayerMovement playerMovement;
    public bool shopConsu;
    public bool shopEquip;
   

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
       
        playerHealth = player.GetComponent<PlayerHealth>();
        experience = player.GetComponent<Experience>();
        playerMovement = player.GetComponent<PlayerMovement>();
       

    }


    


    void Update()
    {
        ShopCanvas();

        if (Input.GetKeyDown(KeyCode.K) && !inOptions)
        {
            if (GameIsPaused)
            {
                hudCanvas.SetActive(true);
                skillPanel.SetActive(false);
                Time.timeScale = 1f;
                GameIsPaused = false;
                playerWeapon = GameObject.FindGameObjectWithTag("Weapon");
                weapon = playerWeapon.GetComponent<Weapon>();
                experience.UpdateBonus();
                weapon.MultiplicarDamage();
                playerMovement.Velocidade();





            }
            else
            {
                hudCanvas.SetActive(false);
                skillPanel.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;

            }
        }
        if (Input.GetKeyDown(KeyCode.K) && isInPanel == false)
        {
            skillPanel.SetActive(true);
            Debug.Log("painel de skill");
            isInPanel = true;
            Time.timeScale = 0f;
        }
        /*
    if (Input.GetKeyDown(KeyCode.K) && isInPanel == true)
    {
        isInPanel = false;
        skillPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    */
        if (Input.GetKeyDown(KeyCode.Escape) && !inOptions)
        {
            if (GameIsPaused)
            {
                Resume();

            }
            else
            {
                Pause();
            }
        }
        if (playerHealth.currentHealth <= 0)
        {
            Invoke("ShowRestartUI", 1.5f);

        }

        if (Input.GetKeyDown(KeyCode.Escape) && inOptions)
        {
            inGameOptionsUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            inOptions = false;
        }


    }

    private void ShopCanvas()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isInShop && shopConsu)
        {
            shopConsuCanvas.SetActive(true);
            isInShop = true;
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.P) && !inOptions && shopConsu)
        {
            if (GameIsPaused)
            {
                hudCanvas.SetActive(true);

                shopConsuCanvas.SetActive(false);
                Time.timeScale = 1f;
                GameIsPaused = false;






            }
            else
            {
                hudCanvas.SetActive(false);
                shopConsuCanvas.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;

            }
        }
        if (Input.GetKeyDown(KeyCode.P) && !isInShop && shopEquip)
        {
            shopCanvas.SetActive(true);
            isInShop = true;
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.P) && !inOptions && shopEquip)
        {
            if (GameIsPaused)
            {
                hudCanvas.SetActive(true);

                shopCanvas.SetActive(false);
                Time.timeScale = 1f;
                GameIsPaused = false;






            }
            else
            {
                hudCanvas.SetActive(false);
                shopCanvas.SetActive(true);
                Time.timeScale = 0f;
                GameIsPaused = true;

            }
        }
    }

    void ShowRestartUI()
    {
        restartUI.SetActive(true);


    }

   public void Resume()
    {
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;       
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InGameOptions()
    {
        inGameOptionsUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        inOptions = true;
    }

    public void RestartLevel()
    {
       
        SceneManager.LoadScene("Jogo");
        Time.timeScale = 1f;
    }



   
}