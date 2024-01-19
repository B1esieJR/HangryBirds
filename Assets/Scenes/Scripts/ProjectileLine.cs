using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;
    [SerializeField] private float minDistance = 0.1f;
    private LineRenderer lineRend;
    private GameObject _poi;
    private List<Vector3> points;
    private void Awake()
    {
        S = this;
        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
        points = new List<Vector3>();
    }
    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                lineRend.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public Vector3 LastPoint
    {
        get
        {
            if (points == null)
            {
                return Vector3.zero;
            }
            return (points[points.Count - 1]);
        }
      
    }
    public void Clear()
    {
        _poi = null;
        lineRend.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - LastPoint).magnitude < minDistance)
        {
            return;
        }
        if (points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            lineRend.positionCount = 2;
            lineRend.SetPosition(0, points[0]);
            lineRend.SetPosition(1, points[1]);
            lineRend.enabled = true;
        }
        else 
        {
            points.Add(pt);
            lineRend.positionCount = points.Count;
            lineRend.SetPosition(points.Count - 1, LastPoint);
            lineRend.enabled = true;
        }
    }
    private void FixedUpdate()
    {
        if (poi == null)
        {
            if (cameraFollow.POI != null)
            {
                if (cameraFollow.POI.tag == "Projectile")
                {
                    poi = cameraFollow.POI;
                }
                else return;
            }
            else return;
        }
 
        AddPoint();
        if (cameraFollow.POI == null)
        {
            poi = null;
        }
    }
}
