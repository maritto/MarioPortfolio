// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(window).on("load", ScaleWith1To1Ratio);
$(window).on("resize", ScaleWith1To1Ratio);
$(window).on("load", ScaleFontSize);
$(window).on("resize", ScaleFontSize);
$("#tab-easy").on("click", ActivateTabEasy);
$("#tab-not-so-easy").on("click", ActivateTabNotSoEasy);

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
    $.ajax({
        async: true,
        type: 'POST',
        dataType: 'json',
        data: { GameId: document.URL.split('/')[5] , Coordinates: coord[0] + "," + coord[1] },
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        url: "/TicTacToe/PlayerAction/",
        success: function (data) {
            if (!data)
                return;
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
    });
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
    }
    else {
        $("#result").text("O won !");
    }
    if (document.URL.split('/')[4] == "Easy") {
        $.post("/TicTacToe/GameCountEasy/", $("#user-id").data("userid"), function (data) {
            $("#game-count").text(data);
        });
        $.post("/TicTacToe/WinCountEasy/", $("#user-id").data("userid"), function (data) {
            $("#win-count").text(data);
        });
        $.post("/TicTacToe/WinRateEasy/", $("#user-id").data("userid"), function (data) {
            $("#win-rate").text(data);
        });
    } else {
        $.post("/TicTacToe/GameCount/", $("#user-id").data("userid"), function (data) {
            $("#game-count").text(data);
        });
        $.post("/TicTacToe/WinCount/", $("#user-id").data("userid"), function (data) {
            $("#win-count").text(data);
        });
        $.post("/TicTacToe/WinRate/", $("#user-id").data("userid"), function (data) {
            $("#win-rate").text(data);
        });
    }
    $("#result-screen").removeClass("hide");
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

function ActivateTabEasy() {
    if ($("#tab-easy").hasClass("active"))
        return;
    else {
        $("#tab-easy").addClass("active");
        $("#easy-leaderboards").removeClass("hide");
        $("#tab-not-so-easy").removeClass("active");
        $("#not-so-easy-leaderboards").addClass("hide");
    }
}

function ActivateTabNotSoEasy() {
    if ($("#tab-not-so-easy").hasClass("active"))
        return;
    else {
        $("#tab-not-so-easy").addClass("active");
        $("#not-so-easy-leaderboards").removeClass("hide");
        $("#tab-easy").removeClass("active");
        $("#easy-leaderboards").addClass("hide");
    }
}