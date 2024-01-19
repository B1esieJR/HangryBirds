using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    
   [Header ("Настройка облака")]
   [SerializeField]  private GameObject cloudSphere;
   [SerializeField] private int numSpheresMin = 6;
   [SerializeField] private int numSpheresMax = 10;
   [SerializeField] Vector3 sphereOffsetScale=new Vector3(5,2,1);
   [SerializeField] Vector2 sphereScaleRangeX = new Vector2(4, 8);
   [SerializeField] Vector2 sphereScaleRangeY = new Vector2(3, 4);
   [SerializeField] Vector2 sphereScaleRangeZ = new Vector2(2, 4);
   [SerializeField]  float ScaleYMin=2f;

    private List<GameObject> spheres;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();
        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (var i = 0; i < num; i++)
        {
            GameObject sphere = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sphere);
            Transform sphereTransform = sphere.transform;
            sphereTransform.SetParent(this.transform);

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y*= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            sphereTransform.localPosition = offset;

            Vector3 scale= Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);
            scale.y *= 1 - (Mathf.Abs(offset.x)/sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, ScaleYMin);
            sphereTransform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }
    void Restart()
    {
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}
