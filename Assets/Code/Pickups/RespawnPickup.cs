using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPickup : MonoBehaviour {

    //SpawnerTS, SpawnerNCD, SpawnerHJ, SpawnerI, SpawnerL, SpawnerRPU

    public GameObject ts, ncd, hj, i, l;

    void Awake ()
    {
		if (this.tag == "SpawnerTS")
        {
            (Instantiate(ts)).transform.parent = this.transform;
        }
        else if (this.tag == "SpawnerNCD")
        {
            (Instantiate(ncd)).transform.parent = this.transform;
        }
        else if (this.tag == "SpawnerHJ")
        {
            (Instantiate(hj)).transform.parent = this.transform;
        }
        else if (this.tag == "SpawnerI")
        {
            (Instantiate(i)).transform.parent = this.transform;
        }
        else if (this.tag == "SpawnerL")
        {
            (Instantiate(l)).transform.parent = this.transform;
        }
        else if (this.tag == "SpawnerRPU")
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    (Instantiate(ts)).transform.parent = this.transform;
                    break;
                case 1:
                    (Instantiate(ncd)).transform.parent = this.transform;
                    break;
                case 2:
                    (Instantiate(hj)).transform.parent = this.transform;
                    break;
                case 3:
                    (Instantiate(i)).transform.parent = this.transform;
                    break;
                case 4:
                    (Instantiate(l)).transform.parent = this.transform;
                    break;
            }
        }
    } 
}