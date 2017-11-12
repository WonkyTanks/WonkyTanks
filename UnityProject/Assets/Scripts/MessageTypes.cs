﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GameUtilities : MonoBehaviour//extends mono purely for the benefits of printing to the game console
{   //broad, useful functions and data for the game

    public static void FindGame(ref GameObject game_in)
    {   //finds the main game object
        var RootGMs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var GObj in RootGMs)
        {
            if (GObj.CompareTag("Game"))
            {
                game_in = GObj;
                break;
            }
        }
    }

    public static void GetAllPlayers(ref GameObject[] players_in)
    {
        GameObject Game = null;
        FindGame(ref Game);

        if(Game!=null)
        {
            GameObject Tanks = Game.transform.Find("Tanks").gameObject;

            int NoChildren = Tanks.transform.childCount;
            players_in = new GameObject[NoChildren];
            for(int iChild=0;iChild<NoChildren;iChild++)
            {
                players_in[iChild] = Tanks.transform.GetChild(iChild).gameObject.transform.Find("TankBody").gameObject;
            }
        }
    }

    public const SendMessageOptions DONT_CARE_RECIEVER = SendMessageOptions.DontRequireReceiver;
    public const SendMessageOptions DO_CARE_RECIEVER = SendMessageOptions.RequireReceiver;

    public const byte INVALID_TANK_ID = 255;
    public const byte INVALID_ENEMY_ID = INVALID_TANK_ID;
}

namespace TankMessages
{
    enum ShotType { InvalidShotType = -1, Bouncy = 0 }

    class TankComponentMovementMsg
    {
        public TankComponentMovementMsg() { TankID = GameUtilities.INVALID_TANK_ID; FrameNo = 0; Direction = false; }
        public TankComponentMovementMsg(byte tid, int fno, bool direction) { TankID = tid; FrameNo = fno; Direction = direction; }

        public bool Direction;
        public byte TankID;
        public int FrameNo;
    };

    class CreateProjectileMsg
    {
        public CreateProjectileMsg()
        {
            PlayerFriendly = false;
            FrameNo = 0;
            TankID = GameUtilities.INVALID_TANK_ID;
            TypeFired = ShotType.InvalidShotType;
            InitialPosition = Vector3.zero;
            DirectionReference = Vector3.zero;
        }

        public CreateProjectileMsg(bool pf, int fno, byte tid, ShotType tf, Vector3 ip, Vector3 dr)
        {
            PlayerFriendly = pf;
            FrameNo = fno;
            TankID = tid;
            TypeFired = tf;
            InitialPosition = ip;
            DirectionReference = dr;
        }

        public bool PlayerFriendly;//whether or not the projectile can harm/affect players
        public int FrameNo;
        public byte TankID;
        public ShotType TypeFired;
        public Vector3 InitialPosition;//starting position of the projectile
        public Vector3 DirectionReference;//A vector to use in conjuction with InitialPosition to create direction of projectile
    }

    class DamageTankMsg
    {
        public DamageTankMsg() { TankID = GameUtilities.INVALID_TANK_ID; FrameNo = 0; Amount = 0.0f; }
        public DamageTankMsg(byte tid, int fno, float amt)
        {
            TankID = tid;
            FrameNo = fno;
            Amount = amt;
        }

        public byte TankID;
        public int FrameNo;
        public float Amount; 
    }
}//TankMessages

namespace MapMessages
{
    class GetCollectableMsg
    {
        public GameObject collectableObj;
        //public int FrameNo;

        public GetCollectableMsg(GameObject collectable) { collectableObj = collectable; }
    }
	class GetBulletMsg
    {
        public GameObject bulletObj;
        //public int FrameNo;

        public GetBulletMsg(GameObject bullet) { bulletObj = bullet; }
    }
}

namespace EnemyMessages
{
    enum EnemyType { InvalidEnemyType = -1, Guardian = 0, Chaser = 1 }

    class DamageEnemyMsg
    {
        public DamageEnemyMsg()
        {
            EType = EnemyType.InvalidEnemyType;
            EnemyID = GameUtilities.INVALID_ENEMY_ID;
            Amount = 0f;
        }

        public DamageEnemyMsg(EnemyType et, byte eid, byte tid, float amt)
        {
            EType = et;
            EnemyID = eid;
            TankID = tid;
            Amount = amt;
        }

        public EnemyType EType; 
        public byte EnemyID;
        public byte TankID;//the tank this damage is from
        public float Amount;
    }

    class EnemyIDMsg
    {
        public EnemyIDMsg()
        {
            EnemyID = GameUtilities.INVALID_ENEMY_ID;
            EType = EnemyType.InvalidEnemyType;
        }

        public EnemyIDMsg(byte eid, EnemyType et)
        {
            EnemyID = eid;
            EType = et;
        }

        public byte EnemyID;
        public EnemyType EType; 
    }
}