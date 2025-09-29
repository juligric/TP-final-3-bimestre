using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] string sceneToLoad = "SampleScene"; // nombre claro y coincide con Build Settings

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"Vida actual: {health}");

        if (health <= 0)
        {
            Debug.Log("Jugador muerto -> cargando escena...");
            Die();
        }
    }

    void Die()
    {
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
            Debug.Log("✅ Escena cargada correctamente!");
        }
        else
        {
            Debug.LogError("❌ La escena '" + sceneToLoad + "' no está en Build Settings o el nombre no coincide.");
        }
    }
}

