using Shgames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyData : MonoBehaviour
{
    public static storyData currStory;
    int stage;
    int gameOver;
    int totalReward;

    public storyData()
    {
        // 현재 진행 스테이지
        stage = 1;
        // 게임 오버 횟수
        gameOver = 0;
    }

    public int getStage()
    {
        return stage;
    }

    public int getGameOver()
    {
        return gameOver;
    }

    public void setStage(int stage)
    {
        this.stage = stage;
    }

    public void inc_GameOver()
    {
        gameOver += 1;
    }

    public void reset_GameOver()
    {
        gameOver = 0;
    }

    public int calc_reward()
    {
        totalReward = 20000 - gameOver * 2000;
        return totalReward;
    }
}
