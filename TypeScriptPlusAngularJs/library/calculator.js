/// <reference path="jquery.d.ts"/>
$(function () {
    $("label.operator").click(function () {
        $("label.operator").removeClass("operator-selected");
        $(this).addClass("operator-selected");
    });
    $("label.operator").first().click();
});

function CalculatorController($scope) {
    $scope.number1 = 0;
    $scope.number2 = 0;
    $scope.operator = "+";

    $scope.result = function () {
        var operation = getOperation($scope.operator);
        var number1 = parseInt($scope.number1) || 0;
        var number2 = parseInt($scope.number2) || 0;

        return operation.calculate(number1, number2);
    };
}

function getOperation(operator) {
    switch (operator) {
        case "+":
            return new AddOperation();
        case "-":
            return new SubtractOperation();
        case "*":
            return new MultiplyOperation();
        case "/":
            return new DivideOperaration();
    }
}

var AddOperation = (function () {
    function AddOperation() {
    }
    AddOperation.prototype.calculate = function (number1, number2) {
        return number1 + number2;
    };
    return AddOperation;
})();
var SubtractOperation = (function () {
    function SubtractOperation() {
    }
    SubtractOperation.prototype.calculate = function (number1, number2) {
        return number1 - number2;
    };
    return SubtractOperation;
})();
var MultiplyOperation = (function () {
    function MultiplyOperation() {
    }
    MultiplyOperation.prototype.calculate = function (number1, number2) {
        return number1 * number2;
    };
    return MultiplyOperation;
})();
var DivideOperaration = (function () {
    function DivideOperaration() {
    }
    DivideOperaration.prototype.calculate = function (number1, number2) {
        return number1 / (number2 || 1);
    };
    return DivideOperaration;
})();
