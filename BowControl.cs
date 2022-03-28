using System.Collections;
using UnityEngine;

public class BowControl : MonoBehaviour
{
    public bool isFiring;

    [SerializeField] private ArrowController arrow;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float pullArrow;
    [SerializeField] private Transform arrowPivot;

    private bool isReady;

    void Update()
    {
        if (isFiring)
        {
            StartCoroutine(PullBow());
        }
    }
    private IEnumerator PullBow()
    {
        yield return new WaitForSeconds(pullArrow);
        isReady = true;
        if (Input.GetMouseButtonUp(1) && isReady)
        {
            ArrowController newArrow = Instantiate(arrow, arrowPivot.position, arrowPivot.rotation) as ArrowController;
            newArrow._speed = arrowSpeed;
            isReady = false;
        }
    }
}
