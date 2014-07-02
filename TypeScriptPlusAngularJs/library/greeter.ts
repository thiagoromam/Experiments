/// <reference path="jquery.d.ts"/>

interface Person {
    firstname: string;
    lastname: string;
}

class Student {
    fullname: string;

    constructor(public firstname, public middleinitial, public lastname) {
        this.fullname = firstname + " " + middleinitial + " " + lastname;
    }
}

function greeter(person: Person) {
    return "Hello, " + person.firstname + " " + person.lastname;
}

var user = new Student("Jane", "M.", "User");

$(function () {
    $("#userPanel").html(greeter(user));
});