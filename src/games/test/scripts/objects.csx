// alarm = {
//     id = "alarm",
//     type = "object",
//     classes = { class_untouchable }
// }

// beam_button = {
//     id = "beam_button",
//     type = "object",
//     name = "big button",
//     state = "on",
//     use_dir = face_back,
//     use_pos = pos_infront,
//     verbs = {
//         look_at = function(obj)
//             say_line("That's a big button!")
//         end,
//         push = function(obj)
//             obj.verbs.use(obj)
//         end,
//         use = function(obj)
//             if obj.state == "on" then
//                 say_line("* DEACTIVATING BEAM SECURITY PROTOCOL *", narrator)
//                 change_state(obj, "off")
//             else
//                 say_line("* ACTIVATING BEAM SECURITY PROTOCOL *", narrator)
//                 change_state(obj, "on")
//                 change_state(door_terminal, "closed")
//             end
//         end
//     }
// }

// beam_glow = {
//     id = "beam_glow",
//     type = "object",
//     classes = { class_untouchable },
//     depends_on = "beam_button.on",
//     z_offset = 100
// }

GameObject parkBeam = Object(nameof(parkBeam))
    .Untouchable()
    .Build();

// beam_terminal = {
//     id = "beam_terminal",
//     type = "object",
//     classes = { class_untouchable },
//     depends_on = "beam_button.on"
// }

// bottle = {
//     id = "bottle",
//     type = "object",
//     name = "bottle",
//     classes = { class_use_with },
//     depends_on = "fridge.open",
//     use_pos = pos_infront,
//     use_dir = face_back,
//     z_offset = 91,
//     verbs = {
//         give_to = function(obj, actor)
//             if actor == richard or actor == carl then
//                 walk_to(245, 420, face_back)
//                 say_line("No thanks, we're recording right now.", carl)
//                 say_line("Maybe after the show.", richard)
//             end
//         end,
//         look_at = function(obj)
//             say_line("It's a bottle with some liquid in it.")
//             say_line("'Canes Venatici Cosmic Scotch. Drink at your own risk!'")
//             face_dir(face_front)
//             say_line("Strong stuff!")
//         end,
//         pick_up = function(obj)
//             set_owner(obj, selected_actor)
//         end,
//         use_with = function(obj, other)
//             if other == cooker then
//                 if cooker.state ~= "on" then
//                     say_line("I should turn it on first.")
//                 else
//                     say_line("Ok, I guess it's practically the same thing as water.")
//                     say_line("Wow, it's boiling already!")
//                     set_owner(obj, nobody)
//                     world.cooker_boiling = true
//                 end
//             end
//         end
//     }
// }

// claw_hammer = {
//     id = "claw_hammer",
//     type = "object",
//     name = "claw hammer",
//     classes = { class_use_with },
//     use_dir = face_back,
//     use_pos = pos_infront,
//     verbs = {
//         give_to = function(obj)
//             say_line("I think I'll keep it for now.")
//         end,
//         look_at = function(obj)
//             say_line("From wikipedia:")
//             say_line("1. a hammer with one side of the head split and curved, used for extracting nails.")
//             say_line("2. a style of banjo playing in which the thumb and fingers strum or pluck the strings in a downward motion.")
//             face_dir(face_front)
//             say_line("I'd say option 1 is applicable here.")
//         end,
//         pick_up = function(obj)
//             say_line("Hey, we need that for the show!", carl);
//         end,
//         use_with = function(obj, other)
//             if other == podcast_booth then
//                 start_talking("knock_knock")
//             elseif other == crate_left or other == crate_top then
//                 say_line("The nails in this crate are stuck.")
//             elseif other == crate_right then
//                 change_state(crate_right, "open")
//                 say_line("There's something inside!")
//             end
//         end
//     }
// }

