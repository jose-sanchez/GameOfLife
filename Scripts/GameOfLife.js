
var stop;
var receiving = false;
var started = false;
function loop() {
    if (stop) {
        return;
    }
    setTimeout(loop, 10);

    if (!receiving) {
        receiving = true;
        NextGeneration();
    }

}
$(document).ready(function () {
    var x;
    x = $('td');

    x.click(ToggleCellVisility);
    x = $('#Board');
    x.css('visibility', 'visible');
    x = $('#BtStart');
    x.click(start);
    x = $('#BtStop');
    x.click(function () {
        stop = true;
        var y = $('#BtStart');
        y.removeAttr("disabled");
        y = $('#BtStop');
        y.attr("disabled", "disabled");
    });
    x = $('#BtReset');
    x.click(Reset);
    x = $('#BtStop');
    x.attr("disabled", "disabled");
    $("#state").html("Select in the map the initial Cells Alive and press reset to start a new game");
});
function ToggleCellVisility() {

    if (started == false) {

        var x;
        x = $(this);
        x = x.find("div");
        x.toggle();
    } 
}
function Coordinates(x, y) {
    this.X = x;
    this.Y = y;
}
function NextGeneration() {
    $("#state").html("Loading");
    $.ajax({
        url: "Main/NextGeneration",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var myObject = data;
            $.each(myObject, function (index, item) {

                var x = $("#" + item.X + "-" + item.Y);
                x = x.find("div");
                x.toggle();
            });
            receiving = false;
            if (myObject.length > 0) {

                $("#state").html("");
            }
            else {
                $("#state").html("Stable state reached,press reset to start a new game");
                stop = true;
                var y = $('#BtStart');
                y.attr("disabled", "disabled");
                y = $('#BtStop');
                y.attr("disabled", "disabled");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#state").html("Unexpected error. Please refresh the page to load again");
        }
    });
}
function start() {
    var x;
    if (!started) {

        var coor = [];
        var t;
        var parent;

        var y;
        var cell;

        t = $('.Cell').filter(':visible');
        for (index = 0; index < t.length; index++) {
            cell = t[index];
            parent = GetParents(cell);
            y = parent[0];
            x = getX(y.id);
            y = getY(y.id);

            var newcoor;
            newcoor = new Coordinates(x, y);
            coor.push(newcoor);
        }
        var message;
        $("#state").html("Loading");

        $.ajax({
            type: "POST",
            url: "Main/Start",
            data: { coor: JSON.stringify(coor) },
            dataType: 'json',
            traditional: true,
            success: function (data) {
                var myObject = data;
                if (myObject) {
                    $("#state").html(" ");
                }
                else {
                    $("#state").html("Unexpected error. Please refresh the page to load again");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $("#state").html("Unexpected error. Please refresh the page to load again");
            }
        });
        started = true;
    }
    x = $('#BtStart');
    x.attr("disabled", "disabled");
    x = $('#BtStop');
    x.removeAttr("disabled");
    stop = false;
    setTimeout(loop, 100);
}
function GetParents(e) {
    return $(e).parents();

}
function getX(str) {
    return str.split('-')[0];
}
function getY(str) {
    return str.split('-')[1];
}
function Reset() {
    stop = true;
    if (receiving) {
        setTimeout(Reset, 10);
        return;
    }
    var t;
    var x;

    $("#state").html("Select in the map the initial Cells Alive and press reset to start a new game");
    x = $('#BtStop');
    x.attr("disabled", "disabled");
    x = $('#BtStart');
    x.removeAttr("disabled");
    t = $('.Cell').filter(':visible');
    t.toggle();
    started = false;
}

            
       
