using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private Vector3 _resetPosition;
    private float _horizontalBound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _horizontalBound = -20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameOver)
        {
            transform.Translate(Vector3.left * (Time.deltaTime * scrollSpeed));
            if (transform.position.x < _horizontalBound)
            {
                _resetPosition = new Vector3(-_horizontalBound - 2, 0, 3);
                transform.position = _resetPosition;
            }
        }
    }
}