// cheese_grater = {
//     id = "cheese_grater",
//     type = "object",
//     name = "cheese grater",
//     use_dir = face_back,
//     use_pos = pos_infront,
//     verbs = {
//         give_to = function(obj, actor)
//             if actor == richard or actor == carl then
//                 walk_to(245, 420, face_back)
//                 say_line("Can you use this for the show?")
//                 say_line("What is it?", carl)
//                 say_line("It's a highly advanced alien artifact with holes edged by slightly raised cutting edges, used for grating a certain type of yellow nutritious substance.")
//                 say_line("That's just what we need for the show!", richard)
//                 say_line("We'll trade you for the claw hammer.", carl)
//                 say_line("Er, *alien* claw hammer.", carl)
//                 say_line("Sure!")
//                 set_owner(claw_hammer, selected_actor)
//                 put_object(cheese_grater, 245, 350)
//                 world.traded_hammer = true
//                 face_dir(face_front)
//             else
//                 say_line("I think I'll keep it for now.")
//             end
//         end,
//         look_at = function(obj)
//             say_line("It's an alien cheese grater.")
//         end,
//         pick_up = function(obj)
//             if world.traded_hammer then
//                 say_line("Hey, we need that for the show!", carl);
//             else
//                 set_owner(obj, selected_actor)
//             end
//         end
//     }
// }

// cooker = {
//     id = "cooker",
//     type = "object",
//     name = "cooker",
//     classes = { class_use_with },
//     state = "off",
//     use_dir = face_back,
//     use_pos = pos_infront,
//     verbs = {
//         give_to = function(obj, actor)
//             say_line("I think I'll keep it for now.")
//         end,
//         look_at = function(obj)
//             if obj.state == "smoke" then
//                 say_line("Never trust hot dogs with green smoke coming off them.")
//             elseif obj.state == "burned" then
//                 say_line("They look a bit overcooked.")
//             else
//                 say_line("It's an electrical cooker.")
//             end
//         end,
//         use_with = function(obj, other)
//             if other == countertop then
//                 put_object(obj, 1485, 327, bridge)
//             end
//         end
//     }
// }

// cooker_in_crate = {
//     id = "cooker_in_crate",
//     type = "object",
//     name = "thing in crate",
//     use_dir = face_front,
//     use_pos = pos_above,
//     depends_on = "crate_right.open",
//     verbs = {
//         look_at = function(obj)
//             say_line("It looks like some kind of cooking device.")
//         end,
//         pick_up = function(obj)
//             remove_object(cooker_in_crate)
//             set_owner(cooker, selected_actor)
//         end
//     }
// }

// countertop = {
//     id = "countertop",
//     type = "object",
//     name = "countertop",
//     use_dir = face_back,
//     use_pos = pos_infront,
//     z_offset = -1000,
//     verbs = {
//         look_at = function(obj)
//             say_line("It's a countertop, not much to add here.")
//         end
//     }
// }

// crate_left = {
//     id = "crate_left",
//     type = "object",
//     name = "left crate",
//     use_dir = face_front,
//     use_pos = pos_above,
//     verbs = {
//         look_at = function(obj)
//             say_line("It's a decidedly low-tech wooden crate.")
//         end
//     }
// }

// crate_right = {
//     id = "crate_right",
//     type = "object",
//     name = "right crate",
//     state = "closed",
//     use_dir = face_front,
//     use_pos = pos_above,
//     verbs = {
//         look_at = function(obj)
//             say_line("It's a decidedly low-tech wooden crate.")
//             if obj.state == "closed" then
//                 say_line("The label says: 'don't open until mission is complete!'")
//             end
//         end,
//         open = function(obj)
//             say_line("It's nailed shut.")
//         end
//     }
// }

// crate_right_front = {
//     id = "crate_right_front",
//     type = "object",
//     classes = { class_untouchable },
//     depends_on = "crate_right.open",
//     z_offset = 10
// }

// crate_top = {
//     id = "crate_top",
//     type = "object",
//     name = "top crate",
//     use_dir = face_front,
//     use_pos = pos_above,
//     z_offset = 50,
//     verbs = {
//         look_at = function(obj)
//             say_line("It's a decidedly low-tech wooden crate.")
//         end
//     }
// }

