using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    /// <summary>
    /// reset app
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}