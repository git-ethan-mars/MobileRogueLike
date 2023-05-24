using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerDownHandler
{
    private bool _isOpened;
    [SerializeField] public Experience[] experiencePrefabs;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        var physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster is null)
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        var player = FindObjectOfType<Player>().gameObject.GetComponent<Player>();
        var distance = Vector2.Distance(player.transform.position, transform.position);
        if (!_isOpened && distance < 3)
        {
            _isOpened = true;
            _animator.Play("Open");
            StartCoroutine(Utils.DoActionAfterDelay(0.45f, () =>
            {
                StartCoroutine(Utils.DoActionAfterAnimationFinished(_animator, "Open", () =>
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var color = Random.Range(0, 3);
                        Instantiate(experiencePrefabs[color],
                            transform.position + new Vector3(-0.5f + i * 0.5f, 1f), transform.rotation);
                    }

                    _boxCollider.enabled = false;
                }));
            }));
        }
    }
}
