using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateUponPlayer : MonoBehaviour
{
    [SerializeField] private float activationDistance;
    [SerializeField] private GameObject objectToAtivate;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) < activationDistance)
        {
            objectToAtivate.SetActive(true);
        }
    }
}
