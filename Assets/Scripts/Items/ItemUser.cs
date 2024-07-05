using UnityEngine;

public class ItemUser : MonoBehaviour
{
    public LayerMask machineMask;

    private Rigidbody2D _rigi;

    private bool _canInteract, _hitted;

    public ItemSO _scriptableItem;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rigi = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (transform.childCount > 0)
        {
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _spriteRenderer.transform.localScale = new Vector3(6, 6, 6);
            _spriteRenderer.sprite = _scriptableItem.ItemSprite;
        }
    }

    /// <summary>
    /// Fizik hesaplamalar�n�n �a�r�ld��� method'dur nesne e�er hareket halindeyse etkile�ime ge�ebilece�i Makineleri arar
    /// </summary>
    private void FixedUpdate()
    {
        if (_canInteract && _rigi.velocity.magnitude < 0.2f)
        {
            _canInteract = false;
        }

        if (_canInteract)
            TryInteract();
    }

    public void CanInteract()
    {
        _canInteract = true;
        TryInteract();
    }

    private void TryInteract()
    {
        RaycastHit2D hit =
            Physics2D.CircleCast(
                transform.position,
                transform.lossyScale.x * 0.7f,
                transform.up,
                0,
                machineMask
            );

        if (hit)
        {
            if (hit.collider.TryGetComponent(out CraftingTable crafting))
            {
                _rigi.velocity = Vector3.zero;
                //print(hit.collider.gameObject.name);
                print(_scriptableItem + " Ekledi");
                crafting.AddItem(this.gameObject, _scriptableItem);
            }
            if (hit.collider.TryGetComponent(out ConveyorBelt belt))
            {
                BeltSystem.Instance.AddNewItem2PathCenter(belt, this.gameObject);
                this.enabled = false;
            }
        }
    }
}
