using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : Actor
{
    public List<Collider2d> triggerList;

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        HeroPawn OtherActor = other.gameObject.GetComponentInParent<HeroPawn>();

        if (!TriggerList.Contains(other))
        {
        
        triggerList.Add(Other);
        }
}


}
