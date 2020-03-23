using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    //see if I am carrying
    public bool isPositive = false;

    //Emit virus
    float spreadFrequency;

    //survive virus
    float lifeTime;
    float dieTime;
    float survivalChance;

    //walk
    public Vector3 walkingDirection;
    float walkingSpeed;
    float changeDirectionProb = 0.25f;
    float changeSpeedProb = 0.25f;
    float changeFrequency;
    //
    void Start()
    {
        walkingSpeed = Random.Range(Manager.s.walkingSpeedRange.x, Manager.s.walkingSpeedRange.y);
        RandomizeDirection();
        StartCoroutine("ChangeDirection");
        changeFrequency = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        if (isPositive)
        {
            if(lifeTime < dieTime)
            {
                lifeTime += Time.deltaTime;
            }
            else
            {
                if(Random.value > Manager.s.surviveChance)
                {
                    Survived();
                }
                else
                {
                    Manager.s.diedNum++;
                    Destroy(gameObject);
                }
                
            }
        }

    }

    void Walk()
    {
        transform.localPosition += walkingDirection * Time.deltaTime * walkingSpeed * Manager.s.movementSpeedMultiplier;
        
        //walking boundaries
        if(transform.localPosition.x > Manager.s.walkingRange.x || transform.localPosition.x < Manager.s.walkingRange.x * -1)
        {
            ReverseDirection();
        }
        if (transform.localPosition.z > Manager.s.walkingRange.y || transform.localPosition.z < Manager.s.walkingRange.y * -1)
        {
            ReverseDirection();
        }
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            if(Random.value < changeDirectionProb)
            {
                RandomizeDirection();
            }

            if(Random.value < changeSpeedProb)
            {
                RandomizeSpeed();
            }
            
            changeFrequency = Random.value;
            yield return new WaitForSeconds(changeFrequency);
        }
    }

    void RandomizeDirection()
    {
        print("randomize direction");
        walkingDirection = new Vector3(Random.value * 2f - 1f, 0, Random.value * 2f - 1f);
        walkingDirection.Normalize();
    }

    void RandomizeSpeed()
    {
        walkingSpeed = Random.Range(Manager.s.walkingSpeedRange.x, Manager.s.walkingSpeedRange.y);
    }

    void ReverseDirection()
    {
        Vector3 dir = Vector3.zero - transform.localPosition;
        walkingDirection = dir.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Virus" && !isPositive)
        {
            isPositive = true;
            dieTime = Random.Range(Manager.s.surviveTime.x, Manager.s.surviveTime.y);
            GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
            StartCoroutine("SpreadDisease");
            Manager.s.infectedNum++;
        }
    }

    private void Survived()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0, Random.value + 0.5f, Random.value + 0.5f, 1));
    }

    IEnumerator SpreadDisease()
    {
        while (true)
        {
            //drop virus
            GameObject g = Instantiate(Manager.s.virusPrefab, transform.parent);
            g.transform.position = transform.position;
            spreadFrequency = Random.Range(Manager.s.spreadDeseaseFrequency.x, Manager.s.spreadDeseaseFrequency.y);
            print("spread");
            yield return new WaitForSeconds(spreadFrequency);
        }
    }
}
