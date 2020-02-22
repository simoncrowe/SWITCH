using UnityEngine;
using UnityEngine.SceneManagement;

public class UserExitScene : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Had" || other.gameObject.name == "pushingCollider")
        { 
            SceneManager.LoadScene(1);
        }
    }

}
