using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Instance;
    public ParticleSystem LightningParticleSystem;
    public ParticleSystem LightningHitParticleSystem;
    public ParticleSystem ChainLightningParticleSystem;
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ParticleNameList:
    /// Lightning
    /// </summary>
    /// <param name="ParticleName"></param>
    public void SpawnParticle(string ParticleName, Vector2 ParticlePosition, int ParticleCount)
    {
        switch (ParticleName)
        {
            case "Lightning":
                LightningParticleSystem.transform.position = ParticlePosition + new Vector2(0, 10f);
                LightningParticleSystem.Emit(ParticleCount);

                LightningHitParticleSystem.transform.position = ParticlePosition;
                LightningHitParticleSystem.Emit(5);
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[LightningHitParticleSystem.particleCount];
                LightningHitParticleSystem.GetParticles(particles);

                for (int i = 0; i < particles.Length; i++)
                {
                    Vector3 velocity = particles[i].velocity;
                    float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

                    particles[i].rotation = angle;
                }
                break;
            default:
                Debug.Log("Invalid ParticleName");
                break;
        }
    }

    public void SpawnChainLightning(List<IBrick> Bricks)
    {
        ChainLightningParticleSystem.Emit(Bricks.Count - 1);
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ChainLightningParticleSystem.particleCount];
        ChainLightningParticleSystem.GetParticles(particles);
        for (int i = 0; i < Bricks.Count - 1; i++)
        {
            Vector2 StartPosition = Bricks[i].transform.position;
            Vector2 EndPosition = Bricks[i + 1].transform.position;

            float Length = (EndPosition - StartPosition).magnitude;
            Vector2 Direction = (EndPosition - StartPosition).normalized;

            Vector3 size = particles[i].startSize3D;
            size.x = 1f;
            size.y = Length;

            particles[i].startSize3D = size;
            particles[i].position = StartPosition;

            print("--------------");
            print(StartPosition +
                " " +
                EndPosition +
                " " +
                Direction +
                " " +
                Direction.magnitude +
                " " +
                Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg);
            
            particles[i].rotation3D = new Vector3(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg + 90);

            ChainLightningParticleSystem.SetParticles(particles);
        }

        
    }
}
