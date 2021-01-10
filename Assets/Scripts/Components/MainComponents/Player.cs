using Assets.Scripts.Ai;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int Id;

    [SyncVar]
    public int Mana;

    [SyncVar]
    public int TotalManaCollected = 0;

    [SyncVar]
    public Color Color;

    [SyncVar]
    public Vector3 LastCamPos;

    [SyncVar]
    public string Name;

    [SyncVar]
    public bool IsDisasterCasted = false;

    [SyncVar]
    public bool IsAi;

    void Start()
    {
        if (!NetworkService.IsServer()) return;

        Mana = 10;
    }

    public int GetManaCost()
    {
        if (Singles.World.IsSandbox) return 0;

        var biomeCount = Singles.Grid.GetAllBiomes().Count(v => v.IsOwner(this)) - 15;
        var manaCost = biomeCount / 2;
        if (manaCost < 1) return 1;
        else return manaCost;
    }

    public int GetDisasterManaCost()
    {
        if (Singles.World.IsSandbox) return 0;

        return (int)(GetManaCost() * GetDisasterMultiplier());
    }

    public int GetMidManaCost()
    {
        if (Singles.World.IsSandbox) return 0;

        return GetDisasterManaCost() / 3;
    }

    public int GetDisasterMultiplier()
    {
        if (Singles.World.IsSandbox) return 0;

        var add = GetManaCost() / 50f;
        return (int)(add + 3);
    }

    public int GetMaxMana()
    {
        if (IsAi)
        {
            int maxMana = GetDisasterManaCost() * 10;
            return maxMana.GetMin(20);
        }
        else
        {
            int maxMana = (int)(GetDisasterManaCost() * 1.5f);
            return maxMana.GetMin(20);
        }
    }

    public bool UseMana(int mana)
    {
        if (!NetworkService.IsServer()) return false;

        if (IsEnoughMana(mana))
        {
            Mana -= mana;
            BoundValues();
            return true;
        }
        else
        {
            BoundValues();
            return false;
        }
    }

    public bool IsEnoughMana(int mana)
    {
        return Mana >= mana;
    }

    public void AddMana(int mana)
    {
        if (!NetworkService.IsServer()) return;

        if (mana <= 0) return;
        Mana += mana;
        BoundValues();
    }

    private void BoundValues()
    {
        if (!NetworkService.IsServer()) return;

        Mana = Mana.GetMinMax(0, GetMaxMana());
    }
}
