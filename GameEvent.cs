using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Collections;

namespace projectTower
{
    class GameEvent
    {
        public string eventDescription {get; set;}

        public GameEvent(string eventDescription){
            this.eventDescription = eventDescription;
        }

        public virtual void Start(){
            Console.Clear();
            Program.HUD();
            Writer.WriteText(eventDescription, 5);
            Writer.WriteText("[Continue: type 'next']", 13);
        }

        
    }

    class MonsterEvent : GameEvent
    {   
        public Character monster;

        public MonsterEvent(string eventDescription, Character monster) : base(eventDescription)
            {this.monster = monster;}

        public override void Start()
        {
            Console.Clear();
            monster.UpdateStats();
            monster.currentHp = monster.maxHp;
            monster.currentMp = monster.maxMp;
            Program.HUD();
            Program.ValidInputs.Remove("desc"); Program.ValidInputs.Remove("equip"); Program.ValidInputs.Remove("rdesc");
            Program.ValidInputs.Remove("r1"); Program.ValidInputs.Remove("r2"); Program.ValidInputs.Remove("r3"); 
            Program.ValidInputs.Add("a1"); Program.ValidInputs.Add("a2"); 
            if(Program.chara.weapon2 != null){
                Program.ValidInputs.Add("a3"); Program.ValidInputs.Add("a4");
            }

            Fight();
            WinScreen();
            
            //activate OnBattleEnd abilities---------------------------------------------------------
            Console.Clear();
            Program.HUD();
            foreach (Relic relic in Program.chara.relics)
            {
                relic.OnBattleEnd(Program.chara);
            }
            
            
            if(Program.chara.headEquip != null) Program.chara.headEquip.OnBattleEnd(Program.chara);
            if (Program.chara.chestEquip != null) Program.chara.chestEquip.OnBattleEnd(Program.chara);
            if (Program.chara.legsEquip != null) Program.chara.legsEquip.OnBattleEnd(Program.chara);
            if (Program.chara.feetEquip != null) Program.chara.feetEquip.OnBattleEnd(Program.chara);
            if (Program.chara.accesoryEquip1 != null) Program.chara.accesoryEquip1.OnBattleEnd(Program.chara);
            if (Program.chara.accesoryEquip2 != null) Program.chara.accesoryEquip2.OnBattleEnd(Program.chara);
            if (Program.chara.weapon != null) Program.chara.weapon.OnBattleEnd(Program.chara);
            if (Program.chara.weapon2 != null) Program.chara.weapon2.OnBattleEnd(Program.chara);
            //activate OnBattleEnd abilities---------------------------------------------------------

            
            Program.values = Program.ChooseOptions();
            //change valid inputs
            Program.ValidInputs.Add("desc"); Program.ValidInputs.Add("equip"); Program.ValidInputs.Add("rdesc");
            Program.ValidInputs.Add("r1"); Program.ValidInputs.Add("r2"); Program.ValidInputs.Add("r3"); Program.ValidInputs.Remove("next");
            Program.ValidInputs.Remove("a1"); Program.ValidInputs.Remove("a2"); Program.ValidInputs.Remove("a3"); Program.ValidInputs.Remove("a4");

        }
        
