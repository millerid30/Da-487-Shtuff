using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public CustomerSO customer;
    public string customerID;
    [TextArea]
    [SerializeField] private string customerDescription;

    public float waitTime;
    private bool isServed;
    private GameObject prefab;
    private Arena arena;
    private Transform tipSpawnpoint;

    [Header("Color")]
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.red;
    [SerializeField] private Color winColor = Color.green;
    private Image sprite;

    [Header("Silly")]
    [SerializeField] private float launchForce = 1200f;
    [SerializeField] private float spinForce = 20f;
    [SerializeField] private float killTime = 3f;
    [SerializeField] private float gravScale = 300f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuestSO quest = customer.customerQuest;
        if (quest != null)
        {
            GiveOrder(quest);
            waitTime = customer.maxWaitTime;
        }
        CustomerQueue.Instance.CustomerEntered();
        arena = FindAnyObjectByType<Arena>();
        if (tipSpawnpoint == null)
        {
            tipSpawnpoint = arena.GetTipSpawn();
        }
        sprite = GetComponentInChildren<Image>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;
        waitTime = Mathf.Clamp(waitTime, 0, float.MaxValue);
        Patience();
        if (waitTime <= 0)
        {
            if (isServed)
            {
                OrderComplete();
            }
            else
            {
                OrderFailed();
            }
        }
        if (QuestController.Instance.IsQuestCompleted(customer.customerQuest.questID))
        {
            OrderComplete();
        }
        // TEST
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OrderComplete();
        }
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            OrderFailed();
        }
    }
    void GiveOrder(QuestSO quest)
    {
        QuestController.Instance.AcceptQuest(quest);
    }
    void OrderComplete()
    {
        if (!isServed)
        {
            // Important stuff
            isServed = true;
            // give tip or something
            for (int i = 0; i < customer.rewardMulti; i++)
            {
                var randTip = Random.Range(0, customer.customerTips.Length);
                GiveTip(customer.customerTips[randTip]);
            }
            // Color
            sprite.color = winColor;
            // Silly Shit
            SillySilly();
            StartCoroutine(PartingGift());
        }
    }
    void OrderFailed()
    {
        if (!isServed)
        {
            // Important stuff
            isServed = true;
            // spawn enemy or something
            for (int i = 0; i < customer.failMulti; i++)
            {
                var randTip = Random.Range(0, customer.customerShits.Length);
                string id = customer.customerShits[randTip].GetComponent<Enemy>().enemy.enemyID;
                if (id != null) { WaveSpawner.Instance.ManualSpawn(id); }
            }
            // Color
            sprite.color = endColor;
            // Silly Shit
            SillySilly();
            StartCoroutine(FartingGift());
        }
    }
    void GiveTip(GameObject tip)
    {
        if (tipSpawnpoint == null) { return; }
        prefab = Instantiate(tip, tipSpawnpoint.position, Quaternion.identity);
        Rigidbody2D prb = prefab.GetComponent<Rigidbody2D>();
        if (prb != null)
        {
            var randForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2;
            prb.AddForce(randForce, ForceMode2D.Impulse);
            prb.AddTorque(randForce.x, ForceMode2D.Impulse);
        }
    }
    void Patience()
    {
        if (!isServed)
        {
            sprite.color = Color.Lerp(startColor, endColor, 1 - (waitTime / customer.maxWaitTime));
        }
    }
    void SillySilly()
    {
        rb.gravityScale = gravScale;
        rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-spinForce, spinForce), ForceMode2D.Impulse);
        if (customer.isBoss)
        {
            CinemachineShake.Instance.Shake(10, 1);
        }
    }
    IEnumerator PartingGift()
    {
        // Add to quest script
        DifficultyController.Instance.ordersCompleted++;
        DifficultyController.Instance.IncreaseDifficulty((1 - (waitTime / customer.maxWaitTime)) * 60);
        //
        SendOrderCompletion();
        yield return new WaitForSeconds(killTime);
        CustomerQueue.Instance.CustomerLeft();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield break;
    }
    IEnumerator FartingGift()
    {
        //
        DifficultyController.Instance.ordersFailed++;
        DifficultyController.Instance.IncreaseDifficulty(customer.failMulti * (1 - (waitTime / customer.maxWaitTime)) * 60);
        //
        SendOrderCompletion();
        yield return new WaitForSeconds(killTime);
        CustomerQueue.Instance.CustomerLeft();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield break;
    }
    void SendOrderCompletion()
    {
        QuestController.Instance.CompleteQuest(customer.customerQuest.questID);
        Arena.Instance.IsComplete();
    }
}