using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.ParticleSystem;
using System;
using System.Drawing;
using UnityEngine.UIElements;

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

    public ParticleSystem ExplosionWithBallPS;

    public ParticleSystem SuicideBoomBallPS;
    public ParticleSystem SuicideBoomBallPS2;

    public ParticleSystem CursePS;
    public ParticleSystem BombBallPS;

    public ParticleSystem BallSacrificePS;

    public ParticleSystem AllDamagePS;

    Vector2 Vec = Vector2.zero;
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnParticle(string ParticleName, Vector2 ParticlePosition, int ParticleCount)
    {
        switch (ParticleName)
        {
            case "Lightning":
                LightningPS.transform.position = ParticlePosition + new Vector2(0, 10f);
                LightningPS.Emit(ParticleCount);

                LightningHitPS.transform.position = ParticlePosition;
                LightningHitPS.Emit(5);
                Particle[] particles = new Particle[LightningHitPS.particleCount];
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
                DecayExplosionPS.transform.position = ParticlePosition;
                DecayExplosionPS.Emit(15);
                break;

            case "AdditionalAttack":
                AddAttack1PS.transform.position = ParticlePosition;
                AddAttack1PS.Emit(1);
                break;

            case "StrongAdditionalAttack":
                DecayPunchPS.transform.position = ParticlePosition;
                DecayPunchPS.Emit(1);
                break;

            case "SuicideBoomBall":
                SuicideBoomBallPS.transform.position = ParticlePosition;
                SuicideBoomBallPS.Emit(5);

                SuicideBoomBallPS2.transform.position = ParticlePosition;
                SuicideBoomBallPS2.Emit(10);
                break;

            case "BombBall":
                BombBallPS.transform.position = ParticlePosition;
                BombBallPS.Emit(30);

                SuicideBoomBallPS2.transform.position = ParticlePosition;
                SuicideBoomBallPS2.Emit(40);
                break;

            case "Curse":
                CursePS.transform.position = new Vector2(0, -10);
                SpawnCurseEffect(ParticlePosition);
                break;

            case "BallSacrifice":
                BallSacrificePS.transform.position = ParticlePosition;
                BallSacrificePS.Emit(ParticleCount);
                break;

            case "AllDamage":
                AllDamagePS.transform.position = ParticlePosition + new Vector2(0, 1.4f);
                AllDamagePS.Emit(ParticleCount);
                break;

            default:
                Debug.Log("Invalid ParticleName");
                break;
        }
    }

    public void SpawnCurseEffect(Vector2 Pos)
    {
        Vec = Pos - new Vector2(0, -10);

        CursePS.Emit(10);

        Particle[] CurseParticles = new Particle[CursePS.particleCount];
        CursePS.GetParticles(CurseParticles);

        Array.Sort(CurseParticles, (a, b) => b.remainingLifetime.CompareTo(a.remainingLifetime));

        for (int i = 0; i < 10; i++)
        {
            float angle = Mathf.Atan2(Vec.y, Vec.x) * Mathf.Rad2Deg;
            CurseParticles[i].position = new Vector2(0, -10);
            CurseParticles[i].rotation = 90 - angle + UnityEngine.Random.Range(-0.1f, 0.1f);

            Vector3 size = CurseParticles[i].startSize3D;
            size.x = 1f;
            size.y = Vec.magnitude;
            CurseParticles[i].startSize3D = size;
        }
        CursePS.SetParticles(CurseParticles);
    }

    public void SpawnExplosionWithBallEffect(Vector2 Vec, Vector2 Pos)
    {
        ExplosionWithBallPS.Emit(3);

        Particle[] ExplosionWithBallParticles = new Particle[ExplosionWithBallPS.particleCount];
        ExplosionWithBallPS.GetParticles(ExplosionWithBallParticles);

        Array.Sort(ExplosionWithBallParticles, (a, b) => b.remainingLifetime.CompareTo(a.remainingLifetime));

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Atan2(Vec.y, Vec.x) * Mathf.Rad2Deg;
            ExplosionWithBallParticles[i].position = Pos;
            ExplosionWithBallParticles[i].rotation = angle;

            int Dir = i % 2 == 1 ? 1 : -1;

            float randomAngleRad = UnityEngine.Random.Range(-10f, 10f) * Mathf.Deg2Rad;
            float cosA = Mathf.Cos(randomAngleRad);
            float sinA = Mathf.Sin(randomAngleRad);

            Vector3 randomizedVec = new Vector3(
                Vec.x * sinA + Vec.y * cosA,
                Vec.x * cosA - Vec.y * sinA
            ) * Dir;

            ExplosionWithBallParticles[i].velocity = randomizedVec * 10;
        }
        ExplosionWithBallPS.SetParticles(ExplosionWithBallParticles);
    }
    public void SpawnChainLightning(List<IBrick> Bricks)
    {
        ChainLightningPS.Emit(Bricks.Count - 1);
        Particle[] particles = new Particle[ChainLightningPS.particleCount];
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
        brick.GetComponent<SpriteRenderer>().material.SetFloat("_Decay", 1);
    }

    public void UnSetDecayVisualEffect(IBrick brick)
    {
        brick.GetComponent<SpriteRenderer>().material.SetFloat("_Decay", 0);
    }
}
