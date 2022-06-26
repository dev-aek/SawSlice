using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceController : MonoBehaviour
{
    [SerializeField]
    public Material mat;
    [SerializeField]
    public LayerMask mask;
    public ParticleSystem sliceEffect;


    void Update()
    {
        Collider[] toSliceObjects = Physics.OverlapBox(transform.position, new Vector3(1f, 0.1f, .1f), transform.rotation, mask);

        foreach (Collider obj in toSliceObjects)
        {
            if (obj.gameObject != null)
            {
                Debug.Log("T3");

                SlicedHull slicedObject = Kes(obj.gameObject, mat);
                GameObject slicedUpPart = slicedObject.CreateUpperHull(obj.gameObject, mat);
                GameObject slicedLowPart = slicedObject.CreateLowerHull(obj.gameObject, mat);
                sliceEffect.Play();

                if (slicedUpPart != null)
                    BilesenEkle(slicedUpPart);

                if (slicedLowPart != null)
                    BilesenEkle(slicedLowPart);

                Destroy(obj.gameObject);
            }

        }
    }

    private void Start()
    {
        sliceEffect.Stop();
    }

    /* private void OnTriggerEnter(Collider other)
     {
         Debug.Log("T1");
         if (other.gameObject.tag == "Slicable")
         {
             Debug.Log("T2");

             Collider[] toSliceObjects = Physics.OverlapBox(transform.position, new Vector3(1f, 0.1f, .1f), transform.rotation, mask);

              foreach (Collider obj in toSliceObjects)
              {
                 Debug.Log("T3");

                 SlicedHull slicedObject = Kes(obj.gameObject, mat);
                  GameObject slicedUpPart = slicedObject.CreateUpperHull(obj.gameObject, mat);
                  GameObject slicedLowPart = slicedObject.CreateLowerHull(obj.gameObject, mat);

                  BilesenEkle(slicedUpPart);
                  BilesenEkle(slicedLowPart);

                  Destroy(obj.gameObject);
              }
         }
     }*/

    public SlicedHull Kes(GameObject obj, Material mat = null)
    {
        return obj.Slice(transform.position, transform.right, mat);
    }

    void BilesenEkle(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        obj.GetComponent<Rigidbody>().AddExplosionForce(450, obj.transform.position, 20);
        Destroy(obj.gameObject, 2f);
        //obj.GetComponent<Rigidbody>().AddForce(transform.right * 100);

    }
}
