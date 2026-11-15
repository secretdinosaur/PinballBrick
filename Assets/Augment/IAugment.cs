using UnityEngine;
using System;
using System.Collections;

public interface IAugment
{
    string ID { get; }
    string Name { get; }
    string Description { get; }
    int Rarity { get; }
    string Type { get; }
    float CoolTime => 0;
    string AdditionalDescription { get; }
    bool isFollowUpAugment { get; }
    void Apply() { }
    
    void Remove() { }
    void BeforeAttackEffect(IBrick Brick) { }

    /// <summary>
    /// Called Once When Ball Hits Brick.
    /// </summary>
    /// <param name="Brick"></param>
    void AfterMainAttackEffect(IBrick Brick) { }

    void CriticalAttackEffect(IBrick Brick) { }

    void AfterBirthEffect(IBall ball) { }
    void AfterDeathEffect(IBall ball) { }

    /// <summary>
    /// Called Every Attack Except Dot(Effect) Damage.
    /// </summary>
    /// <param name="Brick"></param>
    void AfterAttackEffect(IBrick Brick) { }

    void AfterBrickDieEffect(IBrick Brick) { }

    void AfterLevelUpEffect() { }
    
}

public abstract class StatAugment : IAugment
{
    public abstract string ID { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Rarity { get; }
    public virtual string Type { get; }
    public abstract string AdditionalDescription { get; }
    public abstract bool isFollowUpAugment { get; }

    public abstract void Apply();
    public abstract void Remove();
    public virtual void BeforeAttackEffect(IBrick Brick) { }
    public virtual void AfterMainAttackEffect(IBrick Brick) { }
    public virtual void CriticalAttackEffect(IBrick Brick) { }
    public virtual void AfterBirthEffect(IBall ball) { }
    public virtual void AfterDeathEffect(IBall ball) { }
    public virtual void AfterAttackEffect(IBrick Brick) { }
    public virtual void AfterBrickDieEffect(IBrick Brick) { }
    public virtual void AfterLevelUpEffect() { }
}

public abstract class FieldAugment : IAugment
{
    public abstract string ID { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Rarity { get; }
    public virtual float CoolTime => 0;
    public abstract string Type { get; }
    public abstract string AdditionalDescription { get; }
    public abstract bool isFollowUpAugment { get; }
    public abstract void Apply();
    public abstract void Remove();
    public virtual void BeforeAttackEffect(IBrick Brick) { }
    public virtual void AfterMainAttackEffect(IBrick Brick) { }
    public virtual void CriticalAttackEffect(IBrick Brick) { }
    public virtual void AfterBirthEffect(IBall ball) { }
    public virtual void AfterDeathEffect(IBall ball) { }
    public virtual void AfterAttackEffect(IBrick Brick) { }
    public virtual void AfterBrickDieEffect(IBrick Brick) { }
    public virtual void AfterLevelUpEffect() { }
}

public abstract class SpecialAugment : IAugment
{
    public abstract string ID { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Rarity { get; }
    public virtual float CoolTime => 0;
    public abstract string Type { get; }
    public abstract string AdditionalDescription { get; }
    public abstract bool isFollowUpAugment {  get; }

    public abstract void Apply();
    public abstract void Remove();
    public virtual void BeforeAttackEffect(IBrick Brick) { }
    public virtual void AfterMainAttackEffect(IBrick Brick) { }
    public virtual void CriticalAttackEffect(IBrick Brick) { }
    public virtual void AfterBirthEffect(IBall ball) { }
    public virtual void AfterDeathEffect(IBall ball) { }
    public virtual void AfterAttackEffect(IBrick Brick) { }
    public virtual void AfterBrickDieEffect(IBrick Brick) { }
    public virtual void AfterLevelUpEffect() { }
}