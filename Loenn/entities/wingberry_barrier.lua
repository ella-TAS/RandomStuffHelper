local barrier = {}

barrier.name = "RandomStuffHelper/WingBerryBarrier"
barrier.depth = 8999
barrier.fillColor = {1, 0, 0, 1/2}
barrier.borderColor = {0.5, 0, 0}
barrier.justification = {0.0, 0.0}
barrier.minimumSize = {8, 8}
barrier.maximumSize = {999, 999}
barrier.canResize = {true, true}
barrier.placements = {
    name = "barrier",
    data = {
        width = 16,
        height = 16
    }
}

return barrier