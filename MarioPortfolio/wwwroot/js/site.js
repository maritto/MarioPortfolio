// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(window).on("load", ScaleWith1To1Ratio);
$(window).on("resize", ScaleWith1To1Ratio);
$(window).on("load", ScaleFontSize);
$(window).on("resize", ScaleFontSize);

var isPlayerTurn = true;
var fontSize = 0.25;
var TicTacToeMap = [,];

function ScaleWith1To1Ratio() {
    $(".scale-height-1to1").each(function () { $(".scale-height-1to1").height($(this).width()) });
}

function ScaleFontSize() {
    $(".text-size").each(function () { $(this).css({ "font-size": $(this).parent().width() * fontSize }) });
}

function Play(coord) {
    if (CheckIfValid(coord)) {
        if (isPlayerTurn) {
            $("#" + coord[0] + "" + coord[1]).html("X");
            TicTacToeMap[coord] = 'X';
        } else {
            $("#" + coord[0] + "" + coord[1]).html("O");
            TicTacToeMap[coord] = 'O';
        }
        if (!ExistMovesAvailable())
            EndGame(false);
        $("#" + coord[0] + coord[1]).parent().removeClass("btn-outline-primary");
        $("#" + coord[0] + coord[1]).parent().addClass("forbidden-action");
        if (DidWeWinYet(coord))
            EndGame(true);
        isPlayerTurn = !isPlayerTurn;
    }
}

function CheckIfValid(coord) {
    if (TicTacToeMap[coord] == undefined || TicTacToeMap == undefined) {
        return true;
    }
    return false;
}

function ExistMovesAvailable() {
    for (var x = 0; x < 3; x++) {
        for (var y = 0; y < 3; y++) {
            if (TicTacToeMap[x + "," + y] == undefined) {
                return true;
            }
        }
    }
    return false;
}

function EndGame(XWin) {
    if (XWin == true)
        alert("Victory");
    else
        alert("Defeat");
}

function DidWeWinYet(coord) {
    if (CheckAbove(coord)) {
        var coordAbove = coord[1] + 1;
        if (CheckAbove(coord[0] + "," + coordAbove)) {
            return true;
        }
        var coordBelow = coord[1] - 1;
        if (CheckBelow(coord[0] + "," + coordBelow)) {
            return true;
        }
    }
}

function CheckAbove(coord) {
    var coordAbove = coord[1] + 1;
    if (TicTacToeMap[coord] == TicTacToeMap[coord[0] + "," + coordAbove]) {
        return true;
    }
    return false
}

function CheckBelow() {
    if (TicTacToeMap[coord] == TicTacToeMap[coord[0] + "," + (coord[1] - 1)]) {
        return true;
    }
    return false;
}

//Try instead using directions and just delete this garbage code. ARRRRGH