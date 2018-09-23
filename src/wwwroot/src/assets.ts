/// <reference path="../../node_modules/phaser/typescript/phaser.d.ts" />

export class Assets {

    public static preload(game: Phaser.Game) {

        game.load.atlas("verbs", "../assets/verbs.png", "../assets/verbs.json");        

        // Room backgrounds
        game.load.image("room-park", "../assets/backgrounds/park.png");
        game.load.image("room-ufo", "../assets/backgrounds/ufo.png");
        game.load.image("room-beach", "../assets/backgrounds/beach.png");
        
        // Room objects
        game.load.image("object-fridge-closed", "../assets/objects/fridge-closed.png");
        game.load.image("object-fridge-open-empty", "../assets/objects/fridge-open-empty.png");
        game.load.image("object-fridge-open-full", "../assets/objects/fridge-open-full.png");
        game.load.image("object-grocerylist", "../assets/objects/grocerylist.png");
        game.load.image("object-newspaper", "../assets/objects/newspaper.png");
        game.load.image("object-todolist", "../assets/objects/todolist.png");
        game.load.image("object-tractorbeam", "../assets/objects/tractorbeam.png");
        
        // Inventory items
        game.load.image("inventory-newspaper", "../assets/inventory/newspaper.png");
        game.load.image("inventory-groceries", "../assets/inventory/groceries.png");
        game.load.image("inventory-todolist", "../assets/inventory/todolist.png");
        game.load.image("inventory-grocerylist", "../assets/inventory/grocerylist.png");
        
        // Closeups
        game.load.image("closeup-newspaper", "../assets/closeups/newspaper.png");
        
        // Actors
        game.load.spritesheet("actor-player", "../assets/actors/guyscotthrie-talk.png", 69, 141);
        game.load.spritesheet("actor-player-walk", "../assets/actors/guyscotthrie-walk.png", 96, 144);
        game.load.image("actor-player-back", "../assets/actors/guyscotthrie-back.png");

        game.load.spritesheet("actor-al", "../assets/actors/al-talk.png", 66, 147);
        game.load.spritesheet("actor-al-walk", "../assets/actors/al-walk.png", 96, 150);
        game.load.image("actor-al-back", "../assets/actors/al-back.png");
        
        game.load.spritesheet("actor-ian", "../assets/actors/ian-talk.png", 66, 147);
        game.load.spritesheet("actor-ian-walk", "../assets/actors/ian-walk.png", 96, 150);
        game.load.image("actor-ian-back", "../assets/actors/ian-back.png");
    }
}
