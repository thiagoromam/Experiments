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

function getOperation(operator: string) : Operation {
    switch (operator) {
        case "+": return new AddOperation();
        case "-": return new SubtractOperation();
        case "*": return new MultiplyOperation();
        case "/": return new DivideOperaration();
    }
}

interface Operation {
    calculate(number1: number, number2: number): number;
}
class AddOperation implements Operation {
    calculate(number1: number, number2: number): number {
        return number1 + number2;
    }
}
class SubtractOperation implements Operation {
    calculate(number1: number, number2: number): number {
        return number1 - number2;
    }
}
class MultiplyOperation implements Operation {
    calculate(number1: number, number2: number): number {
        return number1 * number2;
    }
}
class DivideOperaration implements Operation {
    calculate(number1: number, number2: number): number {
        return number1 / (number2 || 1);
    }
}