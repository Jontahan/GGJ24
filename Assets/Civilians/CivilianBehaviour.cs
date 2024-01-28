using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class CivilianBehaviour : MonoBehaviour
{
    [SerializeField] private float walkingRange;
    [SerializeField] private Animator animator;
    
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private SkinnedMeshRenderer ragDollMeshRenderer;
    [SerializeField] private Transform ragdollRoot;
    
    private Rigidbody[] rigidbodies;
    private bool isRagdoll = false;
    
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent, "CivilianBehaviour script must be attached to a GameObject with a NavMeshAgent component");
        Assert.IsNotNull(animator, "CivilianBehaviour script must be attached to a GameObject with an Animator component");

        // Randomize the color of the civilian
        var randomPantsColor = Random.ColorHSV();
        var randomHaircolor = Random.ColorHSV();
        var randomShirtColor = Random.ColorHSV();

        var materials = meshRenderer.materials;
        materials[0].color = randomHaircolor;
        materials[3].color = randomPantsColor;
        materials[4].color = randomShirtColor;

        var materials1 = ragDollMeshRenderer.materials;
        materials1[0].color = randomHaircolor;
        materials1[3].color = randomPantsColor;
        materials1[4].color = randomShirtColor;
        
        WalkToRandomDestination();
        
        rigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        SetRagdoll(false);
    }
    
    void Update()
    {
        // if destination is reached, generate a new one
        if (!ragdollRoot && agent.remainingDistance < 0.5f)
        {
            WalkToRandomDestination();
        }
        
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    
    private void WalkToRandomDestination()
    {
        // Generate a random position on the NavMesh
        var randomPosition = Random.insideUnitSphere * walkingRange;
        NavMesh.SamplePosition(randomPosition, out var hit, walkingRange, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }
    
    private void SetRagdoll(bool enableRagdoll)
    {
        ragdollRoot.gameObject.SetActive(enableRagdoll);
        meshRenderer.gameObject.SetActive(!enableRagdoll);
        
        agent.enabled = !enableRagdoll;
        
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !enableRagdoll;
        }
        
        isRagdoll = enableRagdoll;
        animator.enabled = !enableRagdoll;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!isRagdoll)
        {
            SetRagdoll(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isRagdoll)
        {
            SetRagdoll(true);
        }
    }
}