// door_bridge = {
//     id = "door_bridge",
//     type = "object",
//     name = "door",
//     z_offset = -1000,
//     verbs = {
//         close = function(obj)
//             say_line("It doesn't seem to close from this side.")
//         end,
//         look_at = function(obj)
//             say_line("It's the doorway to the terminal.");
//         end,
//         open = function(obj)
//             say_line("It's already open.")
//         end,
//         walk_to = function(obj)
//             change_room(terminal)
//         end
//     }
// }

// door_terminal = {
//     id = "door_terminal",
//     type = "object",
//     name = "door",
//     state = "closed",
//     use_dir = face_back,
//     verbs = {
//         close = function(obj)
//             if obj.state == "closed" then
//                 say_line("It's already closed!")
//             else
//                 change_state(obj, "closed")
//             end
//         end,
//         look_at = function(obj)
//             say_line("It looks like a fancy high-tech door.");
//         end,
//         open = function(obj)
//             if obj.state == "open" then
//                 say_line("It's already open!")
//             elseif beam_button.state == "on" then
//                 say_line("It won't budge!");
//             else
//                 change_state(obj, "open")
//             end
//         end,
//         walk_to = function(obj)
//             if (obj.state == "open") then
//                 change_room(bridge)
//             end
//         end
//     }
// }

// fridge = {
//     id = "fridge",
//     type = "object",
//     name = "fridge",
//     classes = {},
//     state = "closed",
//     use_dir = face_back,
//     z_offset = 10,
//     verbs = {
//         open = function(obj)
//              change_state(obj, "open")
//         end,
//         close = function(obj)
//             change_state(obj, "closed")
//         end,
//         look_at = function(obj)
//             if obj.state == "closed" then
//                 say_line("It's a big fridge!")
//                 say_line("The badge on the side reads 'Brrrr-a-tron 9000™'")
//                 face_dir(face_front)
//                 say_line("Never heard of it!")
//                 say_line("It's top of the line!", al)
//             elseif owned_by(groceries, selected_actor) then
//                 say_line("It's empty!")
//                 face_dir(face_front)
//                 say_line("I wonder where these guys do their shopping!")
//             else
//                 say_line("All my groceries are now in the fridge!")
//             end
//         end
//     }
// }

GameObject groceries = Object("groceries")
    .Untouchable()
    .Build();
//     classes = { class_untouchable, class_use_with },
//     verbs = {
//         give_to = function(obj, actor)
//             say_line("I think I'll keep it for now.")
//         end,
//         look_at = function(obj)
//             if owned_by(obj, selected_actor) then
//                 say_line("It's my shopping bag with groceries.")
//                 say_line("There's even a bottle of Info Support Awesome Sauce™ in there!")
//             end
//         end,
//         use_with = function(obj, other)
//             if other == grocerylist then
//                 say_line("Yep, I've got everything on the list!")
//             end
//             if other == fridge then
//                 if fridge.state == "closed" then
//                     say_line("What do you expect me to do? Throw them at the fridge door?")
//                 else
//                     put_object(groceries, fridge.x, fridge.y - 16)
//                 end
//             end
//         end
//     },
//     depends_on = "fridge.open",
//     z_offset = 30
// }

GameObject grocerylist = Object("grocerylist")
    .Named("grocery list")
    .Build();
