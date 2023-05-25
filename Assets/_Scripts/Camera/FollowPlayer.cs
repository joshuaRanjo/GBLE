using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraSpeed = 0.1f;

    private Vector3 minValues, maxValues;
    private GameObject bounds;

    [SerializeField] private bool followingPlayer = true;

    void Start()
    {
        transform.position = player.position + cameraOffset;
        
    }

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float n,s,e,w;
        bounds = GameObject.FindGameObjectWithTag("Border");
        n = bounds.transform.Find("NorthBorder").gameObject.transform.position.y;
        s = bounds.transform.Find("SouthBorder").gameObject.transform.position.y;
        e = bounds.transform.Find("EastBorder").gameObject.transform.position.x;
        w = bounds.transform.Find("WestBorder").gameObject.transform.position.x;
        minValues = new Vector3(w,s,-10);
        maxValues = new Vector3(e,n,-10);
    }

    void FixedUpdate()
    {
        float n,s,e,w;
        n = bounds.transform.Find("NorthBorder").gameObject.transform.position.y;
        s = bounds.transform.Find("SouthBorder").gameObject.transform.position.y;
        e = bounds.transform.Find("EastBorder").gameObject.transform.position.x;
        w = bounds.transform.Find("WestBorder").gameObject.transform.position.x;
        minValues = new Vector3(w,s,-10);
        maxValues = new Vector3(e,n,-10);
        Follow();
        
        
    }

    private void Follow()
    {
        if (followingPlayer)
        {
            Vector3 finalPosition = player.position + cameraOffset;

            ///
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(finalPosition.x, minValues.x, maxValues.x),
                Mathf.Clamp(finalPosition.y, minValues.y, maxValues.y),
                Mathf.Clamp(finalPosition.z, minValues.z, maxValues.z));

            Vector3 lerpPosition = Vector3.Lerp(transform.position, boundPosition, cameraSpeed);
            transform.position = lerpPosition;

        }
    }

    public void StartFollowingPlayer()
    {
        followingPlayer = true;
    }

    public void StopFollowingPlayer()
    {
        followingPlayer = false;
    }
}   
