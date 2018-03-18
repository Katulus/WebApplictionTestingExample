'use strict';
var AddNodeWizard;
(function (AddNodeWizard) {
    var IndexController = /** @class */ (function () {
        function IndexController($scope, nodesService) {
            this.$scope = $scope;
            this.nodesService = nodesService;
            this.nodes = [];
            this.loadNodes();
        }
        IndexController.prototype.loadNodes = function () {
            var _this = this;
            this.nodesService.loadNodes(function (result) {
                _this.nodes = result;
            });
        };
        IndexController.prototype.deleteAll = function () {
            var _this = this;
            this.nodesService.deleteAll(function () {
                _this.nodes = [];
            });
        };
        return IndexController;
    }());
    AddNodeWizard.IndexController = IndexController;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=IndexController.js.map