using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerInput : MonoBehaviour
{
    public FloatingJoystick joystick;
    private Transform _character;
    private PlayerStatManager _playerStatManager;
    
    private Vector2 _inputVector;
    private Vector2 _moveVector;
    private float _speed;
    
    public bool cheatMode = false;
    
    private void Start()
    {
        _character = GetComponent<Transform>();
        _playerStatManager = GetComponent<PlayerStatManager>();
    }

    void Update()
    {
        if (_inputVector.magnitude > 0)
        {
            _moveVector = _inputVector;
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePause) return;
        
        _speed = _playerStatManager.Speed;
        if (_inputVector.magnitude > 0)
        {
            _character.Translate(new Vector3(_moveVector.x, _moveVector.y, 0) * (_speed * Time.fixedDeltaTime));
        }
        else
        {
            float x = joystick.Horizontal;
            float y = joystick.Vertical;
            _character.Translate(new Vector3(x, y, 0) * (_speed * Time.fixedDeltaTime));
        }
    }

    public void ActionMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    public void ActionNextLevel(InputAction.CallbackContext context)
    {
        if (context.performed && cheatMode)
        {
            for (int i = 0; i < 20; ++i)
            {
                _playerStatManager.LevelUp();
            }
        }
    }
    
    public void ActionDie(InputAction.CallbackContext context)
    {
        if (context.performed && cheatMode)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void ActionGameClear(InputAction.CallbackContext context)
    {
        if (context.performed && cheatMode)
        {
            GameManager.Instance.GameClear();
        }
    }
}
