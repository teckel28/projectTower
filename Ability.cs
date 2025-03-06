using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace projectTower{

    abstract class Ability
    {
        //stuff
        public string abilityName {get; set;} = "";
        public string description {get; set;} = "";
        public string descriptionL2 {get; set;} = "";
        public string descriptionL3 {get; set;} = "";
        public string element {get; set;} = "";
        public int mpCost {get; set;} = 0;
        
        //scaling
        public decimal strScale {get; set;} = 0;
        public decimal magScale {get; set;} = 0;
        public decimal tecScale {get; set;} = 0;

        //constructor
        public Ability(string name, string description, string descriptionL2, string descriptionL3, string element, int mpCost, decimal strScale, decimal magScale, decimal tecScale)
        {
            this.abilityName = name;
            this.description = description;
            this.descriptionL2 = descriptionL2;
            this.descriptionL3 = descriptionL3;
            this.element = element;

            this.mpCost = mpCost;

            this.strScale = strScale;
            this.magScale = magScale;
            this.tecScale = tecScale;
        }
        public Ability(){}

        public abstract void PrintAbility(int x, int y, int str, int mag, int tec);

        public abstract void UseAbility(Character user, Character target, int line);
    }
    
    class NullAbility : Ability
    {
        public NullAbility(string name, string description, string descriptionL2, string descriptionL3, string element, int mpCost, decimal strScale, decimal magScale, decimal tecScale)
                            : base (name, description, descriptionL2, descriptionL3, element, mpCost, strScale, magScale, tecScale){}
        public NullAbility(){}

        public override void PrintAbility(int x, int y, int str, int mag, int tec)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("No ability");
        }

        public override void UseAbility(Character user, Character target, int line){
            Writer.WriteText("Does nothing", line);
        }

        
    }


    class DamageAbility : Ability 
    {
        public int power {get; set;}
        public int abilityPrecMod {get; set;}
        public int damage {get; set;}
        public int scaledPower {get; set;}

        public DamageAbility(string name, string description, string descriptionL2, string descriptionL3, string element, int power, int abilityPrecMod, int mpCost, decimal strScale, decimal magScale, decimal tecScale)
                            : base(name, description, descriptionL2, descriptionL3, element, mpCost, strScale, magScale, tecScale)
        {
            this.power = power;
            this.abilityPrecMod = abilityPrecMod;
        }
        public DamageAbility(){}

        public override void PrintAbility(int x, int y, int str, int mag, int tec)
        {
            
            scaledPower = (int)(this.power + (this.strScale*str) + (this.magScale*mag) + (this.tecScale*tec));
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Power: " + this.power + "  MP Cost: " + this.mpCost);
            Console.SetCursorPosition(x, y+1);
            Console.WriteLine("Scaling: STR " + (int)(this.strScale * 100) + "% / MAG " + (int)(this.magScale * 100) + "% / TEC " + (int)(this.tecScale * 100) + "%");
            Console.SetCursorPosition(x, y+2);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Element: " + this.element + " Precision modifier: +" + this.abilityPrecMod);
            Console.SetCursorPosition(x, y+3);
            Console.WriteLine("Scaled Power: " + scaledPower);
            Console.SetCursorPosition(x, y+4);
            Console.WriteLine(this.description);
            Console.SetCursorPosition(x, y+5);
            Console.WriteLine(this.descriptionL2);
            Console.SetCursorPosition(x, y+6);
            Console.WriteLine(this.descriptionL3);
        }
        
        public override void UseAbility(Character user, Character target, int line){
            Writer.WriteText(user.name + " used " + this.abilityName, line);
            //check for MP
            if(user.currentMp >= this.mpCost){
                damage = 0;
                scaledPower = (int)(this.power + (this.strScale * user.strength) + (this.magScale * user.magic) + (this.tecScale * user.technique));//calculate scaled power
                //OnUseAbility
                if (user.headEquip != null) user.headEquip.OnUseAbility(user, target, this);
                if (user.chestEquip != null) user.chestEquip.OnUseAbility(user, target, this);
                if (user.legsEquip != null) user.legsEquip.OnUseAbility(user, target, this);
                if (user.feetEquip != null) user.feetEquip.OnUseAbility(user, target, this);
                if (user.accesoryEquip1 != null) user.accesoryEquip1.OnUseAbility(user, target, this);
                if (user.accesoryEquip2 != null) user.accesoryEquip2.OnUseAbility(user, target, this);
                if (user.weapon != null) user.weapon.OnUseAbility(user, target, this);
                if (user.weapon2 != null) user.weapon2.OnUseAbility(user, target, this);

                user.UpdateStats(); target.UpdateStats();
                //end of OnUseAbility
                
                user.currentMp -= this.mpCost;//reduce mp
                int baseHit = Program.random.Next(1, 21);//roll die
                int hit = baseHit + user.precision + this.abilityPrecMod;//add prec modifiers
                Writer.WriteText("Rolled a [BASE " + baseHit + "] + [PREC " + user.precision + " ] + [ABILITY " + this.abilityPrecMod + "] = [TOTAL " + hit + "]", line + 1);

                if (hit >= target.evasion)//check hit
                {
                    float rand = (Program.random.Next(85, 101) / 100f);
                
                    if (scaledPower >= target.defense)
                    {
                        damage += (int)(((scaledPower) - (target.defense / 2)) * rand);//formula for pow > def
                    }
                    else
                    {                                                                  //formula for def > pow
                        int sc = scaledPower * scaledPower;
                        int df = 2 * target.defense;
                        damage += (int)((sc / df) * rand);
                    }



                    target.currentHp -= damage;//deal damage
                    this.OnAbilityHit(user, target, line);
                    Writer.WriteText(this.abilityName + " hits for " + damage + " damage", line + 2);


                    //OnDamage() and OnTakingDamage() for relics and equipment----------------------------------------
                    foreach (Relic relic in user.relics)
                    {
                        relic.OnDamage(user, target, this);
                    }
                    foreach (Relic relic in target.relics)
                    {
                        relic.OnTakingDamage(user, target, this);
                    }
                    if(user.headEquip != null) user.headEquip.OnDamage(user, target, this);
                    if(user.chestEquip != null) user.chestEquip.OnDamage(user, target, this);
                    if(user.legsEquip != null) user.legsEquip.OnDamage(user, target, this);
                    if(user.feetEquip != null) user.feetEquip.OnDamage(user, target, this);
                    if(user.accesoryEquip1 != null) user.accesoryEquip1.OnDamage(user, target, this);
                    if(user.accesoryEquip2 != null) user.accesoryEquip2.OnDamage(user, target, this);
                    if(user.weapon != null) user.weapon.OnDamage(user, target, this);
                    if(user.weapon2 != null) user.weapon2.OnDamage(user, target, this);
                    if(target.headEquip != null) target.headEquip.OnTakingDamage(user, target, this);
                    if(target.chestEquip != null) target.chestEquip.OnTakingDamage(user, target, this);
                    if(target.legsEquip != null) target.legsEquip.OnTakingDamage(user, target, this);
                    if(target.feetEquip != null) target.feetEquip.OnTakingDamage(user, target, this);
                    if(target.accesoryEquip1 != null) target.accesoryEquip1.OnTakingDamage(user, target, this);
                    if(target.accesoryEquip2 != null) target.accesoryEquip2.OnTakingDamage(user, target, this);
                    if(target.weapon != null) target.weapon.OnTakingDamage(user, target, this);
                    if(target.weapon2 != null) target.weapon2.OnTakingDamage(user, target, this);
                    
                    user.UpdateStats(); target.UpdateStats();
                    //end of OnDamage() and OnTakingDamage()-------------------------------------------------------


                }
                else//miss
                {
                    Writer.WriteText(this.abilityName + " missed", line + 2);
                }
            }else{//not mp
                Writer.WriteText("Not enough MP, deal 1 damage to yourself", line);
                user.currentHp -= 1;
            }
            
            
        }

        public virtual void OnAbilityHit(Character user, Character target, int line){}
    }
    class DamageBloodStrike : DamageAbility{
        public DamageBloodStrike(){}

        public override void OnAbilityHit(Character user, Character target, int line)
        {
            base.UseAbility(user, target, line);
            Writer.WriteText("The weapon takes your HP and grows", line + 3);
            Writer.WriteText("[Continue: press any key]", line + 5);
            Console.ReadKey();
            user.currentHp -= (int)(user.maxHp*0.05m);
            user.weapon.strMod += 2;
        }
    }
    class DamageWound : DamageAbility{
        public DamageWound(){}

        public override void OnAbilityHit(Character user, Character target, int line)
        {
            Writer.WriteText("Inflicts Bleed 3", line + 3);
            Writer.WriteText("[Continue: press any key]", line + 5);
            Console.ReadKey();
            target.bleed += 3;
        }
    }



    class DamagePoison : DamageAbility
    {
        public int poisonChance{get; set;}
        public DamagePoison(){}

        public override void OnAbilityHit(Character user, Character target, int line)
        {
            if(!target.poison && Program.random.Next(1, 101) <= poisonChance){
                Writer.WriteText("The target was poisoned", line + 3);
                target.poison = true;
            }
        }
    }


}
//stuff
/*string name, string description, string element, int precision,
                        int power, int mpCost, int heal, int mpDrain,
                        float strScale, float magScale, float tecScale,
                        int buffStr, int deBuffStr, int buffMag, int deBuffMag, int buffTec, int deBuffTec,
                        int buffDef, int deBuffDef, int buffSpeed, int deBuffSpeed, int buffPrec, int deBuffPrec, int buffEva, int deBuffEva*/