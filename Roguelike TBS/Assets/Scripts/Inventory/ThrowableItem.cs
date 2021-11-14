using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = ("Inventory/Throwable Item"))]
public class ThrowableItem: ActionItem {
    [SerializeField] int throwSpeed = 1;
    [SerializeField] ThrownItem itemPrefab = null;
    [SerializeField] AudioClip sfx = null;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this item.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        ThrownItem spawnedItem = Instantiate(itemPrefab, user.transform.position, Quaternion.identity);
        spawnedItem.SetWielder(user.GetComponent<Fighter>());

        var mouse = Mouse.current.position.ReadValue();
        var screenPoint = Camera.main.WorldToScreenPoint(spawnedItem.transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        spawnedItem.transform.rotation = Quaternion.Euler(0, 0, angle);
        spawnedItem.GetComponent<Rigidbody2D>().velocity = new Vector3(xRatio * throwSpeed, yRatio*throwSpeed, 0);

        if (sfx) { AudioSource.PlayClipAtPoint(sfx, user.transform.position); }
    }
}
