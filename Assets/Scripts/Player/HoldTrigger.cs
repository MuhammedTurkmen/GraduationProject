using UnityEngine;

public class HoldTrigger : MonoBehaviour
{
    public Transform HoldObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Object") && HoldObject == null)
        {
            HoldObject = collision.transform;
        }
    }
}
