using UnityEngine;

public class ColetarMoeda2D : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            
            Debug.Log("Moeda 2D coletada!");

            
            Destroy(gameObject);
        }
    }
}