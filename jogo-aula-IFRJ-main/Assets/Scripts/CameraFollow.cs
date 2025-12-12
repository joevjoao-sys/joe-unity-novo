using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTimeX = 0.3f;
    public float smoothTimeY = 0.3f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z; // Mantém Z constante
        
        // Apply different smoothing for X and Y axes
        float smoothedX = Mathf.SmoothDamp(
            transform.position.x, 
            targetPosition.x, 
            ref velocity.x, 
            smoothTimeX
        );
        
        float smoothedY = Mathf.SmoothDamp(
            transform.position.y, 
            targetPosition.y, 
            ref velocity.y, 
            smoothTimeY
        );
        
        transform.position = new Vector3(smoothedX, smoothedY, transform.position.z);
    }
}