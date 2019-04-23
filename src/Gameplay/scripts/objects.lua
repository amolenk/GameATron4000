beamcontrol = {
    id = "beamcontrol",
    type = "object",
    name = "big button",
    state = "on",
    verbs = {
        push = function(obj)
            obj.verbs.use(obj)
        end,
        use = function(obj)
            if obj.state == "on" then
                change_state(obj, "off")
            else
                change_state(obj, "on")
            end
        end
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
    classes = { class_use_with },
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
    depends_on = {
        object = fridge,
        state = "open"
    },
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

transport_glow = {
    id = "transport_glow",
    type = "object",
    classes = { class_untouchable },
    depends_on = {
        object = beamcontrol,
        state = "on"
    }
}

park_bench = {
    id = "park_bench",
    type = "object",
    classes = { class_untouchable }
}