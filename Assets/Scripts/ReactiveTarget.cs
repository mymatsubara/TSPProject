using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    public int pointsOnKill = 1;

    public void ReactToHit(PlayerCharacter hitter)
    {
        Debug.Log(this.gameObject.name + " was hit");
        StartCoroutine(Die());
        hitter?.IncrementScore(pointsOnKill);
    }

    private IEnumerator Die()
    {
        transform.Rotate(-75, 0, 0);
        GetComponent<Wanderer>()?.SetAlive(false);        

        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
    }
}
