using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class SceneChanger : MonoBehaviour
{
    
    public string sceneToLoad = "Menu"; 

    void Start()
    {
       
        StartCoroutine(ChangeSceneAfterDelay(30f));
    }

    
    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
      
        yield return new WaitForSeconds(delay);

       
        SceneManager.LoadScene(sceneToLoad);
    }
}
