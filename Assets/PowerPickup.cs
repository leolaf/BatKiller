using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PowerPickup : MonoBehaviour
{
    [SerializeField] GameObject visuals;
    [SerializeField] float rotationSpeed;
    [SerializeField] float levitationSpeed;
    [SerializeField] GameObject spotlight;
    [SerializeField] PlayerClass pickupClass;

    private Vector3 baseSpotlightPos;

    // Start is called before the first frame update
    void Start()
    {
        baseSpotlightPos = spotlight.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        visuals.transform.Rotate(new Vector3(0,90*Time.deltaTime*rotationSpeed,0));
        visuals.transform.position = transform.position + new Vector3(0, Mathf.Sin(Time.time*levitationSpeed)+1, 0)/2;
        spotlight.transform.position = baseSpotlightPos + new Vector3(0, Mathf.Sin(Time.time*levitationSpeed)+1, 0)/2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControls>().PickUp(pickupClass);
        }
    }
}
