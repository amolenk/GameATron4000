
-- this script is execute once on game startup
function initialize_game()

    select_actor(guy)

--    camera_follow(guy)

    set_owner(groceries, selected_actor)
    set_owner(grocerylist, selected_actor)
    set_owner(remote, selected_actor)

    put_object(newspaper, 410, 420, park)
    put_object(park_bench, 323, 408, park)
    put_object(fridge, 500, 340, park)    

    put_actor(al, 450, 360, ufo)
    put_actor(ian, 700, 360, ufo)

    change_room(park)

end

function cutscene_test()
    change_room(ufo)
    say_line("Cutscene!", al)
    change_room(park)
    say_line("Wow!")
end

function cutscene_beam_up()
    say_line("It's yesterday's newspaper!")
    put_object(newspaper_headline, 400, 600)
    wait(2000)
    say_line("Hmm, there seem to be a LOT of UFO sightings lately!", narrator)
    wait(1000)
    remove_object(newspaper_headline)
    say_line("What a bunch of nonsense!")
    wait(1000)
    put_object(tractorbeam, selected_actor.x, 450)
    wait(1000)
    say_line("Uh oh...")
    wait(3000)
    change_room(ufo)
end