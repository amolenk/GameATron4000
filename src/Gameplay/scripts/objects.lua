beam_glow = {
    id = "beam_glow",
    type = "object",
    classes = { class_untouchable },
    depends_on = "beamcontrol.on",
    z_offset = 100
}

beam_transportation = {
    id = "beam_transportation",
    type = "object",
    classes = { class_untouchable },
    depends_on = "beamcontrol.on"
}

beamcontrol = {
    id = "beamcontrol",
    type = "object",
    name = "big button",
    state = "on",
    use_dir = face_back,
    use_pos = pos_infront,
    verbs = {
        push = function(obj)
            obj.verbs.use(obj)
        end,
        use = function(obj)
            if obj.state == "on" then
                change_state(obj, "off")
            else
                change_state(obj, "on")
                change_state(transportation_door, "closed")
            end
        end
    }
}

bottle = {
    id = "bottle",
    type = "object",
    name = "bottle",
    depends_on = "fridge.open",
    use_pos = pos_infront,
    use_dir = face_back,
    z_offset = 91,
    verbs = {
        pick_up = function(obj)
            set_owner(obj, selected_actor)
        end
    }
}

bridge_transport_door = {
    id = "bridge_transport_door",
    type = "object",
    name = "door",
    verbs = {
        close = function(obj)
            say_line("It doesn't seem to close from this side.")
        end,
        look_at = function(obj)
            say_line("It's the doorway to the transportation room.");
        end,
        open = function(obj)
            say_line("It's already open.")
        end,
        use = function(obj)
        --walk_to = function(obj)
            change_room(transport)
        end
    }
}

claw_hammer = {
    id = "claw_hammer",
    type = "object",
    name = "claw hammer",
    classes = { class_use_with },
    use_dir = face_back,
    use_pos = pos_infront,
    verbs = {
        give_to = function(obj)
            say_line("I think I'll keep it for now.")
        end,
        look_at = function(obj)
            say_line("From wikipedia:")
            say_line("1. a hammer with one side of the head split and curved, used for extracting nails.")
            say_line("2. a style of banjo playing in which the thumb and fingers strum or pluck the strings in a downward motion.")
            face_dir(face_front)
            say_line("I'd say option 1 is applicable here.")
        end,
        pick_up = function(obj)
            say_line("Hey Richard!");
            say_line("Yeah?", richard)
            say_line("Can I take this hammer?")
            say_line("Er...", richard)
            say_line("Sure, the booth is fine as it is.", richard)
            if not owned_by(power_cord, selected_actor) then
                say_line("Aarghh, now we need to start over again!", carl)
            end
            set_owner(obj, selected_actor)
            face_dir(face_front)
        end,
        use_with = function(obj, other)
            if other == podcast_booth then
                start_talking("knock_knock")
            elseif other == crate_right then
                change_state(crate_right, "open")
            end
        end
    }
}

cooker = {
    id = "cooker",
    type = "object",
    name = "cooker",
    state = "off",
    depends_on = "crate_right.open",
    verbs = {
        pick_up = function(obj)
            set_owner(obj, selected_actor)
        end
    }
}

crate_left = {
    id = "crate_left",
    type = "object",
    name = "left crate",
    use_dir = face_front,
    use_pos = pos_above,
    verbs = {
    }
}

crate_right = {
    id = "crate_right",
    type = "object",
    name = "right crate",
    state = "closed",
    use_dir = face_front,
    use_pos = pos_above,
    verbs = {
        look_at = function(obj)
            say_line("It's a wooden crate.")
            say_line("The address label says 'Edison family'.")
            say_line("Never heard of them.")
        end,
        open = function(obj)
            say_line("It's nailed shut.")
        end
    }
}

crate_right_front = {
    id = "crate_right_front",
    type = "object",
    classes = { class_untouchable },
    depends_on = "crate_right.open",
    z_offset = 10
}

crate_top = {
    id = "crate_top",
    type = "object",
    name = "top crate",
    use_dir = face_front,
    use_pos = pos_above,
    z_offset = 50,
    verbs = {
    }
}

fridge = {
    id = "fridge",
    type = "object",
    name = "fridge",
    classes = {},
    state = "closed",
    use_dir = face_back,
    verbs = {
        open = function(obj)
             change_state(obj, "open")
        end,
        close = function(obj)
            change_state(obj, "closed")
        end,
        push = function(obj)
            start_cutscene(cutscene)
        end,
        look_at = function(obj)
            if obj.state == "closed" then
                say_line("It's a big fridge!")
                say_line("The badge on the side reads 'Brrrr-a-tron 9000™'")
                face_dir(face_front)
                say_line("Never heard of it!")
                say_line("It's top of the line!", al)
            elseif owned_by(groceries, selected_actor) then
                say_line("It's empty!")
                face_dir(face_front)
                say_line("I wonder where these guys do their shopping!")
            else
                say_line("All my groceries are now in the fridge!")
            end
        end
    }
}