        public void Fight(){
            string input = "";
            while(monster.currentHp > 0 && Program.chara.currentHp > 0){
                Console.Clear();
                Program.HUD();
                DisplayMonster();

                Writer.WriteText("[Enter command 'a1', 'a2', 'a3' or 'a4']", 9);
                //read input
                input = Console.ReadLine();
                //checking validiy-----------------------------------
                while(!Program.ValidInputs.Contains(input)){
                    Writer.WriteText("Please enter a valid command", 36);
                    Console.SetCursorPosition(0, 35);
                    input = Console.ReadLine();
                }
                //^^^^^ checking validiy-----------------------------
                
                if(Program.chara.speed >= monster.speed){
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    SwitchInput(input, 11);

                    Program.HUD();
                    DisplayMonster();
                    Writer.WriteText("[Next turn: press any key]", 16);
                    Console.ReadKey();
                    if(monster.currentHp <= 0 || Program.chara.currentHp <= 0) break;

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    ChooseEnemyAbility(18);
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    ChooseEnemyAbility(11);

                    Program.HUD();
                    DisplayMonster();
                    Writer.WriteText("[Next turn: press any key]", 16);
                    Console.ReadKey();
                    if(monster.currentHp <= 0 || Program.chara.currentHp <= 0) break;

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    SwitchInput(input, 18);
                    
                }
                Program.HUD();
                DisplayMonster();
                Writer.WriteText("[Next turn: press any key]", 23);
                Console.ReadKey();
                EndOfTurn();
                Writer.WriteText("[Next turn: press any key]", 13);
                Console.ReadKey();
                
            }
            //death
            if(Program.chara.currentHp <= 0){
                Console.Clear();
                Program.HUD();
                Writer.WriteText("You died [Damn, accept: Press any key]", 5);
                Console.ReadKey();
                Environment.Exit(0);
            }
            //win reset all status
            Program.chara.bleed = 0; Program.chara.poison = false; Program.chara.poisonStack = 0; Program.chara.burn = 0; Program.chara.burnTurnCount = 0;
            
            
        }

        public virtual void WinScreen(){
            Writer.WriteText("You defeated the enemy " + monster.name, 24);
            Equipment loot = ChooseLoot();
            int goldLoot = 0;
            switch (Program.floor)
            {
                case 0: goldLoot = Program.random.Next(1, 6); break;
                case 1: goldLoot = Program.random.Next(5, 11); break;
                case 2: goldLoot = Program.random.Next(15, 25); break;
                case 3: goldLoot = Program.random.Next(30, 51); break;
            }
            Writer.WriteText("It dropped " + loot.itemName + " and " + goldLoot + " gold", 25);
            Writer.WriteText("You earned 3 exp", 26);
            Writer.WriteText("[Pick up: press any key]", 27);
            Console.ReadKey();
            Program.chara.inventory.Add(loot);
            Program.chara.gold += goldLoot;
            Program.chara.exp += 3;
        }


        public void DisplayMonster(){
            monster.UpdateStats();
            Writer.WriteText(monster.name + " stats:", 6);
            Writer.WriteText("          ", 7);
            if(monster.currentHp < 0) monster.currentHp = 0;
            Writer.WriteText("HP: " + monster.currentHp + "/" + monster.maxHp + " Bleed: " + monster.bleed + " Poison: " + monster.poison, 7);
            Writer.WriteText("Evasion: " + monster.evasion + " Burn: " + monster.burn + " for " + monster.burnTurnCount + " turns" , 8);
        }

        public void SwitchInput(string input, int line){
            switch (input)
            {
                case "a1":
                    Program.chara.weapon.ability1.UseAbility(Program.chara, monster, line);
                break;
                case "a2":
                    Program.chara.weapon.ability2.UseAbility(Program.chara, monster, line);
                break;
                case "a3":
                    Program.chara.weapon2.ability1.UseAbility(Program.chara, monster, line);
                break;
                case "a4":
                    Program.chara.weapon2.ability2.UseAbility(Program.chara, monster, line);
                break;
            }
        }

