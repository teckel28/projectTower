using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace projectTower
{
    class Relic : Item
    {
        public Relic(){}

        public Relic(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description) :
         base(name, hpMod, mpMod, strMod, magMod, tecMod, defMod, speedMod, precMod, evaMod, rarity, description){}

    }

    class RelicElfSoul : Relic
    {
        //10% of chara's str and magic is added to tech
        public override void Passive(Character chara){
            chara.technique += ((chara.strength + chara.magic) / 10);
        }

        public RelicElfSoul(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description) :
         base(name, hpMod, mpMod, strMod, magMod, tecMod, defMod, speedMod, precMod, evaMod, rarity, description){}
    }

    class RelicDwarfSoul : Relic
    {
        //add number of items in inventory to defense
        public override void Passive(Character chara){
            chara.defense += chara.inventory.Count();
        }

        public RelicDwarfSoul(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description) :
         base(name, hpMod, mpMod, strMod, magMod, tecMod, defMod, speedMod, precMod, evaMod, rarity, description){}
    }

    class RelicMonsterEater : Relic{
        public RelicMonsterEater(){}

        public override void OnBattleEnd(Character chara)
        {
            int hpGain = (int)(chara.defense*0.1m);
            Console.WriteLine("Monster eater gives you +" + hpGain + " maxHP", 5);
            Console.WriteLine("[Continue: press any key]", 6);
            Console.ReadKey();
            this.hpMod += hpGain;
            Console.Clear();
            Program.chara.UpdateStats();
            Program.HUD();
        }
    }

    class RelicFastKiller : Relic{
        public RelicFastKiller(){}

        public override void OnUseAbility(Character user, Character target, Ability ability)
        {
            if(user.speed >= target.speed && ability is DamageAbility damageAbility){
                int extraDmg = (int)(user.technique*0.3m);
                damageAbility.damage += extraDmg;
            }
        }
    }

}