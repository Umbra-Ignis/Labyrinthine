﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerMovement movement { get; set; }
    public EngineBehaviour engine { get; set; }
    private GameManager manager;
    private bool takenDamage;
    private float timeStorage;
    private bool startInTimer;
    private float timerForInDanger;
    public bool finished = false;
	public bool isDead = false;

    [SerializeField] float shieldTimer;
    [SerializeField] float timeInDangerToTakeDamage;
    [SerializeField] private float hazardHeatDamage = 5.0f;
    [SerializeField] private float dangerZoneHeatDamage = 10.0f;
    [SerializeField] private float coolantAmount = 1.0f;
    [SerializeField] ScreenShake shake;
    [SerializeField] GameObject vehicle;
    [SerializeField] FadeImage coolant;
    [SerializeField] FadeImage hazard;
    [SerializeField] GameObject nuke;
    [SerializeField] GameObject gui;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource jetSoundLeft;
    [SerializeField] AudioSource jetSoundRight;
    // Update is called once per frame
    void Start()
    {
        startInTimer = false;
        takenDamage = false;
        timeStorage = shieldTimer;
        movement = this.gameObject.GetComponent<PlayerMovement>();
        engine = this.gameObject.GetComponent<EngineBehaviour>();
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    private void FixedUpdate()
    {
        if (startInTimer && !takenDamage)
        {
            timerForInDanger += Time.deltaTime;
            if (timerForInDanger >= timeInDangerToTakeDamage)
            {
                takenDamage = true;
                engine.engineHeatAmount += dangerZoneHeatDamage;
                hazard.flash = true;
                timerForInDanger = 0;
            }
        }
		if (takenDamage) 
		{
			if (!isDead) 
			{
				//Shield ability that keeps you from taking damage
				shieldTimer -= Time.deltaTime;
				if (shieldTimer <= 0.0f) 
				{
					takenDamage = false;
					shieldTimer = timeStorage;
				}
				//Mimiks the flashing effect of the player, after being hit
				vehicle.gameObject.SetActive (!vehicle.activeSelf);
			}
			if (!takenDamage && vehicle.gameObject.activeSelf == false)
			{
				vehicle.gameObject.SetActive (true);
			}
		}
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1000.0f);
                engine.CoolEngineByAmount(50.0f);
            }            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "END")
        {
            movement.finished = true;
            gui.SetActive(false);
            manager.StopScore();
            shake.transform.parent = null;
        }
        if(other.tag == "Nuke")
        {
            nuke.SetActive(true);
            Invoke("WinGame", 7.5f);
        }
        if(other.tag == "Hazard")
        {
            if (!takenDamage)
            {
                hitSound.Play();
                //Adds the heat to the engine
                engine.engineHeatAmount += hazardHeatDamage;
                //Shakes your screen
                shake.StartShake();
                //Enables a little shield
                takenDamage = true;
				hazard.flash = true;
            }
        }
        if(other.tag == "Note")
        {
            coolant.ResetValues();
            //Adds to the combo counter
            manager.comboScore += 1;
            //Adds to the total score
            manager.AddScore(10.0f);
            //Flashes the blue color over the gauge
            coolant.flash = true;
            //Cools the engine
            engine.CoolEngineByAmount(coolantAmount);
            //Starts the animation for the note collection
            other.GetComponent<Note>().Collected();
        }
        if(other.tag == "DangerZone")
        {
                //Starts a timer for the duration that you are inside the danger zone
                startInTimer = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "DangerZone")
        {
            //Stops the timer for the time inside the danger zone
            startInTimer = false;
            //Resets the timer
            timerForInDanger = 0.0f;
        }
    }
    public void StartSounds()
    {
        jetSoundLeft.Play();
        jetSoundLeft.loop = true;
        jetSoundRight.Play();
        jetSoundRight.loop = true;
    }

    public void WinGame()
    {
        SceneManager.LoadScene(1);
    }
}
