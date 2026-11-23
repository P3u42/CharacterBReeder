using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CreaturePopulation : MonoBehaviour
{
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private UIDocument igaDocument;
    [SerializeField] private float creatureY = 0f; // altura no mundo onde as criaturas ficam

    private const int POP_SIZE = 5;
    [SerializeField] private int mutationRate = 5;

    private List<GameObject> population = new List<GameObject>();
    private int generation = 1;

    private Slider[] sliders      = new Slider[POP_SIZE];
    private Label[]  sliderLabels = new Label[POP_SIZE];
    private Label    generationLabel;

    // Guarda os VisualElements dos boxes para ler posição depois
    private VisualElement[] creatureBoxes = new VisualElement[POP_SIZE];

    void Start()
    {
        BindUI();
        // Aguarda um frame para o UIToolkit calcular o layout antes de ler worldBound
        StartCoroutine(SpawnAfterLayout());
    }

    private System.Collections.IEnumerator SpawnAfterLayout()
    {
        // UIToolkit só calcula worldBound após o primeiro layout pass
        yield return null;
        GeneratePopulation();
    }

    // Converte o centro de um VisualElement para posição no mundo 2D
    private Vector3 BoxToWorldPosition(VisualElement box)
    {
        // worldBound retorna o rect do elemento em coordenadas de tela (pixels, Y invertido)
        Rect rect = box.worldBound;

        // Centro do box em pixels de tela
        Vector2 screenCenter = new Vector2(
            rect.x + rect.width  * 0.5f,
            rect.y + rect.height * 0.5f
        );

        // UIToolkit usa Y=0 no topo; ScreenToWorldPoint usa Y=0 na base — inverter
        screenCenter.y = Screen.height - screenCenter.y;

        // Converte para posição no mundo na profundidade Z=0
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenCenter.x, screenCenter.y, -Camera.main.transform.position.z)
        );

        // Usa o Y configurável para controlar a altura da criatura dentro do box
        worldPos.y = creatureY;
        return worldPos;
    }

    private void BindUI()
    {
        var root = igaDocument.rootVisualElement;
        generationLabel = root.Q<Label>("GenerationLabel");

        for (int i = 0; i < POP_SIZE; i++)
        {
            int idx = i;
            creatureBoxes[i] = root.Q<VisualElement>($"CreatureBox_{i}");
            sliders[i]       = root.Q<Slider>($"Slider_{i}");
            sliderLabels[i]  = root.Q<Label>($"SliderValue_{i}");

            sliders[i].RegisterValueChangedCallback(evt =>
            {
                sliderLabels[idx].text = Mathf.RoundToInt(evt.newValue).ToString();
                if (idx < population.Count && population[idx] != null)
                    population[idx].GetComponent<CreatureDNA>()
                        .SetFitnessFromSlider(evt.newValue);
            });
        }

        root.Q<Button>("EvolveButton").clicked += GenerateNewPopulation;
    }

    private void GeneratePopulation()
    {
        for (int i = 0; i < POP_SIZE; i++)
        {
            Vector3 pos = BoxToWorldPosition(creatureBoxes[i]);
            GameObject creature = Instantiate(creaturePrefab, pos, Quaternion.identity);
            SetRandomGenes(creature.GetComponent<CreatureDNA>());
            creature.GetComponentInChildren<CreatureBody>().InitFromDNA();
            population.Add(creature);
        }
        UpdateGenerationLabel();
    }

    private void GenerateNewPopulation()
    {
        for (int i = 0; i < population.Count && i < sliders.Length; i++)
            if (population[i] != null)
                population[i].GetComponent<CreatureDNA>().SetFitnessFromSlider(sliders[i].value);

        List<GameObject> sorted = population
            .Where(o => o != null)
            .OrderByDescending(o => o.GetComponent<CreatureDNA>().GetFitness())
            .ToList();

        List<GameObject> previousPopulation = sorted;
        population = new List<GameObject>(POP_SIZE);

        for (int i = 0; i < POP_SIZE && sorted.Count > 0; i++)
        {
            int slotA = i % sorted.Count;
            int slotB = (i + 1) % sorted.Count;
            population.Add(Crossover(sorted[slotA], sorted[slotB], i));
        }

        while (population.Count < POP_SIZE)
        {
            int idx = population.Count;
            Vector3 pos = BoxToWorldPosition(creatureBoxes[idx]);
            GameObject extra = Instantiate(creaturePrefab, pos, Quaternion.identity);
            SetRandomGenes(extra.GetComponent<CreatureDNA>());
            extra.GetComponentInChildren<CreatureBody>().InitFromDNA();
            population.Add(extra);
        }

        foreach (var c in previousPopulation)
            if (c != null) Destroy(c);

        for (int i = 0; i < POP_SIZE; i++)
        {
            sliders[i].value     = 5f;
            sliderLabels[i].text = "5";
            if (i < population.Count && population[i] != null)
                population[i].GetComponent<CreatureDNA>().ResetFitness();
        }

        generation++;
        UpdateGenerationLabel();
    }

    private GameObject Crossover(GameObject parent1, GameObject parent2, int slot)
    {
        Vector3 pos = BoxToWorldPosition(creatureBoxes[slot]);
        GameObject child = Instantiate(creaturePrefab, pos, Quaternion.identity);

        CreatureDNA dna1     = parent1.GetComponent<CreatureDNA>();
        CreatureDNA dna2     = parent2.GetComponent<CreatureDNA>();
        CreatureDNA childDna = child.GetComponent<CreatureDNA>();

        if (Random.Range(0, 1000) > mutationRate)
        {
            childDna.bodyR          = Random.Range(0, 10) < 5 ? dna1.bodyR          : dna2.bodyR;
            childDna.bodyG          = Random.Range(0, 10) < 5 ? dna1.bodyG          : dna2.bodyG;
            childDna.bodyB          = Random.Range(0, 10) < 5 ? dna1.bodyB          : dna2.bodyB;
            childDna.bodyLengthGene = Random.Range(0, 10) < 5 ? dna1.bodyLengthGene : dna2.bodyLengthGene;
            childDna.bodyWidthGene  = Random.Range(0, 10) < 5 ? dna1.bodyWidthGene  : dna2.bodyWidthGene;
        }
        else
        {
            Mutation(childDna);
        }

        child.GetComponentInChildren<CreatureBody>().InitFromDNA();
        return child;
    }

    private void SetRandomGenes(CreatureDNA dna)
    {
        dna.bodyR          = Random.Range(0f, 1f);
        dna.bodyG          = Random.Range(0f, 1f);
        dna.bodyB          = Random.Range(0f, 1f);
        dna.bodyLengthGene = Random.Range(10, 30);
        dna.bodyWidthGene  = Random.Range(0.5f, 2.5f);
    }

    private void Mutation(CreatureDNA dna)
    {
        dna.bodyR          = Random.Range(0f, 1f);
        dna.bodyG          = Random.Range(0f, 1f);
        dna.bodyB          = Random.Range(0f, 1f);
        dna.bodyLengthGene = Random.Range(10, 30);
        dna.bodyWidthGene  = Random.Range(0.5f, 2.5f);
    }

    private void UpdateGenerationLabel()
    {
        if (generationLabel != null)
            generationLabel.text = $"Geração {generation}";
    }
}
