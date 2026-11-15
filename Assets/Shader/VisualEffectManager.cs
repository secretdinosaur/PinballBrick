using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.ParticleSystem;
using System;
using System.Drawing;
using UnityEngine.UIElements;
using Redcode.Pools;
using DG.Tweening;

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

    public PoolManager PoolManager;

    public ParticleSystem SuicideBoomBallPS;
    public ParticleSystem SuicideBoomBallPS2;

    public ParticleSystem CursePS;
    public ParticleSystem BombBallPS;

    public ParticleSystem BallSacrificePS;

    public ParticleSystem AllDamagePS;

    public ParticleSystem[] AugmentPanelPS;
    public ParticleSystem[] AugmentPanelBurstPS;

    Vector2 Vec = Vector2.zero;
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetActiveAugmentPanelPs(int AugmentPanelNum, UnityEngine.Color Color, int Rarity)
    {
        if (Rarity >= 4)
        {
            AugmentPanelPS[AugmentPanelNum].gameObject.SetActive(false);
            AugmentPanelPS[AugmentPanelNum].transform.position = new Vector3(AugmentPanelPS[AugmentPanelNum].transform.position.x, 6.8f, 0);

            return;
        }

        AugmentPanelPS[AugmentPanelNum].gameObject.SetActive(true);
        AugmentPanelPS[AugmentPanelNum].GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color);
    }

    public void UnsetAugmentPanelPs()
    {
        for (int i = 0; i < 3; i++)
        {
            AugmentPanelPS[i].transform.position = new Vector3(AugmentPanelPS[i].transform.position.x, 6.8f, 0);
            AugmentPanelPS[i].gameObject.SetActive(false);
        }
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
                DecayExplosionPS.Emit(50);
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

    public void SpawnCurseEffect(Vector2 Pos, Vector2? StartingPos, float MultipleCount = 1)
    {
        StartCoroutine(SpawnCurseEffectCo(Pos, StartingPos, MultipleCount));
    }

    public IEnumerator SpawnCurseEffectCo(Vector2 Pos, Vector2? StartingPos, float MultipleCount)
    {
        Vector2 Startingpos = StartingPos.HasValue ? StartingPos.Value : new Vector2(UnityEngine.Random.Range(-1.5f, 1.5f), -10);
        Vec = Pos - Startingpos;

        CursePS CursePS = PoolManager.GetFromPool<CursePS>();
        UnityEngine. Color color = new UnityEngine.Color(50f / 255f, 5f / 255f, 1f / 255f) + new UnityEngine.Color(12 / 255f, 1f / 255f, 0f / 255f) * (AugmentManager.instance.CurseDamageValue - 1) * 2;
        CursePS.GetComponent<ParticleSystemRenderer>().material.SetColor("_OuterColor", color * (MultipleCount));

        CursePS.GetComponent<ParticleSystem>().Emit(5);
        
        Particle[] CurseParticles = new Particle[CursePS.GetComponent<ParticleSystem>().particleCount];
        CursePS.GetComponent<ParticleSystem>().GetParticles(CurseParticles);
        Array.Sort(CurseParticles, (a, b) => b.remainingLifetime.CompareTo(a.remainingLifetime));

        for (int i = 0; i < CurseParticles.Length; i++)
        {
            float angle = Mathf.Atan2(Vec.y, Vec.x) * Mathf.Rad2Deg;
            CurseParticles[i].position = new Vector3(Startingpos.x, Startingpos.y, 0);
            CurseParticles[i].rotation = 90 - angle + UnityEngine.Random.Range(-0.1f, 0.1f);

            Vector3 size = CurseParticles[i].startSize3D;
            size.x = 1f;
            size.y = Vec.magnitude;
            CurseParticles[i].startSize3D = size;
        }
        CursePS.GetComponent<ParticleSystem>().SetParticles(CurseParticles);

        yield return new WaitForSeconds(1f);

        PoolManager.TakeToPool<CursePS>(CursePS.GetComponent<CursePS>());
    }

    public void SpawnExplosionWithBallEffect(Vector2 Vec, Vector2 Pos)
    {
        StartCoroutine(SpawnExplosionWithBallEffectCo(Vec, Pos));
    }
    public IEnumerator SpawnExplosionWithBallEffectCo(Vector2 Vec, Vector2 Pos)
    {
        float angle = Mathf.Atan2(Vec.y, Vec.x) * Mathf.Rad2Deg;
        float angle2 = (angle + 180f) % 360f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 10f);
        Quaternion rotation2 = Quaternion.Euler(0f, 0f, angle2 - 10f);

        ExplosionWithBallPS PS = PoolManager.GetFromPool<ExplosionWithBallPS>();
        ExplosionWithBallPS PS2 = PoolManager.GetFromPool<ExplosionWithBallPS>();

        PS.transform.position = Pos;
        PS.transform.rotation = rotation;

        PS2.transform.position = Pos;
        PS2.transform.rotation = rotation2;

        PS.GetComponent<ParticleSystem>().Emit(3);
        PS2.GetComponent<ParticleSystem>().Emit(3);

        yield return new WaitForSeconds(0.12f);

        PoolManager.TakeToPool<ExplosionWithBallPS>(PS);
        PoolManager.TakeToPool<ExplosionWithBallPS>(PS2);
    }
    public IEnumerator SpawnChainLightning(List<IBrick> Bricks)
    {
        ChainLightningPS PS = PoolManager.GetFromPool<ChainLightningPS>();

        PS.GetComponent<ParticleSystem>().Emit(Bricks.Count - 1);
        Particle[] particles = new Particle[PS.GetComponent<ParticleSystem>().particleCount];
        PS.GetComponent<ParticleSystem>().GetParticles(particles);

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

            PS.GetComponent<ParticleSystem>().SetParticles(particles);
        }
        yield return new WaitForSeconds(1);
        PoolManager.TakeToPool<ChainLightningPS>(PS);
    }

    public void SpawnThunderStrike(List<IBall> Balls)
    {
        ChainLightningPS.Emit(Balls.Count);
        Particle[] particles = new Particle[ChainLightningPS.particleCount];
        ChainLightningPS.GetParticles(particles);
        Vector2 StartPosition;
        Vector2 EndPosition;
        float Length;
        Vector2 Direction;
        Vector3 size;
        for (int i = 0; i < Balls.Count - 1; i++)
        {
            StartPosition = Balls[i].transform.position;
            EndPosition = Balls[i + 1].transform.position;

            Length = (EndPosition - StartPosition).magnitude;
            Direction = (EndPosition - StartPosition).normalized;

            size = particles[i].startSize3D;
            size.x = 1f;
            size.y = Length;

            particles[i].startSize3D = size;
            particles[i].position = StartPosition;

            particles[i].rotation3D = new Vector3(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg + 90);
        }
        StartPosition = Balls[Balls.Count - 1].transform.position;
        EndPosition = Balls[0].transform.position;

        Length = (EndPosition - StartPosition).magnitude;
        Direction = (EndPosition - StartPosition).normalized;

        size = particles[Balls.Count - 1].startSize3D;
        size.x = 1f;
        size.y = Length;

        particles[Balls.Count - 1].startSize3D = size;
        particles[Balls.Count - 1].position = StartPosition;

        particles[Balls.Count - 1].rotation3D = new Vector3(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg + 90);

        ChainLightningPS.SetParticles(particles);

    }
    public void SetDecayVisualEffect(IBrick brick) { StartCoroutine(SetDecayVisualEffectC(brick)); }
    public void UnSetDecayVisualEffect(IBrick brick) { StartCoroutine(UnSetDecayVisualEffectC(brick)); }
    public IEnumerator SetDecayVisualEffectC(IBrick brick)
    {
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            brick.GetComponent<SpriteRenderer>().material.SetFloat("_Decay", t);
            yield return null;
        }

        brick.GetComponent<SpriteRenderer>().material.SetFloat("_Decay", 1f);
    }

    public IEnumerator UnSetDecayVisualEffectC(IBrick brick)
    {
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = duration / elapsed;
            brick.GetComponent<SpriteRenderer>().material.SetFloat("_Decay", t);
            yield return null;
        }

        brick.GetComponent<SpriteRenderer>().material.SetFloat("_Decay", 0f);
    }
}
