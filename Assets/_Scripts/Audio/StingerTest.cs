using System.Collections;
using UnityEngine;

public class StingerTest : MonoBehaviour
{
    public AK.Wwise.Event stingerEvent;
    public AK.Wwise.Switch testSwitch;

    public void PlayStinger()
    {
        stingerEvent.Post(gameObject);
    }

    private void PlayStingerRandomly()
    {
        // Example implementation for playing a stinger randomly
        if (Random.value > 0.95f)
        {
            stingerEvent.Post(gameObject);
            Debug.Log("Stinger played!");
        }
    }

    private IEnumerator Start()
    {
        testSwitch.SetValue(gameObject);

        while (true)
        {
            PlayStingerRandomly();
            yield return new WaitForSeconds(5f);
        }
    }
}
