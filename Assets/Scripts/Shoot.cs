using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform FiringPoint;
    public GameObject Muzzle;
    public GameObject HitImpact;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // For PC
        {
            HandleInput(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // For Android
        {
            HandleInput(Input.GetTouch(0).position);
        }
    }

    private void HandleInput(Vector3 inputPosition)
    {
        GetComponent<AudioSource>().Play();

        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Quaternion muzzleRotation = Quaternion.LookRotation(FiringPoint.forward);
            GameObject muzzleParticle = Instantiate(Muzzle, FiringPoint.position, muzzleRotation);

            GameObject hitImpactParticle = Instantiate(HitImpact, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(muzzleParticle, 1);
            Destroy(hitImpactParticle, 1);

            if (hit.collider.gameObject.TryGetComponent<RaycastDetector>(out var raycastDetector))
            {
                raycastDetector.OnRaycastHit();
            }
        }
    }

}
