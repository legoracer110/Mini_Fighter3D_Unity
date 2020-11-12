using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class roundDB
{
    public static roundDB rdb;

    int dmg_player;
    int grd_player;
    int ref_player;

    int dmg_enemy;
    int grd_enemy;
    int ref_enemy;

    int total_round;
    int vic_player;
    int vic_enemy;

    bool victory;

    public roundDB()
    {
        dmg_player = 0;
        grd_player = 0;
        ref_player = 0;

        dmg_enemy = 0;
        grd_enemy = 0;
        ref_enemy = 0;

        total_round = 0;
        vic_player = 0;
        vic_enemy = 0;

        victory = false;
    }

    public int getDmgPlayer()
    {
        return this.dmg_player;
    }

    public int getGrdPlayer()
    {
        return this.grd_player;
    }

    public int getRefPlayer()
    {
        return this.ref_player;
    }

    public int getDmgEnemy()
    {
        return this.dmg_enemy;
    }

    public int getGrdEnemy()
    {
        return this.grd_enemy;
    }

    public int getRefEnemy()
    {
        return this.ref_enemy;
    }

    public int getTotalRound()
    {
        return this.total_round;
    }

    public int getPlayerWin()
    {
        return this.vic_player;
    }

    public int getEnemyWin()
    {
        return this.vic_enemy;
    }

    public bool getVictory()
    {
        return this.victory;
    }

    public void addDmgPlayer(int dmg)
    {
        dmg_player += dmg;
    }

    public void addDmgEnemy(int dmg)
    {
        dmg_enemy += dmg;
    }

    public void addGrdPlayer()
    {
        grd_player += 1;
    }

    public void addGrdEnemy()
    {
        grd_enemy += 1;
    }

    public void addRefPlayer()
    {
        ref_player += 1;
    }

    public void addRefEnemy()
    {
        ref_enemy += 1;
    }

    public void setPlayerWin()
    {
        vic_player += 1;
        total_round += 1;
    }

    public void setEnemyWin()
    {
        vic_enemy += 1;
        total_round += 1;
    }

    public void setVictory()
    {
        victory = true;
    }
}