//     classes = { class_use_with },
//     verbs = {
//         give_to = function(obj, actor)
//             say_line("I think I'll keep it for now.")
//         end,
//         look_at = function(obj)
//             if owned_by(obj, selected_actor) then
//                 say_line("It's my grocery list!")
//                 say_line("I've bought everything that's on it!")
//             else
//                 say_line("It's my old grocery list!")
//                 say_line("I guess these guys have a new to-do list now!")
//             end
//         end,
//         use_with = function(obj, other)
//             if other == groceries then
//                 say_line("Yep, I've got everything on the list!")
//             elseif other == todolist then
//                 if not world.alarm then
//                     local ian_pos = save_actor_pos(ian)
//                     local todolist_pos = save_object_pos(todolist)
//                     remove_object(obj)
//                     walk_to(750, 325, face_front, ian)
//                     say_line("Hey, what are you doing there?", ian)
//                     wait(500)
//                     restore_object_pos(todolist_pos)
//                     say_line("Er, nothing!")
//                     restore_actor_pos(ian_pos)
//                     face_dir(face_front)
//                     say_line("I wish he was more distracted.")
//                 else
//                     set_owner(todolist, selected_actor)
//                     wait(500)
//                     put_object(grocerylist, 660, 250)
//                     face_dir(face_front)
//                     say_line("Nothing better than a surprise mission change!")
//                     say_line("* KITCHEN ZONE CLEAR: ALARM DEACTIVATED *", narrator)
//                     walk_to(810, 300, face_back, ian)
//                     walk_to(990, 375, face_front, al)
//                     remove_object(alarm)
//                     remove_object(power_cord)
//                     change_state(cooker, "burned")
//                     world.alarm = false
//                 end
//             end
//         end
//     }
// }

GameObject newspaper = Object("newspaper")
    .When.Give(_ =>
    {
        SayLine("I think I'll keep it for now.");
    })
    .When.LookAt(() =>
    {
        if (newspaper.Owner == Protagonist)
        {
            SayLine("It's yesterday's paper.");
        }
        else
        {
            SayLine("It looks like an old newspaper.");
        }
    })
    .When.PickUp(() =>
    {
        newspaper.SetOwner(Protagonist);
        CutScene_BeamUp();
    })
    .Build();

GameObject newspaperHeadline = Object(nameof(newspaperHeadline))
    .Untouchable()
    .FixedToCamera()
    .Build();

// onair = {
//     id = "onair",
//     type = "object",
//     classes = { class_untouchable },
//     state = "on"
// }

// outlet_booth = {
//     id = "outlet_booth",
//     type = "object",
//     classes = { class_untouchable }
// }

// outlet_kitchen = {
//     id = "outlet_kitchen",
//     type = "object",
//     classes = { class_untouchable }
// }


GameObject parkBench = Object("park_bench")
    .Untouchable()
    .Build();

// podcast_booth = {
//     id = "podcast_booth",
//     type = "object",
//     name = "podcast booth",
//     use_dir = face_back,
//     z_offset = -400,
//     verbs = {
//         look_at = function(obj)
//             say_line("It's a podcast booth!")
//         end
//     }
// }

// power_cord = {
//     id = "power_cord",
//     type = "object",
//     name = "power cord",
//     classes = { class_use_with },
//     state = "booth",
//     use_pos = pos_center,
//     use_dir = face_back,
//     verbs = {
//         give_to = function(obj, actor)
//             say_line("I think I'll keep it for now.")
//         end,
//         look_at = function(obj)
//             say_line("Looks like the podcast booth gets it electricity from the ship.")
//             say_line("I wonder how the aliens feel about that.")
//         end,
//         pick_up = function(obj)
//             say_line("I'm sure they won't mind me borrowing this power cord.")
//             say_line("...flux capacity of this ship is enormous...", richard)
//             say_line("Hmm, interesting...", carl)
//             walk_to(435, 365, face_back)
//             wait(1000)
//             set_owner(obj, selected_actor)
//             world.booth_disconnected = true
//             wait(500)
//             for i = 0,1,4 
//             do 
//                 change_state(onair, "off")
//                 wait(200)
//                 change_state(onair, "on")
//                 wait(200)
//             end
//             change_state(onair, "off")
//             wait(1000)
//             say_line("Er...Carl...", richard)
//             say_line("Yeah?", carl)
//             say_line("Why is the microphone off?", richard)
//             say_line("Hm, seems like all power is gone!", carl)
//             say_line("Maybe the solar radiation is interfering with the ships primary capacitators.", richard)
//             walk_to(430, 405, face_front)
//         end,
//         pull = function(obj)
//             obj.verbs.pick_up(obj)
//         end,
//         use_with = function(obj, other)
//             if other == cooker then
//                 if owned_by(cooker, selected_actor) then
//                     say_line("I really should put it on a stable surface first.")
//                 else
//                     change_state(power_cord, "kitchen")
//                     put_object(power_cord, 1540, 414, bridge)
//                     change_state(cooker, "on")
//                 end
//             end
//         end
//     }
// }