        public void ChooseEnemyAbility(int line){
            int chosenAb; int roll = 0;
            if(monster.weapon2 == null){    
                if(monster.weapon.ability2 is NullAbility || monster.currentMp < monster.weapon.ability2.mpCost)       //just 1 weapon and w1a2 is null------------------a1
                {
                    chosenAb = 1;
                }
                else                                                                                                    //just 1 weapon and w1 has a2---------------------a1 a2
                {
                    roll = Program.random.Next(1, 3);
                    chosenAb = roll;
                }                                                                                               
            } else {
                if(monster.weapon.ability2 is NullAbility || monster.currentMp < monster.weapon.ability2.mpCost){
                    if(monster.weapon2.ability2 is NullAbility || monster.currentMp < monster.weapon2.ability2.mpCost){//2 weapons and both a2 are null-------------a1    a3 
                        if(monster.currentMp < monster.weapon2.ability1.mpCost){
                            chosenAb = 1;
                        }else{
                            roll = Program.random.Next(1, 3);
                            if (roll == 1)
                                chosenAb = 1;
                            else
                                chosenAb = 3;
                        }    
                    }else{                                                                                              //2 weapons and w1a1 is null-----------------a1    a3 a4
                        roll = Program.random.Next(1, 4);
                        if(roll == 1)
                            chosenAb = 1;
                        else if(roll == 2)
                            chosenAb = 3;
                        else
                            chosenAb = 4;
                    }
                }else{
                    if(monster.weapon2.ability2 is NullAbility || monster.currentMp < monster.weapon2.ability2.mpCost){//2 weapons and  w2a2 are null---------------a1 a2 a3 
                        roll = Program.random.Next(1, 4);
                        chosenAb = roll;
                    }else{                                                                                              //2 weapons and none is null-----------------a1 a2 a3 a4
                        roll = Program.random.Next(1, 5);
                        chosenAb = roll;
                    }
                }

            }
            switch (chosenAb)
            {
                case 1:
                    monster.weapon.ability1.UseAbility(monster, Program.chara, line);
                break;
                case 2:
                    monster.weapon.ability2.UseAbility(monster, Program.chara, line);
                break;
                case 3:
                    monster.weapon2.ability1.UseAbility(monster, Program.chara, line);
                break;
                case 4:
                    monster.weapon2.ability2.UseAbility(monster, Program.chara, line);
                break;
                
            }
        }

