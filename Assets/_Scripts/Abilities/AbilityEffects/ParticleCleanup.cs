using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleanup : MonoBehaviour
{
    [SerializeField]
    float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestorySystem());
    }

    IEnumerator DestorySystem()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
