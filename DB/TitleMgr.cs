using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMgr : MonoBehaviour
{
    // Start is called before the first frame update

    float timer;
    public GameObject text;
    void Start()
    {
        timer = 0f;
        Game.current = new Game();
        userData.data = new userData();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1.2f)
        {
            if (text.activeSelf)
                text.SetActive(false);
            else
                text.SetActive(true);
            timer = 0f;
        }
    }

    // Update is called once per frame
    public void onTouchScreen()
    {
        Game.current.setMode(0);
        SceneManager.LoadScene("scLoading");
    }
}
