
-- this script is execute once on game startup
function initialize_game()

    select_actor(guy)
--    camera_follow(guy)

    set_owner(groceries, selected_actor)
    set_owner(grocerylist, selected_actor)

    put_object(newspaper, 410, 420, park)
    put_object(park_bench, 323, 408, park)
    put_object(fridge, 185, 352, ufo)    
    put_object(todolist, 650, 300, ufo)

    put_actor(al, 440, 370, ufo)
    put_actor(ian, 650, 420, ufo)

    change_room(ufo)

end

-- function no_result()
--     if selected_room != intro then
--         local line = random(canned_responses)
--         say_line(line)
--     end
-- end

cutscene = {

    start = function()
        say_line("Ok, here we go!")
        change_room(beach)
        say_line("Works?")
        cutscene_next = beach_before_enter
    end,

    beach_before_enter = function()
        put_actor(guy, 400, 225, beach)
        cutscene_next = beach_after_enter
    end,

    beach_after_enter = function()
        say_line("I should be feeling sand now!")
        change_room(park)
        end_cutscene()
    end
}


function cutscene_beam_up()
    say_line("It's yesterday's newspaper!")
    put_object(newspaper_headline, 400, 600)
    wait(2000)
    say_line("Hmm, there seem to be a LOT of UFO sightings lately!", narrator)
    wait(1000)
    remove_object(newspaper_headline)
    say_line("What a bunch of nonsense!")
    wait(1000)
    put_object(tractorbeam, selected_actor.x, selected_actor.y + 10)
    wait(1000)
    say_line("Uh oh...")
    wait(1500)
    change_room(ufo)
end

function cutscene_check_list()

    local ianX = ian.x;
    local ianY = ian.y;

    say_line("Shouldn't you check the list?")
    face_dir(face_front)

    if owned_by(grocerylist, guy) then
        walk_to(todolist.x, todolist.y, face_back, ian)
        say_line("hmm, we still need to find this Hans Scottleman guy...", ian)
    else
        walk_to(grocerylist.x, grocerylist.y, face_back, ian)
        if not world.mission_changed then
            say_line("Hmm, looks like the mission parameters have changed!", ian)
            say_line("We need to collect earthly food products now!", ian)
            world.mission_changed = true;
        else
            say_line("Still looking for those earthly food products!", ian)
        end

        walk_to(fridge.x, fridge.y, face_back, ian)
        
        if fridge.state == "open" then
            say_line("Damnit Al, you left the fridge open again!", ian)
            say_line("It wasn't me!", al)
        else
            change_state(fridge, "open")
        end

        if owned_by(groceries, selected_actor) then
            say_line("Hmm, we're not quite there yet!", ian)
            change_state(fridge, "closed")
        else
            say_line("Excellent, looks like we're all set!", ian)
            change_state(fridge, "closed")
            face_dir(face_front, ian)
            say_line("Ehm...", ian)
            say_line("Why is that red shirt guy still in my spaceship?", ian)
            wait(1000)
            change_room(beach)
            return
        end

        walk_to(ianX, ianY, face_front, ian)
    end

end