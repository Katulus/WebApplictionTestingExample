'use strict';
var AddNodeWizard;
(function (AddNodeWizard) {
    var WizardController = /** @class */ (function () {
        function WizardController($scope, $location, wizardService) {
            this.$scope = $scope;
            this.$location = $location;
            this.wizardService = wizardService;
            this.stepDefinitions = [];
            this.title = $scope['wizardTitle'];
            this.wizardServiceAddress = $scope['wizardServiceAddress'];
            this.createNewNode();
            this.reset();
            this.loadSteps();
        }
        WizardController.prototype.loadSteps = function () {
            var _this = this;
            this.wizardService.loadSteps(function (result) {
                _this.stepDefinitions = result;
                _this.reset();
            }, function (error) {
                _this.setError(error);
            });
        };
        WizardController.prototype.back = function () {
            var _this = this;
            this.setError('');
            if (this.cantGoBack)
                return;
            this.wizardService.back(function (result) {
                if (result.CanTransition) {
                    _this.currentStepIndex--;
                    _this.refresh();
                }
                else {
                    _this.setError(result.ErrorMessage);
                }
            }, function (error) {
                _this.setError(error);
            });
        };
        WizardController.prototype.next = function () {
            var _this = this;
            this.setError('');
            if (this.cantGoForward)
                return;
            if (this.currentStep && !this.currentStep.onNext()) {
                // can't go next because step blocks it, inform user
                return;
            }
            this.wizardService.next(this.node, function (result) {
                if (result.CanTransition) {
                    _this.currentStepIndex++;
                    _this.refresh();
                }
                else {
                    _this.setError(result.ErrorMessage);
                }
            }, function (error) {
                _this.setError(error);
            });
        };
        WizardController.prototype.addNode = function () {
            var _this = this;
            this.wizardService.addNode(this.node, function () {
                _this.cancel();
            }, function (error) {
                _this.setError(error);
            });
        };
        WizardController.prototype.cancel = function () {
            this.setError('');
            this.createNewNode();
            this.reset();
            this.$location.path("/");
        };
        WizardController.prototype.refresh = function () {
            this.cantGoBack = this.currentStepIndex == 0;
            this.cantGoForward = this.currentStepIndex == this.stepDefinitions.length - 1;
            if (this.stepDefinitions.length > 0) {
                this.currentStepControlName = this.stepDefinitions[this.currentStepIndex].ControlName;
            }
        };
        WizardController.prototype.setError = function (error) {
            this.errorMessage = error;
            this.showError = error != '';
        };
        WizardController.prototype.reset = function () {
            var _this = this;
            this.wizardService.cancel(function () {
                _this.currentStepIndex = 0;
                _this.refresh();
                if (_this.currentStep) {
                    _this.currentStep.setNode(_this.node);
                }
            }, function (error) {
                _this.setError(error);
            });
        };
        WizardController.prototype.registerStep = function (step) {
            this.currentStep = step;
            this.currentStep.setNode(this.node);
        };
        WizardController.prototype.createNewNode = function () {
            this.node = new AddNodeWizard.Node();
            this.node.PollingMethod = "ICMP";
        };
        return WizardController;
    }());
    AddNodeWizard.WizardController = WizardController;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=WizardController.js.map