        public void EndOfTurn(){
            Writer.WriteText("End of turn effects: ", 5);

            //OnTurnEnd abilities
            foreach (Relic relic in Program.chara.relics)
            {
                relic.OnTurnEnd(Program.chara, monster);
            }
            if (Program.chara.headEquip != null) Program.chara.headEquip.OnTurnEnd(Program.chara, monster);
            if (Program.chara.chestEquip != null) Program.chara.chestEquip.OnTurnEnd(Program.chara, monster);
            if (Program.chara.legsEquip != null) Program.chara.legsEquip.OnTurnEnd(Program.chara, monster);
            if (Program.chara.feetEquip != null) Program.chara.feetEquip.OnTurnEnd(Program.chara, monster);
            if (Program.chara.accesoryEquip1 != null) Program.chara.accesoryEquip1.OnTurnEnd(Program.chara, monster);
            if (Program.chara.accesoryEquip2 != null) Program.chara.accesoryEquip2.OnTurnEnd(Program.chara, monster);
            if (Program.chara.weapon != null) Program.chara.weapon.OnTurnEnd(Program.chara, monster);
            if (Program.chara.weapon2 != null) Program.chara.weapon2.OnTurnEnd(Program.chara, monster);
            if (Program.chara.arcanaEquip != null) Program.chara.arcanaEquip.OnTurnEnd(Program.chara, monster);
            foreach (Relic relic in monster.relics)
            {
                relic.OnTurnEnd(monster, Program.chara);
            }
            if (monster.headEquip != null) monster.headEquip.OnTurnEnd(monster, Program.chara);
            if (monster.chestEquip != null) monster.chestEquip.OnTurnEnd(monster, Program.chara);
            if (monster.legsEquip != null) monster.legsEquip.OnTurnEnd(monster, Program.chara);
            if (monster.feetEquip != null) monster.feetEquip.OnTurnEnd(monster, Program.chara);
            if (monster.accesoryEquip1 != null) monster.accesoryEquip1.OnTurnEnd(monster, Program.chara);
            if (monster.accesoryEquip2 != null) monster.accesoryEquip2.OnTurnEnd(monster, Program.chara);
            if (monster.weapon != null) monster.weapon.OnTurnEnd(monster, Program.chara);
            if (monster.weapon2 != null) monster.weapon2.OnTurnEnd(monster, Program.chara);
            if (monster.arcanaEquip != null) monster.arcanaEquip.OnTurnEnd(monster, Program.chara);
            //OnTurnEnd abilities


            Console.Clear();
            Program.HUD();
            Writer.WriteText("End of turn effects: ", 5);
            //bleed dmg
            if(Program.chara.bleed != 0 || monster.bleed != 0){
                
                if (Program.chara.bleed != 0){
                    Writer.WriteText("                                                                                ", 7);
                    Writer.WriteText(Program.chara.name + " loses " + Program.chara.bleed + " HP due to bleeding. The wound heals a little.", 7);
                    Program.chara.currentHp -= Program.chara.bleed;
                    Program.chara.bleed--;
                }
                if(monster.bleed != 0){
                    Writer.WriteText("                                                                                ", 9);
                    Writer.WriteText(monster.name + " loses " + monster.bleed + " HP due to bleeding. The wound heals a little.", 9);
                    monster.currentHp -= monster.bleed;
                    monster.bleed--;
                }

                Writer.WriteText("[Continue: press any key]", 11);
                Console.ReadKey();
            }
            //poison dmg
            if(Program.chara.poison || monster.poison){
                if (Program.chara.poison)
                {
                    Program.chara.poisonStack++;
                    Writer.WriteText("                                                                                ", 7);
                    Writer.WriteText(Program.chara.name + " loses " + Program.chara.poisonStack + " HP due to poison. The poison worsens.", 7);
                    Program.chara.currentHp -= Program.chara.poisonStack;

                    if (Program.random.Next(1, 101) <= Program.chara.antidoteChance)
                    {
                        Writer.WriteText("The poison is cured!", 8);
                        Program.chara.poison = false;
                        Program.chara.poisonStack = 0;
                    }
                }
                if (monster.poison)
                {
                    monster.poisonStack++;
                    Writer.WriteText("                                                                                ", 9);
                    Writer.WriteText(monster.name + " loses " + monster.poisonStack + " HP due to poison. The poison worsens.", 9);
                    monster.currentHp -= monster.poisonStack;

                    if (Program.random.Next(1, 101) <= monster.antidoteChance)
                    {
                        Writer.WriteText("The poison is cured!", 10);
                        monster.poison = false;
                        monster.poisonStack = 0;
                    }
                }

                Writer.WriteText("[Continue: press any key]", 11);
                Console.ReadKey();
            }
            //burn dmg
            if(Program.chara.burn > 0 || monster.burn > 0){
                if (Program.chara.burn > 0)
                {
                    Program.chara.burnTurnCount--;
                    Writer.WriteText("                                                                                ", 7);
                    Writer.WriteText(Program.chara.name + " loses " + Program.chara.burn + " HP due to burn. "  + Program.chara.burnTurnCount + " turns remaining.", 7);
                    Program.chara.currentHp -= Program.chara.burn;

                    if (Program.chara.burnTurnCount <= 0)
                    {
                        Writer.WriteText("The burn is cured!", 8);
                        Program.chara.burn = 0;
                    }
                }
                if (monster.burn > 0)
                {
                    monster.burnTurnCount--;
                    Writer.WriteText("                                                                                ", 9);
                    Writer.WriteText(monster.name + " loses " + monster.burn + " HP due to burn. " + monster.burnTurnCount + " turns remaining.", 9);
                    monster.currentHp -= monster.burn;

                    if (monster.burnTurnCount <= 0)
                    {
                        Writer.WriteText("The burn is cured!", 8);
                        monster.burn = 0;
                    }
                }

                Writer.WriteText("[Continue: press any key]", 11);
                Console.ReadKey();
            }
            
        }

        //adds all non null equipment to list and takes a random one
        public Equipment ChooseLoot(){
            Equipment loot;
            List<Equipment> possibleLoot = new List<Equipment>();

            if(monster.headEquip != null) possibleLoot.Add(monster.headEquip);
            if(monster.chestEquip != null) possibleLoot.Add(monster.chestEquip);
            if(monster.legsEquip != null) possibleLoot.Add(monster.legsEquip);
            if(monster.feetEquip != null) possibleLoot.Add(monster.feetEquip);
            if(monster.accesoryEquip1 != null) possibleLoot.Add(monster.accesoryEquip1);
            if(monster.accesoryEquip2 != null) possibleLoot.Add(monster.accesoryEquip2);
            if(monster.weapon != null) possibleLoot.Add(monster.weapon);
            if(monster.weapon2 != null) possibleLoot.Add(monster.weapon2);
            loot = possibleLoot[Program.random.Next(0, possibleLoot.Count())];
            return loot;
        }
    }

