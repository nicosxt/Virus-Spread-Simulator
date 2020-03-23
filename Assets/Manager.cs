using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    protected static Manager singleton;
    public static Manager s { get { return singleton; } }
    private void Awake()
    {
        singleton = this;
    }

    //walking
    public Vector2 walkingRange = new Vector2(20,20);
    public Vector2 walkingSpeedRange = new Vector2(0.5f, 5f);
    public int agentAmount = 30;
    public float movementSpeedMultiplier;

    //spreading
    public Vector2 spreadDeseaseFrequency = new Vector2(0.5f, 2.5f);
    public Vector2 surviveTime = new Vector2(5f, 10f);
    public float surviveChance = 0.3f;

    //
    public GameObject agentPrefab;
    public GameObject virusPrefab;

    //UI
    public GameObject simulationSpeed;
    public GameObject virusEndurance;
    public GameObject movementSpeed;
    public float virusEnduranceFloat;
    public Text infected, died;
    public int infectedNum, diedNum;
    void Start()
    {
        GenerateAgents(agentAmount);
        StartCoroutine("GenerateAgentNumerator");
    }



    // Update is called once per frame
    void Update()
    {
        //Time.timeScale = 5 * simulationSpeed.GetComponent<Slider>().value + 1f;
        Time.timeScale = 5f;
        //virusEnduranceFloat = 5f * virusEndurance.GetComponent<Slider>().value;
        movementSpeedMultiplier = 3f * movementSpeed.GetComponent<Slider>().value + 1f;
        infected.text = infectedNum.ToString();
        died.text = diedNum.ToString();
    }

    void GenerateAgents(int _amount)
    {
        for(int i=0; i < _amount; i++)
        {
            GenerateOneAgent();
        }
    }

    void GenerateOneAgent()
    {
        GameObject g = Instantiate(agentPrefab, transform);
        Vector3 _pos = new Vector3(Random.Range(-1 * walkingRange.x, walkingRange.x), 0f, Random.Range(-1 * walkingRange.y, walkingRange.y));
        g.transform.localPosition = _pos;
        Material mat = agentPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        g.GetComponent<MeshRenderer>().material = new Material(mat);
        g.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0, Random.value + 0.5f, Random.value + 0.5f, 1));
    }

    IEnumerator GenerateAgentNumerator()
    {
        while (true)
        {
            GenerateOneAgent();
            yield return new WaitForSeconds(1f);
        }
    }

    public void SpawnVirus()
    {
        Vector3 pos = new Vector3(Random.Range(-1 * walkingRange.x, walkingRange.x), 0f, Random.Range(-1 * walkingRange.y, walkingRange.y));
        GameObject virus = Instantiate(virusPrefab, transform);
        virus.transform.localPosition = pos;
    }
}
