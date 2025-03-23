using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishnet : MonoBehaviour
{
    public Transform netAnchor;
    public Rigidbody netProjectile;
    public float throwForce = 15f;
    public LineRenderer ropeLine;
    private SpringJoint ropeJoint;
    private Rigidbody netRB;

    void Start()
    {
        ropeLine = GetComponent<LineRenderer>();
        ropeLine.positionCount = 2;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowNet();
        }

        if (netRB != null)
        {
            ropeLine.SetPosition(0, netAnchor.position);
            ropeLine.SetPosition(1, netRB.position);
        }
    }

    void ThrowNet()
    {
        Rigidbody netInstance = Instantiate(netProjectile, netAnchor.position, Quaternion.identity);
        netInstance.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        ropeJoint = netInstance.gameObject.AddComponent<SpringJoint>();
        ropeJoint.connectedBody = GetComponent<Rigidbody>();
        ropeJoint.spring = 20f;
        ropeJoint.damper = 5f;
        ropeJoint.autoConfigureConnectedAnchor = false;
        ropeJoint.anchor = Vector3.zero;
        ropeJoint.connectedAnchor = netAnchor.localPosition;

        netRB = netInstance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Vector3 pullDir = (netAnchor.position - other.transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(pullDir * 30f, ForceMode.Force);
        }
    }
}
