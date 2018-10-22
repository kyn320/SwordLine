using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    [Header("회복 량")]
    public int healAmount;

    private void Awake()
    {
        GetComponent<InteractiveObject>().interactiveEventToGameObject += Heal;
    }

    public void Heal(GameObject _object)
    {
        if (_object.CompareTag("Player"))
        {
            _object.GetComponent<PlayerBehaviour>().Heal(healAmount);
        }
    }

}
