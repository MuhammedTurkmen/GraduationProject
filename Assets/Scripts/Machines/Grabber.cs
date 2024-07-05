using UnityEngine;

public class Grabber : PlacedObject
{
    private float _grabTimer;

    private Vector2 _grabPosition;
    private Vector2 _endPosition;

    [SerializeField]
    private LayerMask mask;

    public void Setup()
    {
        _grabPosition = transform.position - transform.right;
        _endPosition = transform.position + transform.right;
    }

    private void Start()
    {
        Setup();

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            TryPlaceItem();
        };
    }

    public GameObject TakedObject;

    private void TryPlaceItem()
    {
        TakedObject = GridSystem.Instance.TakeItem(Origin);

        RaycastHit2D hit =
            Physics2D.BoxCast(
                transform.position,
                new Vector2(0.2f, 0.2f),
                0,
                transform.right,
                1.5f,
                mask
            );

        if (hit)
        {
            if (hit.collider.TryGetComponent(out ConveyorBelt belt) && TakedObject != null)
            {
                print("eþya banda yerleþtirildi");
                TakedObject.SetActive(true);
                BeltSystem.Instance.AddNewItem2Path(belt, TakedObject);
                TakedObject = null;
                this.enabled = false;
            }
        }

        if (TakedObject == null)
        {
            print($"{Origin}");
        }
        else
        {
            print(TakedObject);
        }
    }
}