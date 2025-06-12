using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed = 2.0f;              // Скорость движения ленты
    public Vector3 direction = Vector3.forward; // Направление движения (относительно ленты)

    private void OnTriggerStay(Collider other)
    {
        // Если объект, находящийся на ленте, имеет Rigidbody, двигаем его
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Перемещаем объект в направлении ленты с заданной скоростью
            Vector3 move = direction.normalized * speed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
            // Важно: Rigidbody объекта желательно сделать кинематическим, 
            // чтобы он не скатывался с ленты под действием гравитации, но при этом 
            // реагировал на MovePosition.
        }
    }
}
