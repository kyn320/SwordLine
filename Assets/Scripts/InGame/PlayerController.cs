using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerBehaviour player;

    private float h, v;

    public bool isInput = true;
    public bool isInteracted = false;

    public float changeWeaponTime = 1.5f;
    private float changeWeaponCurrentTime;


    public float evasionTime = 1.5f;
    private float evasionCurrentTime;

    private void Awake()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        if (!isInput)
            return;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        player.SetDirection(new Vector3(h, v, 0));

        //상호작용 처리
        if (isInteracted && Input.GetKeyDown(KeyCode.E))
        {
            //isInput = false;
            ////TODO :: 상호작용 구현
            //isInteracted = false;
            //player.UpdateState(PlayerState.Interactive);
            player.GetSightChecker().recentSightInGameObject.GetComponent<InteractiveObject>().Interactive();
        }

        //무기체인지 처리
        if (changeWeaponCurrentTime <= 0 && Input.GetKeyDown(KeyCode.Q))
        {
            changeWeaponCurrentTime = changeWeaponTime;
            //TODO :: 착용 무기 변경 구현

        }
        else if (changeWeaponCurrentTime > 0)
        {
            changeWeaponCurrentTime -= Time.deltaTime;
        }

        //회피 처리
        if (player.state != PlayerState.Evasion && Input.GetKeyDown(KeyCode.Space))
        {
            isInput = false;
            player.Evasion(evasionTime);

        }
    }

    public void OnInteractive(bool _isInteractive)
    {
        isInteracted = _isInteractive;

    }


}