    class LootEvent : GameEvent{
        
        Equipment item1;
        Equipment? item2;
        Equipment? item3;

        public LootEvent(string eventDescription, Equipment item1, Equipment? item2, Equipment? item3) : base (eventDescription){
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
        }

        public override void Start()
        {
            Console.Clear();
            Program.HUD();
            Writer.WriteText("You encounter the following items: ", 5);
            Writer.WriteText(item1.itemName, 6);
            if(item2 != null) Writer.WriteText(item2.itemName, 7);
            if(item3 != null) Writer.WriteText(item3.itemName, 8);

            Writer.WriteText("[Pick up: press any key]", 13);
            Console.ReadKey();
            Program.chara.inventory.Add(item1);
            if(item2 != null) Program.chara.inventory.Add(item2);
            if(item3 != null) Program.chara.inventory.Add(item3);

            Program.chara.exp++;
            Program.values = Program.ChooseOptions();
        }
    }

    class ShopEvent : GameEvent{
        List<Item?> shopItems = new List<Item?>();
        public ShopEvent(string eventDescription, List<Item?> shopItems) : base(eventDescription){
            this.shopItems = shopItems;
        }

        public override void Start()
        {
            
            Console.Clear();
            Program.HUD();

            Program.ValidInputs.Remove("r1"); Program.ValidInputs.Remove("r2"); Program.ValidInputs.Remove("r3");
            Program.ValidInputs.Add("buy"); Program.ValidInputs.Add("done");
            Program.ValidInputs.Add("sell"); Program.ValidInputs.Add("sdesc");

            ShopLoop();
            
            Program.values = Program.ChooseOptions();

            Program.ValidInputs.Add("r1"); Program.ValidInputs.Add("r2"); Program.ValidInputs.Add("r3");
            Program.ValidInputs.Remove("buy"); Program.ValidInputs.Remove("done");
            Program.ValidInputs.Remove("sell"); Program.ValidInputs.Remove("sdesc");
        }       

        public void ShopLoop(){
            string input = "";
            while(input != "done"){
                Console.Clear();
                Program.HUD();
                PrintShop();

                //read input
                input = Console.ReadLine();
                Writer.WriteText("               ", 35);
                //checking validiy-----------------------------------
                while(!Program.ValidInputs.Contains(input)){
                    Writer.WriteText("Please enter a valid command", 36);
                    Console.SetCursorPosition(0, 35);
                    input = Console.ReadLine();
                    Writer.WriteText("               ", 35);
                }
                //^^^^^ checking validiy-----------------------------
                switch (input)
                {
                    case "buy":
                        Writer.WriteText("Which item would you like to buy? [Type '0'-'5']", 22);
                        //read input
                        input = Console.ReadLine(); int itemBought = 0;
                        Writer.WriteText("                   ", 35);
                        //checking validiy-----------------------------------
                        while (!Int32.TryParse(input, out itemBought) || itemBought < 0 || itemBought > 5 || shopItems[itemBought] == null)
                        {
                            Writer.WriteText("That's not an item you can buy", 36);
                            Console.SetCursorPosition(0, 35);
                            input = Console.ReadLine();
                            Writer.WriteText("                 ", 35);
                        }
                        //^^^^^ checking validiy-----------------------------
                        if(shopItems[itemBought].price * 2 > Program.chara.gold){
                            Writer.WriteText("You can't afford this [Continue: press any key]", 24);
                        } else {
                            BuyItem(itemBought);
                        }
                    break;
                    case "sell":
                        Writer.WriteText("Which item would you like to sell? [Type number in inventory]", 22);
                        //read input
                        input = Console.ReadLine(); int itemSold = 0;
                        Writer.WriteText("                         ", 35);
                        //checking validiy-----------------------------------
                        while (!Int32.TryParse(input, out itemSold) || itemSold < 0 || itemSold >= Program.chara.inventory.Count())
                        {
                            Writer.WriteText("That's not an item you can sell", 36);
                            Console.SetCursorPosition(0, 35);
                            input = Console.ReadLine();
                            Writer.WriteText("                         ", 35);
                        }
                        //^^^^^ checking validiy-----------------------------
                        SellItem(itemSold);
                    break;
                    case "equip":
                        Program.CommandEquip();
                    break;
                    case "desc":
                        Program.CommandDesc();
                    break;
                    case "rdesc":
                        Program.CommandRelicDesc();
                    break;
                    case "sdesc":
                        CommandShopDesc();
                    break;
                }
                
            }
            Program.chara.exp++;
        }

