using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace projectTower
{
    abstract class Item
    {
        public string itemName {get; set;}
        public int hpMod {get; set;}
        public int mpMod {get; set;}
        public int strMod {get; set;}
        public int magMod {get; set;}
        public int tecMod {get; set;}
        public int defMod {get; set;}
        public int speedMod {get; set;}
        public int precMod {get; set;}
        public int evaMod {get; set;}
        public string rarity {get; set;}
        public string description {get; set;}
        public int price {get; set;}

        public Item(){}

        public Item(string name, int hpMod, int mpMod, int strMod, int magMod, int tecMod, int defMod, int speedMod, int precMod, int evaMod, string rarity, string description)
        {
            this.itemName = name;
            this.hpMod = hpMod;
            this.mpMod = mpMod;
            this.strMod = strMod;
            this.magMod = magMod;
            this.tecMod = tecMod;
            this.defMod = defMod;
            this.speedMod = speedMod;
            this.precMod = precMod;
            this.evaMod = evaMod;
            this.rarity = rarity;
            this.description = description;
        }
        
        public virtual void Passive(Character chara){}
        public virtual void OnUseAbility(Character user, Character target, Ability ability){}
        public virtual void OnDamage(Character user, Character target, DamageAbility ability){}
        public virtual void OnTakingDamage(Character user, Character target, DamageAbility ability){}
        public virtual void OnTurnEnd(Character chara, Character monster){}
        public virtual void OnBattleEnd(Character chara){}
    }
}