using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour 
{
    [SerializeField]float rcsThrust = 100f; // similar to public
    [SerializeField]float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] Vector3 rotationSpeed;

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip levelLoadSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem levelLoadParticles;
    [SerializeField] ParticleSystem deathParticles;

    float directionVer;
    float directionHor;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive , Dying, Transcending}
    State state = State.Alive;

    float moveHorizontal;
    float moveVertical;


    public float startingVolume = 0.8f;
    public float startingPitch = 0.8f;
    public int timeToDecrease = 1;

    bool collisionsAreEnabled = true;

    int currentSceneIndex;

    //Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        int scenecount = SceneManager.sceneCountInBuildSettings;
        print("Total Scene Count is " + scenecount);
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active scene is '" + scene.name + "'.");
    }
	
	void Update () 
    {
        moveHorizontal = CrossPlatformInputManager.GetAxis("HorizontalLeft");
        if (state == State.Alive)
        {
            RespondToThrustInput();
            //RespondToThrustInput_Test();
            RespondToRotateInput();
            
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
	}

    void FixedUpdate() {
        TouchRotateInput();
    }

    private void RespondToDebugKeys() 
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //toggle collision
            collisionsAreEnabled = !collisionsAreEnabled;
        }

    }

    void OnCollisionEnter(Collision collision) {
        if (state != State.Alive || !collisionsAreEnabled) { return; } // ignore collisions when dead

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                 break;
            case "Finish":
                StartLevelLoadSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartLevelLoadSequence() 
    {
            state = State.Transcending;
            print("Go Probe Go!");
            audioSource.Stop();
            audioSource.PlayOneShot(levelLoadSound);
            levelLoadParticles.Play();
            Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence() 
    {
        state = State.Dying;
        print("You are Dead");
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadCurrentLevel", levelLoadDelay);
    }

    private void LoadNextLevel() 
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        //int nextSceneIndex = SceneManager.sceneCountInBuildSettings;

        //SceneManager.LoadScene(nextSceneIndex); 
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
            //nextSceneIndex = 0;
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void LoadCurrentLevel() {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadFirstLevel() 
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput() {

        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            //directionVer = CrossPlatformInputManager.GetAxis("Vertical");
            ApplyThrust();
        }
        else if (CrossPlatformInputManager.GetButton("Jump")) //need to DualTouchControls
        {
            //rigidBody.AddRelativeForce(0, 1 * mainThrust * Time.deltaTime, 0);
            TouchControlThrust();
        }
        else
        {
            StopThrusting();
        }
    }

    private void TouchControlThrust() {
        rigidBody.AddRelativeForce(new Vector3(0, 1 * mainThrust * Time.deltaTime, 0));
        if (!audioSource.isPlaying) // so it doesn't layer on top of eachother
        {
            audioSource.PlayOneShot(mainEngineSound);
            mainEngineParticles.Play();
        }
    }

    private void StopThrusting() {
        audioSource.Stop();
        mainEngineParticles.Stop();
        rigidBody.AddRelativeForce(0, 0 ,0);
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(0, 1 * mainThrust * Time.deltaTime, 0); // =>Shorter version (Vector3.up)
        if (!audioSource.isPlaying) // so it doesn't layer on top of eachother
        {
            audioSource.PlayOneShot(mainEngineSound);
            mainEngineParticles.Play();
        }
    }

    private void RespondToRotateInput() 
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        if (state != State.Dying)
        {
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * rotationThisFrame); //make it compatible for mobile
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(-Vector3.forward * rotationThisFrame);
            }
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation
        
    }

    private void TouchRotateInput() 
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        if (state != State.Dying)
        {
            float rotationThisFrame = rcsThrust * Time.deltaTime;

            if (CrossPlatformInputManager.GetButton("HorizontalLeft"))
            {
                transform.Rotate(Vector3.forward * rotationThisFrame);
            }
            else if (CrossPlatformInputManager.GetButton("HorizontalRight"))
            {
                transform.Rotate(-Vector3.forward * rotationThisFrame);
            }
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}
