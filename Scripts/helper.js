function newGame(level) {
 
    $.ajax({
        url: "/home/newgame/?Level=" + level,
        type: 'GET'
    }).done(function (partialView) {
        $("#game").html(partialView);
        $('#guess').focus();
    });
}
function validate_guess() {
    var reg = new RegExp('^\\d+$');
    var val = $('#guess').val();

    if (!val) {
        return (false);
    }

    if (!val.match(reg)) {
        $('#guess').val('');
        showAdvice($('#guess'), "Integer values only");
        return (false);
    }

    submit_guess();
}
function submit_guess() {
    try {
        var generator = {
            Level: $('#level').val(),
            Random: $('#random').val(),
            Range: $('#range').data('range'),
            Guess: $('#guess').val(),
            Guesses: $('#guesses').text()
        };

        $.ajax({
            url: "/Home/Guess",
            type: 'POST',
            cache: false,
            data: generator,
        }).done(function (partialView) {
            $("#div_guess").html(partialView);
            $('#guess').focus();
        });
    }
    catch (err) {
        captureError(err);
    }
}
function showAdvice(obj, msg) {
    $("#singleAdvice").stop(true, false).remove();
    $('<p id="singleAdvice" class="fail small">' + msg + '</p>').insertAfter(obj);
    $("#singleAdvice").delay(4000).fadeOut(500);
}

function CreateError() {
    throw new Error('Whoops!') 
}