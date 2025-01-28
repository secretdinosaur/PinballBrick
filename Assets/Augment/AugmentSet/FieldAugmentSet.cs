using UnityEngine;

/*
public class Example : FieldAugment
{
     public Example() : base( "FIELD_n", "Example" , "Just Example", 1 ) { }

    public override void Apply(){ Augment Effect (Plus) }

    public override void Remove(){ Augment Effect (Minus) }
}
 */
 
public class AllDamage : FieldAugment
{
    public AllDamage() : base( "FIELD_1", "All Damage" , "All Damage", 3 ) { }

    public override void Apply(){ }

    public override void Remove(){ }
}