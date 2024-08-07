using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;

    [Header("UI Elements")]
    [SerializeField] GameObject Kills_UI;
    [SerializeField] GameObject Money_UI;
    [SerializeField] GameObject Round_UI;
    [SerializeField] GameObject Health_UI;
    [SerializeField] GameObject MagazineCap_UI;
    [SerializeField] GameObject MagazineRem_UI;
    [SerializeField] GameObject PressR_UI;

    [Header("Upgrades UI - Guns")]
    [SerializeField] GameObject magUp_UI;
    [SerializeField] GameObject relUp_UI;
    [SerializeField] GameObject rateUp_UI;

    [Header("Upgrades UI - Player")]
    [SerializeField] GameObject defUp_UI;
    [SerializeField] GameObject spdUp_UI;

    [Header("Animated")]
    [SerializeField] GameObject Reload_UI;
    [SerializeField] GameObject Progress_UI;
    [SerializeField] GameObject Reloading_UI;

    private Dictionary<string, Image>      GunUpgrades;    // List of gun upgrades and their image
    private Dictionary<string, Image>      PlayerUpgrades; // List of player upgrades and their image
    private Dictionary<string, GameObject> GunUp_UI;       // GameObjects relating to each gun upgrades
    private Dictionary<string, GameObject> PlayerUp_UI;    // GameObjects relating to each player upgrades

    private TextMeshProUGUI t_kills;
    private TextMeshProUGUI t_money;
    private TextMeshProUGUI t_round;
    private TextMeshProUGUI t_health;
    private TextMeshProUGUI t_magCap;
    private TextMeshProUGUI t_magRem;
    private GameObject[]    spawners;
    private ParticleSystem  hp_anim;

    public int   round       { get; private set; } // Current round
    public int   numEnemies  { get; private set; } // Number of enemies for given round
    public int   kills       { get; private set; } // Total kills across all rounds
    public int   money       { get; private set; } // Current amount of money
    public int   roundKills  { get; private set; } // Total kills for given round
    public float magCapacity { get; private set; } // Magazine capacity for equipped gun
    public float remRounds   { get; private set; } // Current amount oof rounds in magazine
    public bool  isReloading { get; set; }         // Determine if the player is reloading
    public bool  isStampede  { get; set; }         // Determine if the player has stepped outside

    void Awake()
    {
        // Singleton init
        if ( manager == null) {
            manager = this;
        } else {
            Destroy( this );
        }

        // Start UI elements as not active
        Reload_UI.SetActive( false );    // Reloading progress bar
        PressR_UI.SetActive( false );    // Animated "R" button
        Reloading_UI.SetActive( false ); // Animated "Reloading" text

        // Initialize related "Gun Upgrades" dictionaries
        GunUpgrades = new Dictionary<string, Image> {
            { "mag", magUp_UI.GetComponent<Image>() },
            { "rel", relUp_UI.GetComponent<Image>() },
            { "rate", rateUp_UI.GetComponent<Image>() }
        }; // Images
        GunUp_UI = new Dictionary<string, GameObject> {
            { "mag", magUp_UI },
            { "rel", relUp_UI },
            { "rate", rateUp_UI }
        }; // GameObjects

        // Initialize related "Player Upgrades" dictionaries
        PlayerUpgrades = new Dictionary<string, Image> {
            { "def", defUp_UI.GetComponent<Image>() },
            { "spd", spdUp_UI.GetComponent<Image>() }
        }; // Images
        PlayerUp_UI = new Dictionary<string, GameObject> {
            { "def", defUp_UI },
            { "spd", spdUp_UI }
        }; // GameObjects

        // Start "Gun and Player Upgrades" objects as not active
        foreach ( KeyValuePair<string, GameObject> obj in GunUp_UI )
            obj.Value.SetActive( false );
        foreach ( KeyValuePair<string, GameObject> obj in PlayerUp_UI )
            obj.Value.SetActive( false );

        // Assign UI elements to private variables from their GameObject
        t_kills  = Kills_UI.GetComponent<TextMeshProUGUI>();
        t_money  = Money_UI.GetComponent<TextMeshProUGUI>();
        t_round  = Round_UI.GetComponent<TextMeshProUGUI>();
        t_health = Health_UI.GetComponent<TextMeshProUGUI>();
        t_magCap = MagazineCap_UI.GetComponent<TextMeshProUGUI>();
        t_magRem = MagazineRem_UI.GetComponent<TextMeshProUGUI>();
        hp_anim  = Health_UI.GetComponentInChildren<ParticleSystem>();

        // Initialize game data
        kills        = 0;
        money        = 0;
        roundKills   = 0;
        isReloading  = false;
        round        = 1;
        numEnemies   = round * 2;
        spawners     = GameObject.FindGameObjectsWithTag("Spawner");

        // Update UI elements to reflect game data
        UpdateUI( t_kills, kills );
        UpdateUI( t_money, money );
        UpdateUI( t_round, round );
        UpdateUI( t_health, 100 );
        UpdateUI( t_magCap, 15 );
        UpdateUI( t_magRem, 15 );
    }

    /*
     * Update the displayed text (num) on a given UI element (tmp)
     */
    private void UpdateUI( TextMeshProUGUI tmp, int num )
    {
        tmp.text = num.ToString();
        
        // TODO animate
    }

    /*
     * Increase the round number, reset round kills, and
     * determine the number of enemies for the next round
     */
    private void IncreaseRound()
    {
        round++;
        roundKills = 0;
        numEnemies = (int)Math.Round( round * 1.25f, MidpointRounding.AwayFromZero );
    }

    /*
     * Change the color of the gun upgrade UI element based on upgrade level
     * Activate the corresponding GameObject to display the UI element
     */
    public void ColorGunUpgrade( string up, int lvl )
    {
        if ( lvl == 1 ) {
            GunUp_UI[up].SetActive( true );
            GunUpgrades[up].color = new Color32( 94, 140, 159, 255 );
        } else
            GunUpgrades[up].color = new Color32( 160, 95, 95, 255 );
    }

    /*
     * Deactive all gun upgrade objects
     */
    public void ResetGunUpgrade()
    {
        foreach ( KeyValuePair<string, GameObject> obj in GunUp_UI ) {
            obj.Value.SetActive( false );
        }
    }
    
    /*
     * Change the color of the gun upgrade UI element based on upgrade level
     * Activate the corresponding GameObject to display the UI element
     */
    public void ColorPlayerUpgrade( string up, int lvl )
    {
        if ( lvl == 1 ) {
            PlayerUp_UI[up].SetActive( true );
            PlayerUpgrades[up].color = new Color32( 94, 140, 159, 255 );
        } else
            PlayerUpgrades[up].color = new Color32( 160, 95, 95, 255 );
    }
    
    /*
     * Deactivate all player upgrade objects
     */
    public void ResetPlayerUpgrade()
    {
        foreach ( KeyValuePair<string, GameObject> obj in PlayerUp_UI ) {
            obj.Value.SetActive( false );
        }
    }

    public void AddKill()
    {
        kills++;
        roundKills++;
        UpdateUI( t_kills, kills );
        if ( roundKills >= numEnemies * 5 ) {
            if ( !isStampede )
                EOR();
        }
    }

    /*
     * Add money (mny) to the current count (money) and update its UI element
     * Used for adding money to the player dropped by enemies
     */
    public void AddMoney( int mny )
    {
        money += mny;
        UpdateUI( t_money, money );
    }

    /*
     * Set the current money amount by (mny)
     * Used by the shop if the player purchases a gun or upgrades
     * The calculation is made within ShoppHandler
     */
    public void SetMoney( int mny )
    {
        money = mny;
        UpdateUI( t_money, money );
    }

    /*
     * Update the player's current health.
     * Used by Health to deduct HP (num) when the player takes damage
     * Used by ShopHandler to increase HP (num) if the player purchases a heal
     */
    public void UpdateHP( int num )
    {
        UpdateUI( t_health, num );
        hp_anim.Play();
    }

    /*
     * Update the current equipped gun's bullet count
     * Used in Gun's functions Shoot() and Reload()
     * Shoot() decreases the current amount by (num)
     * Reload() increases the current amount by (num)
     */
    public void UpdateBullet( float num )
    {
        UpdateUI( t_magRem, (int)num );
        remRounds = (int)num;

        if ( remRounds == 0f ) { PressR_UI.SetActive( true ); }
    }

    /*
     * Update's the current equipped gun's magazine capacity
     * Used in Gun's functions SetMagCapacity() and SetGun()
     * Both functions update the current gun's magazine capacity by (num)
     */
    public void UpdateCapacity( float num )
    {
        UpdateUI( t_magCap, (int)num );
        magCapacity = (int)num;
    }

    /*
     * Activate UI elements to denote to the player that
     * The gun is currently being reloaded
     */
    public void ActivateReload()
    {
        Reload_UI.SetActive( true );
        PressR_UI.SetActive( false );
        Reloading_UI.SetActive( true );
    }

    /*
     * Increases the progress bar's shape based on the
     * current gun's reload speed
     */
    public void IncreaseProgress( float num )
    {
        Progress_UI.transform.localScale = new Vector3( num, 1, 0 );
        if ( num >= 1 ) {
            Progress_UI.transform.localScale = new Vector3( 0, 1, 0 );
            Reload_UI.SetActive( false );
            Reloading_UI.SetActive( false );
        }
    }

    /*
     * Return the number of enemies to be spawned in a given round
     */
    public int GetNumEnemies()
    {
        return numEnemies;
    }

    /*
     * End of Round
     * Display shop UI after a short delay (0.5s)
     * Reset the enemy spawners
     * Increase round number, and update round's UI element
     */
    public void EOR()
    {
        StartCoroutine( OpenShop() );
        IEnumerator OpenShop() {
            yield return new WaitForSeconds( 0.5f );
            EndOfRound.manager.SetCanvas( true );
            EndOfRound.manager.ToggleShop();
        }

        foreach ( GameObject spawner in spawners ) {
            spawner.GetComponent<EnemySpawner>().Reset();
        }
        IncreaseRound();
        t_round.text = round.ToString();
    }
}
