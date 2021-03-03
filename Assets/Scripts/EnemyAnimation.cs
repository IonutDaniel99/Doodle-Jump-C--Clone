using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    public AnimationClip[] animationer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animation>().clip = animationer[Random.Range(0, animationer.Length)];
        GetComponent<Animation>().Play();
    }

}
