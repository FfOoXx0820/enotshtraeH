using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public Shop Shop1;
    public Shop Shop2;
    public Player Player1;
    public Player Player2;
    public GameManager GameManager;
    private bool _isDragActive = false;
    private Vector2 _screenPosition;
    private Vector3 _worldPosition;
    private Draggable _lastDragged;
    private void Awake()
    {
        DragController[] controller = FindObjectsOfType<DragController>();
        if (controller.Length > 1)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (_isDragActive && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))){
            Drop();
            return;
        }
        if (Input.GetMouseButton(0)){
            Vector3 mousePos = Input.mousePosition;
            _screenPosition = new Vector2(mousePos.x, mousePos.y);
        } else if (Input.touchCount > 0)
        {
            _screenPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

        if (_isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    _lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }
    void InitDrag()
    {
        _lastDragged.LastPosition = _lastDragged.transform.position;
        UpdateDragStatus(true);
    }

    void Drag()
    {
        _lastDragged.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
    }

    void Drop()
    {
        if (GameManager.player_turn_count == 0)
        {
            if (_lastDragged.ValidDropOnHand)
            {
                if (!Shop1.Buy(_lastDragged.gameObject))
                {
                    _lastDragged.transform.position = _lastDragged.LastPosition;
                }
            }
            else if (_lastDragged.ValidDropOnBattleGround)
            {
                if (!Player1.Play(_lastDragged.gameObject))
                {
                    _lastDragged.transform.position = _lastDragged.LastPosition;
                }
            }
            else if (_lastDragged.ValidDropOnSell)
            {
                if (!Player1.Sell(_lastDragged.gameObject))
                {
                    _lastDragged.transform.position = _lastDragged.LastPosition;
                }
            }
            else
            {
                _lastDragged.transform.position = _lastDragged.LastPosition;
            }
        } else if (GameManager.player_turn_count == 1)
        {
            if (_lastDragged.ValidDropOnHand)
            {
                if (!Shop2.Buy(_lastDragged.gameObject))
                {
                    _lastDragged.transform.position = _lastDragged.LastPosition;
                }
            }
            else if (_lastDragged.ValidDropOnBattleGround)
            {
                if (!Player2.Play(_lastDragged.gameObject))
                {
                    _lastDragged.transform.position = _lastDragged.LastPosition;
                }
            }
            else if (_lastDragged.ValidDropOnSell)
            {
                if (!Player2.Sell(_lastDragged.gameObject))
                {
                    _lastDragged.transform.position = _lastDragged.LastPosition;
                }
            }
            else
            {
                _lastDragged.transform.position = _lastDragged.LastPosition;
            }
        }
        UpdateDragStatus(false);
    }

    void UpdateDragStatus(bool isDragging)
    {
        _isDragActive = _lastDragged.IsDragging = isDragging;
        _lastDragged.gameObject.layer = isDragging ? Layer.Dragging : Layer.Default;
    }
}
