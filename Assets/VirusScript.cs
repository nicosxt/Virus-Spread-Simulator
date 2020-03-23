using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusScript : MonoBehaviour
{
    float lifeTime = 5;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        t += Time.deltaTime;
        if(t > lifeTime + Manager.s.virusEnduranceFloat)
        {
            Destroy(gameObject);
        }
    }
}
