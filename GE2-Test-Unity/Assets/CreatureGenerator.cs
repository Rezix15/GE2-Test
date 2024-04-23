using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private float frequency;
    [SerializeField] private float startAngle;
    [SerializeField] private float baseSize;
    [SerializeField] private float multiplier;
    [SerializeField] private GameObject headBoid;
    [SerializeField] private GameObject bodyBoid;

    private SpineAnimator boidSpineAnimator;

    private int state;

    private float size;

    private List<GameObject> bones;

    private List<GameObject> boids;

    private GameObject headBoidObj;
    
    // Start is called before the first frame update
    void Start()
    {
        size = baseSize * multiplier;
        Time.timeScale = state;
        var sizeOffsetHead = size + Mathf.Sin((Mathf.PI * 2 /  frequency) * 0 * size);
        headBoidObj = Instantiate(headBoid, new Vector3(sizeOffsetHead, 0.5f + sizeOffsetHead, 6 + sizeOffsetHead), Quaternion.identity);
        headBoidObj.transform.localScale = new Vector3(sizeOffsetHead, sizeOffsetHead, sizeOffsetHead);
        boidSpineAnimator = headBoidObj.GetComponent<SpineAnimator>();
        bones = new List<GameObject>();
        boids = new List<GameObject>();
        
        boids.Add(headBoidObj);
        

        for (int i = 1; i < length; i++)
        {
            var sizeOffset = size + Mathf.Sin((Mathf.PI * 2 /  frequency) * i * size);
            var bodyBoidObj = Instantiate(bodyBoid, new Vector3(sizeOffset, 0.5f + sizeOffset, (6 - (i * 3)) + sizeOffset), Quaternion.identity);
            bodyBoidObj.transform.localScale = new Vector3(sizeOffset, sizeOffset, sizeOffset);
            bones.Add(bodyBoidObj);
            boidSpineAnimator.bones = bones.ToArray();
            boids.Add(bodyBoidObj);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //Simplistic pausing
        if (Input.GetKeyDown(KeyCode.P))
        {
            state = state == 0 ? 1 : 0;

            Time.timeScale = state;
        }

        UpdateParams();
        
    }

    //Function to dynamically change the parameters of the boid during play
    private void UpdateParams()
    {
        if (boids.Count > length)
        {
            boids.RemoveAt(boids.Count - 1);
            bones.RemoveAt(bones.Count - 1);
        }
        else if(boids.Count < length)
        {
            var sizeOffset = size + Mathf.Sin((Mathf.PI * 2 /  frequency) * (boids.Count - 1) * size);
            var bodyBoidObj = Instantiate(bodyBoid, new Vector3(size + sizeOffset, 0.5f + sizeOffset, (6 - ((boids.Count - 1) * 3)) + size + sizeOffset), Quaternion.identity);
            bodyBoidObj.transform.localScale = new Vector3(sizeOffset, sizeOffset, sizeOffset);
            bones.Add(bodyBoidObj);
            boidSpineAnimator.bones = bones.ToArray();
            boids.Add(bodyBoidObj);
        }
        
        foreach (var boid in boids)
        {
            var sizeOffset = size + Mathf.Sin((Mathf.PI * 2 / frequency) * boids.IndexOf(boid) * size);
            boid.transform.localScale = new Vector3(sizeOffset, sizeOffset, sizeOffset);
        }
    }

    private void OnDrawGizmos()
    {
        size = baseSize * multiplier;
        var gizmosPosition = new Vector3();
        var gizmosSize = 0f;
        
        for (int i = 0; i < length; i++)
        {
            var position = new Vector3(0, 0.5f, (6 - (i * 3)));
            gizmosPosition = position;
            gizmosSize = size + Mathf.Sin((Mathf.PI * 2 / frequency) * i * size);
            Gizmos.DrawWireCube(gizmosPosition, new Vector3(gizmosSize,  gizmosSize, gizmosSize));
            Gizmos.color = Color.white;
        }
        
        
    }
}
