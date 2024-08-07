using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI kills;
    [SerializeField] TextMeshProUGUI round;
    
    void Start()
    {
        kills.text = UIManager.manager.kills.ToString();
        round.text = UIManager.manager.round.ToString();
    }
}
