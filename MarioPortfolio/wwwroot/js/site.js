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
var gameOver = false;

function ScaleWith1To1Ratio() {
    $(".scale-height-1to1").each(function () { $(".scale-height-1to1").height($(this).width()) });
}

function ScaleFontSize() {
    $(".text-size").each(function () { $(this).css({ "font-size": $(this).parent().width() * fontSize }) });
}

function Play(coord) {
    if (gameOver == true) {
        return;
    }
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
            if (DidWeWinYet(coord)) {
                if (isPlayerTurn) {
                    EndGame(true);
                } else {
                    EndGame(false);
                }
            }
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
    gameOver = true;
    if (XWin == true) {
        $("#result").text("X won !");
        $("#result-screen").removeClass("hide");
    }
    else {
        $("#result").text("O won !");
        $("#result-screen").removeClass("hide");
    }
}

function DidWeWinYet(coord) {
    if (TicTacToeMap[coord[0] + ",0"] == TicTacToeMap[coord[0] + ",1"] && TicTacToeMap[coord[0] + ",1"] == TicTacToeMap[coord[0] + ",2"])
        return true;
    if (TicTacToeMap["0," + coord[1]] == TicTacToeMap["1," + coord[1]] && TicTacToeMap["1," + coord[1]] == TicTacToeMap["2," + coord[1]])
        return true;
    if (TicTacToeMap["1,1"] != undefined && TicTacToeMap["0,0"] == TicTacToeMap["1,1"] && TicTacToeMap["1,1"] == TicTacToeMap["2,2"])//diagonal check
        return true;
    if (TicTacToeMap["1,1"] != undefined && TicTacToeMap["0,2"] == TicTacToeMap["1,1"] && TicTacToeMap["1,1"] == TicTacToeMap["2,0"])//diagonal check
        return true;
    return false;
}