        public void PrintShop(){
            Writer.WriteText("Welcome to the shop, please take your time", 5);
            Writer.WriteText("Available items:", 6);
            for (int i = 0; i < 6; i++)
            {
                if(shopItems[i] != null)
                    Writer.WriteText(i + ": " + shopItems[i].itemName + " (" + shopItems[i].rarity + ") - " + shopItems[i].price*2 + " gold", 8+i);
            }
            Writer.WriteText("Buy an item: [type command 'buy']", 15);
            Writer.WriteText("Sell an item: [type command 'sell']", 16);
            Writer.WriteText("Equip an item: [type command 'equip']", 17);
            Writer.WriteText("See an inventory item description: [type command 'desc']", 18);
            Writer.WriteText("See a relic description: [type command 'rdesc']", 19);
            Writer.WriteText("See a shop item description: [type command 'sdesc']", 20);
        }

        public void BuyItem(int item){
            string itemName = shopItems[item].itemName;
            Program.chara.gold -= shopItems[item].price*2;
            if(shopItems[item] is Equipment shopEquip) Program.chara.inventory.Add(shopEquip);
            else if(shopItems[item] is Relic shopRelic) Program.chara.relics.Add(shopRelic);
            shopItems[item] = null;
            Console.Clear();
            Program.HUD();
            Writer.WriteText("You bought " + itemName, 5);
            Writer.WriteText("[Continue: press any key]", 7);
            Console.ReadKey();
        }

        public void SellItem(int item){
            string itemName = Program.chara.inventory[item].itemName;
            Program.chara.gold += Program.chara.inventory[item].price;
            //unequip if sold item is equipped
            if(System.Object.ReferenceEquals(Program.chara.headEquip, Program.chara.inventory[item])) Program.chara.headEquip = null;
            if(System.Object.ReferenceEquals(Program.chara.chestEquip, Program.chara.inventory[item])) Program.chara.chestEquip = null;
            if(System.Object.ReferenceEquals(Program.chara.legsEquip, Program.chara.inventory[item])) Program.chara.legsEquip = null;
            if(System.Object.ReferenceEquals(Program.chara.feetEquip, Program.chara.inventory[item])) Program.chara.feetEquip = null;
            if(System.Object.ReferenceEquals(Program.chara.accesoryEquip1, Program.chara.inventory[item])) Program.chara.accesoryEquip1 = null;
            if(System.Object.ReferenceEquals(Program.chara.accesoryEquip2, Program.chara.inventory[item])) Program.chara.accesoryEquip2 = null;
            if(System.Object.ReferenceEquals(Program.chara.weapon, Program.chara.inventory[item])) Program.chara.weapon = null;
            if(System.Object.ReferenceEquals(Program.chara.weapon2, Program.chara.inventory[item])) Program.chara.weapon2 = null;
            
            Program.chara.inventory.Remove(Program.chara.inventory[item]);
            Console.Clear();
            Program.HUD();
            Writer.WriteText("You sold " + itemName, 5);
            Writer.WriteText("[Continue: press any key]", 7);
            Console.ReadKey();
        }

