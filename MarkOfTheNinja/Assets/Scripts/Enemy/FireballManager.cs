using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballManager : MonoBehaviour
{
    private int fireballCount = 0;
    private static readonly object _lock = new object();
    private LevelManagerController levelManagerController;

    void Start()
    {
        levelManagerController = FindObjectOfType<LevelManagerController>();
        levelManagerController.LevelWasReset += Reset;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool canShootFireball()
    {
        lock(_lock)
        {
            return fireballCount < 3;
            //Debug.Log("entered can shoot fireball");
        }
    }

    public void addFireball()
    {
        lock(_lock)
        {
            fireballCount++;
            //Debug.Log("Fireball count: " + fireballCount);
        }

    }

    public void removeFireball()
    {
        lock(_lock)
        {
            if(fireballCount > 0) fireballCount--;
        }
    }

    private void Reset()
    {
        fireballCount = 0;
    }
}
