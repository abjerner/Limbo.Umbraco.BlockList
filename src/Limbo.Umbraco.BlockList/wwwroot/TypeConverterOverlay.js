﻿angular.module("umbraco").controller("Limbo.Umbraco.BlockList.TypeConverterOverlay", function ($scope) {

    const vm = this;

    vm.close = function () {
        $scope.model.close();
    };

    vm.select = function (model) {
        $scope.model.submit(model);
    };

});