angular.module("umbraco").controller("Limbo.Umbraco.BlockList.TypeConverter", function ($scope, $http, editorService) {

    // Get the base URL for the API controller
    const baseUrl = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath;

    const vm = this;

    vm.loaded = false;
    vm.models = [];

    // Get the query string of the view URL
    const urlParts = $scope.model.view.split("?");
    const urlQuery = new URLSearchParams(urlParts.length === 1 ? "" : urlParts[1]);

    // Get the "editor" parameter from the query string
    const v = urlQuery.get("v");

    vm.changed = function () {
        $scope.model.value = vm.selected ? vm.selected.alias : "";
    };

    vm.reset = function () {
        vm.selected = null;
        $scope.model.value = "";
        delete vm.notFound;
    };

    vm.add = function () {

        editorService.open({
            title: "Select type converter",
            size: "medium",
            view: `/App_Plugins/Limbo.Umbraco.BlockList/TypeConverterOverlay.html?v=${v}`,
            filter: true,
            availableItems: vm.models,
            submit: function (model) {
                vm.selected = model;
                $scope.model.value = { type: model.type };
                delete vm.notFound;
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        });

    };

    function init() {

        if (!$scope.model.value) {
            $scope.model.value = "";
        } else if (typeof $scope.model.value === "string") {
            $scope.model.value = { type: $scope.model.value };
        } else if ($scope.model.value.key) {
            $scope.model.value.type = $scope.model.value.key;
            delete $scope.model.value.key;
        } else if (!$scope.model.value.type) {
            $scope.model.value = "";
        }

        $scope.model.value.type = $scope.model.value.type.split(", Version=")[0];

        $http.get(`${baseUrl}/backoffice/Limbo/BlockList/GetTypeConverters`).then(function (response) {

            vm.loaded = true;
            vm.models = response.data;

            vm.selected = $scope.model.value ? vm.models.find(x => x.type === $scope.model.value.type) : null;

            if ($scope.model.value && !vm.selected) {
                const m = $scope.model.value.type.match(/^([a-zA-Z0-9\\.]+), ([a-zA-Z0-9\\.]+)$/);
                if (m) vm.selected = vm.models.find(x => x.key.indexOf(`${$scope.model.value.type},`) === 0);
                if (!vm.selected) vm.notFound = true;
            }

        });

    }

    init();

});