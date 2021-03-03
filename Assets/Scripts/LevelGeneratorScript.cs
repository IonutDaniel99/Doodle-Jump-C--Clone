using UnityEngine;
public class LevelGeneratorScript : MonoBehaviour
{

    public static LevelGeneratorScript Instance { set; get; }

    public GameObject Player;
    public GameObject thePlatform;
    public Sprite sprite;
    public Transform generationPoint;
    public Transform generateRocket;
    public Transform generateCoin;
    public Transform generateEnemy;
    public bool RandomPlatform = false;
    private float distanceBetweenGenerationPointAndPlayer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (SaveManager.Instance.state.activePlatform == 9)
        {
            RandomPlatform = true;
        }
        else
        {
            var a = Manager.Instance.Platforms[SaveManager.Instance.state.activePlatform];
            sprite = Sprite.Create(a, new Rect(0f, 0f, a.width, a.height), new Vector2(0.5f, 0.5f), 100f);
            thePlatform.GetComponent<SpriteRenderer>().sprite = sprite;
        }

    }


    void Update()
    {
        if (Player == null)
        {
            return;
        }
        else
        {
            distanceBetweenGenerationPointAndPlayer = generationPoint.transform.position.y - Player.transform.position.y;
            if (distanceBetweenGenerationPointAndPlayer < 10) SpawnPlatform();
        }
    }
    public void SpawnPlatform()
    {

        if (RandomPlatform == true)
        {
            int r = Random.Range(0, 8);
            var a = Manager.Instance.Platforms[r];
            sprite = Sprite.Create(a, new Rect(0f, 0f, a.width, a.height), new Vector2(0.5f, 0.5f), 100f);
            thePlatform.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        int RandomNumber = Random.Range(0, 100);

        transform.position = new Vector2(Random.Range(-3f, 3f), transform.position.y + 3);
        Instantiate(thePlatform, transform.position, transform.rotation);

        if (RandomNumber <= 15 && SaveManager.Instance.state.activeUpgrade == 0)
        {
            SpawnRocket();
        }
        if (SaveManager.Instance.state.activeUpgrade != 2)
        {
            if ((RandomNumber > 40 && RandomNumber < 60) && generationPoint.transform.position.y > 15)
            {
                SpawnEnemy();
            }
        }
        if (RandomNumber >= 65)
        {
            Instantiate(generateCoin, transform.position + new Vector3(-0.1f, 0.4f, 0f), Quaternion.identity);
        }
    }

    public void SpawnRocket()
    {
        Instantiate(generateRocket, transform.position + new Vector3(-0.1f, 0.6f, 0f), Quaternion.Euler(0, 0, 45));
    }

    public void ExtraLife()
    {
        Vector2 pos = PlayerScript.Instance.LastPlatformHit;
        pos.y = Player.gameObject.transform.position.y + 5f;
        Player.gameObject.transform.position = new Vector2(pos.x, pos.y);

    }

    public void SpawnEnemy()
    {
        Instantiate(generateEnemy, new Vector3(0, generationPoint.transform.position.y, 0), Quaternion.identity);
    }
}