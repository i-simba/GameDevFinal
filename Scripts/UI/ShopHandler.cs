using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject      shopObj;
    [SerializeField] Image           shopContainer;

    [Header("Infrastructure")]
    [SerializeField] TextMeshProUGUI repairCostDisplay;

    [Header("Guns")]
    [SerializeField] TextMeshProUGUI smgCostDisplay;
    [SerializeField] TextMeshProUGUI rifleCostDisplay;
    [SerializeField] TextMeshProUGUI lmgCostDisplay;
    [SerializeField] TextMeshProUGUI deagleCostDisplay;

    [Header("Upgrade Texts - Gun")]
    [SerializeField] TextMeshProUGUI magCapDisplay;
    [SerializeField] TextMeshProUGUI reloadSpdDisplay;
    [SerializeField] TextMeshProUGUI fireRateDisplay;

    [Header("Upgrade Texts - Player")]
    [SerializeField] TextMeshProUGUI defDisplay;
    [SerializeField] TextMeshProUGUI spdDisplay;
    [SerializeField] TextMeshProUGUI healDisplay;

    [Header("Upgrade Images - Gun")]
    [SerializeField] Image           magUp_i;
    [SerializeField] Image           relUp_i;
    [SerializeField] Image           rateUp_i;

    [Header("Upgrade Images - Player")]
    [SerializeField] Image           defUp_i;
    [SerializeField] Image           spdUp_i;
    
    private Dictionary<string, TextMeshProUGUI> gunText;
    private const int MAX_UPGRADE = 3;

    private GameObject[] breakables;
    private GameObject   gunObj;
    private GameObject   playerObj;
    private Gun          gun;
    private Player       player;

    private int magUpCost;
    private int relUpCost;
    private int fireUpCost;
    private int defUpCost;
    private int spdUpCost;
    private int healCost;

    void Awake()
    {
        gunText = new Dictionary<string, TextMeshProUGUI>
        {
            { "smg",    smgCostDisplay },
            { "rifle",  rifleCostDisplay },
            { "lmg",    lmgCostDisplay },
            { "deagle", deagleCostDisplay }
        };
        

        breakables = GameObject.FindGameObjectsWithTag("Breakable");
        gunObj     = GameObject.FindGameObjectWithTag("Gun");
        playerObj  = GameObject.FindGameObjectWithTag("Player");
        gun        = gunObj.GetComponent<Gun>();
        player     = playerObj.GetComponent<Player>();

        shopContainer.transform.localPosition = Vector3.zero;
    }

    private int CheckBalanceRepair()
    {
        return UIManager.manager.money - EndOfRound.manager.costOfRepair;
    }

    private int CheckBalance( int mny )
    {
        return UIManager.manager.money - mny;
    }

    private int CheckUpgradeCost( int lvl, int cost, string type )
    {
        switch ( type ) {
            case "mag":
                if      ( lvl == 1 ) { return (int)( cost * 0.25f ); }
                else if ( lvl == 2 ) { return (int)( cost * 0.5f ); }
                break;
            case "rel":
                if      ( lvl == 1 ) { return (int)( cost * 0.35f ); }
                else if ( lvl == 2 ) { return (int)( cost * 0.6f ); }
                break;
            case "fire":
                if      ( lvl == 1 ) { return (int)( cost * 0.45f ); }
                else if ( lvl == 2 ) { return (int)( cost * 0.7f ); }
                break;
            case "def":
                if      ( lvl == 1 ) { return (int)( ( cost * 5 ) + 75f ); }
                else if ( lvl == 2 ) { return (int)( ( cost * 10 ) + 100f ); }
                break;
            case "spd":
                if      ( lvl == 1 ) { return (int)( ( cost * 10 ) + 90f ); }
                else if ( lvl == 2 ) { return (int)( ( cost * 15 ) + 120f ); }
                break;
            default:
                return 0;
        }
        return -1;
    }

    private void ApplyColor( Image img, int lv )
    {
        if ( lv == 1 )
            img.color = new Color32( 94, 140, 159, 255 );
        else if ( lv == 2 )
            img.color = new Color32( 160, 95, 95, 255 );
        else
            img.color = new Color32( 109, 147, 112, 255 );
    }

    private void ResetColor()
    {
        magUp_i.color  = new Color32( 109, 147, 112, 255 );
        relUp_i.color  = new Color32( 109, 147, 112, 255 );
        rateUp_i.color = new Color32( 109, 147, 112, 255 );
    }

    /*
     * Update the cost of each item inside the shop
     */
    public void UpdateCost()
    {
        repairCostDisplay.text = EndOfRound.manager.costOfRepair.ToString();
        
        // Display either the cost of the gun, or 'EQUIPPED!' if that gun is currently equipped
        foreach ( KeyValuePair<string, TextMeshProUGUI> cost in gunText ) {
            if ( cost.Key != gun.GetEquippedGun() ) {
                cost.Value.fontSize = 20;
                cost.Value.text     = "$ " + gun.GetGunCost( cost.Key ).ToString();
            } else {
                cost.Value.fontSize = 16;
                cost.Value.text = "EQUIPPED!";
            }
        }

        // Determine the cost of each gun upgrades based on their upgrade level
        magUpCost  = CheckUpgradeCost( gun.magCapacityLvl, gun.GetEquippedCost(), "mag" );
        relUpCost  = CheckUpgradeCost( gun.reloadSpeedLvl, gun.GetEquippedCost(), "rel" );
        fireUpCost = CheckUpgradeCost( gun.fireRateLvl, gun.GetEquippedCost(), "fire");

        // Determine the cost of each player upgrades based on the round and upgrade level
        defUpCost = CheckUpgradeCost( player.defenseLvl, UIManager.manager.round, "def" );
        spdUpCost = CheckUpgradeCost( player.speedLvl, UIManager.manager.round, "spd" );

        // Display either the upgrade's price or 'MAX!', if that upgrade is maxxed out (GUN)
        if ( magUpCost >= 0 ) magCapDisplay.text    = "$ " + magUpCost.ToString();
        else magCapDisplay.text    = "MAX!";
        if ( relUpCost >= 0 ) reloadSpdDisplay.text = "$ " + relUpCost.ToString();
        else reloadSpdDisplay.text = "MAX!";
        if ( fireUpCost >= 0 ) fireRateDisplay.text = "$ " + fireUpCost.ToString();
        else fireRateDisplay.text  = "MAX!";

        // Display either the upgrade's price or 'MAX!', if that upgrade is maxxed out (PLAYER)
        if ( defUpCost >= 0 ) defDisplay.text = "$ " + defUpCost.ToString();
        else defDisplay.text = "MAX!";
        if ( spdUpCost >= 0 ) spdDisplay.text = "$ " + spdUpCost.ToString();
        else spdDisplay.text = "MAX!";

        // Update heal cost
        healCost = (int)( ( UIManager.manager.round * ( 100 - player.GetHealth() ) ) * 0.75f );
        healDisplay.text = "$" + healCost.ToString();
    }

    public void ResetBreakables()
    {
        int change = CheckBalanceRepair();
        if ( change >= 0 ) {
            foreach ( GameObject brk in breakables ) {
                if ( !brk.activeSelf )
                    brk.SetActive( true );
                brk.GetComponent<Health>().health = brk.GetComponent<Health>().GetMaxHealth();
            }
            UIManager.manager.SetMoney( change );
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UpdateCost();
    }

    public void BuyGun( string gunName )
    {
        int change = CheckBalance( gun.GetGunCost( gunName ) );
        if ( gun.GetEquippedGun() == gunName )
            return;

        if ( change >= 0 ) {
            gun.GetComponent<Gun>().SetGun( gunName );
            UIManager.manager.SetMoney( change );
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UIManager.manager.ResetGunUpgrade();
        ResetColor();
        UpdateCost();
    }

    public void IncreaseMagCap()
    {
        int lvl    = gun.magCapacityLvl;
        int change = CheckBalance( magUpCost );
        if ( lvl >= MAX_UPGRADE )
            return;

        if ( change >= 0 ) {
            switch ( gun.GetEquippedGun() ) {
                case "smg":
                    gun.SetMagCapacity( 1.5f );
                    break;
                case "rifle":
                    gun.SetMagCapacity( 1.3f );
                    break;
                case "lmg":
                    gun.SetMagCapacity( 1.29f );
                    break;
                case "deagle":
                    gun.SetMagCapacity( 1.55f );
                    break;
                default:
                    gun.SetMagCapacity( 1.2f );
                    break;
            }
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UIManager.manager.SetMoney( change );
        UIManager.manager.ColorGunUpgrade( "mag", lvl );
        ApplyColor( magUp_i, lvl );
        UpdateCost();
    }

    public void DecreaseReloadSpeed()
    {
        int lvl    = gun.reloadSpeedLvl;
        int change = CheckBalance( relUpCost );
        if ( lvl >= MAX_UPGRADE )
            return;

        if ( change >= 0 ) {
            switch ( gun.GetEquippedGun() ) {
                case "smg":
                    gun.SetReloadSpeed( 0.8f );
                    break;
                case "rifle":
                    gun.SetReloadSpeed( 0.6f );
                    break;
                case "lmg":
                    gun.SetReloadSpeed( 1.5f );
                    break;
                case "deagle":
                    gun.SetReloadSpeed( 1f );
                    break;
                default:
                    gun.SetReloadSpeed( 0.25f );
                    break;
            }
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UIManager.manager.SetMoney( change );
        UIManager.manager.ColorGunUpgrade( "rel", lvl );
        ApplyColor( relUp_i, lvl );
        UpdateCost();
    }

    public void IncreaseFireRate()
    {
        int lvl    = gun.fireRateLvl;
        int change = CheckBalance( relUpCost );
        if ( lvl >= MAX_UPGRADE )
            return;

        if ( change >= 0 ) {
            switch ( gun.GetEquippedGun() ) {
                case "smg":
                    gun.SetFireRate( 0.015f );
                    break;
                case "rifle":
                    gun.SetFireRate( 0.05f );
                    break;
                case "lmg":
                    gun.SetFireRate( 0.02f );
                    break;
                case "deagle":
                    gun.SetFireRate( 0.1f );
                    break;
                default:
                    gun.SetFireRate( 0.1f );
                    break;
            }
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UIManager.manager.SetMoney( change );
        UIManager.manager.ColorGunUpgrade( "rate", lvl );
        ApplyColor( rateUp_i, lvl );
        UpdateCost();
    }

    public void IncreaseDefense()
    {
        int lvl = player.defenseLvl;
        int change = CheckBalance( defUpCost );
        if ( lvl >= MAX_UPGRADE )
            return;

        if ( change > 0 ) {
            player.SetDefense( 1.5f );
            UIManager.manager.SetMoney( change );
            UIManager.manager.ColorPlayerUpgrade( "def", lvl );
            ApplyColor( defUp_i, lvl );
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UpdateCost();
    }

    public void IncreaseSpeed()
    {
        int lvl = player.speedLvl;
        int change = CheckBalance( spdUpCost );
        if ( lvl >= MAX_UPGRADE )
            return;

        if ( change > 0 ) {
            player.IncreaseSpeed( 0.5f );
            UIManager.manager.SetMoney( change );
            UIManager.manager.ColorPlayerUpgrade( "spd", lvl );
            ApplyColor( spdUp_i, lvl );
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UpdateCost();
    }

    public void Heal()
    {
        int change = CheckBalance( healCost );
        if ( player.GetHealth() == 100 )
            return;

        if ( change >= 0 ) {
            UIManager.manager.UpdateHP( player.Heal() );
            UIManager.manager.SetMoney( change );
            AudioManager.manager.PlaySFX( AudioManager.manager.coin );
        } else {
            AudioManager.manager.Click();
        }
        UpdateCost();
    }

    public void Resume()
    {
        AudioManager.manager.Click();
        shopObj.GetComponent<Canvas>().enabled = false;
        EndOfRound.manager.isPaused = false;
        EndOfRound.canva.enabled = false;
        Time.timeScale = 1;
        if ( UIManager.manager.round == 5 || UIManager.manager.round == 10 )
            AudioManager.manager.PlaySFX( AudioManager.manager.bell );
    }
}
