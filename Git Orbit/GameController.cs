using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CharacterController character;
 
    public static GameController instance;
    public GameObject[] orbitPrefab;

    public GameObject emptyPrefab;
    public bool isGameStarted;
   
    public Orbit lastOrbit;

    public List<ColorPalette> allPalettes;

    public ColorLerp gradient1;
    public ColorLerp gradient2;

    public List<Transform> planets;

    public float score;
    public int scoreModifier;

    int stage;
    int palettesStage;
    int currentPalette;

    public float currentSpeedModifier;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeGradients();
        StartGame();

        currentSpeedModifier = 1;
        stage++;
        palettesStage++;
        currentPalette++;

    }

    private void Update()
    {
        if (score / 1000 >= palettesStage)
        { 
            ChangeColors();
        }
        if (score/500 >= stage)
        {
            IncreaseSpeed();
        }
            
        /*if (CheckForLastObjectDistance()<= 17 && isGameStarted == true)
        {
            GenerateOrbit();
        }*/
    }

    void ChangeColors()
    {
        if (currentPalette == allPalettes.Count)
        {
            currentPalette = 0;
        }
        ChangePalette(allPalettes[currentPalette++]);
        palettesStage ++;

    }

    void IncreaseSpeed() {
        currentSpeedModifier += 0.1f;
        SpeedUpAllUnits();
        stage++;
    }

    void SpeedUpAllUnits() {
        character.speedModifier = currentSpeedModifier;
        character.needValidation = true;
        for (int i = 0; i < planets.Count; i++)
        {
            if (planets[i] != null)
            {
                planets[i].parent.GetComponent<Orbit>().speedModifier = currentSpeedModifier;
            }
        }
    }
  

    private void InitializeGradients()
    {
        gradient1.colors = allPalettes[0].gradient1Colors;
        gradient2.colors = allPalettes[0].gradient2Colors;
    }

    private void ChangePalette(ColorPalette palette)
    {
        gradient1.colors = palette.gradient1Colors;
        gradient2.colors = palette.gradient2Colors;
    }

    private void StartGame()
    {
        for (int i = 1; i < 15; i++)
        {
            lastOrbit = Instantiate(emptyPrefab, Vector3.zero, Quaternion.identity).GetComponent<Orbit>();
            lastOrbit.startposition = new Vector3(i, 0, 0);
            planets.Add(lastOrbit.orbit.transform);
        }


        for (int i = 15; i < 18; i++)
        {
            int index = Random.Range(0, orbitPrefab.Length);
            lastOrbit = Instantiate(orbitPrefab[index], Vector3.zero, Quaternion.identity).GetComponent<Orbit>();
            lastOrbit.startposition = new Vector3(i, 0, 0);
            ValidateList();
            planets.Add(lastOrbit.orbit.transform);
        }
        isGameStarted = true;
    }

    private void GenerateOrbit() {
        int index = Random.Range(0, orbitPrefab.Length);
        lastOrbit = Instantiate(orbitPrefab[index], Vector3.zero, Quaternion.identity).GetComponent<Orbit>();
        lastOrbit.startposition = new Vector3(18, 0, 0);
        lastOrbit.speedModifier = currentSpeedModifier;
        ValidateList();
        planets.Add(lastOrbit.orbit.transform);
    }

    public void ValidateList() {
        for (int i = 0; i < planets.Count; i++)
        {
            if (planets[i] == null)
            {
                planets.RemoveAt(i);
                i = 0;
            }
        }
    }

    private float CheckForLastObjectDistance() {
        float distance = (lastOrbit.orbit.transform.position - lastOrbit.transform.position).magnitude;
        return distance;
    }
}
