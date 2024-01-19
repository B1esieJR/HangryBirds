using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Настройка плотности облаков")]
   [SerializeField] private int numClouds = 40;
    [SerializeField] GameObject cloudPrefab;
    [SerializeField] Vector3 CloudPosMin = new Vector3(-50, -5, 10);
    [SerializeField] Vector3 CloudPosMax = new Vector3(150, 100, 10);
    [SerializeField] float cloudScaleMin = 1;
    [SerializeField] float cloudScaleMax = 3;
    [SerializeField] float cloudSpeedMult = 0.5f;
    private GameObject[] cloudInstances;
    private void Awake()
    {
        cloudInstances = new GameObject[numClouds];
        GameObject anchor=GameObject.Find("CloudAnchor");
        GameObject cloud;
        for (var i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(CloudPosMin.x, CloudPosMax.x);
            cPos.y = Random.Range(CloudPosMin.y, CloudPosMax.y);
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            cPos.y = Mathf.Lerp(CloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.SetParent(anchor.transform);
            cloudInstances[i] = cloud;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            if (cPos.x <= CloudPosMin.x)
                cPos.x = CloudPosMax.x;
            cloud.transform.position = cPos;
        }
    }
}
