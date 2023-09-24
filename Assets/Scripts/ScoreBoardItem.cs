using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;

    Player player;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;
        UpdateStats();
    }

    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsText.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathsText.text = deaths.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == player)
        {
            if(changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                UpdateStats();
            }
        }
    }

}
