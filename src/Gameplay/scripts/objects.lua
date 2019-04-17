fridge = {
    id = "fridge",
    type = "object",
    name = "fridge",
    classes = {},
    state = "closed",
    verbs = {
        open = function(obj)
             change_state(obj, "open")
        end,
        close = function(obj)
            change_state(obj, "closed")
        end,
        look_at = function(obj)
            if obj.state == "open" then
                say_line("It's an open fridge!")
            else
                say_line("It's a closed fridge!")
            end
        end
    }
}

groceries = {
    id = "groceries",
    type = "object",
    name = "bag of groceries",
    classes = { class_use_with },
    verbs = {
        look_at = function(obj)
            say_line("It's my shopping bag with groceries.")
            say_line("There's even a bottle of Info Support Awesome Sauce™ in there!")
        end,
        use_with = function(obj, other)
            if other == grocerylist then
                say_line("Yep, I've got everything on the list!")
            end
            if other == fridge then
                if fridge.state == "closed" then
                    say_line("What do you expect me to do? Throw them at the fridge door?")
                else
                    put_object(groceries, fridge.x, fridge.y - 83, park)
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
            say_line("It's my grocery list!")
            say_line("I've bought everything that's on it!")
        end,
        use_with = function(obj, other)
            if other == groceries then
                say_line("Yep, I've got everything on the list!")
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

remote = {
    id = "remote",
    type = "object",
    name = "teleportation device",
    verbs = {
        look_at = function(obj)
            say_line("It's my teleportation device.")
            say_line("Great for debugging!")
        end,
        use = function(obj)
            cutscene_test()
            -- world.teleported = true
            -- if current_room == park then
            --     change_room(ufo)
            -- else
            --     change_room(park)
            -- end
        end,
        give_to = function(obj, actor)
            -- am i being given to the right person?
            if actor == al then
                say_line("here you go")
                set_owner(obj, actor);
                say_line("thank you!", actor)
            else
                say_line("i might need this")
            end
        end
    }
}

tractorbeam = {
    id = "tractorbeam",
    type = "object",
    classes = { class_untouchable }
}

park_bench = {
    id = "park_bench",
    type = "object",
    classes = { class_untouchable }
}