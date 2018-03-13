var AddNodeWizard;
(function (AddNodeWizard) {
    var DefineNodeWizardStepController = (function () {
        function DefineNodeWizardStepController() {
        }
        DefineNodeWizardStepController.prototype.setNode = function (node) {
            this.node = node;
            this.nodeTypeChanged();
        };
        DefineNodeWizardStepController.prototype.onNext = function () {
            this.ipAddressOrHostnameIsValid = true;
            this.credentialsAreValid = true;
            this.ipAddressOrHostnameIsValid = this.validateIpOrHostname();
            this.credentialsAreValid = this.validateCredentials();
            return this.ipAddressOrHostnameIsValid
                && this.credentialsAreValid;
        };
        DefineNodeWizardStepController.prototype.validateIpOrHostname = function () {
            if (!this.node.IpOrHostname) {
                this.ipAddressOrHostnameIsValid = false;
                this.ipAddressOrHostnameValidationError = 'IP Address or hostname must be filled in';
                return false;
            }
            return true;
        };
        DefineNodeWizardStepController.prototype.validateCredentials = function () {
            switch (this.node.PollingMethod) {
                case AddNodeWizard.PollingMethod.SNMP:
                    return this.validateSnmpCredentials();
                case AddNodeWizard.PollingMethod.WMI:
                    return this.validateWmiCredentials();
                default:
                    return true;
            }
        };
        DefineNodeWizardStepController.prototype.validateSnmpCredentials = function () {
            if (!this.node.SnmpCredentials.Port) {
                this.credentialsValidationError = 'SNMP port number must be filled in';
                return false;
            }
            if (!this.node.SnmpCredentials.Port.toString().match(/^\d+$/)) {
                this.credentialsValidationError = 'SNMP port number must be a number';
                return false;
            }
            if (!this.node.SnmpCredentials.CommunityString) {
                this.credentialsValidationError = 'SNMP community string must be filled in';
                return false;
            }
            return true;
        };
        DefineNodeWizardStepController.prototype.validateWmiCredentials = function () {
            if (!this.node.WmiCredentials.Username) {
                this.credentialsValidationError = 'WMI username must be filled in';
                return false;
            }
            if (!this.node.WmiCredentials.Password) {
                this.credentialsValidationError = 'WMI password must be filled in';
                return false;
            }
            if (this.node.WmiCredentials.Password !== this.node.WmiCredentials.PasswordConfirm) {
                this.credentialsValidationError = 'WMI passwords must be same';
                return false;
            }
            return true;
        };
        DefineNodeWizardStepController.prototype.testCredentials = function () {
            switch (this.node.PollingMethod) {
                case AddNodeWizard.PollingMethod.SNMP:
                    break;
                case AddNodeWizard.PollingMethod.WMI:
                    break;
            }
        };
        DefineNodeWizardStepController.prototype.nodeTypeChanged = function () {
            this.showSnmpCredentials = this.node.PollingMethod === AddNodeWizard.PollingMethod.SNMP;
            this.showWmiCredentials = this.node.PollingMethod === AddNodeWizard.PollingMethod.WMI;
        };
        return DefineNodeWizardStepController;
    }());
    AddNodeWizard.DefineNodeWizardStepController = DefineNodeWizardStepController;
})(AddNodeWizard || (AddNodeWizard = {}));
//# sourceMappingURL=DefineNodeWizardStepController.js.map