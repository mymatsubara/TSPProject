using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    private GameObject fireball;
    private bool alive;

    public float speed = 6.0f;
    public float distanceToTurn = 2.0f;    
    public float detectionRadius = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;

        transform.Translate(0, 0, speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit raycast;
        if (Physics.SphereCast(ray, detectionRadius, out raycast))
        {
            PlayerCharacter player = raycast.transform.gameObject.GetComponent<PlayerCharacter>();
            if (fireball == null && player != null)            
                ShootFireball();            

            if (raycast.distance < distanceToTurn)            
                TurnArround();            
        }
    }

    private void TurnArround()
    {
        float angle = Random.Range(-110, 110);
        transform.Rotate(0, angle, 0);
    }

    public void SetAlive(bool alive)
    {
        this.alive = alive;
    }

    void ShootFireball()
    {
        fireball = Instantiate(fireballPrefab) as GameObject;
        fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
        fireball.transform.rotation = transform.rotation;
    }
}
