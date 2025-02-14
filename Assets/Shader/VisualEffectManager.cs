using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Instance;
    public ParticleSystem LightningPS;
    public ParticleSystem LightningHitPS;
    public ParticleSystem ChainLightningPS;

    public ParticleSystem FireBlastPS;
    public ParticleSystem InfinityPoisonPS;

    public Material DecayMaterial;
    public Material DefaultMaterial;

    public ParticleSystem DecayPunchPS;
    public ParticleSystem DecayExplosionPS;

    public ParticleSystem AddAttack1PS;
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
                LightningPS.transform.position = ParticlePosition + new Vector2(0, 10f);
                LightningPS.Emit(ParticleCount);

                LightningHitPS.transform.position = ParticlePosition;
                LightningHitPS.Emit(5);
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[LightningHitPS.particleCount];
                LightningHitPS.GetParticles(particles);

                for (int i = 0; i < particles.Length; i++)
                {
                    Vector3 velocity = particles[i].velocity;
                    float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

                    particles[i].rotation = angle;
                }
                break;
            case "FireBlast":
                FireBlastPS.transform.position = ParticlePosition;
                FireBlastPS.Emit(ParticleCount);
                break;

            case "InfinityPoison":
                InfinityPoisonPS.transform.position = ParticlePosition;
                InfinityPoisonPS.Emit(ParticleCount);
                break;

            case "DecayExplosion":
                DecayPunchPS.transform.position = ParticlePosition;
                DecayPunchPS.Emit(1);
                StartCoroutine(SpawnDecayExplosion(ParticlePosition));
                break;

            case "AdditionalAttack":
                AddAttack1PS.transform.position = ParticlePosition;
                AddAttack1PS.Emit(1);
            break;

            default:
                Debug.Log("Invalid ParticleName");
                break;
        }
    }

    private IEnumerator SpawnDecayExplosion(Vector2 ParticlePosition)
    {
        yield return new WaitForSeconds(0.2f);
        DecayExplosionPS.transform.position = ParticlePosition;
        DecayExplosionPS.Emit(30);
    }

    public void SpawnChainLightning(List<IBrick> Bricks)
    {
        ChainLightningPS.Emit(Bricks.Count - 1);
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ChainLightningPS.particleCount];
        ChainLightningPS.GetParticles(particles);
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
            
            particles[i].rotation3D = new Vector3(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg + 90);

            ChainLightningPS.SetParticles(particles);
        }
    }

    public void SetDecayVisualEffect(IBrick brick)
    {
        brick.GetComponent<SpriteRenderer>().material = DecayMaterial;
    }

    public void UnSetDecayVisualEffect(IBrick brick)
    {
        brick.GetComponent<SpriteRenderer>().material = DefaultMaterial;
    }
}
