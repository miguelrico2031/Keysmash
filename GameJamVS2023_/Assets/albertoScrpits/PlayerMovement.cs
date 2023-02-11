using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        float horizontal = 0.0f;
        float vertical = 0.0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1.0f;
        }
        if (Input.GetKey(KeyCode.A)&& Input.GetKey(KeyCode.W))
        {
            Debug.Log("arriba izquierda");
        }else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            Debug.Log("arriba derecha");
        }
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1.0f;
        }

        Vector2 movement = new Vector2(horizontal, vertical);
        transform.position += (Vector3)movement * speed * Time.deltaTime;
    }
}
