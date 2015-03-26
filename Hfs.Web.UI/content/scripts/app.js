(function () {
    var app = angular.module('myapp', []);

    app.directive('helloWorld', function () {
        return {
            template: '<h3>Hello World!!</h3>'
        };
    });
})();