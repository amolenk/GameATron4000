
--game.initial_room = "park"

if game then
    guy = game:register_actor(PLAYER_ACTOR, "Guy Scotthrie", "white")
end
-- guy.say_hello = function()
--     guy.say_line("Hello!")
-- end


-- Deprecated
function initialize_game()

    set_initial_room("park")

    register_actor(PLAYER_ACTOR, "Guy Scotthrie", "white")
    register_actor("al", "Al", "magenta")
    register_actor("ian", "Ian", "yellow")

--  fridge = register_object("fridge", ...)
--  fridge.open = ...
--  fridge.close = ...

    register_object("fridge-closed", "fridge")
    register_object("fridge-open-empty", "empty fridge")
    register_object("fridge-open-full", "full fridge")
    register_object("groceries", "bag of groceries")
    register_object("grocerylist", "grocery list")
    register_object("newspaper", "newspaper")
    register_object("newspaper-headline", "newspaper headline")
    register_object("todolist", "to-do list")
    register_object("tractorbeam", "rays of death and transportation")

    register_inventory_item("groceries", "bag of groceries", true)
    register_inventory_item("grocerylist", "grocery list", true)
    register_inventory_item("newspaper", "newspaper")
    register_inventory_item("todolist", "to-do list")

    register_canned_response("I can't do that.")
    register_canned_response("Why?")
    register_canned_response("Hmm, better not.")
    register_canned_response("That will probably crash the game!")

    return "park"
end

-- this script is execute once on game startup
-- function startup_script()	
-- 	-- set ui colors
-- 	reset_ui()

-- 	-- set which room to start the game in 
-- 	-- (e.g. could be a "pseudo" room for title screen!)
-- 	change_room(rm_title, 1) -- iris fade

-- 	-- set initial inventory (if applicable)
-- 	-- pickup_obj(obj_switch_tent, main_actor)
-- 	-- pickup_obj(obj_switch_player, purp_tentacle)
	
-- 	-- pickup_obj(obj_bucket, main_actor)
-- 	-- obj_bucket.state = "state_closed"
	
-- 	-- set which actor the player controls by default
-- --	selected_actor = main_actor
	
-- 	-- init actor
-- --	put_at(selected_actor, 100, 48, rm_kitchen)
-- 	--put_at(selected_actor, 60, 48, rm_hall)
-- 	--put_at(selected_actor, 16, 48, rm_computer)
-- --	put_at(selected_actor, 110, 38, rm_garden)
-- --	put_at(selected_actor, 110, 38, rm_library)
	
-- 	-- make camera follow player
-- 	-- (setting now, will be re-instated after cutscene)
-- --	camera_follow(selected_actor)



-- 	--room_curr = rm_title
-- 	--room_curr = rm_kitchen
-- 	--room_curr = rm_hall
-- 	--room_curr = rm_computer
-- --	room_curr = rm_garden
-- 	--room_curr = rm_library
-- end
