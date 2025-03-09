using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime;

namespace projectTower
{
    class Equipment : Item
    {
        public string[] tags;

        public Equipment(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description, int price) :
         base(name, hpMod, mpMod, strMod, magMod, tecMod, defMod, speedMod, precMod, evaMod, rarity, description){
            this.price = price;
         }

        public Equipment(){}

        public virtual void GiveStats(Character chara){
            chara.maxHp += this.hpMod;
            chara.maxMp += this.mpMod;
            chara.strength += this.strMod;
            chara.magic += this.magMod;
            chara.technique += this.tecMod;
            chara.defense += this.defMod;
            chara.speed += this.speedMod;
            chara.precision += this.precMod;
            chara.evasion += this.evaMod;
        }



    }

    //------------------------------------------Head----------------------------------------------------------------
    class HeadEquipment : Equipment
    {
        public HeadEquipment(){}
    }
    class HeadRatCrown : HeadEquipment{
        public HeadRatCrown(){}

        public override void Passive(Character chara)
        {
            if((chara.weapon != null && chara.weapon.tags.Contains("Beast")) || (chara.weapon2 != null && chara.weapon2.tags.Contains("Beast"))){
                chara.strength += 5;
            }
        }
    }
    class HeadMagicHat : HeadEquipment{
        public HeadMagicHat(){}

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {
            user.currentHp += (int)(ability.magScale*10);

        }
    }
    class HeadChariotHelm : HeadEquipment{
        public HeadChariotHelm(){}

        public override void Passive(Character chara)
        {
            if(chara.weapon.tags.Contains("Spear")){
                chara.strength += 15;
            }
        }
    }

