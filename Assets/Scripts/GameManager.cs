using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player playerPrefab;
    public PlatForm platFormPrefab;

    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnY;
    public float maxSpawnY;

    private Player player;
    private int score;

    public CamController mainCam; 
    public override void Awake()
    {
        MakeSingleton(false);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameGUIManager.Ins.UpdateScoreCouting(score);
        GameGUIManager.Ins.ShowGameGUI(false);
    }

    public void PlayGame()
    {
        StartCoroutine(PlatformInit());
        GameGUIManager.Ins.ShowGameGUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //xử lý công việc cần đợi trong một khoảng thời gian
    IEnumerator PlatformInit()
    {
        PlatForm platFormClone = null;
        if (platFormPrefab)
        {
            platFormClone = Instantiate(platFormPrefab, new Vector2(0, Random.Range(minSpawnY, maxSpawnY)), Quaternion.identity);
            platFormClone.id = platFormClone.gameObject.GetInstanceID();
        }
        yield return new WaitForSeconds(0.5f);
        if (playerPrefab)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.lastPlastFormId = platFormClone.id;
        }
        if (platFormPrefab)
        {
            float spawnX = player.transform.position.x + minSpawnX;
            PlatForm platFormClone2 = Instantiate(platFormPrefab, new Vector2(spawnX, Random.Range(minSpawnY, maxSpawnY)), Quaternion.identity);
            platFormClone.id = platFormClone.gameObject.GetInstanceID();
        }
    }
    
    public void CreatePlatform()
    {
        if (!platFormPrefab || !player) return;

        float spawnX = Random.Range(player.transform.position.x + minSpawnX, player.transform.position.x + minSpawnX);
        float spawnY = Random.Range(minSpawnY, maxSpawnY);
        PlatForm platFormClone = Instantiate(platFormPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
        platFormClone.id = platFormClone.gameObject.GetInstanceID();
    }
    public void CreatePlatformAndLerp(float playerXpos)
    {
        if (mainCam)
        {
            mainCam.lerpTrigger(playerXpos + minSpawnX);
        }
        CreatePlatform();
    }

    public void AddScore()
    {
        score++;
        GameGUIManager.Ins.UpdateScoreCouting(score);
    }
}
