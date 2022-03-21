using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowControl : MonoBehaviour
{
    public bool isFiring;

    public ArrowController arrow;
    public float arrowSpeed;
    public float timeBtwAttack;
    private float shotCounter;

    public bool test;

    public Transform arrowPivot;


    void Update()
    {
        testfunc();
        if (isFiring)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter < 0f)
            {
                shotCounter = timeBtwAttack;
                ArrowController newArrow = Instantiate(arrow, arrowPivot.position, arrowPivot.rotation) as ArrowController;
                newArrow._speed = arrowSpeed;
            }
        }
        else
        {
            shotCounter = 0f;
        }
    }
    private void testfunc()
    {
        if (!test) return;
        {
            Debug.Log(test);
        }
    }
}
