using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
      static public GameObject launchpoint;
    public GameObject prefabProjectile;
    private GameObject launchPoint;
    private Vector3 launchPos;
    private GameObject projectile;
    private bool aimingMode;
    private float velocityMult = 8f;
    private Rigidbody projectileRigid;
    static private Slingshot S;
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }
    private void Awake()
    {
        S = this;
        Transform launchPointTransform=transform.Find("LaunchPoint");
        launchpoint = launchPointTransform.gameObject;
        launchpoint.SetActive(false);
        launchPos = launchpoint.transform.position;
        
    }
    private void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchpoint.SetActive(true);
    }
    private void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchpoint.SetActive(false);
    }
    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile);
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigid = projectile.GetComponent<Rigidbody>();
        projectileRigid.isKinematic = true;
    }
    private void Update()
    {
        ProjectileMove();
    }
    private void ProjectileMove()
    {
        if (!aimingMode)
            return;
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = true;
            projectileRigid.isKinematic = false;
            projectileRigid.velocity = -mouseDelta * velocityMult;
            cameraFollow.POI = projectile;
            projectile = null;
            GameController.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