groceries = {
    id = "groceries",
    type = "object",
    name = "groceries",
    classes = { class_untouchable, class_use_with },
    verbs = {
        look_at = function(obj)
            if owned_by(obj, selected_actor) then
                say_line("It's my shopping bag with groceries.")
                say_line("There's even a bottle of Info Support Awesome Sauce™ in there!")
            else
                say_line("All my groceries stay nice and cool in the fridge!")
            end
        end,
        pick_up = function(obj)
            set_owner(obj, selected_actor)
        end,
        use_with = function(obj, other)
            if other == grocerylist then
                say_line("Yep, I've got everything on the list!")
            end
            if other == fridge then
                if fridge.state == "closed" then
                    say_line("What do you expect me to do? Throw them at the fridge door?")
                else
                    put_object(groceries, fridge.x, fridge.y - 83)
                end
            end
        end
    },
    depends_on = "fridge.open",
    z_offset = 84
}

grocerylist = {
    id = "grocerylist",
    type = "object",
    name = "grocery list",
    classes = { class_use_with },
    verbs = {
        look_at = function(obj)
            if owned_by(obj, selected_actor) then
                say_line("It's my grocery list!")
                say_line("I've bought everything that's on it!")
            else
                say_line("It's my old grocery list!")
                say_line("I guess these guys have a new to-do list now!")
            end
        end,
        use_with = function(obj, other)
            if other == groceries then
                say_line("Yep, I've got everything on the list!")
            elseif other == todolist then
                set_owner(todolist, selected_actor)
                put_object(grocerylist, 650, 300)
                face_dir(face_front)
                say_line("That should do the trick!")
            end
        end
    }
}

newspaper = {
    id = "newspaper",
    type = "object",
    classes = { class_use_with },
    name = "newspaper",
    verbs = {
        look_at = function(obj)
            if owned_by(obj, selected_actor) then
                obj.verbs.open(obj);
            else
                say_line("It looks like an old newspaper!")
            end
        end,
        open = function(obj)
            if owned_by(obj, selected_actor) then
                if world.beamed_up then
                    say_line("It's a newspaper full of facts!")
                else
                    world.beamed_up = true;
                    cutscene_beam_up();
                end
            end
        end,
        pick_up = function(obj)
            if not owned_by(obj, selected_actor) then
                set_owner(obj, selected_actor)
            else
                say_line("I've already picked it up!")
            end
        end
    }
}

newspaper_headline = {
    id = "newspaper_headline",
    type = "object",
    classes = { class_fixed_to_camera, class_untouchable }
}

onair = {
    id = "onair",
    type = "object",
    classes = { class_untouchable },
    state = "on"
}

podcast_booth = {
    id = "podcast_booth",
    type = "object",
    name = "podcast booth",
    use_dir = face_back,
    z_offset = -400,
    verbs = {
        look_at = function(obj)
            say_line("It's a podcast booth!")
        end
    }
}

power_cord = {
    id = "power_cord",
    type = "object",
    name = "alien power cord",
    use_pos = pos_infront,
    use_dir = face_back,
    verbs = {
        pick_up = function(obj)
            say_line("I'm sure they won't mind me borrowing this power cord.")
            say_line("...flux capacity of this ship is enormous...", richard)
            say_line("Hmm, interesting...", carl)
            set_owner(obj, selected_actor)
            wait(500)
            change_state(onair, "off")
            wait(200)
            change_state(onair, "on")
            wait(200)
            change_state(onair, "off")
            wait(200)
            change_state(onair, "on")
            wait(100)
            change_state(onair, "off")
            wait(1000)
            say_line("Er...Carl...", richard)
            say_line("Yeah?", carl)
            say_line("Why is the microphone off?", richard)
            say_line("Aarghh, now we need to start over again!", carl)
        end
    }
}

todolist = {
    id = "todolist",
    type = "object",
    name = "to-do list",
    use_dir = face_back,
    verbs = {
        look_at = function(obj)
            if owned_by(obj, selected_actor) then
                say_line("It's the alien to-do list I've replaced with my grocery list!")
            else
                say_line("It's a list with names.")
                say_line("The title says: 'Body-snatch list'.")
                face_dir(face_front)
                say_line("YIKES, this can't be good!!")
            end
        end,
        pick_up = function(obj)
            if not owned_by(obj, selected_actor) then
                remove_object(obj)
                face_dir(face_front)
                say_line("Got it!")
                say_line("Hey, put that back!", ian)
                say_line("We need that to finish the mission!", al)
                face_dir(face_back)
                wait(500)
                put_object(obj, 650, 300)
                face_dir(face_front)
                say_line("I guess I have to think of something sneakier!")
            end
        end
    }
}

tractorbeam = {
    id = "tractorbeam",
    type = "object",
    classes = { class_untouchable }
}

transportation_door = {
    id = "transportation_door",
    type = "object",
    name = "door",
    state = "closed",
    use_dir = face_back,
    verbs = {
        close = function(obj)
            if obj.state == "closed" then
                say_line("It's already closed!")
            else
                change_state(obj, "closed")
            end
        end,
        look_at = function(obj)
            say_line("It looks like a fancy high-tech door.");
        end,
        open = function(obj)
            if obj.state == "open" then
                say_line("It's already open!")
            elseif beamcontrol.state == "on" then
                say_line("It won't budge!");
            else
                change_state(obj, "open")
            end
        end,
        walk_to = function(obj)
            if (obj.state == "open") then
                change_room(bridge)
            end
        end
    }
}

park_bench = {
    id = "park_bench",
    type = "object",
    classes = { class_untouchable }
}