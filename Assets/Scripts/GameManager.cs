/*
This script, GameManager, is a Unity script written in C# that manages the overall game state, including the player’s lives and coins, the current world and stage, and the game’s audio settings. Here’s a breakdown of its key parts:
•	Singleton Pattern: The GameManager uses the Singleton pattern to ensure that there is only one instance of the GameManager in the game. This is done through the Instance property and the Awake method.
•	Events: The GameManager has two events, OnLivesChanged and OnCoinsChanged, which are triggered when the player’s lives or coins change.
•	Game State: The GameManager keeps track of the current game state, including the current world and stage, the player’s lives and coins, and whether the game is paused or muted.
•	Audio: The GameManager manages the game’s audio settings, including the volume and whether the audio is paused. It also plays sound effects when the player collects a coin or a life.
•	Game Flow: The GameManager controls the flow of the game, including starting a new game, loading a level, resetting a level, and ending the game. It also handles user input to pause the game, mute the audio, or return to the start menu.
•	Coroutines: The GameManager uses coroutines to delay certain actions, such as resetting the level.
*/


using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance { get; private set; }

    // Events to notify when lives or coins change
    public event Action<int> OnLivesChanged;
    public event Action<int> OnCoinsChanged;

    // Game state variables
    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

    // Audio clips and source for sound effects
    public AudioClip coinSound; // Audio clip for coin sound
    public AudioClip lifeSound; // Audio clip for life pickup sound
    private AudioSource audioSource; // Reference to AudioSource component

    // Game settings variables
    private bool isPaused = false;
    private bool isMuted = false;
    private bool wasAudioMuted = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            // If an instance already exists, destroy this one
            DestroyImmediate(gameObject);
            return;
        }

        // Set this as the instance and don't destroy it when loading new scenes
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // This function is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        // Clear the instance when this object is destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Pause/unpause the game when 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        // Mute/unmute the game when 'M' key is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }

        // Return to StartMenu scene when Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToStartMenu();
        }
    }

    // Function to toggle pause state
    void TogglePause()
    {
        // Toggle pause state
        isPaused = !isPaused;

        // Pause or unpause the game time
        Time.timeScale = isPaused ? 0 : 1;

        // Mute or unmute the audio if the game is paused
        if (isPaused)
        {
            // Store the previous state of audio mute
            wasAudioMuted = AudioListener.pause;
            // Mute the audio
            AudioListener.pause = true;
        }
        else
        {
            // Restore the previous state of audio mute when unpausing
            AudioListener.pause = wasAudioMuted;
        }
    }

    // Function to toggle mute state
    void ToggleMute()
    {
        // Toggle mute state
        isMuted = !isMuted;

        // Mute or unmute the audio
        AudioListener.volume = isMuted ? 0 : 1;
    }

    // Function to return to the StartMenu scene
    private void ReturnToStartMenu()
    {
        // Load the StartMenu scene
        SceneManager.LoadScene("StartMenu");
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Set the target frame rate
        Application.targetFrameRate = 60;
        // Start a new game
        NewGame();
    }

    // Function to start a new game
    public void NewGame()
    {
        // Set initial game state
        lives = 3;
        coins = 0;
        // Load the first level
        LoadLevel(1, 1);
    }

    // Function to load a level
    public void LoadLevel(int world, int stage)
    {
        // Set the current world and stage
        this.world = world;
        this.stage = stage;
        // Load the scene for the specified world and stage
        SceneManager.LoadScene($"{world}-{stage}");
    }

    // Function to reset the level with a delay
    public void ResetLevel(float delay)
    {
        // Call the ResetLevel function after a delay
        Invoke(nameof(ResetLevel), delay);
    }

    // Function to reset the level
    public void ResetLevel()
    {
        // Decrease lives
        lives--;
        // Notify subscribers about lives change
        OnLivesChanged?.Invoke(lives);

        // If there are still lives left, reload the level
        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            // If no lives are left, end the game
            GameOver();
        }
    }

    // Function to end the game
    public void GameOver()
    {
        // Load the start menu scene
        SceneManager.LoadScene("StartMenu");
    }

    // Function to add a coin
    public void AddCoin()
    {
        // Increase coins
        coins++;
        // Notify subscribers about coins change
        OnCoinsChanged?.Invoke(coins);

        // If the player has collected 30 coins, reset the coin count and add a life
        if (coins == 30)
        {
            coins = 0;
            AddLife();
        }

        // Play the coin sound effect
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound, 0.5f);
        }
    }

    // Function to add a life
    public void AddLife()
    {
        // Check if the player has less than 5 lives
        if (lives < 5)
        {
            // Increment lives if less than 5
            lives++;
            // Notify subscribers about lives change
            OnLivesChanged?.Invoke(lives);

            // Play the life pickup sound effect
            if (lifeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(lifeSound, 0.5f);
            }
        }
    }
}