    //------------------------------------------Chest----------------------------------------------------------------
    class ChestEquipment : Equipment
    {
        public ChestEquipment(){}
    }
    class SpikeChest : ChestEquipment
    {
        public SpikeChest(){}

        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            user.currentHp -= 3;
        }
    }
    class ChestMagicRobes : ChestEquipment{
        public ChestMagicRobes(){}

        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            if(ability.magScale > 0){
                ability.damage -= (int)(ability.damage*0.25);
            }
        }
    }
    class ChestChariotPlate : ChestEquipment{
        public ChestChariotPlate(){
        }

        public override void Passive(Character chara)
        {
            chara.defense += (int)(chara.defense*0.2);
        }
    }
    class ChestFeyWings : ChestEquipment{
        public ChestFeyWings(){}

        public override void OnTurnEnd(Character chara, Character monster)
        {
            this.speedMod = monster.burn;
        }
        public override void OnBattleEnd(Character chara)
        {
            this.speedMod = 0;
        }
    }
    class ChestHermitTunic : ChestEquipment{
        public ChestHermitTunic(){}

        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            user.bleed += 2;
        }
    }

    //------------------------------------------Legs----------------------------------------------------------------
    class LegsEquipment : Equipment
    {
        public LegsEquipment(){}
    }
    class LegsKingBelt : LegsEquipment{
        public LegsKingBelt(){}

        public override void Passive(Character chara)
        {
            if((chara.weapon != null && chara.weapon.tags.Contains("Beast")) || (chara.weapon2 != null && chara.weapon2.tags.Contains("Beast"))){
                chara.magic += 10;
            }
        }
    }
    class LegsTrollRags : LegsEquipment{
        public LegsTrollRags(){}

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {
            if(target.poison){
                this.strMod += 10;
            }
        }
        public override void OnTurnEnd(Character chara, Character monster)
        {
            if(this.strMod >= 10){
                this.strMod -= 10;
            }
        }
    }
    class LegsChariotBraces : LegsEquipment{
        public LegsChariotBraces(){}

        
        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            if(ability.damage <= 30){
                this.strMod++;
                this.tecMod++;
            }
        }

    }

    ////-----------------------------------------Feet----------------------------------------------------------------
    class FeetEquipment : Equipment
    {
        public FeetEquipment(){}
    }
    class FeetArcaneBoots : FeetEquipment{
        public FeetArcaneBoots(){}

        public override void Passive(Character chara)
        {
            chara.speed += (int)(chara.magic*0.1m);
        }
    }
    class FeetChariotBoots : FeetEquipment{
        public FeetChariotBoots(){}

        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            ability.damage -= 4;
        }
    }
    
    //------------------------------------------Accesories----------------------------------------------------------------
    class AccesoryEquipment : Equipment
    {
        public AccesoryEquipment(){}
    }
    class AccBabyDracoSoul : AccesoryEquipment
    {
        public AccBabyDracoSoul(){}

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {   
            if(ability is DamageAbility damageAbility){
                if(damageAbility.element == "Fire"){
                    damageAbility.damage += 3;
                }
            }
            
        }
    }
    class AccGloves : AccesoryEquipment
    {
        public AccGloves(){}

        public override void Passive(Character user)
        {
            if(user.weapon.isHeavy){
                user.precision += 2;
            }
        }
    }
    class AccMagicScarf : AccesoryEquipment{
        public AccMagicScarf(){}

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {
            if(ability is DamageAbility damageAbility){
                damageAbility.scaledPower += (int)(user.magic*0.1m);
            }
        }
    }
    class AccSnackPouch : AccesoryEquipment{
        public AccSnackPouch(){}

        public override void OnBattleEnd(Character chara)
        {
            Writer.WriteText("Snack pouch restores 5 hp", 5);
            Writer.WriteText("[Continue: press any key]", 6);
            Console.ReadKey();
            chara.currentHp += 5;
            if(chara.currentHp > chara.maxHp) chara.currentHp = chara.maxHp;
            Console.Clear();
            Program.HUD();
        }
    }
    class AccBloodlustAmulet : AccesoryEquipment{
        public AccBloodlustAmulet(){}

        public override void OnDamage(Character user, Character target, DamageAbility ability)
        {
            Writer.WriteText("Bloodlust amulet gives 2 Bleed", 22);
            Writer.WriteText("[Continue: press any key]", 23);
            target.bleed += 2;
            Writer.WriteText("                                            ", 22);
            Writer.WriteText("                         ", 23);
        }
    }
    class AccQualMagicScarf : AccesoryEquipment{
        public AccQualMagicScarf(){}

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {
            if(ability is DamageAbility damageAbility){
                damageAbility.scaledPower += (int)(user.magic*0.25m);
            }
        }
    }
    class AccFirefeyCore : AccesoryEquipment{
        public AccFirefeyCore(){}

        public override void OnDamage(Character user, Character target, DamageAbility ability)
        {
            if((target.burn < 3 || target.burnTurnCount < 3) && Program.random.Next(1, 101) <= 30){
                target.burn = 3;
                target.burnTurnCount = 3;
            }
        }
    }
    class AccRosary : AccesoryEquipment{
        public AccRosary(){}
        bool firstTurn = true;
        public override void OnTurnEnd(Character chara, Character monster)
        {
            if((monster.burn < 3 || monster.burnTurnCount < 10) && firstTurn){
                monster.burn = 3;
                monster.burnTurnCount = 10;
            }
            firstTurn = false;
        }

        public override void OnBattleEnd(Character chara)
        {
            firstTurn = true;
        }
    }
    class AccHerb : AccesoryEquipment{
        public AccHerb(){}

        bool firstTurn = true;
        public override void OnTurnEnd(Character chara, Character monster)
        {
            monster.poison = true;
            firstTurn = false;
        }

        public override void OnBattleEnd(Character chara)
        {
            firstTurn = true;
        }
    }

    //-------------------------------------------Weapons------------------------------------------------------------------
    class WeaponEquipment : Equipment
    {
        public bool isHeavy {get; set;}
        public Ability? ability1 {get; set;}
        public Ability? ability2 {get; set;}
        
        public WeaponEquipment(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description, int price, string[] tags, bool isHeavy, Ability? ability1, Ability? ability2) :
         base(name, hpMod, mpMod, strMod, magMod, tecMod, defMod, speedMod, precMod, evaMod, rarity, description, price)
        {
            this.tags = tags;
            this.isHeavy = isHeavy;
            this.ability1 = ability1;
            this.ability2 = ability2;
        }
        
        public WeaponEquipment(){}
        
    }
    class WeaponBloodSword : WeaponEquipment{
        public WeaponBloodSword(){}

        public override void OnBattleEnd(Character chara)
        {
            this.strMod += 4;
        }
    }



    //------------------------------------------Arcana----------------------------------------------------------------
    class Arcana : Equipment
    {
        public Arcana(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description, int price) :
         base(name, hpMod, mpMod, strMod, magMod, tecMod, defMod, speedMod, precMod, evaMod, rarity, description, price){}
        public Arcana(){}
    }
    class ArcanaTheMagician : Arcana{
        public ArcanaTheMagician(){}

        public override void Passive(Character chara)
        {
            int magDif = chara.magic - chara.strength;
            chara.magic += magDif;
        }
    }
    class ArcanaTheChariot : Arcana{
        public ArcanaTheChariot(){}

        int charge;

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {
            if(user.speed >= target.speed && ability is DamageAbility damageAbility){
                this.strMod += (user.speed - target.speed)/2;
            }
        }
        public override void OnTurnEnd(Character chara, Character monster)
        {
            if(this.strMod > 0){
                this.strMod = 0;
            }
        }
        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            charge++;
            this.speedMod++;
        }

        public override void OnBattleEnd(Character chara)
        {
            this.speedMod -= charge;
            this.charge = 0;
        }
    }
    class ArcanaTheHermit : Arcana{
        public ArcanaTheHermit(){}

        public override void Passive(Character chara)
        {
            if(chara.gold <= 10){
                chara.magic += (int)(chara.magic*0.3m);
            }
        }
    }
}
/*
bool used = false;
        public override void OnTakingDamage(Character user, Character target, DamageAbility ability)
        {
            if(ability.damage >= target.currentHp && !used){
                ability.damage = 0;
                target.currentHp = 20;
                used = true;
            }
        }


        public override void OnBattleEnd(Character chara)
        {
            used = false;
        }
*/