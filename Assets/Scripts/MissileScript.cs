using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : Actor
{
    public List<Collider2D> triggerList;
    public GameObject playerFinder;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Collider2D coll in triggerList)
        {
            if(coll.gameObject.GetComponentInParent<HeroPawn>())
            {
                player = coll.gameObject;

                playerFinder.SetActive(false);
            }
        }
    }



    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        HeroPawn OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (!triggerList.Contains(other))
        {
        
            triggerList.Add(other);
        }
}


}
