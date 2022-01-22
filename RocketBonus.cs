using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBonus : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            player.OnRocketEnable();
            
        }
    }
}