        public void CommandShopDesc(){
            Writer.WriteText("Which shop item do you wish to see?", 22);
            Writer.WriteText("[Type the number '0'-'5']", 24);

            string item = "";
            Console.SetCursorPosition(0, 35);
            item = Console.ReadLine();
            int itemNum = 0;
            while (!Int32.TryParse(item, out itemNum) || itemNum < 0 || itemNum > 5 || shopItems[itemNum] == null)
            {
                Console.SetCursorPosition(0, 35);
                Console.WriteLine("That's not an item you can see");
                item = Console.ReadLine();
            }
            Writer.WriteText(shopItems[itemNum].itemName + ":", 26);
            Writer.WriteText(shopItems[itemNum].description, 28);
            Writer.WriteText("[Continue: press any key]", 30);
            Console.ReadKey();
        }
    }

    class CampfireEvent : GameEvent{
        public CampfireEvent(string eventDescription) : base(eventDescription){}

        public override void Start()
        {
            Program.ValidInputs.Remove("r1"); Program.ValidInputs.Remove("r2"); Program.ValidInputs.Remove("r3");
            Program.ValidInputs.Remove("equip"); Program.ValidInputs.Remove("desc"); Program.ValidInputs.Remove("rdesc");
            Program.ValidInputs.Add("rest");Program.ValidInputs.Add("fullrest");Program.ValidInputs.Add("hp");Program.ValidInputs.Add("mp");
            Program.ValidInputs.Add("str");Program.ValidInputs.Add("mag");Program.ValidInputs.Add("tec");Program.ValidInputs.Add("def");Program.ValidInputs.Add("spe");
            Program.ValidInputs.Add("pre");Program.ValidInputs.Add("eva");
            CampLoop();
            Program.ChooseOptions();
            Program.ValidInputs.Add("r1"); Program.ValidInputs.Add("r2"); Program.ValidInputs.Add("r3");
            Program.ValidInputs.Add("equip"); Program.ValidInputs.Add("desc"); Program.ValidInputs.Add("rdesc");
            Program.ValidInputs.Remove("rest");Program.ValidInputs.Remove("fullrest");Program.ValidInputs.Remove("hp");Program.ValidInputs.Remove("mp");
            Program.ValidInputs.Remove("str");Program.ValidInputs.Remove("mag");Program.ValidInputs.Remove("tec");Program.ValidInputs.Remove("def");Program.ValidInputs.Remove("spe");
            Program.ValidInputs.Remove("pre");Program.ValidInputs.Remove("eva");
        }

