using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenu : MonoBehaviour
{
    public void OnClickReturn()
    {
        GameManager.Compo.LoadScene(SceneName.Menu);
    }
}