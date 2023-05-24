using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool IsDragging;
    public bool ValidDropOnBattleGround;
    public bool ValidDropOnHand;
    public bool ValidDropOnSell;
    public Vector3 LastPosition;
    private float _movementTime = 15f;
    private Vector3? _movementDestination;
    private void FixedUpdate()
    {
        if (_movementDestination.HasValue)
        {
            if (IsDragging)
            {
                _movementDestination = null;
                return;
            }
            if (transform.position == _movementDestination)
            {
                gameObject.layer = Layer.Default;
                _movementDestination = null;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _movementDestination.Value, _movementTime * Time.fixedDeltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DropValid"))
        {
            if (other.name == "HandSlot")
            {
                ValidDropOnHand = true;
            }
            else if (other.name == "BattleGroundSlot")
            {
                ValidDropOnBattleGround = true;
            }
            else if (other.name == "SellSlot")
            {
                ValidDropOnSell = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DropValid"))
        {
            if (other.name == "HandSlot")
            {
                ValidDropOnHand = false;
            }
            else if (other.name == "BattleGroundSlot")
            {
                ValidDropOnBattleGround = false;
            }
            else if (other.name == "SellSlot")
            {
                ValidDropOnSell = false;
            }
        }
    }
}
