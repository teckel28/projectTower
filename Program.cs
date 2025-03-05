using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace projectTower
{
   
    internal class Program
    {
        public static Random random = new Random();
        public static int floor = 0;
        public static int room = 1;
        public static Character chara;

        public static List<string[]> CSVabilities = new List<string[]>();
        public static List<string[]> CSVequips = new List<string[]>();
        public static List<string[]> CSVuncommons = new List<string[]>();
        public static List<string[]> CSVrares = new List<string[]>();
        public static List<string[]> CSVepics = new List<string[]>();
        public static List<string[]> CSVlegendaries = new List<string[]>();
        public static List<string[]> CSVmonsters = new List<string[]>();
        public static List<string[]> CSVrelicRares = new List<string[]>();
        public static List<string[]> CSVrelicEpics = new List<string[]>();
        public static List<string[]> CSVrelicLegendaries = new List<string[]>();

        public static int[] values = new int[6];
        public static string[] rarities = new string[3];

        public static List<string> ValidInputs = new List<string>(){
                "next",
                "exit",
            };

        static void Main()
        {
            SetWindow();
            Prologue();
            GameLoop();


            Console.WriteLine("Press any key to close");
            Console.ReadKey(true);
        }

        public static void SetWindow(){
            Console.SetBufferSize(190, 50);
            Console.SetWindowSize(190, 50);
        }

        public static void Prologue(){
            Console.ForegroundColor = ConsoleColor.DarkRed;
            //welcome------------------------------------------------------
            Console.WriteLine("Welcome to the Obsidian Tower, adventurer");
            Console.WriteLine("Tell me, who are you?");
            Console.ForegroundColor = ConsoleColor.White;
            string name = Console.ReadLine();
            

            //race selection-----------------------------------------------
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Select your race:");
            Console.WriteLine("1-Elf 2-Dwarf");

            Console.ForegroundColor = ConsoleColor.White;
            int pcrace; Int32.TryParse(Console.ReadLine(), out pcrace);
            while (pcrace < 1 || pcrace > 2){Console.WriteLine("Please enter a valid option"); Int32.TryParse(Console.ReadLine(), out pcrace);}

            //class selection---------------------------------------------
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Select your class:");
            Console.WriteLine("1-Warrior 2-Mage");

            Console.ForegroundColor = ConsoleColor.White;
            int pcclass; Int32.TryParse(Console.ReadLine(), out pcclass);
            while (pcclass < 1 || pcclass > 2){Console.WriteLine("Please enter a valid option"); Int32.TryParse(Console.ReadLine(), out pcclass);}


            //creation---------------------------------------------------------------------------------------------------------------------
            chara = new Character(name, pcrace, pcclass);
            switch (pcrace)
            {
                //if elf------------------------------------------------
                case 1:
                    RelicElfSoul elfsoul;
                    elfsoul = new RelicElfSoul("Elf soul", 10, 12, 5, 5, 10, 7, 5, 3, 3, "Rare", "10% of your STR and MAG are added to your TEC [+10 HP, +12 MP, +5 STR, +5 MAG, +10 TEC, +7 DEF, +5 SPE, +3 PREC, +2 EVA]");
                    chara.relics.Add(elfsoul);

                break;


                //if dwarf----------------------------------------------
                case 2:
                    RelicDwarfSoul dwarfsoul;
                    dwarfsoul = new RelicDwarfSoul("Dwarf soul", 20, 5, 10, 3, 2, 8, 1, 1, 1, "Rare", "+1 DEF per item in your inventory [+20 HP, +5 MP, +10 STR, +3 MAG, +2 TEC, +8 DEF, +1 SPE, +1 PREC, +3 EVA]");
                    chara.relics.Add(dwarfsoul);
                break;
            }
            switch (pcclass)
            {
                //if warrior-------------------------------------------
                case 1:
                    Relic warriorsoul;
                    warriorsoul = new Relic("Warrior soul", 12, 2, 10, 2, 10, 9, 4, 3, 3, "Rare", "[+12 HP, +2 MP, +10 STR, +2 MAG, +10 TEC, +9 DEF, +4 SPE, +3 PREC, +3 EVA]");
                    chara.relics.Add(warriorsoul);

                    //give sword------------------------
                    DamageAbility slash;
                    slash = new DamageAbility("Slash", "Deals little damage to the enemy", "", "", "-", 7, 0, 0, 0.5m, 0, 0.4m);
                    NullAbility nullA;
                    nullA = new NullAbility("", "", "", "", "", 0, 0, 0, 0);

                    WeaponEquipment woodenSword;
                    woodenSword = new WeaponEquipment("Wooden sword", 0, 0, 0, 0, 0, 0, 0, 0, 0, "Common", "A begginer's (not so) trusty weapon", 2, false, slash, nullA);
                    chara.inventory.Add(woodenSword);
                    chara.Equip(woodenSword, 1);

                    //give shield-----------------------
                    DamageAbility shieldBash;
                    shieldBash = new DamageAbility("Shield bash", "Easier to hit than a sword slash but not ", "as effective", "", "-", 6, 2, 0, 0.55m, 0, 0.05m);
                    WeaponEquipment woodenShield;
                    woodenShield = new WeaponEquipment("Wooden shield", 0, 0, 0, 0, 0, 1, 0, 0, 0, "Common", "Is that a... pot lid? [+1 DEF]", 2, false, shieldBash, nullA);
                    chara.inventory.Add(woodenShield);
                    chara.Equip(woodenShield, 2);

                break;
                //if mage------------------------------------------------
                case 2:
                    Relic magesoul;
                    magesoul = new Relic("Mage soul", 5, 20, 2, 18, 5, 5, 3, 3, 2, "Rare", "[+8 HP, +20 MP, +2 STR, +18 MAG, +5 TEC, +5 DEF, +3 SPE, +3 PREC, +2 EVA]");
                    chara.relics.Add(magesoul);

                    //give wand
                    DamageAbility magic;
                    magic = new DamageAbility("Magic spell", "Deals little damage to the enemy,", "scales with magic", "", "-", 5, 0, 2, 0, 0.7m, 0.2m);
                    DamageAbility fire;
                    fire = new DamageAbility("Fire blast", "More damage but really unprecise", "", "", "Fire", 10, -3, 5, 0m, 0.7m, 0.3m);
                    

                    WeaponEquipment wand;
                    wand = new WeaponEquipment("Apprentice wand", 0, 0, 0, 0, 0, 0, 0, 0, 0, "Common", "It choose you... remember the memories at the academy?", 2, false, magic, fire);
                    chara.inventory.Add(wand);
                    chara.Equip(wand, 1);
                break;
            }
            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Well then, " + chara.name + ", I wish you good luck in conquering the Obsidian Tower");
            Console.WriteLine("Press any key to start");
            Console.ReadKey(true);
            
        }   

        public static void GameLoop()
        {
            StreamReader abilitiesCSV = new StreamReader("Abilities.csv");
            StreamReader equipsCSV = new StreamReader("Equips.csv");
            StreamReader monstersCSV = new StreamReader("Monsters.csv");
            StreamReader relicsCSV = new StreamReader("Relics.csv");

            while(!abilitiesCSV.EndOfStream){
                var line = abilitiesCSV.ReadLine();
                CSVabilities.Add(line.Split(";"));
            }
            while(!equipsCSV.EndOfStream){
                var line = equipsCSV.ReadLine();
                string[] currentItem = line.Split(";");
                CSVequips.Add(currentItem);
                
                switch (currentItem[11])
                {
                    case "Uncommon":
                        CSVuncommons.Add(currentItem);
                    break;
                    case "Rare":
                        CSVrares.Add(currentItem);
                    break;
                    case "Epic":
                        CSVepics.Add(currentItem);
                    break;
                    case "Legendary":
                        CSVlegendaries.Add(currentItem);
                    break;
                    default:
                    break;
                }
            }
            while(!monstersCSV.EndOfStream){
                var line = monstersCSV.ReadLine();
                CSVmonsters.Add(line.Split(";"));
            }
            while(!relicsCSV.EndOfStream){
                var line = relicsCSV.ReadLine();
                string[] currentItem = line.Split(";");
                
                switch (currentItem[11])
                {
                    case "Rare":
                        CSVrelicRares.Add(currentItem);
                    break;
                    case "Epic":
                        CSVrelicEpics.Add(currentItem);
                    break;
                    case "Legendary":
                        CSVrelicLegendaries.Add(currentItem);
                    break;
                    default:
                    break;
                }
            }

            abilitiesCSV.Close();
            equipsCSV.Close();
            monstersCSV.Close();
            relicsCSV.Close();

            chara.UpdateStats();
            chara.currentHp = chara.maxHp;
            chara.currentMp = chara.maxMp;
            Console.Clear();
            HUD();

            GameEvent gameEvent;
            Console.ForegroundColor = ConsoleColor.White;
            gameEvent = new GameEvent(" You enter the dark Obsidian Tower, ready to face the challenge  that awaits you. You encounter yourself in a hallway dimly lit by torches in the walls and you walk foward without fear until you're faced with your first choice. Three doors await before you, which one will you take?");
            gameEvent.Start();


            //read first input
            string? input;
            input = Console.ReadLine();
            //checking validiy-----------------------------------
            while(!ValidInputs.Contains(input)){
                Console.WriteLine("Please enter a valid command");
                Console.SetCursorPosition(0, 35);
                input = Console.ReadLine();
                }
            //^^^^^ checking validiy-----------------------------

            values = ChooseOptions();
            //game loop---------------------------------------------------------------------------------------------------------------
            string room1 = ""; string room2 = ""; string room3 = "";
            ValidInputs.Remove("next"); ValidInputs.Add("r1"); ValidInputs.Add("r2"); ValidInputs.Add("r3"); ValidInputs.Add("equip"); ValidInputs.Add("desc"); ValidInputs.Add("rdesc");
            while(input != "exit")
            {   
                
                Console.Clear();
                HUD();
                GiveOptions(ref room1, ref room2, ref room3);
                
                //read input
                input = Console.ReadLine();
                //checking validiy-----------------------------------
                while(!ValidInputs.Contains(input)){
                    Writer.WriteText("Please enter a valid command", 36);
                    Console.SetCursorPosition(0, 35);
                    input = Console.ReadLine();
                }
                //^^^^^ checking validiy-----------------------------
                
                GameEvent currEvent = new GameEvent("");
                switch (input)
                {
                    case "r1":
                        room++;
                        SwitchInstance(room1, 0, ref currEvent);
                        currEvent.Start();
                    break;
                    case "r2":
                        room++;
                        SwitchInstance(room2, 1, ref currEvent);
                        currEvent.Start();
                    break;
                    case "r3":
                        room++;
                        SwitchInstance(room3, 2, ref currEvent);
                        currEvent.Start();
                    break;
                    case "equip":
                        CommandEquip();
                    break;
                    case "desc":
                        CommandDesc();
                    break;
                    case "rdesc":
                        CommandRelicDesc();
                    break;
                
                }
            }
        }

        

        public static void HUD()
        {

            chara.UpdateStats();
            
            //Name, hp, mp
            Console.SetCursorPosition(1, 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(chara.name + ", Level " + chara.level + " (" + chara.exp + "/10) "  + ", Gold: " + chara.gold);
            Console.SetCursorPosition(1, 2);
            Console.WriteLine("                   ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(1, 2);
            if(chara.currentHp < 0) chara.currentHp = 0;
            Console.WriteLine("HP: " + chara.currentHp + "/" + chara.maxHp);
            Console.SetCursorPosition(1, 3);
            Console.WriteLine("                   ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(1, 3);
            if(chara.currentMp < 0) chara.currentMp = 0;
            Console.WriteLine("MP: " + chara.currentMp + "/" + chara.maxMp);
            Console.ForegroundColor = ConsoleColor.Magenta;

            //Floor, room
            Console.SetCursorPosition(0, 39);
            Console.WriteLine("Floor " + floor + "   Room " + room);

            //gear
            PrintGear();   
            
            //Stats
            chara.PrintChar();
            

            //Relics
            Console.SetCursorPosition(110, 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Relics"); 
            Console.ForegroundColor = ConsoleColor.Magenta;
            int i = 0;
            foreach (Relic relic in chara.relics)
            {
                Console.SetCursorPosition(105, 3 + i);
                Console.WriteLine(i + ": " + relic.itemName);
                i++;
            }

            //Inventory
            Console.SetCursorPosition(140, 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Inventory");
            Console.ForegroundColor = ConsoleColor.Magenta;
            i = 0;
            foreach (Equipment item in chara.inventory)
            {
                Console.SetCursorPosition(132, 3 + i);
                Console.WriteLine(i + ": " + item.itemName + " (" + item.price + " gold)");
                i++;
            }

            //Abilities
            chara.PrintAbility1();
            chara.PrintAbility2();
            if(chara.weapon2 != null){
                chara.PrintAbility3();
                chara.PrintAbility4();
            }
            


            //Input
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 33);
            Console.WriteLine("                                                                                   ");
            Console.SetCursorPosition(0, 34);
            Console.WriteLine("Enter command");
            
        }
        
        public static void PrintGear(){
            //Equip
            string headname;
            string chestname;
            string legsname;
            string feetname;
            string acc1name;
            string acc2name;
            string weaponname;
            string weapon2name;
            string arcananame;
            //check if equip is null
            if(chara.headEquip == null) headname = "";
            else headname = chara.headEquip.itemName;

            if(chara.chestEquip == null) chestname = "";
            else chestname = chara.chestEquip.itemName;

            if(chara.legsEquip == null) legsname = "";
            else legsname = chara.legsEquip.itemName;

            if(chara.feetEquip == null) feetname = "";
            else feetname = chara.feetEquip.itemName;

            if(chara.accesoryEquip1 == null) acc1name = "";
            else acc1name = chara.accesoryEquip1.itemName;

            if(chara.accesoryEquip2 == null) acc2name = "";
            else acc2name = chara.accesoryEquip2.itemName;

            if(chara.weapon == null) weaponname = "";
            else weaponname = chara.weapon.itemName;

            if(chara.weapon2 == null) weapon2name = "";
            else weapon2name = chara.weapon2.itemName;

            if(chara.arcanaEquip == null) arcananame = "";
            else arcananame = chara.arcanaEquip.itemName;


            //displayEquip
            Console.SetCursorPosition(80, 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Equipment");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(70, 3);
            Console.WriteLine("Head: " + headname);
            Console.SetCursorPosition(70, 4);
            Console.WriteLine("Chest: " + chestname);
            Console.SetCursorPosition(70, 5);
            Console.WriteLine("Legs: " + legsname);
            Console.SetCursorPosition(70, 6);
            Console.WriteLine("Feet: " + feetname);
            Console.SetCursorPosition(70, 7);
            Console.WriteLine("Accessory1: " + acc1name);
            Console.SetCursorPosition(70, 8);
            Console.WriteLine("Accessory2: " + acc2name);
            Console.SetCursorPosition(70, 9);
            Console.WriteLine("Hand1: " + weaponname);
            Console.SetCursorPosition(70, 10);
            Console.WriteLine("Hand2: " + weapon2name);
            Console.SetCursorPosition(70, 11);
            Console.WriteLine("Arcana: " + arcananame);
        }
        
        //depending on room and floor generates an array of 6 ints
        //first 3 ints are the types of the 3 rooms: 1 - monster, 2 - loot room, 3 - shop, 4 - black market, 5 - campfire
        //last 3 are the specific event happening in the room, for example, the specific monster in case of a monster room, or the rarity of the item for a loot room
        public static int[] ChooseOptions()
        {
            
            
            //create array
            
            //select the first 3 numbers (types of rooms)
            if(floor == 0 && room < 3){
                for (int i = 0; i < 3; i++)  
                    values[i] = 1;
            }
            else if(room == 4 || room == 7){
                values[0] = 6;
                for (int i = 1; i < 3; i++){
                    int roomProbability = random.Next(1, 101);
                    if(roomProbability <= 40)//40% of a monster room
                        values[i] = 1;
                    else if(roomProbability <= 73)//33% of a loot room
                        values[i] = 2;
                    else if(roomProbability <= 96) //23% of a shop room
                        values[i] = 3;
                    else if(roomProbability <= 97) //1% of a black market
                        values[i] = 4;
                    else if(roomProbability <= 100) //3% of a reliquary
                        values[i] = 5;
                }
            } else if (room != 10){
                for (int i = 0; i < 3; i++){
                    int roomProbability = random.Next(1, 101);
                    if(roomProbability <= 40)//40% of a monster room
                        values[i] = 1;
                    else if(roomProbability <= 73)//33% of a loot room
                        values[i] = 2;
                    else if(roomProbability <= 96) //23% of a shop room
                        values[i] = 3;
                    else if(roomProbability <= 97) //1% of a black market
                        values[i] = 4;
                    else if(roomProbability <= 100) //3% of a reliquary
                        values[i] = 5;
                }
            }else{ //boss time
                for (int i = 0; i < 3; i++)
                    values[i] = 7;
            }

            

            
            
            //in case of a monster or loot room select the specific event (last 3 numbers of array)
            for (int i = 0; i < 3; i++)
            {
                switch (values[i])
                {
                    case 1://monster room
                        values[i+3] = random.Next(1, CSVmonsters.Count());
                    break;
                    case 2: //loot room
                        int lootRarity = random.Next(1, 101); 
                        switch (floor)
                        {
                            case 0://in floor 0: 90% uncommon, 10% rare
                                if(lootRarity <= 90){
                                    values[i+3] = random.Next(0, CSVuncommons.Count());
                                    rarities[i] = "Uncommon";
                                }
                                else{
                                    values[i+3] = random.Next(0, CSVrares.Count());
                                    rarities[i] = "Rare";
                                }
                            break;
                            case 1://in floor 1: 65% 2 uncommons, 30% rare, 5% epic
                                if(lootRarity <= 65){
                                    values[i+3] = random.Next(0, CSVuncommons.Count());
                                    rarities[i] = "Uncommon";
                                }
                                else if (lootRarity <= 95){
                                    values[i+3] = random.Next(0, CSVrares.Count());
                                    rarities[i] = "Rare";
                                }
                                else{
                                    values[i+3] = random.Next(0, CSVepics.Count());
                                    rarities[i] = "Epic";
                                }
                            break;
                            case 2://in floor 2: 30% 3 uncommons, 40% 2 rares, 25% epic, 5% legendary
                                if(lootRarity <= 30){
                                    values[i+3] = random.Next(0, CSVuncommons.Count());
                                    rarities[i] = "Uncommon";
                                }
                                else if (lootRarity <= 70){
                                    values[i+3] = random.Next(0, CSVrares.Count());
                                    rarities[i] = "Rare";
                                }
                                else if(lootRarity <= 95){
                                    values[i+3] = random.Next(0, CSVepics.Count());
                                    rarities[i] = "Epic";
                                }
                                else{
                                    values[i+3] = random.Next(0, CSVlegendaries.Count());
                                    rarities[i] = "Legendary";
                                }
                            break;
                            case 3://in floor 3: 40% 3 rares, 45% 2 epic, 15% legendary
                                if(lootRarity <= 49){
                                    values[i+3] = random.Next(0, CSVrares.Count());
                                    rarities[i] = "Rare";
                                }
                                else if(lootRarity <= 95){
                                    values[i+3] = random.Next(0, CSVepics.Count());
                                    rarities[i] = "Epic";
                                }
                                else{
                                    values[i+3] = random.Next(0, CSVlegendaries.Count());
                                    rarities[i] = "Legendary";
                                }
                            break;
                            
                        }
                    break;
                    case 3: //shop
                        int shopTier = random.Next(1, 101);
                        switch (floor)
                        {//(Tier 1: 2U) (Tier 2: 2U, 1R) (Tier 3: 2U, 2R) (Tier 4: 2U, 2R, 1E) (Tier 5: 2U, 3R, 1E) (Tier 6: 3R, 2E) (Tier 7: 3R, 2E, 1L) (Tier 8: 3R, 2E, 1Relic)
                            case 0://floor 0: 80% tier 1, 20% tier 2
                                if(shopTier <= 80){
                                    values[i+3] = 1;
                                }else{
                                    values[i+3] = 2;
                                }
                            break;
                            case 1://floor 1: 60% tier 2, 40% tier 3, 10% tier 4
                                if(shopTier <= 60){
                                    values[i+3] = 2;
                                } else if(shopTier <= 90){
                                    values[i+3] = 3;
                                } else {
                                    values[i+3] = 4;
                                }
                            break;
                            case 2://floor 2: 20% tier 3, 40% tier 4, 20% tier 5, 15% tier 6, 3% tier 7, 2% tier 8
                                if(shopTier <= 20){
                                    values[i+3] = 3;
                                } else if(shopTier <= 60){
                                    values[i+3] = 4;
                                } else if(shopTier <= 80){
                                    values[i+3] = 5;
                                } else if(shopTier <= 95){
                                    values[i+3] = 6;
                                } else if(shopTier <= 98){
                                    values[i+3] = 7;
                                }else{
                                    values[i+3] = 8;
                                }
                            break;
                            case 3://floor 3: 60% tier 6, 30% tier 7,  10% tier 8
                                if(shopTier <= 60){
                                    values[i+3] = 6;
                                } else if(shopTier <= 90){
                                    values[i+3] = 7;
                                }else{
                                    values[i+3] = 8;
                                }
                            break;
                        }
                    break;
                    case 4: //black market

                    break;
                    case 5: //reliquary

                    break;
                    case 6: //campfire
                        values[i+3] = 0;
                    break;
                    case 7: //boss

                    break;
                    
                    
                }
            }
        
            return values;
    
        }

        //takes the array given by ChooseOptions and prints the 3 options to the player
        //also returns by refernce in each string the type of room each door is so the main program can start the event when player selects it
        public static void GiveOptions(ref string room1, ref string room2, ref string room3){
            Writer.WriteText("You're faced with the choice between 3 doors: ", 5);
            Writer.WriteText("First door: [type command 'r1']", 7);
            room1 = PrintRoom(0);
            Writer.WriteText("Second door: [type command 'r2']", 10);
            room2 = PrintRoom(1);
            Writer.WriteText("Third door: [type command 'r3']", 13);
            room3 = PrintRoom(2);
            Writer.WriteText("Equip an item: [type command 'equip']", 17);
            Writer.WriteText("See a description: [type command 'desc']", 19);
            Writer.WriteText("Close program (no saving): [type command 'exit']", 21);
        }

        //prints the room type and also returns a string for GiveOptions() to return to the main program
        public static string PrintRoom(int op){
            string roomType = "GameEvent";
            switch (values[op])
                {
                    case 1:
                        //monster
                        roomType = "MonsterEvent";
                        Writer.WriteText("Monster room: " + CSVmonsters[values[op + 3]][0], 8+(op*3));
                        break;
                    case 2:
                        //random item
                        roomType = "LootEvent";
                        switch (floor)
                        {
                            case 0:
                                if(rarities[op] == "Uncommon"){
                                    Writer.WriteText("Loot room: get a random uncommon item", 8+(op*3));
                                }else if(rarities[op] == "Rare"){ 
                                    Writer.WriteText("Loot room: get a random rare item", 8+(op*3));
                                }
                            break;
                            case 1:
                                if(rarities[op] == "Uncommon"){
                                    Writer.WriteText("Loot room: get 2 random uncommon items", 8+(op*3));
                                }else if(rarities[op] == "Rare"){
                                    Writer.WriteText("Loot room: get a random rare item", 8+(op*3));
                                }else if(rarities[op] == "Epic"){
                                    Writer.WriteText("Loot room: get a random epic item", 8+(op*3));
                                }
                            break;
                            case 2:
                                if(rarities[op] == "Uncommon"){
                                    Writer.WriteText("Loot room: get 3 random uncommon items", 8+(op*3));
                                }else if(rarities[op] == "Rare"){
                                    Writer.WriteText("Loot room: get 2 random rare items", 8+(op*3));
                                }else if(rarities[op] == "Epic"){
                                    Writer.WriteText("Loot room: get a random epic item", 8+(op*3));
                                }else if(rarities[op] == "Legendary"){
                                    Writer.WriteText("Loot room: get a random legendary item", 8+(op*3));
                                }
                            break;
                            case 3:
                                if(rarities[op] == "Rare"){
                                    Writer.WriteText("Loot room: get 3 random rare items", 8+(op*3));
                                }else if(rarities[op] == "Epic"){
                                    Writer.WriteText("Loot room: get 2 random epic items", 8+(op*3));
                                }else if(rarities[op] == "Legendary"){
                                    Writer.WriteText("Loot room: get a random legendary item", 8+(op*3));
                                }
                            break;
                        }
                        break;
                    case 3:
                        //shop
                        roomType = "ShopEvent";
                        Writer.WriteText("Shop room: buy and sell items", 8+(op*3));
                        break;
                    case 4:
                        //black market
                        roomType = "BMEvent";
                        Writer.WriteText("Black market: get powerful items in exchange for curses [NOT AVAILABLE YET]", 8+(op*3));
                        break;
                    case 5:
                        //reliquary
                        roomType = "RelicEvent";
                        Writer.WriteText("Reliquary room: get a relic", 8+(op*3));
                        break;
                    case 6:
                        //campfire
                        roomType = "CampfireEvent";
                        Writer.WriteText("This is a campfire", 8+(op*3));
                    break;
                    case 7:
                        //boss
                        roomType = "BossEvent";
                        Writer.WriteText("This is a boss", 8+(op*3));
                    break;
                    
                }
            return roomType;
        }
        
        //switch case called when a room is selected that instantiates the room depending on its type and specific event
        public static void SwitchInstance(string room, int op, ref GameEvent currEvent){
            switch (room){
                case "MonsterEvent":
                    //take stats from monster sheet and assign it to monster values
                    string monName = CSVmonsters[values[op+3]][0];
                    int monHP = Convert.ToInt32(CSVmonsters[values[op+3]][1]);
                    int monMP = Convert.ToInt32(CSVmonsters[values[op+3]][2]);
                    int monSTR = Convert.ToInt32(CSVmonsters[values[op+3]][3]);
                    int monMAG = Convert.ToInt32(CSVmonsters[values[op+3]][4]);
                    int monTEC = Convert.ToInt32(CSVmonsters[values[op+3]][5]);
                    int monDEF = Convert.ToInt32(CSVmonsters[values[op+3]][6]);
                    int monSPE = Convert.ToInt32(CSVmonsters[values[op+3]][7]);
                    int monPRE = Convert.ToInt32(CSVmonsters[values[op+3]][8]);
                    int monEVA = Convert.ToInt32(CSVmonsters[values[op+3]][9]);
                    Relic monSoul = new Relic("Monster soul", monHP, monMP, monSTR, monMAG, monTEC, monDEF, monSPE, monPRE, monEVA, "Common", "Base stats for mon");

                    HeadEquipment? monHead = null;
                    if(CSVmonsters[values[op+3]][10] != "") monHead = (HeadEquipment)CreateEquip(values[op+3], 10);
                    ChestEquipment? monChest = null;
                    if(CSVmonsters[values[op+3]][11] != "") monChest = (ChestEquipment)CreateEquip(values[op+3], 11);
                    LegsEquipment? monLegs = null;
                    if(CSVmonsters[values[op+3]][12] != "") monLegs = (LegsEquipment)CreateEquip(values[op+3], 12);
                    FeetEquipment? monFeet = null;
                    if(CSVmonsters[values[op+3]][13] != "") monFeet = (FeetEquipment)CreateEquip(values[op+3], 13);
                    AccesoryEquipment? monAcc1 = null;
                    if(CSVmonsters[values[op+3]][14] != "") monAcc1 = (AccesoryEquipment)CreateEquip(values[op+3], 14);
                    AccesoryEquipment? monAcc2 = null;
                    if(CSVmonsters[values[op+3]][15] != "") monAcc2 = (AccesoryEquipment)CreateEquip(values[op+3], 15);
                    WeaponEquipment? monW1 = null;
                    if(CSVmonsters[values[op+3]][16] != "") monW1 = (WeaponEquipment)CreateEquip(values[op+3], 16);
                    WeaponEquipment? monW2 = null;
                    if(CSVmonsters[values[op+3]][17] != "") monW2 = (WeaponEquipment)CreateEquip(values[op+3], 17);


                    Character monster = new Character(monName, monSoul, monHead, monChest, monLegs, monFeet, monAcc1, monAcc2, monW1, monW2);
                    currEvent = new MonsterEvent("You encounter an enemy " + monName, monster); 
                break;
                case "LootEvent":
                    Equipment item1; Equipment? item2 = null; Equipment? item3 = null;
                    item1 = CreateLoot(values[op+3], rarities[op]);
                    if(rarities[op] == "Uncommon" && floor >= 1){
                        item2 = CreateLoot(random.Next(0, CSVuncommons.Count()), rarities[op]);
                        if(floor >= 2) item3 = CreateLoot(random.Next(0, CSVuncommons.Count()), rarities[op]);
                    }
                    if(rarities[op] == "Rare" && floor >= 2){
                        item2 = CreateLoot(random.Next(0, CSVrares.Count()), rarities[op]);
                        if(floor >= 3) item3 = CreateLoot(random.Next(0, CSVrares.Count()), rarities[op]);
                    }
                    if(rarities[op] == "Epic" && floor >= 3){
                        item2 = CreateLoot(random.Next(0, CSVepics.Count()), rarities[op]);
                    }
                    

                    currEvent = new LootEvent("This is a loot room", item1, item2, item3);
                break;
                case "ShopEvent":
                    List<Item?> shopItems = new List<Item?>();
                    for (int i = 0; i < 6; i++) shopItems.Add(null);
                    //(Tier 1: 2U) (Tier 2: 2U, 1R) (Tier 3: 2U, 2R) (Tier 4: 2U, 2R, 1E) (Tier 5: 2U, 3R, 1E) (Tier 6: 3R, 2E) (Tier 7: 3R, 2E, 1L) (Tier 8: 3R, 2E, 1Relic)
                    switch (values[op+3])
                    {   
                        case 8:
                            /*int relicRarity = random.Next(1, 11);
                            if(relicRarity <= 30){
                                shopItems[6] = CreateRelic(random.Next(0, CSVrelicRares.Count()), "Rare");
                            }else if(relicRarity <= 80){
                                shopItems[6] = CreateRelic(random.Next(0, CSVrelicEpics.Count()), "Epic");
                            }else{
                                shopItems[6] = CreateRelic(random.Next(0, CSVrelicLegendaries.Count()), "Legendary");
                            } temporary ---| */
                            shopItems[5] = CreateRelic(random.Next(0, CSVrelicRares.Count()), "Rare");
                        goto case 6;
                        case 7:
                            shopItems[5] = CreateLoot(random.Next(0, CSVlegendaries.Count()), "Legendary");
                        goto case 6;
                        case 6:
                            for (int i = 0; i < 3; i++)
                            {
                               shopItems[i] = CreateLoot(random.Next(0, CSVrares.Count()), "Rare"); 
                            }
                            for (int i = 3; i < 5; i++)
                            {
                               shopItems[i] = CreateLoot(random.Next(0, CSVepics.Count()), "Epic"); 
                            }
                        break;
                        case 5:
                            shopItems[5] = CreateLoot(random.Next(0, CSVrares.Count()), "Rare"); 
                        goto case 4;
                        case 4:
                            shopItems[4] = CreateLoot(random.Next(0, CSVepics.Count()), "Epic"); 
                        goto case 3;
                        case 3:
                            shopItems[3] = CreateLoot(random.Next(0, CSVrares.Count()), "Rare"); 
                        goto case 2;
                        case 2: 
                            shopItems[2] = CreateLoot(random.Next(0, CSVrares.Count()), "Rare");  
                        goto case 1;
                        case 1:
                            for (int i = 0; i < 2; i++)
                            {
                               shopItems[i] = CreateLoot(random.Next(0, CSVuncommons.Count()), "Uncommon"); 
                            }
                        break;
                    }

                    currEvent = new ShopEvent("This is a shop", shopItems);

                break;
                case "CampfireEvent":
                    currEvent = new CampfireEvent("You stop and rest at a campfire, what would you like to do?");
                break;
            }
        }

        //creates equipment for a monster
        //takes the name from the CSVmonsters list with the specified index, searches the CSVequips list for a matching name and assings the data to some ints
        //then creates an Equipment object and assigns that data to it
        public static Equipment CreateEquip(int index, int equipType){
            int equipIndex = 0; int count = 0;
            string equipName = CSVmonsters[index][equipType];
            int equipHP = 0; int equipMP = 0; int equipSTR = 0; int equipMAG = 0; int equipTEC = 0; int equipDEF = 0; int equipSPE = 0; int equipPRE = 0; int equipEVA = 0;
            string equipRarity = ""; string equipDesc = ""; string[] equipTags = {}; int equipPrice = 0;
            string equipHeavy = ""; string equipAb1 = ""; string equipAb2 = "";
            foreach (string[] item in CSVequips) //searching list
            {
                if (item[1] == equipName)
                {
                    equipIndex = count;
                    if (item[2] == "") equipHP = 0;
                    else equipHP = Convert.ToInt32(item[2]);
                    if (item[3] == "") equipMP = 0;
                    else equipMP = Convert.ToInt32(item[3]);
                    if (item[4] == "") equipSTR = 0;
                    else equipSTR = Convert.ToInt32(item[4]);
                    if (item[5] == "") equipMAG = 0;
                    else equipMAG = Convert.ToInt32(item[5]);
                    if (item[6] == "") equipTEC = 0;
                    else equipTEC = Convert.ToInt32(item[6]);
                    if (item[7] == "") equipDEF = 0;
                    else equipDEF = Convert.ToInt32(item[7]);
                    if (item[8] == "") equipSPE = 0;
                    else equipSPE = Convert.ToInt32(item[8]);
                    if (item[9] == "") equipPRE = 0;
                    else equipPRE = Convert.ToInt32(item[9]);
                    if (item[10] == "") equipEVA = 0;
                    else equipEVA = Convert.ToInt32(item[10]);
                    equipRarity = item[11];
                    equipDesc = item[12];
                    equipTags = item[13].Split(',');
                    equipPrice = Convert.ToInt32(item[14]);
                    equipHeavy = item[15];
                    equipAb1 = item[16];
                    equipAb2 = item[17];
                }
                count++;
            }

            //creating object
            string typename = ("projectTower." + CSVequips[equipIndex][0]);
            Type type = Type.GetType(typename);
            Equipment equip = (Equipment)Activator.CreateInstance(type);



            //take care of weapon type specific variables (abilities and heaviness)
            Type weapontype = Type.GetType("projectTower.WeaponEquipment");
            if (equip is WeaponEquipment weaponEquipment)
            {
                if (equipHeavy == "NO") weaponEquipment.isHeavy = false;
                else weaponEquipment.isHeavy = true;

                weaponEquipment.ability1 = CreateAbility(equipAb1);
                weaponEquipment.ability2 = CreateAbility(equipAb2);
            }


            //assign data
            equip.itemName = equipName;
            equip.hpMod = equipHP;
            equip.mpMod = equipMP;
            equip.strMod = equipSTR;
            equip.magMod = equipMAG;
            equip.tecMod = equipTEC;
            equip.defMod = equipDEF;
            equip.speedMod = equipSPE;
            equip.precMod = equipPRE;
            equip.evaMod = equipEVA;
            equip.rarity = equipRarity;
            equip.description = equipDesc;
            equip.tags = equipTags;
            equip.price = equipPrice;

            return equip;
        }

        //searches the ability name on the CSVabilities list, finds the index and assigns it the general variables for all abilites
        //creates the ability based on its type and then assigns the specific variables for certain ability types if it is of that certain type
        public static Ability CreateAbility(string abname){
            int abIndex = 0; int count = 0;
            string abElem = ""; int abMpCost = 0; decimal abSTR = 0; decimal abMAG = 0; decimal abTEC = 0;
            string abdesc1 = ""; string abdesc2 = ""; string abdesc3 = "";
            foreach (string[] item in CSVabilities)
            {
                if(abname == item[1]){
                    abIndex = count;
                    abElem = item[5];
                    if(item[6] == "") abMpCost = 0;
                    else abMpCost = Convert.ToInt32(item[6]);
                    if(item[7] == "") abSTR = 0;
                    else abSTR = Convert.ToDecimal(item[7]);
                    if(item[8] == "") abMAG = 0;
                    else abMAG = Convert.ToDecimal(item[8]);
                    if(item[9] == "") abTEC = 0;
                    else abTEC = Convert.ToDecimal(item[9]);
                    abdesc1 = item[2];
                    abdesc2 = item[3];
                    abdesc3 = item[4];
                }
                count++;
            }

            string abtype = ("projectTower." + CSVabilities[abIndex][0]);
            Ability ab = (Ability)Activator.CreateInstance(Type.GetType(abtype));

            ab.abilityName = abname;
            ab.element = abElem;
            ab.mpCost = abMpCost;
            ab.strScale = abSTR;
            ab.magScale = abMAG;
            ab.tecScale = abTEC;
            ab.description = abdesc1;
            ab.descriptionL2 = abdesc2;
            ab.descriptionL3 = abdesc3;

            if(ab is DamageAbility damageAb){
                damageAb.power = Convert.ToInt32(CSVabilities[abIndex][10]);
                if (CSVabilities[abIndex][11] == "") damageAb.abilityPrecMod = 0;
                    else damageAb.abilityPrecMod = Convert.ToInt32(CSVabilities[abIndex][11]);
            }
            if(ab is DamagePoison poisonAb){
                if (CSVabilities[abIndex][12] == "") poisonAb.poisonChance = 0;
                else poisonAb.poisonChance = Convert.ToInt32(CSVabilities[abIndex][12]);
            }


            return ab;
        }
        

        //gets item from corresponding list and creates the object from corresponding type with its data
        //CHANGE STRING[] SIZE WHEN ADDING COLUMNS TO equips.csv!!!!!!
        public static Equipment CreateLoot(int index, string rarity){
            string[] equipmentData = new string[17];
            Equipment equipment;
            switch (rarity)
            {
                case "Uncommon":
                    equipmentData = CSVuncommons[index];
                break;
                case "Rare":
                    equipmentData = CSVrares[index];
                break;
                case "Epic":
                    equipmentData = CSVepics[index];
                break;
                case "Legendary":
                    equipmentData = CSVlegendaries[index];
                break;
            }

            //creating object
            string typename = "projectTower." + equipmentData[0];
            Type type = Type.GetType(typename);
            equipment = (Equipment)Activator.CreateInstance(type);

            equipment.itemName = equipmentData[1];
            if (equipmentData[2] == "") equipment.hpMod = 0;
            else equipment.hpMod = Convert.ToInt32(equipmentData[2]);
            if (equipmentData[3] == "") equipment.mpMod = 0;
            else equipment.mpMod = Convert.ToInt32(equipmentData[3]);
            if (equipmentData[4] == "") equipment.strMod = 0;
            else equipment.strMod = Convert.ToInt32(equipmentData[4]);
            if (equipmentData[5] == "") equipment.magMod = 0;
            else equipment.magMod = Convert.ToInt32(equipmentData[5]);
            if (equipmentData[6] == "") equipment.tecMod = 0;
            else equipment.tecMod = Convert.ToInt32(equipmentData[6]);
            if (equipmentData[7] == "") equipment.defMod = 0;
            else equipment.defMod = Convert.ToInt32(equipmentData[7]);
            if (equipmentData[8] == "") equipment.speedMod = 0;
            else equipment.speedMod = Convert.ToInt32(equipmentData[8]);
            if (equipmentData[9] == "") equipment.precMod = 0;
            else equipment.precMod = Convert.ToInt32(equipmentData[9]);
            if (equipmentData[10] == "") equipment.evaMod = 0;
            else equipment.evaMod = Convert.ToInt32(equipmentData[10]);
            equipment.rarity = equipmentData[11];
            equipment.description = equipmentData[12];
            equipment.tags = equipmentData[13].Split(',');
            equipment.price = Convert.ToInt32(equipmentData[14]);
            
            Type weapontype = Type.GetType("projectTower.WeaponEquipment");
            if (equipment is WeaponEquipment weaponEquipment)
            {
                if (equipmentData[15] == "NO") weaponEquipment.isHeavy = false;
                else weaponEquipment.isHeavy = true;

                weaponEquipment.ability1 = CreateAbility(equipmentData[16]);
                weaponEquipment.ability2 = CreateAbility(equipmentData[17]);
            }

            return equipment;
        }
        
        //gets relic from corresponding list and creates the object from corresponding type with its data
        //CHANGE STRING[] SIZE WHEN ADDING COLUMNS TO relics.csv!!!!!!
        public static Relic CreateRelic(int index, string rarity){
            string[] relicData = new string[13];
            Relic relic;
            switch (rarity)
            {
                case "Rare":
                    relicData = CSVrelicRares[index];
                break;
                case "Epic":
                    relicData = CSVrelicEpics[index];
                break;
                case "Legendary":
                    relicData = CSVrelicLegendaries[index];
                break;
            }

            //creating object
            string typename = "projectTower." + relicData[0];
            Type type = Type.GetType(typename);
            relic = (Relic)Activator.CreateInstance(type);

            relic.itemName = relicData[1];
            if (relicData[2] == "") relic.hpMod = 0;
            else relic.hpMod = Convert.ToInt32(relicData[2]);
            if (relicData[3] == "") relic.mpMod = 0;
            else relic.mpMod = Convert.ToInt32(relicData[3]);
            if (relicData[4] == "") relic.strMod = 0;
            else relic.strMod = Convert.ToInt32(relicData[4]);
            if (relicData[5] == "") relic.magMod = 0;
            else relic.magMod = Convert.ToInt32(relicData[5]);
            if (relicData[6] == "") relic.tecMod = 0;
            else relic.tecMod = Convert.ToInt32(relicData[6]);
            if (relicData[7] == "") relic.defMod = 0;
            else relic.defMod = Convert.ToInt32(relicData[7]);
            if (relicData[8] == "") relic.speedMod = 0;
            else relic.speedMod = Convert.ToInt32(relicData[8]);
            if (relicData[9] == "") relic.precMod = 0;
            else relic.precMod = Convert.ToInt32(relicData[9]);
            if (relicData[10] == "") relic.evaMod = 0;
            else relic.evaMod = Convert.ToInt32(relicData[10]);
            relic.rarity = relicData[11];
            relic.description = relicData[12];
            relic.price = 0;
            

            return relic;
        }

        public static void CommandEquip(){
            Console.Clear();
            HUD();
            Writer.WriteText("Which item do you wish to equip?", 5);
            Writer.WriteText("[Type the number in your inventory of the item]", 6);

            string equip = "";
            Console.SetCursorPosition(0, 35);
            equip = Console.ReadLine();
            int equipNum = 0;
            while(!Int32.TryParse(equip, out equipNum) || equipNum >= chara.inventory.Count()){
                Console.WriteLine("Please enter a valid command");
                equip = Console.ReadLine();
            }
            int slot = 0;
            switch (chara.inventory[equipNum])
            {
                case AccesoryEquipment equipAcc:
                    Writer.WriteText("Which slot do you wish to equip to?", 8);
                    Writer.WriteText("[Type '1' or '2']", 9);
                    equip = Console.ReadLine();
                    while(!Int32.TryParse(equip, out slot) || (slot != 1 && slot != 2)){
                        Console.WriteLine("Please enter a valid command");
                        equip = Console.ReadLine();
                    }
                    chara.Equip(equipAcc, slot);
                break;
                case WeaponEquipment equipWeap:
                    Writer.WriteText("Which slot do you wish to equip to?", 8);
                    Writer.WriteText("[Type '1' or '2']", 9);
                    equip = Console.ReadLine();
                    while(!Int32.TryParse(equip, out slot) || (slot != 1 && slot != 2)){
                        Console.WriteLine("Please enter a valid command");
                        equip = Console.ReadLine();
                    }
                    chara.Equip(equipWeap, slot);
                break;
                default:
                    chara.Equip((Equipment)chara.inventory[equipNum]);
                break;
                
            }

        }

        public static void CommandDesc(){
            Console.Clear();
            HUD();
            Writer.WriteText("Which item do you wish to see?", 5);
            Writer.WriteText("[Type the number in your inventory of the item]", 6);

            string item = "";
            Console.SetCursorPosition(0, 35);
            item = Console.ReadLine();
            int itemNum = 0;
            while (!Int32.TryParse(item, out itemNum) || itemNum > chara.inventory.Count())
            {
                Console.WriteLine("Please enter a valid command");
                item = Console.ReadLine();
            }
            Writer.WriteText(chara.inventory[itemNum].itemName + ":", 8);
            Writer.WriteText(chara.inventory[itemNum].description, 9);
            Writer.WriteText("Tags: " + string.Join(", ", chara.inventory[itemNum].tags), 11);
            Writer.WriteText("[Continue: press any key]", 20);
            Console.ReadKey();
        }

        public static void CommandRelicDesc(){
            Console.Clear();
                HUD();
                Writer.WriteText("Which relic do you wish to see?", 5);
                Writer.WriteText("[Type the number in your relic list of the relic]", 6);

                string item = "";
                Console.SetCursorPosition(0, 35);
                item = Console.ReadLine();
                int itemNum = 0;
                while (!Int32.TryParse(item, out itemNum) || itemNum > chara.relics.Count())
                {
                    Console.WriteLine("Please enter a valid command");
                    item = Console.ReadLine();
                }
                Writer.WriteText(chara.relics[itemNum].itemName + ":", 8);
                Writer.WriteText(chara.relics[itemNum].description, 10);
                Writer.WriteText("[Continue: press any key]", 20);
                Console.ReadKey();
        }
    }
}