// saucages = {
//     id = "saucages",
//     type = "object",
//     name = "hot dogs",
//     classes = { class_use_with },
//     use_dir = face_back,
//     use_pos = pos_infront,
//     depends_on = "fridge.open",
//     z_offset = 91,
//     verbs = {
//         look_at = function(obj)
//             say_line("'100% premium mystery meat hot dogs!'")
//             say_line("'Heat up in boiled water for 10 minutes!'")
//         end,
//         give_to = function(obj, actor)
//             if actor == al then
//                 say_line("No thanks, I've already got a package in the fridge.", al)
//                 say_line("I'm saving them to celebrate when the mission is done!", al)
//             else
//                 say_line("I think I'll keep them for now.")
//             end
//         end,
//         pick_up = function(obj)
//             set_owner(obj, selected_actor)
//         end,
//         use_with = function(obj, other)
//             if other == cooker then
//                 if not world.cooker_boiling then
//                     say_line("According to the package, I should boil some water first.")
//                 else
//                     say_line("Here goes nothing!");
//                     set_owner(obj, nobody)
//                     change_state(other, "full")
//                     wait(1500)
//                     say_line("I wonder if they're supposed to turn green...");
//                     change_state(other, "smoke")
//                     say_line("Yikes!")
//                     walk_to(1310, 425, face_front)
//                     say_line("This can't be good!")
//                     put_object(alarm, 1449, 330)
//                     say_line("* WARNING: SMOKE DETECTED IN KITCHEN ZONE *", narrator)
//                     walk_to(1490, 425, face_back, al)
//                     say_line("WHAT ARE YOU DOING TO MY HOT DOGS?", al)
//                     walk_to(1395, 425, face_back, ian)
//                     say_line("What's going on here?", ian)
//                     say_line("MY HOT DOGS!", al)
//                     say_line("I told you to wait until the mission was done!", ian)
//                     world.alarm = true
//                 end
//             end
//         end
//     }
// }

// todolist = {
//     id = "todolist",
//     type = "object",
//     name = "to-do list",
//     use_dir = face_back,
//     use_pos = pos_infront,
//     verbs = {
//         look_at = function(obj)
//             if owned_by(obj, selected_actor) then
//                 say_line("It's the alien to-do list I've replaced with my grocery list!")
//             else
//                 say_line("It's a list with names.")
//                 say_line("The title says: 'Body-snatch list'.")
//                 face_dir(face_front)
//                 say_line("YIKES!!")
//             end
//         end,
//         pick_up = function(obj)
//             if not owned_by(obj, selected_actor) then
//                 local ian_pos = save_actor_pos(ian)
//                 local todolist_pos = save_object_pos(todolist)
//                 remove_object(obj)
//                 face_dir(face_front)
//                 say_line("Got it!")
//                 if world.alarm then
//                     walk_to(860, 430, face_back, ian)
//                 else
//                     walk_to(750, 325, face_front, ian)
//                 end
//                 say_line("Hey, put that back!", ian)
//                 say_line("We need that to finish the mission!", ian)
//                 restore_actor_pos(ian_pos)
//                 face_dir(face_back)
//                 wait(500)
//                 restore_object_pos(todolist_pos)
//                 face_dir(face_front)
//                 say_line("I guess I have to think of something sneakier!")
//              end
//         end
//     }
// }