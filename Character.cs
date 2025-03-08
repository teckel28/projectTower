using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace projectTower{

    class Character
    {
        //variables
        public string name {get; set;}
        public int level {get; set;}
        public int exp {get; set;} = 0;
        public int gold {get; set;} = 0;

        public int? race {get; set;}
        public string? raceString {get; set;}

        public int? pcClass {get; set;}
        public string? classString{get; set;}

        public int basehp = 70;
        public int maxHp {get; set;}
        public int currentHp {get; set;}
        public int basemp = 70;
        public int maxMp {get; set;}
        public int currentMp {get; set;}

        public int strength {get; set;}
        public int magic {get; set;}
        public int technique {get; set;}

        public int defense{get; set;}
        public int speed {get; set;}

        public int precision {get; set;}
        public int evasion {get; set;}

        public List<Relic> relics = new List<Relic>();
        public List<Equipment> inventory = new List<Equipment>();

        public HeadEquipment? headEquip{get; set;} 
        public ChestEquipment? chestEquip {get; set;}
        public LegsEquipment? legsEquip {get; set;}
        public FeetEquipment? feetEquip {get; set;}
        public AccesoryEquipment? accesoryEquip1 {get; set;}
        public AccesoryEquipment? accesoryEquip2 {get; set;}
        public WeaponEquipment? weapon {get; set;}
        public WeaponEquipment? weapon2 {get; set;}
        public Arcana? arcanaEquip {get; set;}

        public int bleed {get; set;} = 0;
        public bool poison = false;
        public int poisonStack {get; set;} = 0;
        public int antidoteChance {get; set;} = 20;
        
        //constructor for PC
        public Character(string name, int pcrace, int pcclass){
            this.name = name;
            this.race = pcrace;
            switch (race)
            {
                case 1:
                this.raceString = "Elf";
                break;

                case 2:
                this.raceString = "Dwarf";
                break;
            }
            this.pcClass = pcclass;
            switch (pcClass)
            {
                case 1:
                this.classString = "Warrior";
                break;
                
                case 2:
                this.classString = "Mage";
                break;
            }
            this.level = 1;
            this.maxHp = basehp;
            this.currentHp = basehp;
            this.maxMp = basemp;
            this.currentMp = basemp;
            this.strength = 0;
            this.magic = 0;
            this.technique = 0;
            this.defense = 0;
            this.speed = 0;
            this.precision = 0;
            this.evasion = 10;
            
        }

        public Character(string name, Relic monsterSoul, HeadEquipment? head, ChestEquipment? chest, LegsEquipment? legs, FeetEquipment? feet, AccesoryEquipment? acc1, AccesoryEquipment? acc2, WeaponEquipment? w1, WeaponEquipment? w2, Arcana? arc){
            this.name = name;
            this.relics.Add(monsterSoul);
            this.headEquip = head;
            this.chestEquip = chest;
            this.legsEquip = legs;
            this.feetEquip = feet;
            this.accesoryEquip1 = acc1;
            this.accesoryEquip2 = acc2;
            this.weapon = w1;
            this.weapon2 = w2;
            this.arcanaEquip = arc;
            this.basehp = 0;
            this.basemp = 0;
        }

        

        public void UpdateStats()
        {
            this.maxHp = basehp;
            this.maxMp = basemp;
            this.strength = 0;
            this.magic = 0;
            this.technique = 0;
            this.defense = 0;
            this.speed = 0;
            this.precision = 0;
            this.evasion = 10;

            
            EquipmentStats();

            foreach (Relic relic in relics)
            {
                this.maxHp += relic.hpMod;
                this.maxMp += relic.mpMod;
                this.strength += relic.strMod;
                this.magic += relic.magMod;
                this.technique += relic.tecMod;
                this.defense += relic.defMod;
                this.speed += relic.speedMod;
                this.precision += relic.precMod;
                this.evasion += relic.evaMod;        
            }

            EquipmentPassives();

            foreach (Relic relic in relics)
            {
                relic.Passive(this);
            }

        }

        public void EquipmentStats(){
            if(headEquip != null) headEquip.GiveStats(this);
            if(chestEquip != null) chestEquip.GiveStats(this);
            if(legsEquip != null) legsEquip.GiveStats(this);
            if(feetEquip != null) feetEquip.GiveStats(this);
            if(accesoryEquip1 != null) accesoryEquip1.GiveStats(this);
            if(accesoryEquip2 != null) accesoryEquip2.GiveStats(this);
            if(weapon != null) weapon.GiveStats(this);
            if(weapon2 != null) weapon2.GiveStats(this);
            if(arcanaEquip != null) arcanaEquip.GiveStats(this);
        }

        public void EquipmentPassives(){
            if(headEquip != null) headEquip.Passive(this);
            if(chestEquip != null) chestEquip.Passive(this);
            if(legsEquip != null) legsEquip.Passive(this);
            if(feetEquip != null) feetEquip.Passive(this);
            if(accesoryEquip1 != null) accesoryEquip1.Passive(this);
            if(accesoryEquip2 != null) accesoryEquip2.Passive(this);
            if(weapon != null) weapon.Passive(this);
            if(weapon2 != null) weapon2.Passive(this);
            if(arcanaEquip != null) arcanaEquip.Passive(this);
        }

        public void Equip(Equipment equipment, int pos = 0)
        {
            switch (equipment)
            {   
                case HeadEquipment headEquipment:
                    this.headEquip = headEquipment;
                break;

                case ChestEquipment chestEquipment:
                    this.chestEquip = chestEquipment;
                break;

                case LegsEquipment legsEquipment:
                    this.legsEquip = legsEquipment;
                break;

                case FeetEquipment feetEquipment:
                    this.feetEquip = feetEquipment;
                break;

                case AccesoryEquipment accesoryEquipment:
                    if(pos == 1) {
                        this.accesoryEquip1 = accesoryEquipment;
                        if(this.accesoryEquip1 == this.accesoryEquip2) this.accesoryEquip2 = null;
                    }
                    if(pos == 2) {
                        this.accesoryEquip2 = accesoryEquipment;
                        if(this.accesoryEquip1 == this.accesoryEquip2) this.accesoryEquip1 = null;
                    }
                break;
                
                case WeaponEquipment weaponEquipment:
                    if(weaponEquipment.isHeavy) {
                        this.weapon2 = null;
                        this.weapon = weaponEquipment;
                    } //if not heavy you can equip in either hand
                    else{
                        if(pos == 1) {
                            this.weapon = weaponEquipment;
                        }
                        if(pos == 2) {
                            
                            if(weapon.isHeavy){
                                this.weapon = weaponEquipment;
                            }else this.weapon2 = weaponEquipment;
                        }
                    }
                break;

                case Arcana arcana:
                    this.arcanaEquip = arcana;
                break;
            }
        }

        //printer
        public void PrintChar(){
            Console.SetCursorPosition(80, 12);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Stats");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(70, 13);
            Console.WriteLine("-----------");
            Console.SetCursorPosition(70, 14);
            Console.WriteLine("Race: " + raceString);
            Console.SetCursorPosition(70, 15);
            Console.WriteLine("Class: " + classString);
            Console.SetCursorPosition(70, 16);
            Console.WriteLine("-----------");
            Console.SetCursorPosition(70, 17);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Strength: " + strength);
            Console.SetCursorPosition(70, 18);
            Console.WriteLine("Magic: " + magic);
            Console.SetCursorPosition(70, 19); 
            Console.WriteLine("Technique: " + technique);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(70, 20);
            Console.WriteLine("Defense: " + defense);
            Console.SetCursorPosition(70, 21);
            Console.WriteLine("Speed: " + speed);
            Console.SetCursorPosition(70, 22);
            Console.WriteLine("Precision: +" + precision);
            Console.SetCursorPosition(70, 23);
            Console.WriteLine("Evasion: " + evasion);
            Console.SetCursorPosition(70, 24);
            Console.WriteLine("-----------");
        }

        public void PrintAbility1()
        {
            Console.SetCursorPosition(70, 26);
            if(weapon == null){
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Ability 1: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(70, 27);
                Console.WriteLine("No ability");
            }else{
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Ability 1: " + weapon.ability1.abilityName);
            Console.ForegroundColor = ConsoleColor.Magenta;
            weapon.ability1.PrintAbility(70, 27, strength, magic, technique);
            }
        }
        public void PrintAbility2()
        {
            Console.SetCursorPosition(130, 26);
            if(weapon == null){
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Ability 2: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(130, 27);
                Console.WriteLine("No ability");
            }else{
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Ability 2: " + weapon.ability2.abilityName);
            Console.ForegroundColor = ConsoleColor.Magenta;
            weapon.ability2.PrintAbility(130, 27, strength, magic, technique);
            }
        }
        public void PrintAbility3()
        {
            Console.SetCursorPosition(70, 36);
            if(weapon2 == null){
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Ability 3: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(70, 37);
                Console.WriteLine("No ability");
            }else{
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Ability 3: " + weapon2.ability1.abilityName);
            Console.ForegroundColor = ConsoleColor.Magenta;
            weapon2.ability1.PrintAbility(70, 37, strength, magic, technique);
            }
        }
        public void PrintAbility4()
        {
            Console.SetCursorPosition(130, 36);
            if(weapon2 == null){
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Ability 4: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(130, 37);
                Console.WriteLine("No ability");
            }else{
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Ability 4: " + weapon2.ability2.abilityName);
            Console.ForegroundColor = ConsoleColor.Magenta;
            weapon2.ability2.PrintAbility(130, 37, strength, magic, technique);
            }
        }

    }

}