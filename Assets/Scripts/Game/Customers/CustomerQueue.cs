using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{
    public static CustomerQueue Instance { get; private set; }
    public Queue<GameObject> queue;
    [SerializeField] private int count;
    public GameObject[] customerPool;
    public GameObject[] bossPool;
    public bool isWaiting;
    public bool bossQueued;
    public Transform spawnpoint;
    private GameObject prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        queue = new Queue<GameObject>();
        isWaiting = false;
        bossQueued = false;
        if (spawnpoint == null)
        {
            spawnpoint = transform.Find("Customer UI");
        }
        RandomQueueCustomers(3);
    }

    // Update is called once per frame
    void Update()
    {
        count = queue.Count;
        if (queue != null)
        {
            if (queue.Count > 0)
            {
                if (queue.Peek().gameObject.GetComponent<Customer>() != null)
                {
                    CallNextCustomer();
                }
            }
            else
            {
                RandomQueueCustomers(3);
            }
            if (!bossQueued && DifficultyController.Instance.ordersCompleted % 5 == 0 && DifficultyController.Instance.ordersCompleted > 0)
            {
                QueueBoss();
            }
            foreach (GameObject item in queue)
            {
                Customer castomer = item.GetComponent<Customer>();
                if (castomer.customer.isBoss)
                {
                    bossQueued = true;
                }
                else
                {
                    bossQueued = false;
                }
            }
        }
    }
    public void CallNextCustomer()
    {
        if (queue == null) { return; }
        if (!isWaiting)
        {
            if (queue.Count > 0)
            {
                prefab = Instantiate(queue.Peek(), spawnpoint);
                queue.Dequeue();
            }
            isWaiting = false;
        }
    }
    public void QueueCustomer(GameObject custy)
    {
        queue.Enqueue(custy);
    }
    public void RandomQueueCustomers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randCust = Random.Range(0, customerPool.Length);
            queue.Enqueue(customerPool[randCust]);
        }
    }
    public void QueueBoss()
    {
        bossQueued = true;
        int randBoss = Random.Range(0, bossPool.Length);
        queue.Enqueue(bossPool[randBoss]);
    }
    public void CustomerEntered()
    {
        isWaiting = true;
    }
    public void CustomerLeft()
    {
        isWaiting = false;
    }
}