        public void CampLoop(){
            
            Console.Clear();
            Program.HUD();
            Writer.WriteText(eventDescription, 5);
            Writer.WriteText("Rest and recover 30% of your HP and MP: ", 7);
            Writer.WriteText("[type command 'rest']", 8);
            Writer.WriteText("Rest and recover all of your HP and MP (no EXP this room):", 10);
            Writer.WriteText("[type command 'fullrest']", 11);
            Writer.WriteText("Gain +15 maxHP/maxMP: ", 13);
            Writer.WriteText("[type command 'hp'/'mp']", 14);
            Writer.WriteText("Gain +3 STR/MAG/TEC/SPE or +2 DEF: ", 16);
            Writer.WriteText("[type command 'str'/'mag'/'tec'/'def'/'spe']", 17);
            Writer.WriteText("Gain +1 PRE/EVA (no EXP this room): ", 19);
            Writer.WriteText("[type command 'pre'/'eva']", 20);

            //read input
            string input = "";
            input = Console.ReadLine();
            Writer.WriteText("               ", 35);
            //checking validiy-----------------------------------
            while (!Program.ValidInputs.Contains(input))
            {
                Writer.WriteText("Please enter a valid command", 36);
                Console.SetCursorPosition(0, 35);
                input = Console.ReadLine();
                Writer.WriteText("               ", 35);
            }
            //^^^^^ checking validiy-----------------------------

            switch (input){
                case "rest": 
                    int restoreHp = (int)(Program.chara.maxHp*0.3m);
                    int restoreMp = (int)(Program.chara.maxMp*0.3m);
                    Program.chara.currentHp += restoreHp;
                    if(Program.chara.currentHp >= Program.chara.maxHp) Program.chara.currentHp = Program.chara.maxHp;
                    Program.chara.currentMp += restoreMp;
                    if(Program.chara.currentMp >= Program.chara.maxMp) Program.chara.currentMp = Program.chara.maxMp;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You sat by the fire and restored some Hp and Mp", 5);
                    Program.chara.exp++;
                break;

                case "fullrest":
                    Program.chara.currentHp = Program.chara.maxHp;
                    Program.chara.currentMp = Program.chara.maxMp;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You set up a safe space and sleep to recover fully", 5);
                break;

                case "hp": 
                    Program.chara.basehp += 15;
                    Program.chara.currentHp += 15;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You do some stamina exercises to train your endurance, +15 max HP", 5);
                    Program.chara.exp++;
                break;

                case "mp":
                    Program.chara.basemp += 15;
                    Program.chara.currentMp += 15;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You do some meditation to train your mana, +15 max MP", 5);
                    Program.chara.exp++;
                break;
                
                case "str":
                    Program.chara.relics[0].strMod += 3;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You lift some heavy weight and improve your physique, +3 STR", 5);
                    Program.chara.exp++;
                break;
                
                case "mag":
                    Program.chara.relics[0].magMod += 3;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You read some books and improve your magical talent, +3 MAG", 5);
                    Program.chara.exp++;
                break;
                
                case "tec":
                    Program.chara.relics[0].tecMod += 3;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You train with your weapon and improve your skill with it, +3 TEC", 5);
                    Program.chara.exp++;
                break;

                case "def":
                    Program.chara.relics[0].defMod += 2;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You burn yourself with the fire and improve your resistance, +2 DEF", 5);
                    Program.chara.exp++;
                break;

                case "spe":
                    Program.chara.relics[0].speedMod += 3;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You jog for a bit and improve your quickness, +3 SPE", 5);
                    Program.chara.exp++;
                break;

                case "pre":
                    Program.chara.relics[0].precMod += 1;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You shoot at a target and improve your aim, +1 PRE", 5);
                break;

                case "eva":
                    Program.chara.relics[0].evaMod += 1;
                    Console.Clear();
                    Program.HUD();
                    Writer.WriteText("You listen to your instincts and improve your reflexes, +1 EVA", 5);
                break;
                
            }

            Writer.WriteText("Continue journey: [Press any key]", 6);
            Console.ReadKey();

        }
        }

    class BossEvent : MonsterEvent{
        public BossEvent(string eventDescription, Character monster) : base (eventDescription, monster){}

        public override void WinScreen()
        {
            Writer.WriteText("You defeated " + monster.name, 24);
            Equipment loot = ChooseLoot();
            int goldLoot = 0;
            switch (Program.floor)
            {
                case 0: goldLoot = Program.random.Next(10, 20); break;
                case 1: goldLoot = Program.random.Next(5, 11); break;
                case 2: goldLoot = Program.random.Next(15, 25); break;
                case 3: goldLoot = Program.random.Next(30, 51); break;
            }
            Writer.WriteText("You obtained " + monster.arcanaEquip + " arcana", 25);
            Writer.WriteText("It dropped " + loot.itemName + " and " + goldLoot + " gold", 26);
            Writer.WriteText("You earned 3 exp", 27);
            Writer.WriteText("[Pick up: press any key]", 28);
            Console.ReadKey();
            Program.chara.inventory.Add(monster.arcanaEquip);
            Program.chara.inventory.Add(loot);
            Program.chara.gold += goldLoot;
            Program.chara.exp += 3;
            Program.floor++; Program.room = 1;
        }
    }

    class RelicEvent : GameEvent{
        
        Relic relic;

        public RelicEvent(string eventDescription, Relic relic) : base (eventDescription){this.relic = relic;}
        

        public override void Start()
        {
            Console.Clear();
            Program.HUD();
            Writer.WriteText("You encounter the following relic: ", 5);
            Writer.WriteText(relic.itemName, 6);
            Writer.WriteText(relic.description, 7);

            Writer.WriteText("[Pick up: press any key]", 13);
            Console.ReadKey();
            Program.chara.relics.Add(relic);

            Program.chara.exp++;
            Program.values = Program.ChooseOptions();
        }
    }
}