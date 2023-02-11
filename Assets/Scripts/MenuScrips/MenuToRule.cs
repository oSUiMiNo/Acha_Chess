using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToRule : MonoBehaviour
{
    public void OnClickRuleButton()
    {
        SceneManager.LoadScene("Rule");
    }
}