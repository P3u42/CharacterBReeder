using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBody : MonoBehaviour
{
    [SerializeField] private int bodyLength = 10;
    [SerializeField] private LineRenderer lineBody;
    [SerializeField] private Transform targetDir;
    [SerializeField] private Vector3[] segmentPoses;
    [SerializeField] private float targetDist = 0.35f;
    [SerializeField] private Vector3[] segmentV;
    [SerializeField] private float smoothSpeed = 0.08f;
    [SerializeField] private float wiggleSpeed = 2f;
    [SerializeField] private float wiggleMagnitude = 1f;
    [SerializeField] private Transform wiggleDir;
    [SerializeField] private Material[] bodyTextures;
    [SerializeField] private Sprite[] headSprites;
    [SerializeField] private Transform limb;
    [SerializeField] private Transform[] bodyParts;

    private CreatureDNA creatureDNA;
    [SerializeField] private SpriteRenderer creatureHead;
    private LineRenderer lineRenderer;

    void Start()
    {
        if (lineBody == null)
            lineBody = GetComponentInChildren<LineRenderer>();

        lineRenderer = lineBody;
        SetSegments();
    }

    void Update()
    {
        MoveSegmemts();
    }

    private void SetSegments()
    {
        if (lineBody == null)
            return;

        lineBody.positionCount = Mathf.Max(2, bodyLength);
        segmentPoses = new Vector3[lineBody.positionCount];
        segmentV = new Vector3[lineBody.positionCount];
    }

    private void MoveSegmemts()
    {
        if (wiggleDir == null || targetDir == null || lineBody == null || segmentPoses == null)
            return;

        wiggleDir.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed);

            if (bodyParts != null && i - 1 >= 0 && i - 1 < bodyParts.Length && bodyParts[i - 1] != null)
                bodyParts[i - 1].position = segmentPoses[i];
        }

        lineBody.SetPositions(segmentPoses);
    }

    public void InitFromDNA()
    {
        creatureDNA = GetComponentInParent<CreatureDNA>();
        if (creatureHead == null && creatureDNA != null)
            creatureHead = creatureDNA.GetComponentInChildren<SpriteRenderer>(true);

        if (creatureDNA == null || creatureHead == null || lineBody == null)
            return;

        if (lineRenderer == null)
            lineRenderer = lineBody;

        lineRenderer.material.color = new Color(creatureDNA.bodyR, creatureDNA.bodyG, creatureDNA.bodyB);
        lineRenderer.startColor = new Color(creatureDNA.bodyR, creatureDNA.bodyG, creatureDNA.bodyB);
        lineRenderer.endColor = new Color(creatureDNA.bodyR, creatureDNA.bodyG, creatureDNA.bodyB);
        creatureHead.color = new Color(creatureDNA.bodyR, creatureDNA.bodyG, creatureDNA.bodyB);

        lineBody.startWidth = creatureDNA.bodyWidthGene;
        lineBody.endWidth = creatureDNA.bodyWidthGene;
        bodyLength = creatureDNA.bodyLengthGene;

        BodyPosition();
    }

    private void BodyPosition()
    {
        if (lineBody == null)
            return;

        lineBody.positionCount = Mathf.Max(2, bodyLength);
        segmentPoses = new Vector3[lineBody.positionCount];
        segmentV = new Vector3[lineBody.positionCount];

        if (targetDir != null)
            segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            if (targetDir != null)
            {
                segmentPoses[i] = segmentPoses[i - 1] + targetDir.right * targetDist;
                if (bodyParts != null && i - 1 >= 0 && i - 1 < bodyParts.Length && bodyParts[i - 1] != null)
                    bodyParts[i - 1].position = segmentPoses[i];
            }
        }

        lineBody.SetPositions(segmentPoses);
    }
}
