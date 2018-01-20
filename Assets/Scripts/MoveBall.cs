using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {
    public float initialSpeed = 3f;

    private float m_Speed;
    private bool bWireOn;
    private bool bHitMinDistance;
    private float m_LastDistance;
    private LineRenderer m_LineRend;
    private Target m_NextTarget;
    private Target m_ActiveTarget;
    private List<GameObject> m_TargetList;
    private Vector3 m_RotateAxis;

    void Awake()
    {
        m_TargetList = new List<GameObject>();
    }
    void Start()
    {
        m_Speed = initialSpeed;
        m_LineRend = GetComponent<LineRenderer>();
        SetWireOff();
    }

    // Update is called once per frame
    void Update() {

        if (bWireOn)
        {
            SetWirePositions();
            if (bHitMinDistance)
            {
                transform.RotateAround(m_ActiveTarget.transform.position, m_RotateAxis, m_Speed * Time.deltaTime / ((2 * Mathf.PI * m_LastDistance)/360));
            }
            else
            {
                TravelForward();
                float distance = Vector3.Distance(m_ActiveTarget.transform.position, transform.position);
                if (m_LastDistance >= distance)
                {
                    m_LastDistance = distance;
                }
                else
                {
                    bHitMinDistance = true;
                    float angle = AngleDir(transform.up, transform.position - m_ActiveTarget.transform.position, Vector3.forward);
                    Vector3 dir = transform.position - m_ActiveTarget.transform.position;
                    if(angle > 0)
                    {
                        m_RotateAxis = Vector3.back;
                        dir = -dir;
                    }
                    else
                    {
                        m_RotateAxis = -Vector3.back;
                    }

                    float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(deg, Vector3.forward);
                }
            }
        }
        else
        {
            TravelForward();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            SetWireOn();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            SetWireOff();
        }

    }

    private void TravelForward()
    {
        transform.position += transform.up * (m_Speed * Time.deltaTime);
    }
    public void SetWireOn()
    {
        m_LineRend.enabled = true;
        m_LineRend.positionCount = 2;
        m_ActiveTarget = m_NextTarget;
        m_LastDistance = Vector3.Distance(m_ActiveTarget.transform.position, transform.position);

        SetWirePositions();
        bWireOn = true;
    }
    public void SetWireOff()
    {
        m_LineRend.enabled = false;
        bWireOn = false;
        bHitMinDistance = false;
    }

    private void SetWirePositions()
    {
        m_LineRend.SetPosition(0, transform.position);
        m_LineRend.SetPosition(1, m_ActiveTarget.transform.position);
    }

    private void SetNewNextTarget(Target newTargetInRange)
    {
        m_NextTarget = newTargetInRange;
        foreach (GameObject t in m_TargetList)
        {
            if (t == newTargetInRange.gameObject)
                t.transform.localScale = Vector3.one * 1.3f;
            else
                t.transform.localScale = Vector3.one;
        }
    }
    private void ResetNextToCurrentTarget(Target lostTarget)
    {
        if (bWireOn && lostTarget == m_NextTarget)
        {
            m_NextTarget = m_ActiveTarget;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        m_Speed = newSpeed;
    }

    public void RegisterTarget(GameObject target)
    {
        m_TargetList.Add(target);
        target.GetComponent<Target>().TargetTriggered += SetNewNextTarget;
        target.GetComponent<Target>().TargetUntriggered += ResetNextToCurrentTarget;
    }
    public void UnregisterTarget(GameObject target)
    {
        if (m_TargetList.Contains(target))
            m_TargetList.Remove(target);
    }

    private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        return Vector3.Dot(perp, up);
    }
}
